# 🔍 Guide de Dépannage - Caméra PNC

## 🚨 **PROBLÈME : Cadre vidéo noir**

Si vous voyez un **cadre vidéo noir** au lieu de l'image de votre caméra, voici comment diagnostiquer et résoudre le problème.

## 🧪 **ÉTAPE 1 : Diagnostic automatique**

Accédez à la page de débogage : `/camera-debug`

Cette page va automatiquement :
- ✅ Vérifier votre navigateur et sa version
- ✅ Tester la disponibilité des APIs de caméra
- ✅ Vérifier les permissions
- ✅ Tester l'accès à la caméra
- ✅ Lister toutes les caméras disponibles

## 🔍 **ÉTAPE 2 : Vérifications manuelles**

### **1. Vérifier les permissions du navigateur**

#### **Chrome/Edge :**
1. Cliquez sur l'icône **🔒** (cadenas) dans la barre d'adresse
2. Vérifiez que l'accès à la **caméra** est autorisé
3. Si bloqué, cliquez sur "Autoriser"

#### **Firefox :**
1. Cliquez sur l'icône **🛡️** (bouclier) dans la barre d'adresse
2. Vérifiez les permissions de la caméra
3. Autorisez l'accès si nécessaire

### **2. Vérifier qu'aucune autre application n'utilise la caméra**

- ❌ **Fermez** : Zoom, Teams, Skype, Discord
- ❌ **Fermez** : Applications de visioconférence
- ❌ **Fermez** : Logiciels de capture vidéo
- ❌ **Fermez** : Applications de streaming

### **3. Vérifier la connexion de la caméra**

#### **Caméra USB :**
- ✅ Vérifiez que le câble USB est bien connecté
- ✅ Essayez un autre port USB
- ✅ Testez sur un autre ordinateur

#### **Caméra intégrée :**
- ✅ Vérifiez qu'elle n'est pas désactivée dans le BIOS
- ✅ Vérifiez les pilotes Windows

## 🛠️ **ÉTAPE 3 : Solutions par erreur**

### **Erreur : "NotAllowedError"**
```
❌ Accès refusé à la caméra
```

**Solutions :**
1. **Autoriser l'accès** dans les paramètres du navigateur
2. **Rafraîchir la page** après autorisation
3. **Vérifier** que vous n'êtes pas en mode navigation privée

### **Erreur : "NotFoundError"**
```
❌ Aucune caméra trouvée
```

**Solutions :**
1. **Vérifier** la connexion physique de la caméra
2. **Redémarrer** l'ordinateur
3. **Vérifier** les pilotes dans le Gestionnaire de périphériques
4. **Tester** la caméra dans l'application "Appareil photo" Windows

### **Erreur : "NotReadableError"**
```
❌ Caméra déjà utilisée par une autre application
```

**Solutions :**
1. **Fermer** toutes les applications qui utilisent la caméra
2. **Redémarrer** le navigateur
3. **Redémarrer** l'ordinateur si nécessaire

### **Erreur : "OverconstrainedError"**
```
❌ Contraintes vidéo non supportées
```

**Solutions :**
1. **Réduire** la résolution demandée
2. **Utiliser** des contraintes plus souples
3. **Vérifier** les capacités de votre caméra

## 🌐 **ÉTAPE 4 : Vérifications navigateur**

### **Chrome (Recommandé)**
- ✅ **Version minimale** : Chrome 60+
- ✅ **Support complet** des APIs MediaDevices
- ✅ **Gestion des permissions** avancée

### **Firefox**
- ✅ **Version minimale** : Firefox 55+
- ✅ **Support complet** des APIs MediaDevices
- ⚠️ **Permissions** parfois plus strictes

### **Edge**
- ✅ **Version minimale** : Edge 79+
- ✅ **Support complet** des APIs MediaDevices
- ✅ **Intégration Windows** native

### **Safari**
- ✅ **Version minimale** : Safari 11+
- ⚠️ **Support limité** sur Windows
- ⚠️ **Permissions** plus restrictives

## 🔧 **ÉTAPE 5 : Solutions techniques**

