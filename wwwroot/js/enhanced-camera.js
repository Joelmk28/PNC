// Enhanced Camera System for PNC Application
// Gestion avancée des caméras avec sélection et capture optimisée

let currentStream = null;
let currentVideoDevice = null;
let availableDevices = [];
let isCameraActive = false;

// Initialisation du système de caméra
async function initializeCameraSystem() {
    try {
        console.log('🔧 Initialisation du système de caméra...');
        
        // Vérifier si l'API MediaDevices est supportée
        if (!navigator.mediaDevices || !navigator.mediaDevices.enumerateDevices) {
            throw new Error('API MediaDevices non supportée par ce navigateur');
        }
        
        // Énumérer les périphériques vidéo disponibles
        await enumerateVideoDevices();
        
        console.log('✅ Système de caméra initialisé');
        return true;
    } catch (error) {
        console.error('❌ Erreur d\'initialisation:', error);
        return false;
    }
}

// Énumérer les périphériques vidéo disponibles
async function enumerateVideoDevices() {
    try {
        console.log('📹 Énumération des périphériques vidéo...');
        
        // Demander la permission d'accès aux médias d'abord
        await navigator.mediaDevices.getUserMedia({ video: true });
        
        // Énumérer tous les périphériques
        const devices = await navigator.mediaDevices.enumerateDevices();
        
        // Filtrer les périphériques vidéo
        availableDevices = devices.filter(device => device.kind === 'videoinput');
        
        console.log(`📹 ${availableDevices.length} périphérique(s) vidéo trouvé(s):`);
        availableDevices.forEach((device, index) => {
            console.log(`  ${index + 1}. ${device.label || `Caméra ${index + 1}`} (${device.deviceId})`);
        });
        
        // Créer la liste de sélection des caméras
        createCameraSelector();
        
        return availableDevices;
    } catch (error) {
        console.error('❌ Erreur lors de l\'énumération des périphériques:', error);
        throw error;
    }
}

// Créer le sélecteur de caméra
function createCameraSelector() {
    const container = document.getElementById('camera-selector-container');
    if (!container) {
        console.warn('⚠️ Conteneur de sélection de caméra non trouvé');
        return;
    }
    
    container.innerHTML = '';
    
    if (availableDevices.length === 0) {
        container.innerHTML = '<p class="text-muted">Aucune caméra détectée</p>';
        return;
    }
    
    const select = document.createElement('select');
    select.id = 'camera-selector';
    select.className = 'form-select mb-3';
    select.innerHTML = '<option value="">Sélectionner une caméra...</option>';
    
    availableDevices.forEach((device, index) => {
        const option = document.createElement('option');
        option.value = device.deviceId;
        option.textContent = device.label || `Caméra ${index + 1}`;
        select.appendChild(option);
    });
    
    select.addEventListener('change', (e) => {
        if (e.target.value) {
            startCameraWithDevice(e.target.value);
        }
    });
    
    container.appendChild(select);
}

// Démarrer la caméra avec un périphérique spécifique
async function startCameraWithDevice(deviceId) {
    try {
        console.log(`📹 Démarrage de la caméra: ${deviceId}`);
        
        // Arrêter la caméra actuelle si elle est active
        if (currentStream) {
            await stopCurrentCamera();
        }
        
        // Contraintes vidéo avec le périphérique spécifique
        const constraints = {
            video: {
                deviceId: { exact: deviceId },
                width: { ideal: 1280, min: 640 },
                height: { ideal: 720, min: 480 },
                frameRate: { ideal: 30, min: 15 }
            }
        };
        
        // Démarrer le flux vidéo
        currentStream = await navigator.mediaDevices.getUserMedia(constraints);
        currentVideoDevice = deviceId;
        
        // Afficher la vidéo
        const videoElement = document.getElementById('policierVideo');
        if (videoElement) {
            videoElement.srcObject = currentStream;
            videoElement.style.display = 'block';
            
            // Attendre que la vidéo soit prête
            await new Promise((resolve) => {
                videoElement.onloadedmetadata = resolve;
            });
            
            console.log(`✅ Caméra démarrée: ${videoElement.videoWidth}x${videoElement.videoHeight}`);
        }
        
        isCameraActive = true;
        updateCameraUI(true);
        
        return true;
    } catch (error) {
        console.error('❌ Erreur lors du démarrage de la caméra:', error);
        
        // Essayer avec des contraintes moins strictes
        try {
            console.log('🔄 Tentative avec des contraintes moins strictes...');
            const fallbackConstraints = {
                video: {
                    deviceId: { exact: deviceId }
                }
            };
            
            currentStream = await navigator.mediaDevices.getUserMedia(fallbackConstraints);
            currentVideoDevice = deviceId;
            
            const videoElement = document.getElementById('policierVideo');
            if (videoElement) {
                videoElement.srcObject = currentStream;
                videoElement.style.display = 'block';
            }
            
            isCameraActive = true;
            updateCameraUI(true);
            return true;
        } catch (fallbackError) {
            console.error('❌ Échec même avec contraintes réduites:', fallbackError);
            throw fallbackError;
        }
    }
}

