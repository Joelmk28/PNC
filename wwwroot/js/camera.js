// Gestion de la caméra pour la capture de photos
let stream = null;
let video = null;
let canvas = null;

// Initialisation de la caméra
async function initializeCamera() {
    try {
        console.log('🚀 Initialisation de la caméra...');
        
        // Vérifier que les éléments DOM sont présents
        video = document.getElementById('video');
        canvas = document.getElementById('canvas');
        
        if (!video) {
            console.error('❌ Élément vidéo non trouvé');
            return;
        }
        
        if (!canvas) {
            console.error('❌ Élément canvas non trouvé');
            return;
        }
        
        console.log('✅ Éléments DOM trouvés');
        
        // Vérifier la disponibilité des périphériques
        const devices = await navigator.mediaDevices.enumerateDevices();
        const videoDevices = devices.filter(device => device.kind === 'videoinput');
        
        console.log('📹 Périphériques vidéo disponibles:', videoDevices.length);
        
        if (videoDevices.length === 0) {
            throw new Error('Aucune caméra détectée. Vérifiez que votre caméra est bien connectée et reconnue par le système.');
        }
        
        // Afficher les caméras disponibles dans la console
        videoDevices.forEach((device, index) => {
            console.log(`📹 Caméra ${index + 1}: ${device.label || `Caméra ${index + 1}`}`);
        });
        
        // Prioriser la caméra intégrée (généralement la première)
        const preferredDevice = videoDevices.find(device => 
            device.label.toLowerCase().includes('integrated') || 
            device.label.toLowerCase().includes('built-in') ||
            device.label.toLowerCase().includes('webcam') ||
            device.label.toLowerCase().includes('front')
        );
        
        if (preferredDevice) {
            console.log('🎯 Caméra préférée trouvée:', preferredDevice.label);
        }
        
        // Demander l'accès à la caméra
        console.log('📹 Demande d\'accès à la caméra...');
        
        // Vérifier d'abord les permissions
        if (navigator.permissions) {
            const permission = await navigator.permissions.query({ name: 'camera' });
            console.log('📋 Permission caméra:', permission.state);
            
            if (permission.state === 'denied') {
                // Au lieu de lancer une erreur, essayer quand même de demander l'accès
                console.log('⚠️ Permission refusée précédemment, tentative de nouvelle demande...');
            }
        }
        
        // Essayer différentes caméras par ordre de priorité
        let cameraFound = false;
        
        // 1. Essayer d'abord une caméra USB si disponible
        for (let i = 0; i < videoDevices.length; i++) {
            const device = videoDevices[i];
            const isUSB = device.label.toLowerCase().includes('usb') || 
                         device.label.toLowerCase().includes('camera') ||
                         device.label.toLowerCase().includes('webcam');
            
            if (isUSB) {
                try {
                    console.log(`📹 Tentative avec caméra USB: ${device.label}`);
                    stream = await navigator.mediaDevices.getUserMedia({ 
                        video: { 
                            deviceId: { exact: device.deviceId },
                            width: { ideal: 1280, min: 640 },
                            height: { ideal: 720, min: 480 },
                            frameRate: { ideal: 30, min: 15 }
                        } 
                    });
                    console.log('✅ Caméra USB connectée avec succès');
                    cameraFound = true;
                    break;
                } catch (error) {
                    console.log(`⚠️ Échec avec ${device.label}: ${error.message}`);
                    continue;
                }
            }
        }
        
        // 2. Si pas de caméra USB, essayer la caméra intégrée
        if (!cameraFound) {
            try {
                console.log('📹 Tentative avec caméra intégrée...');
                stream = await navigator.mediaDevices.getUserMedia({ 
                    video: { 
                        width: { ideal: 1280, min: 640 },
                        height: { ideal: 720, min: 480 },
                        facingMode: 'user',
                        frameRate: { ideal: 30, min: 15 }
                    } 
                });
                console.log('✅ Caméra intégrée utilisée');
                cameraFound = true;
            } catch (error) {
                console.log('⚠️ Caméra intégrée non disponible');
            }
        }
        
        // 3. Fallback : n'importe quelle caméra disponible
        if (!cameraFound) {
            console.log('📹 Tentative avec n\'importe quelle caméra disponible...');
            stream = await navigator.mediaDevices.getUserMedia({ 
                video: { 
                    width: { ideal: 640 },
                    height: { ideal: 480 }
                } 
            });
            console.log('✅ Caméra trouvée (fallback)');
        }
        
        console.log('✅ Accès à la caméra accordé');
        
        // La vidéo est maintenant prête - pas besoin de message de permission
        
        // Configurer la vidéo
        video.srcObject = stream;
        video.style.display = 'block';
        canvas.style.display = 'none';
        
        // Attendre que la vidéo soit prête
        video.onloadedmetadata = () => {
            console.log('✅ Vidéo prête, dimensions:', video.videoWidth, 'x', video.videoHeight);
            
            // Afficher le bouton de capture
            const captureBtn = document.getElementById('captureBtn');
            if (captureBtn) {
                captureBtn.style.display = 'inline-block';
            }
            
            // Mettre à jour le statut
            const statusElement = document.getElementById('camera-status');
            if (statusElement) {
                statusElement.innerHTML = '<p class="text-success"><i class="bi bi-check-circle"></i> Caméra prête - Vous pouvez prendre une photo</p>';
            }
        };
        
        // Gérer les déconnexions de la caméra
        stream.getVideoTracks().forEach(track => {
            track.onended = () => {
                console.log('⚠️ Flux vidéo interrompu, tentative de reconnexion...');
                // Essayer de redémarrer la caméra après 1 seconde
                setTimeout(() => {
                    if (document.getElementById('camera-video')) {
                        console.log('🔄 Redémarrage automatique de la caméra...');
                        initializeCamera();
                    }
                }, 1000);
            };
        });
        
        // Empêcher la caméra de s'endormir
        setInterval(() => {
            if (video && video.srcObject && !video.paused) {
                // Ping silencieux pour maintenir la connexion
                const tracks = video.srcObject.getVideoTracks();
                if (tracks.length > 0 && tracks[0].readyState === 'live') {
                    console.log('📡 Caméra active');
                }
            }
        }, 30000); // Vérifier toutes les 30 secondes
        
        console.log('🎉 Caméra initialisée avec succès !');
    } catch (error) {
        console.error('❌ Erreur lors de l\'initialisation de la caméra:', error);
        
        let errorMessage = 'Impossible d\'accéder à la caméra. ';
        
        if (error.name === 'NotAllowedError') {
            errorMessage += 'Permission refusée. Veuillez autoriser l\'accès à la caméra dans votre navigateur.';
        } else if (error.name === 'NotFoundError') {
            errorMessage += 'Aucune caméra trouvée. Vérifiez que votre caméra est bien connectée.';
        } else if (error.name === 'NotReadableError') {
            errorMessage += 'La caméra est utilisée par une autre application. Fermez les autres applications qui utilisent la caméra.';
        } else {
            errorMessage += `Erreur: ${error.message}`;
        }
        
        alert(errorMessage);
        
        // Afficher des instructions détaillées dans la console
        console.log('🔧 Instructions pour résoudre le problème:');
        console.log('1. Vérifiez que votre caméra est bien connectée');
        console.log('2. Assurez-vous qu\'aucune autre application n\'utilise la caméra');
        console.log('3. Vérifiez les permissions du navigateur (icône cadenas dans la barre d\'adresse)');
        console.log('4. Essayez de rafraîchir la page et de réessayer');
    }
}

