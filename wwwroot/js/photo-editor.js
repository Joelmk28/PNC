// Photo Editor - Module JavaScript pour Blazor
console.log('📸 Photo Editor Module chargé - DEBUT DU SCRIPT');

// Export comme module ES6 pour Blazor
export function initPhotoEditor() {
    console.log('🔧 Initialisation du Photo Editor...');
    return new Promise((resolve) => {
        const checkElements = () => {
            const videoElement = document.getElementById('video');
            const canvasElement = document.getElementById('canvas');
            
            if (!videoElement || !canvasElement) {
                console.log('⏳ Éléments DOM pas encore prêts, attente...');
                setTimeout(checkElements, 100);
                return;
            }
            
            console.log('✅ Photo Editor initialisé');
            resolve();
        };
        checkElements();
    });
}

// Variables globales
let videoElement;
let canvasElement;
let cropOverlay;
let cropSelection;
let context;
let currentImageData = null;
let isCropping = false;
let cropRect = { x: 0, y: 0, width: 0, height: 0 };

// Variables pour le drag & drop du crop
let isDragging = false;
let isResizing = false;
let startX, startY;
let resizeHandle = '';



// Capturer la photo depuis la vidéo
export function capturePhotoForEditing() {
    console.log('🚀 DÉMARRAGE capturePhotoForEditing() - Fonction appelée !');
    console.log('📸 Capture de la photo pour édition...');
    
    // Vérifier que les éléments sont disponibles
    if (!videoElement) {
        videoElement = document.getElementById('video');
    }
    if (!canvasElement) {
        canvasElement = document.getElementById('canvas');
        if (canvasElement) {
            context = canvasElement.getContext('2d');
        }
    }
    
    if (!videoElement || !canvasElement || !context) {
        console.error('❌ Éléments requis non trouvés');
        console.log('Debug - videoElement:', !!videoElement);
        console.log('Debug - canvasElement:', !!canvasElement);
        console.log('Debug - context:', !!context);
        updateStatus('Erreur: Éléments DOM manquants', 'error');
        return;
    }
    
    if (videoElement.readyState < 2) {
        console.error('❌ Vidéo non prête, readyState:', videoElement.readyState);
        updateStatus('Caméra non prête, veuillez patienter...', 'error');
        return;
    }
    
    try {
        // Obtenir les dimensions de la vidéo
        const width = videoElement.videoWidth || videoElement.clientWidth;
        const height = videoElement.videoHeight || videoElement.clientHeight;
        
        console.log('📹 DIMENSIONS DE LA VIDÉO SOURCE:');
        console.log('   - videoWidth:', videoElement.videoWidth);
        console.log('   - videoHeight:', videoElement.videoHeight);
        console.log('   - clientWidth:', videoElement.clientWidth);
        console.log('   - clientHeight:', videoElement.clientHeight);
        console.log('   - Utilisées pour capture:', width + 'x' + height);
        
        if (width === 0 || height === 0) {
            console.error('❌ Dimensions vidéo invalides');
            updateStatus('Erreur: Dimensions vidéo invalides', 'error');
            return;
        }
        
        // Configurer le canvas
        canvasElement.width = width;
        canvasElement.height = height;
        
        // Dessiner l'image de la vidéo sur le canvas
        context.drawImage(videoElement, 0, 0, width, height);
        
        // Vérifier que l'image est bien dessinée
        const imageData = context.getImageData(0, 0, Math.min(10, width), Math.min(10, height));
        const hasData = imageData.data.some(pixel => pixel > 0);
        console.log('🖼️ Image dessinée sur canvas:', hasData ? 'OUI' : 'NON');
        console.log('🖼️ Premières données pixels:', imageData.data.slice(0, 12));
        
        // Sauvegarder l'image
        currentImageData = context.getImageData(0, 0, width, height);
        
        // Changer l'interface
        showCapturedPhoto();
        
        console.log('✅ Photo capturée:', width + 'x' + height);
        console.log('📏 TAILLE DE LA PHOTO CAPTURÉE:');
        console.log('   - Largeur:', width, 'pixels');
        console.log('   - Hauteur:', height, 'pixels');
        console.log('   - Ratio:', (width/height).toFixed(2));
        console.log('   - Taille totale:', (width * height).toLocaleString(), 'pixels');
        console.log('🎨 Canvas affiché:', canvasElement.style.display);
        console.log('🎨 Canvas dimensions:', canvasElement.width + 'x' + canvasElement.height);
        updateStatus('Photo capturée - Vous pouvez la rogner ou l\'enregistrer', 'success');
        
    } catch (error) {
        console.error('❌ Erreur lors de la capture:', error);
        console.log('Error details:', error);
        updateStatus('Erreur lors de la capture: ' + error.message, 'error');
    }
}