### **Solution 1 : Forcer l'activation de la caméra**
```javascript
// Dans la console du navigateur
navigator.mediaDevices.getUserMedia({ video: true })
    .then(stream => {
        console.log('✅ Caméra activée');
        const video = document.querySelector('video');
        if (video) video.srcObject = stream;
    })
    .catch(err => console.error('❌ Erreur:', err));
```

### **Solution 2 : Vérifier les caméras disponibles**
```javascript
// Lister toutes les caméras
navigator.mediaDevices.enumerateDevices()
    .then(devices => {
        const videoDevices = devices.filter(d => d.kind === 'videoinput');
        console.log('📹 Caméras:', videoDevices);
    });
```

### **Solution 3 : Tester avec des contraintes minimales**
```javascript
// Contraintes minimales
const constraints = { 
    video: { 
        width: { min: 320, ideal: 640 },
        height: { min: 240, ideal: 480 }
    } 
};

navigator.mediaDevices.getUserMedia(constraints)
    .then(stream => console.log('✅ Succès'))
    .catch(err => console.error('❌ Échec:', err));
```

## 📱 **ÉTAPE 6 : Solutions mobiles**

### **Android**
- ✅ **Chrome mobile** : Support complet
- ✅ **Firefox mobile** : Support complet
- ⚠️ **Permissions** : Vérifier dans Paramètres > Applications

### **iOS**
- ✅ **Safari** : Support complet iOS 11+
- ⚠️ **Chrome/Firefox** : Support limité (utiliser Safari)

## 🚀 **ÉTAPE 7 : Test de la solution**

### **1. Ouvrir la page de débogage**
Accédez à `/camera-debug`

### **2. Cliquer sur "Tester la Caméra"**
Le système va automatiquement :
- Vérifier les permissions
- Tester l'accès à la caméra
- Afficher les logs détaillés

### **3. Vérifier les logs**
Les logs vous diront exactement où est le problème :
- 📋 **Permissions** : Autorisées/Refusées
- 📹 **Caméra** : Trouvée/Non trouvée
- 🎥 **Flux vidéo** : Réussi/Échoué
- ▶️ **Lecture** : Démarrée/Erreur

### **4. Tester la capture**
Si la caméra fonctionne, testez la capture de photo

## 🔄 **ÉTAPE 8 : Si rien ne fonctionne**

### **1. Redémarrer le navigateur**
- Fermez complètement le navigateur
- Rouvrez-le et testez à nouveau

### **2. Redémarrer l'ordinateur**
- Parfois nécessaire pour libérer la caméra

### **3. Vérifier les pilotes**
- Ouvrir le **Gestionnaire de périphériques**
- Vérifier que la caméra apparaît sans erreur
- Mettre à jour les pilotes si nécessaire

### **4. Tester sur un autre navigateur**
- Si Chrome ne fonctionne pas, essayez Firefox
- Si Firefox ne fonctionne pas, essayez Edge

### **5. Vérifier l'antivirus**
- Certains antivirus bloquent l'accès à la caméra
- Ajoutez une exception pour votre navigateur

## 📞 **Support technique**

Si le problème persiste après avoir suivi toutes ces étapes :

1. **Notez** les messages d'erreur exacts
2. **Capturez** une capture d'écran de la page de débogage
3. **Vérifiez** la console du navigateur pour les erreurs JavaScript
4. **Contactez** l'équipe technique avec ces informations

## 🎯 **Résumé des étapes**

1. **🧪 Diagnostic automatique** → `/camera-debug`
2. **🔍 Vérifications manuelles** → Permissions, connexion, autres apps
3. **🛠️ Solutions par erreur** → Traitement spécifique selon l'erreur
4. **🌐 Vérifications navigateur** → Version et support
5. **🔧 Solutions techniques** → Code de débogage
6. **📱 Solutions mobiles** → Spécificités Android/iOS
7. **🚀 Test de la solution** → Vérification du bon fonctionnement
8. **🔄 Solutions de dernier recours** → Redémarrage, pilotes, support

---

**💡 Conseil : Commencez toujours par la page de débogage `/camera-debug` qui vous donnera un diagnostic précis du problème !**
