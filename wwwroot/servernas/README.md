# Dossier des Images des Policiers

Ce dossier contient les images des policiers organisées par policier.

## Structure des dossiers

```
servernas/
├── NomPolicier_ID/
│   └── images/
│       ├── photo_20241220_143022.jpg
│       ├── photo_20241220_143045.jpg
│       └── ...
├── AutrePolicier_ID/
│   └── images/
│       └── ...
└── ...
```

## Format des noms

- **Dossier policier** : `NomPolicier_ID` (ex: `Jean_Dupont_123`)
- **Fichier image** : `photo_YYYYMMDD_HHMMSS.jpg`

## Gestion automatique

- Les images sont automatiquement organisées lors de la capture
- Chaque policier a son propre dossier
- Les anciennes images sont supprimées lors de la prise d'une nouvelle photo
- Les images temporaires sont déplacées vers le bon dossier après sauvegarde

## Sécurité

- Seules les images des policiers sont stockées ici
- Les noms de fichiers sont sanitisés pour éviter les caractères dangereux
- Les chemins sont validés avant utilisation