// Afficher la photo capturée
function showCapturedPhoto() {
    console.log('🔄 Changement d\'affichage: vidéo → canvas');
    
    // Masquer la vidéo
    const videoContainer = document.querySelector('.video-container');
    if (videoContainer) {
        videoContainer.style.display = 'none';
        console.log('📹 Vidéo masquée');
    } else {
        console.log('❌ Container vidéo non trouvé');
    }
    
    // Afficher le canvas
    const canvasContainer = document.querySelector('.canvas-container');
    if (canvasContainer) {
        canvasContainer.style.display = 'block';
        console.log('🎨 Canvas affiché');
    } else {
        console.log('❌ Container canvas non trouvé');
    }
    
    // Masquer le bouton capture
    const captureBtn = document.getElementById('captureBtn');
    if (captureBtn) {
        captureBtn.style.display = 'none';
        console.log('🔘 Bouton capture masqué');
    }
    
    // Afficher les contrôles d'édition
    const editingControls = document.getElementById('editingControls');
    if (editingControls) {
        editingControls.style.display = 'block';
        console.log('🎛️ Contrôles d\'édition affichés');
    } else {
        console.log('❌ Contrôles d\'édition non trouvés');
    }
}

// Reprendre une photo
export function retakePhoto() {
    console.log('🔄 Reprise de photo...');
    
    // Réinitialiser les données
    currentImageData = null;
    isCropping = false;
    
    // Masquer le canvas
    document.querySelector('.canvas-container').style.display = 'none';
    
    // Afficher la vidéo
    document.querySelector('.video-container').style.display = 'block';
    
    // Masquer les contrôles d'édition
    document.getElementById('editingControls').style.display = 'none';
    document.getElementById('croppingControls').style.display = 'none';
    
    // Afficher le bouton capture
    document.getElementById('captureBtn').style.display = 'block';
    
    // Masquer l'overlay de crop
    if (cropOverlay) {
        cropOverlay.style.display = 'none';
    }
    
    updateStatus('Prêt à capturer une nouvelle photo', 'info');
}