// Capturer une photo avec qualité optimisée
async function capturePhotoOptimized() {
    try {
        if (!currentStream || !isCameraActive) {
            throw new Error('Caméra non active');
        }
        
        console.log('📸 Capture de photo en cours...');
        
        const videoElement = document.getElementById('policierVideo');
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');
        
        // Utiliser les dimensions réelles de la vidéo
        const width = videoElement.videoWidth;
        const height = videoElement.videoHeight;
        
        canvas.width = width;
        canvas.height = height;
        
        // Dessiner l'image de la vidéo sur le canvas
        ctx.drawImage(videoElement, 0, 0, width, height);
        
        // Convertir en JPEG avec qualité élevée (0.95)
        const photoDataUrl = canvas.toDataURL('image/jpeg', 0.95);
        
        console.log(`✅ Photo capturée: ${width}x${height}px, taille: ${Math.round(photoDataUrl.length / 1024)}KB`);
        
        // Masquer la vidéo et afficher la photo
        videoElement.style.display = 'none';
        
        const photoElement = document.getElementById('policierPhoto');
        if (photoElement) {
            photoElement.src = photoDataUrl;
            photoElement.style.display = 'block';
        }
        
        // Arrêter la caméra
        await stopCurrentCamera();
        
        // Retourner les données de la photo
        return photoDataUrl;
        
    } catch (error) {
        console.error('❌ Erreur lors de la capture:', error);
        throw error;
    }
}

// Arrêter la caméra actuelle
async function stopCurrentCamera() {
    try {
        if (currentStream) {
            currentStream.getTracks().forEach(track => {
                track.stop();
                console.log(`🛑 Piste ${track.kind} arrêtée`);
            });
            currentStream = null;
        }
        
        currentVideoDevice = null;
        isCameraActive = false;
        
        // Masquer la vidéo
        const videoElement = document.getElementById('policierVideo');
        if (videoElement) {
            videoElement.style.display = 'none';
        }
        
        updateCameraUI(false);
        console.log('✅ Caméra arrêtée');
        
        return true;
    } catch (error) {
        console.error('❌ Erreur lors de l\'arrêt de la caméra:', error);
        return false;
    }
}

// Mettre à jour l'interface utilisateur de la caméra
function updateCameraUI(isActive) {
    const startButton = document.getElementById('start-camera-btn');
    const stopButton = document.getElementById('stop-camera-btn');
    const captureButton = document.getElementById('capture-photo-btn');
    const cameraSelector = document.getElementById('camera-selector');
    
    if (startButton) startButton.style.display = isActive ? 'none' : 'inline-block';
    if (stopButton) stopButton.style.display = isActive ? 'inline-block' : 'none';
    if (captureButton) captureButton.style.display = isActive ? 'inline-block' : 'none';
    if (cameraSelector) cameraSelector.disabled = isActive;
}

// Fonctions globales pour Blazor
window.startCamera = async function(videoId) {
    try {
        // Utiliser la première caméra disponible par défaut
        if (availableDevices.length === 0) {
            await enumerateVideoDevices();
        }
        
        if (availableDevices.length > 0) {
            return await startCameraWithDevice(availableDevices[0].deviceId);
        } else {
            throw new Error('Aucune caméra disponible');
        }
    } catch (error) {
        console.error('❌ Erreur dans startCamera:', error);
        return false;
    }
};

window.capturePhoto = async function(videoId, photoId) {
    try {
        const photoData = await capturePhotoOptimized();
        
        // Mettre à jour l'élément photo
        const photoElement = document.getElementById(photoId);
        if (photoElement) {
            photoElement.src = photoData;
        }
        
        return photoData;
    } catch (error) {
        console.error('❌ Erreur dans capturePhoto:', error);
        return null;
    }
};

window.stopCamera = async function() {
    return await stopCurrentCamera();
};

window.getAvailableCameras = function() {
    return availableDevices;
};

window.selectCamera = async function(deviceId) {
    return await startCameraWithDevice(deviceId);
};

// Initialisation automatique au chargement de la page
document.addEventListener('DOMContentLoaded', async () => {
    console.log('🚀 Initialisation automatique du système de caméra...');
    await initializeCameraSystem();
});

// Gestion des erreurs globales
window.addEventListener('error', (event) => {
    console.error('❌ Erreur globale:', event.error);
});

// Nettoyage lors de la fermeture de la page
window.addEventListener('beforeunload', () => {
    if (currentStream) {
        stopCurrentCamera();
    }
});

console.log('📹 Système de caméra amélioré chargé');