// Capture de la photo
function takePhoto() {
    if (!video || !stream) {
        console.error('❌ Caméra non initialisée');
        alert('Veuillez d\'abord initialiser la caméra');
        return;
    }
    
    if (video.readyState < 2) {
        console.error('❌ Vidéo pas encore prête');
        alert('Attendez que la caméra soit complètement chargée');
        return;
    }
    
    canvas = document.getElementById('camera-canvas');
    if (!canvas) {
        console.error('❌ Canvas non trouvé');
        return;
    }
    
    const context = canvas.getContext('2d');
    
    // Utiliser les dimensions réelles de la vidéo pour une meilleure qualité
    const width = video.videoWidth || video.clientWidth;
    const height = video.videoHeight || video.clientHeight;
    
    canvas.width = width;
    canvas.height = height;
    
    console.log(`📸 Capture photo: ${width}x${height}px`);
    
    // Dessiner l'image de la vidéo sur le canvas avec une meilleure qualité
    context.drawImage(video, 0, 0, width, height);
    
    // Convertir le canvas en base64 avec une qualité élevée
    const photoData = canvas.toDataURL('image/jpeg', 0.95);
    
    // Masquer la vidéo et afficher la photo
    video.style.display = 'none';
    canvas.style.display = 'block';
    
    // Envoyer la photo au composant Blazor
    if (window.DotNet) {
        window.DotNet.invokeMethodAsync('PNC', 'OnPhotoCaptured', photoData);
    } else {
        console.log('Photo capturée:', photoData.substring(0, 100) + '...');
    }
    
    // Arrêter le flux vidéo
    if (stream) {
        stream.getTracks().forEach(track => track.stop());
        stream = null;
    }
}

// Nettoyage de la caméra
function stopCamera() {
    if (stream) {
        stream.getTracks().forEach(track => track.stop());
        stream = null;
    }
    
    if (video) {
        video.srcObject = null;
        video = null;
    }
    
    if (canvas) {
        canvas = null;
    }
    
    console.log('Caméra arrêtée');
}