// Commencer le rognage
export function startCropping() {
    console.log('✂️ Début du rognage...');
    
    if (!currentImageData) {
        console.error('❌ Aucune image à rogner');
        return;
    }
    
    // Vérifier et initialiser les éléments nécessaires
    if (!cropOverlay) {
        cropOverlay = document.getElementById('crop-overlay');
        console.log('🔍 Recherche crop-overlay:', !!cropOverlay);
    }
    if (!cropSelection) {
        cropSelection = document.querySelector('.crop-selection');
        console.log('🔍 Recherche crop-selection:', !!cropSelection);
    }
    if (!canvasElement) {
        canvasElement = document.getElementById('canvas');
        console.log('🔍 Recherche canvas:', !!canvasElement);
    }
    
    console.log('🔍 État des éléments:');
    console.log('- cropOverlay:', !!cropOverlay);
    console.log('- cropSelection:', !!cropSelection);
    console.log('- canvasElement:', !!canvasElement);
    console.log('- currentImageData:', !!currentImageData);
    
    if (!cropOverlay) {
        console.error('❌ Élément crop-overlay non trouvé');
        updateStatus('Erreur: Overlay de rognage non disponible', 'error');
        return;
    }
    
    if (!canvasElement) {
        console.error('❌ Élément canvas non trouvé');
        updateStatus('Erreur: Canvas non disponible', 'error');
        return;
    }
    
    isCropping = true;
    
    // Afficher l'overlay de crop
    cropOverlay.style.display = 'block';
    
    // L'overlay est dans le même conteneur que le canvas, donc position relative
    // Pas besoin de calculer la position absolue, juste couvrir le canvas parent
    cropOverlay.style.position = 'absolute';
    cropOverlay.style.top = '0';
    cropOverlay.style.left = '0';
    cropOverlay.style.width = '100%';
    cropOverlay.style.height = '100%';
    cropOverlay.style.zIndex = '10';
    
    console.log('📐 Overlay positionné sur le canvas');
    
    // Obtenir les dimensions du canvas pour la zone de crop
    const canvasRect = canvasElement.getBoundingClientRect();
    const overlayRect = cropOverlay.getBoundingClientRect();
    
    // Initialiser la zone de crop (60% au centre)
    const boxWidth = overlayRect.width * 0.6;
    const boxHeight = overlayRect.height * 0.6;
    cropRect = {
        x: (overlayRect.width - boxWidth) / 2,
        y: (overlayRect.height - boxHeight) / 2,
        width: boxWidth,
        height: boxHeight
    };
    
    console.log('📏 Zone de crop initialisée:', cropRect);
    
    updateCropSelection();
    setupCropHandlers();
    
    // Changer les contrôles
    document.getElementById('editingControls').style.display = 'none';
    document.getElementById('croppingControls').style.display = 'block';
    
    updateStatus('Sélectionnez la zone à conserver', 'info');
}

// Mettre à jour la sélection de crop
function updateCropSelection() {
    if (!cropSelection) return;
    
    cropSelection.style.left = cropRect.x + 'px';
    cropSelection.style.top = cropRect.y + 'px';
    cropSelection.style.width = cropRect.width + 'px';
    cropSelection.style.height = cropRect.height + 'px';
}

// Configurer les gestionnaires de crop
function setupCropHandlers() {
    if (!cropSelection) return;
    
    cropSelection.addEventListener('mousedown', onCropMouseDown);
    document.addEventListener('mousemove', onCropMouseMove);
    document.addEventListener('mouseup', onCropMouseUp);
}

// Gestionnaires d'événements pour le crop
function onCropMouseDown(e) {
    e.preventDefault();
    isDragging = true;
    startX = e.clientX;
    startY = e.clientY;
}

function onCropMouseMove(e) {
    if (!isDragging) return;
    
    const deltaX = e.clientX - startX;
    const deltaY = e.clientY - startY;
    
    // Déplacer la zone de crop
    cropRect.x = Math.max(0, Math.min(cropRect.x + deltaX, cropOverlay.offsetWidth - cropRect.width));
    cropRect.y = Math.max(0, Math.min(cropRect.y + deltaY, cropOverlay.offsetHeight - cropRect.height));
    
    updateCropSelection();
    
    startX = e.clientX;
    startY = e.clientY;
}

function onCropMouseUp(e) {
    isDragging = false;
}

