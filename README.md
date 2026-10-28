# Rockfeller · magasin en réalité virtuelle

Projet d'équipe du Master Sciences et Technologies du Métavers (INSA Hauts-de-France), sous Unity 6. Le joueur évolue dans un magasin et des bureaux en VR : clients non joueurs, dialogues et négociation, inventaire, et un ordinateur utilisable dans la scène.

## Ma partie

- **Mise en place de l'environnement VR** (XR Interaction Toolkit) et portage des scènes
- **Un ordinateur jouable en VR** (`Assets/Script pc/`) : bureau avec icônes, fenêtres d'application, curseur piloté à la main
- **Un quiz d'entretien sur cet ordinateur** : questions chargées depuis des données, logique de réponse et interface (`QuizLoaderAndManager`, `QuizUI`)
- **Le système d'inventaire** : une ceinture qui suit le joueur (`VRBeltFollower`), avec ses objets et textures

## Stack

Unity 6 (6000.0), C#, XR Interaction Toolkit 3, XR Hands, OpenXR.

## Lancer

1. Cloner le dépôt et l'ouvrir avec Unity Hub (Unity 6000.0).
2. Brancher un casque compatible OpenXR (Quest via Link par exemple), puis Play.