// Fonction pour lister les caméras disponibles
async function listAvailableCameras() {
    try {
        const devices = await navigator.mediaDevices.enumerateDevices();
        const videoDevices = devices.filter(device => device.kind === 'videoinput');
        
        console.log('📹 Caméras disponibles:');
        videoDevices.forEach((device, index) => {
            const label = device.label || `Caméra ${index + 1}`;
            const isUSB = label.toLowerCase().includes('usb') || 
                         label.toLowerCase().includes('camera') ||
                         label.toLowerCase().includes('webcam');
            const icon = isUSB ? '🔌' : '💻';
            console.log(`${icon} ${index + 1}. ${label} (ID: ${device.deviceId.substring(0, 8)}...)`);
        });
        
        return videoDevices;
    } catch (error) {
        console.error('❌ Erreur lors de la liste des caméras:', error);
        return [];
    }
}

// Variables pour l'édition photo
let editorCanvas;
let editorContext;
let originalImageData;
let currentImageData;
let cropMode = false;
let cropData = null;

// Fonction pour initialiser l'éditeur photo
async function initPhotoEditor(photoDataUrl) {
    console.log('🖼️ Initialisation de l\'éditeur photo...');
    
    editorCanvas = document.getElementById('photo-editor-canvas');
    if (!editorCanvas) {
        console.error('❌ Canvas éditeur non trouvé');
        return;
    }
    
    editorContext = editorCanvas.getContext('2d');
    
    // Charger l'image
    const img = new Image();
    img.onload = function() {
        // Ajuster la taille du canvas
        editorCanvas.width = img.width;
        editorCanvas.height = img.height;
        
        // Dessiner l'image
        editorContext.drawImage(img, 0, 0);
        
        // Sauvegarder les données originales
        originalImageData = editorContext.getImageData(0, 0, img.width, img.height);
        currentImageData = editorContext.getImageData(0, 0, img.width, img.height);
        
        console.log('✅ Éditeur photo initialisé');
    };
    img.src = photoDataUrl;
}

// Fonction pour obtenir les données de l'image éditée
function getEditedPhotoData() {
    if (!editorCanvas) return null;
    return editorCanvas.toDataURL('image/jpeg', 0.95);
}

// Fonction pour faire pivoter l'image
function rotatePhoto(degrees) {
    if (!editorCanvas || !currentImageData) return;
    
    console.log(`🔄 Rotation de ${degrees} degrés...`);
    
    const tempCanvas = document.createElement('canvas');
    const tempContext = tempCanvas.getContext('2d');
    
    if (degrees === 90 || degrees === -90 || degrees === 270) {
        // Pour rotations de 90°, inverser width/height
        tempCanvas.width = editorCanvas.height;
        tempCanvas.height = editorCanvas.width;
        
        // Centrer et faire pivoter
        tempContext.translate(tempCanvas.width / 2, tempCanvas.height / 2);
        tempContext.rotate((degrees * Math.PI) / 180);
        tempContext.drawImage(editorCanvas, -editorCanvas.width / 2, -editorCanvas.height / 2);
        
        // Ajuster le canvas principal
        editorCanvas.width = tempCanvas.width;
        editorCanvas.height = tempCanvas.height;
    } else {
        // Pour rotations de 180°
        tempCanvas.width = editorCanvas.width;
        tempCanvas.height = editorCanvas.height;
        
        tempContext.translate(tempCanvas.width / 2, tempCanvas.height / 2);
        tempContext.rotate((degrees * Math.PI) / 180);
        tempContext.drawImage(editorCanvas, -editorCanvas.width / 2, -editorCanvas.height / 2);
    }
    
    // Copier le résultat vers le canvas principal
    editorContext.clearRect(0, 0, editorCanvas.width, editorCanvas.height);
    editorContext.drawImage(tempCanvas, 0, 0);
    
    // Mettre à jour les données courantes
    currentImageData = editorContext.getImageData(0, 0, editorCanvas.width, editorCanvas.height);
}

// Fonction pour redimensionner l'image
function resizePhoto(percentage) {
    if (!editorCanvas || !originalImageData) return;
    
    console.log(`📏 Redimensionnement à ${percentage}%...`);
    
    const scale = percentage / 100;
    const newWidth = Math.floor(originalImageData.width * scale);
    const newHeight = Math.floor(originalImageData.height * scale);
    
    // Créer un canvas temporaire avec l'image originale
    const tempCanvas = document.createElement('canvas');
    const tempContext = tempCanvas.getContext('2d');
    tempCanvas.width = originalImageData.width;
    tempCanvas.height = originalImageData.height;
    tempContext.putImageData(originalImageData, 0, 0);
    
    // Redimensionner le canvas principal
    editorCanvas.width = newWidth;
    editorCanvas.height = newHeight;
    
    // Dessiner l'image redimensionnée
    editorContext.drawImage(tempCanvas, 0, 0, newWidth, newHeight);
    
    // Mettre à jour les données courantes
    currentImageData = editorContext.getImageData(0, 0, newWidth, newHeight);
}