// Appliquer le crop
export function applyCrop() {
    console.log('✂️ Application du rognage...');
    
    if (!currentImageData || !isCropping) {
        console.error('❌ Pas en mode rognage');
        return;
    }
    
    try {
        // Calculer les proportions
        const scaleX = canvasElement.width / canvasElement.offsetWidth;
        const scaleY = canvasElement.height / canvasElement.offsetHeight;
        
        // Coordonnées réelles sur le canvas
        const realX = cropRect.x * scaleX;
        const realY = cropRect.y * scaleY;
        const realWidth = cropRect.width * scaleX;
        const realHeight = cropRect.height * scaleY;
        
        // Créer un nouveau canvas temporaire pour le crop
        const tempCanvas = document.createElement('canvas');
        const tempContext = tempCanvas.getContext('2d');
        
        tempCanvas.width = realWidth;
        tempCanvas.height = realHeight;
        
        // Copier la zone rognée
        tempContext.drawImage(
            canvasElement,
            realX, realY, realWidth, realHeight,
            0, 0, realWidth, realHeight
        );
        
        // Redimensionner le canvas principal
        canvasElement.width = realWidth;
        canvasElement.height = realHeight;
        
        // Dessiner l'image rognée
        context.drawImage(tempCanvas, 0, 0);
        
        // Mettre à jour les données d'image
        currentImageData = context.getImageData(0, 0, realWidth, realHeight);
        
        // Sortir du mode crop
        cancelCrop();
        
        console.log('✅ Rognage appliqué');
        updateStatus('Image rognée avec succès', 'success');
        
    } catch (error) {
        console.error('❌ Erreur lors du rognage:', error);
        updateStatus('Erreur lors du rognage', 'error');
    }
}

// Annuler le crop
export function cancelCrop() {
    console.log('❌ Annulation du rognage...');
    
    isCropping = false;
    
    // Masquer l'overlay
    cropOverlay.style.display = 'none';
    
    // Revenir aux contrôles d'édition
    document.getElementById('croppingControls').style.display = 'none';
    document.getElementById('editingControls').style.display = 'block';
    
    updateStatus('Rognage annulé', 'info');
}

// Sauvegarder la photo éditée
export function saveEditedPhoto() {
    console.log('💾 Sauvegarde de la photo éditée...');
    
    if (!currentImageData || !canvasElement) {
        console.error('❌ Aucune image à sauvegarder');
        updateStatus('Aucune image à sauvegarder', 'error');
        return;
    }
    
    try {
        // Convertir en base64
        const photoData = canvasElement.toDataURL('image/jpeg', 0.9);
        
        if (!window.currentPolicierId) {
            console.error('❌ ID Policier non défini');
            updateStatus('Erreur: ID Policier manquant', 'error');
            return;
        }
        
        updateStatus('Sauvegarde en cours...', 'loading');
        
        // Appeler la méthode Blazor
        DotNet.invokeMethodAsync('PNC', 'SaveCapturedPhoto', window.currentPolicierId, photoData)
            .then(() => {
                console.log('✅ Photo sauvegardée avec succès');
                updateStatus('Photo sauvegardée!', 'success');
                
                // Fermer le modal après un délai
                setTimeout(() => {
                    if (window.closeCameraModal) {
                        window.closeCameraModal();
                    }
                }, 1500);
            })
            .catch(error => {
                console.error('❌ Erreur Blazor:', error);
                updateStatus('Erreur lors de la sauvegarde', 'error');
            });
            
    } catch (error) {
        console.error('❌ Erreur lors de la sauvegarde:', error);
        updateStatus('Erreur lors de la sauvegarde', 'error');
    }
}

// Mettre à jour le statut
function updateStatus(message, type = 'info') {
    const statusElement = document.getElementById('camera-status');
    if (!statusElement) return;
    
    let icon = 'bi-info-circle';
    let className = 'text-info';
    
    switch (type) {
        case 'success':
            icon = 'bi-check-circle';
            className = 'text-success';
            break;
        case 'error':
            icon = 'bi-exclamation-triangle';
            className = 'text-danger';
            break;
        case 'loading':
            icon = 'bi-hourglass-split';
            className = 'text-primary';
            break;
    }
    
    statusElement.innerHTML = `<p class="${className}"><i class="bi ${icon}"></i> ${message}</p>`;
}

// Module ES6 pour Blazor - pas d'auto-initialisation
console.log('📸 Module Photo Editor prêt pour import Blazor');
