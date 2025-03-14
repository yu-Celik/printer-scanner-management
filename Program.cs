using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POOAV_EX4
{

    interface IConnectable
    {
        bool IsConnected { get; }
        enum ConnectionType { Ethernet, Wifi, Bluetooth, USB };
        ConnectionType TypeConnection { get; set; }
        void Connect();
        void Disconnect();
    }

    interface IPrintable
    {
        int InkLevel { get; }
        void Print(string document);
        void RefillInk(int amount);
    }

    interface IScanable
    {
        int ScanResolution { get; set; }
        void Scan(string document);
    }

    class Printer : IConnectable, IPrintable
    {
        private const int MAX_INK_LEVEL = 100;
        private const int MIN_INK_LEVEL = 0;
        private const int LOW_INK_THRESHOLD = 10;
        private const int EMPTY_INK_THRESHOLD = 5;
        private const int INK_PER_PRINT = 5;

        public string Model { get; private set; }
        public IConnectable.ConnectionType TypeConnection { get; set; }
        public bool IsConnected { get; private set; } = false;
        public int InkLevel { get; private set; }

        public Printer(
            string model,
            int inkLevel,
            IConnectable.ConnectionType typeConnection = IConnectable.ConnectionType.Ethernet)
        {
            if (string.IsNullOrWhiteSpace(model)) { throw new ArgumentException("Le modèle de l'imprimante ne peut pas être vide"); }
            if (inkLevel > MAX_INK_LEVEL || inkLevel < MIN_INK_LEVEL) { throw new ArgumentException($"Le niveau d'encre doit être entre {MIN_INK_LEVEL} et {MAX_INK_LEVEL}"); }
            Model = model;
            TypeConnection = typeConnection;
            InkLevel = inkLevel;
        }

        public Printer(string model) : this(model, MAX_INK_LEVEL, IConnectable.ConnectionType.Ethernet) { }

        public void Connect()
        {
            IsConnected = true;
            Console.WriteLine($"Imprimante {Model} connectée via {TypeConnection}");
        }

        public void Disconnect()
        {
            IsConnected = false;
            Console.WriteLine($"Imprimante {Model} déconnectée");
        }

        public void Print(string document)
        {
            if (string.IsNullOrWhiteSpace(document)) { Console.WriteLine("Le document à imprimer ne peut pas être vide"); return; }

            if (!IsConnected) { Console.WriteLine($"L'imprimante {Model} n'est pas connectée. Veuillez la connecter d'abord."); return; }

            if (InkLevel <= EMPTY_INK_THRESHOLD) { Console.WriteLine($"L'imprimante {Model} n'a plus d'encre. Niveau actuel: {InkLevel}%"); return; }

            if (InkLevel <= LOW_INK_THRESHOLD) { Console.WriteLine($"Attention: Niveau d'encre bas ({InkLevel}%). Pensez à recharger bientôt."); }

            InkLevel -= INK_PER_PRINT;
            Console.WriteLine($"Impression de '{document}' en cours... Niveau d'encre restant: {InkLevel}%");
        }

        public void RefillInk(int amount)
        {
            if (amount > MAX_INK_LEVEL || amount < MIN_INK_LEVEL)
            {
                Console.WriteLine($"La quantité d'encre doit être entre {MIN_INK_LEVEL} et {MAX_INK_LEVEL}");
                return;
            }

            if (InkLevel + amount > MAX_INK_LEVEL)
            {
                Console.WriteLine($"Impossible d'ajouter {amount}%. Le niveau maximum est de {MAX_INK_LEVEL}%. Niveau actuel: {InkLevel}%");
                return;
            }

            InkLevel += amount;
            Console.WriteLine($"Rechargement effectué. Nouveau niveau d'encre: {InkLevel}%");
        }

        public override string ToString()
        {
            return $"Imprimante {Model} - Connexion: {TypeConnection} ({(IsConnected ? "Connectée" : "Déconnectée")}) - Niveau d'encre: {InkLevel}%";
        }
    }

    class Scanner : IConnectable, IScanable
    {
        private const int MIN_SCAN_RESOLUTION = 100;
        private const int MAX_SCAN_RESOLUTION = 1200;

        public string Model { get; private set; }
        public IConnectable.ConnectionType TypeConnection { get; set; }
        public bool IsConnected { get; private set; } = false;
        public int ScanResolution { get; set; }

        public Scanner(string model, int scanResolution, IConnectable.ConnectionType typeConnection = IConnectable.ConnectionType.Ethernet)
        {
            if (string.IsNullOrWhiteSpace(model)) { throw new ArgumentException("Le modèle de l'imprimante ne peut pas être vide"); }
            if (scanResolution < MIN_SCAN_RESOLUTION || scanResolution > MAX_SCAN_RESOLUTION) { throw new ArgumentException($"La résolution du scanner doit être entre {MIN_SCAN_RESOLUTION} et {MAX_SCAN_RESOLUTION}"); }
            Model = model;
            ScanResolution = scanResolution;
            TypeConnection = typeConnection;
        }

        public Scanner(string model) : this(model, MIN_SCAN_RESOLUTION, IConnectable.ConnectionType.Ethernet) { }

        public void Scan(string document)
        {
            if (string.IsNullOrWhiteSpace(document)) { Console.WriteLine("Le document à scanner ne peut pas être vide"); return; }
            if (!IsConnected) { Console.WriteLine($"Le scanner {Model} n'est pas connecté. Veuillez le connecter d'abord."); return; }
            Console.WriteLine($"Scan de '{document}' en cours... Résolution: {ScanResolution} dpi");
        }

        public void Connect()
        {
            IsConnected = true;
            Console.WriteLine($"Scanner {Model} connecté via {TypeConnection}");
        }

        public void Disconnect()
        {
            IsConnected = false;
            Console.WriteLine($"Scanner {Model} déconnecté");
        }

        public override string ToString()
        {
            return $"Scanner {Model} - Connexion: {TypeConnection} ({(IsConnected ? "Connecté" : "Déconnecté")}) - Résolution: {ScanResolution} dpi";
        }



    }

    class PrinterScanner : IConnectable, IPrintable, IScanable
    {
        private const int MAX_INK_LEVEL = 100;
        private const int MIN_INK_LEVEL = 0;
        private const int LOW_INK_THRESHOLD = 10;
        private const int EMPTY_INK_THRESHOLD = 5;
        private const int INK_PER_PRINT = 5;
        private const int MIN_SCAN_RESOLUTION = 100;
        private const int MAX_SCAN_RESOLUTION = 1200;
        private const int DEFAULT_SCAN_RESOLUTION = 600;

        public string Model { get; private set; }
        public IConnectable.ConnectionType TypeConnection { get; set; }
        public bool IsConnected { get; private set; } = false;
        public int InkLevel { get; private set; }
        public int ScanResolution { get; set; }

        public PrinterScanner(
            string model,
            IConnectable.ConnectionType typeConnection = IConnectable.ConnectionType.Wifi,
            int inkLevel = MAX_INK_LEVEL,
            int scanResolution = DEFAULT_SCAN_RESOLUTION)
        {
            if (string.IsNullOrWhiteSpace(model)) { throw new ArgumentException("Le modèle de l'appareil multifonction ne peut pas être vide"); }

            if (inkLevel > MAX_INK_LEVEL || inkLevel < MIN_INK_LEVEL) { throw new ArgumentException($"Le niveau d'encre doit être entre {MIN_INK_LEVEL} et {MAX_INK_LEVEL}"); }

            if (scanResolution < MIN_SCAN_RESOLUTION || scanResolution > MAX_SCAN_RESOLUTION) { throw new ArgumentException($"La résolution du scanner doit être entre {MIN_SCAN_RESOLUTION} et {MAX_SCAN_RESOLUTION}"); }

            Model = model;
            TypeConnection = typeConnection;
            InkLevel = inkLevel;
            ScanResolution = scanResolution;
        }

        public void Connect()
        {
            IsConnected = true;
            Console.WriteLine($"Appareil multifonction {Model} connecté via {TypeConnection}");
        }

        public void Disconnect()
        {
            IsConnected = false;
            Console.WriteLine($"Appareil multifonction {Model} déconnecté");
        }

        public void Print(string document)
        {
            if (!IsConnected)
            {
                Console.WriteLine($"L'appareil {Model} n'est pas connecté. Veuillez le connecter d'abord.");
                return;
            }

            if (string.IsNullOrWhiteSpace(document))
            {
                Console.WriteLine("Le document à imprimer ne peut pas être vide");
                return;
            }

            if (InkLevel <= EMPTY_INK_THRESHOLD)
            {
                Console.WriteLine($"L'appareil {Model} n'a plus d'encre. Niveau actuel: {InkLevel}%");
                return;
            }

            if (InkLevel <= LOW_INK_THRESHOLD)
            {
                Console.WriteLine($"Attention: Niveau d'encre bas ({InkLevel}%). Pensez à recharger bientôt.");
            }

            InkLevel -= INK_PER_PRINT;
            Console.WriteLine($"Impression de '{document}' en cours... Niveau d'encre restant: {InkLevel}%");
        }

        public void RefillInk(int amount)
        {
            if (!IsConnected)
            {
                Console.WriteLine($"L'appareil {Model} n'est pas connecté. Veuillez le connecter d'abord.");
                return;
            }

            if (amount > MAX_INK_LEVEL || amount < MIN_INK_LEVEL)
            {
                Console.WriteLine($"La quantité d'encre doit être entre {MIN_INK_LEVEL} et {MAX_INK_LEVEL}");
                return;
            }

            if (InkLevel + amount > MAX_INK_LEVEL)
            {
                Console.WriteLine($"Impossible d'ajouter {amount}%. Le niveau maximum est de {MAX_INK_LEVEL}%. Niveau actuel: {InkLevel}%");
                return;
            }

            InkLevel += amount;
            Console.WriteLine($"Rechargement effectué. Nouveau niveau d'encre: {InkLevel}%");
        }

        public void Scan(string document)
        {
            if (!IsConnected)
            {
                Console.WriteLine($"L'appareil {Model} n'est pas connecté. Veuillez le connecter d'abord.");
                return;
            }

            if (string.IsNullOrWhiteSpace(document))
            {
                Console.WriteLine("Le document à scanner ne peut pas être vide");
                return;
            }

            Console.WriteLine($"Scan de '{document}' en cours... Résolution: {ScanResolution} dpi");
        }

        public override string ToString()
        {
            return
                $"Appareil multifonction {Model}\n" +
                $"- État: {(IsConnected ? $"Connecté via {TypeConnection}" : "Déconnecté")}\n" +
                $"- Niveau d'encre: {InkLevel}%\n" +
                $"- Résolution de scan: {ScanResolution} dpi";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Test de l'Imprimante ===");
            var printer = new Printer("HP LaserJet", 80);
            Console.WriteLine(printer.ToString());

            printer.Connect();
            printer.Print("Document1.pdf");
            printer.Print("Document2.pdf");
            printer.RefillInk(30);
            Console.WriteLine(printer.ToString());
            printer.Disconnect();
            Console.WriteLine();

            Console.WriteLine("=== Test du Scanner ===");
            var scanner = new Scanner("Epson V39", 600);
            Console.WriteLine(scanner.ToString());

            scanner.Connect();
            scanner.Scan("Photo1.jpg");
            scanner.Scan("Document3.pdf");
            Console.WriteLine(scanner.ToString());
            scanner.Disconnect();
            Console.WriteLine();

            Console.WriteLine("=== Test de l'Imprimante Multifonction ===");
            var multifunction = new PrinterScanner("HP OfficeJet Pro", IConnectable.ConnectionType.Ethernet, 50, 1200);
            Console.WriteLine(multifunction.ToString());

            multifunction.Connect();
            multifunction.Print("Document4.pdf");
            multifunction.Scan("Photo2.jpg");
            multifunction.RefillInk(20);
            Console.WriteLine(multifunction.ToString());
            multifunction.Disconnect();
            Console.WriteLine();

            Console.WriteLine("=== Test des cas d'erreur ===");
            try
            {
                // Test d'une impression sans connexion
                multifunction.Print("Document5.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur attendue: {ex.Message}");
            }

            try
            {
                // Test d'un scan sans connexion
                scanner.Scan("Photo3.jpg");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur attendue: {ex.Message}");
            }

            try
            {
                // Test d'un rechargement d'encre sans connexion
                multifunction.RefillInk(30);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur attendue: {ex.Message}");
            }
        }
    }
}