# POOAV-EX4: Système de Gestion d'Imprimantes et Scanners

Ce projet est une implémentation en C# d'un système de gestion d'imprimantes et scanners, démontrant les concepts de programmation orientée objet.

## Structure du Projet

Le projet comprend les classes suivantes :

### Interfaces
- `IPrintable` : Interface définissant les opérations de base pour l'impression
- `IScannable` : Interface définissant les opérations de base pour la numérisation

### Classes
- `PrintableDevice` : Classe abstraite implémentant la logique commune pour les appareils d'impression
- `Printer` : Classe représentant une imprimante basique
- `Scanner` : Classe représentant un scanner
- `PrinterScanner` : Classe représentant un appareil multifonction (imprimante + scanner)

## Fonctionnalités

### Imprimante
- Connexion/déconnexion
- Impression de documents
- Gestion des niveaux d'encre
- Recharge d'encre

### Scanner
- Connexion/déconnexion
- Numérisation de documents
- Gestion de la résolution

### Imprimante-Scanner
- Toutes les fonctionnalités de l'imprimante et du scanner
- Gestion unifiée des connexions

## Utilisation

Le projet inclut un programme de test dans la méthode `Main` qui démontre l'utilisation de toutes les classes et leurs fonctionnalités.

## Prérequis
- .NET Framework ou .NET Core
- Un environnement de développement C# (Visual Studio, VS Code, etc.) 