// Fonction pour appliquer des filtres
function applyFilter(filterType) {
    if (!editorCanvas || !currentImageData) return;
    
    console.log(`🎨 Application du filtre: ${filterType}...`);
    
    const imageData = editorContext.getImageData(0, 0, editorCanvas.width, editorCanvas.height);
    const data = imageData.data;
    
    for (let i = 0; i < data.length; i += 4) {
        const r = data[i];
        const g = data[i + 1];
        const b = data[i + 2];
        
        if (filterType === 'grayscale') {
            const gray = 0.299 * r + 0.587 * g + 0.114 * b;
            data[i] = gray;
            data[i + 1] = gray;
            data[i + 2] = gray;
        } else if (filterType === 'sepia') {
            data[i] = Math.min(255, (r * 0.393) + (g * 0.769) + (b * 0.189));
            data[i + 1] = Math.min(255, (r * 0.349) + (g * 0.686) + (b * 0.168));
            data[i + 2] = Math.min(255, (r * 0.272) + (g * 0.534) + (b * 0.131));
        }
    }
    
    editorContext.putImageData(imageData, 0, 0);
    currentImageData = imageData;
}

// Fonction pour démarrer le mode rognage
function startCropMode() {
    cropMode = true;
    const overlay = document.getElementById('crop-overlay');
    if (overlay) {
        overlay.style.display = 'block';
        initCropHandlers();
    }
    console.log('✂️ Mode rognage activé');
}

// Fonction pour arrêter le mode rognage
function endCropMode() {
    cropMode = false;
    const overlay = document.getElementById('crop-overlay');
    if (overlay) {
        overlay.style.display = 'none';
    }
    
    if (cropData) {
        applyCrop();
        cropData = null;
    }
    console.log('✂️ Mode rognage désactivé');
}

// Fonction pour initialiser les gestionnaires de rognage
function initCropHandlers() {
    // Implémentation basique - peut être améliorée
    const overlay = document.getElementById('crop-overlay');
    const cropBox = document.getElementById('crop-box');
    
    if (!overlay || !cropBox) return;
    
    // Position initiale du crop box (centré, 50% de la taille)
    const rect = editorCanvas.getBoundingClientRect();
    const boxWidth = rect.width * 0.5;
    const boxHeight = rect.height * 0.5;
    const boxLeft = (rect.width - boxWidth) / 2;
    const boxTop = (rect.height - boxHeight) / 2;
    
    cropBox.style.left = boxLeft + 'px';
    cropBox.style.top = boxTop + 'px';
    cropBox.style.width = boxWidth + 'px';
    cropBox.style.height = boxHeight + 'px';
}

// Fonction pour appliquer le rognage
function applyCrop() {
    if (!cropData || !editorCanvas) return;
    
    console.log('✂️ Application du rognage...');
    
    const { x, y, width, height } = cropData;
    const imageData = editorContext.getImageData(x, y, width, height);
    
    editorCanvas.width = width;
    editorCanvas.height = height;
    editorContext.putImageData(imageData, 0, 0);
    
    currentImageData = imageData;
}

// Fonction pour réinitialiser l'image
function resetPhoto(originalPhotoData) {
    if (!originalPhotoData) return;
    
    console.log('🔄 Reset de l\'image...');
    
    const img = new Image();
    img.onload = function() {
        editorCanvas.width = img.width;
        editorCanvas.height = img.height;
        editorContext.drawImage(img, 0, 0);
        currentImageData = editorContext.getImageData(0, 0, img.width, img.height);
        console.log('✅ Image réinitialisée');
    };
    img.src = originalPhotoData;
}

// Fonctions exposées globalement pour Blazor
window.initCamera = initializeCamera;
window.capturePhoto = takePhoto;
window.cleanupCamera = stopCamera;
window.listCameras = listAvailableCameras;

// Nouvelles fonctions d'édition
window.initPhotoEditor = initPhotoEditor;
window.getEditedPhotoData = getEditedPhotoData;
window.rotatePhoto = rotatePhoto;
window.resizePhoto = resizePhoto;
window.applyFilter = applyFilter;
window.startCropMode = startCropMode;
window.endCropMode = endCropMode;
window.resetPhoto = resetPhoto;

// Vérification de la compatibilité
if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
    console.log('✅ API getUserMedia supportée');
} else {
    console.error('❌ API getUserMedia non supportée');
}
