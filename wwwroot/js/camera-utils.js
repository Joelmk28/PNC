// Utilitaires pour la gestion des caméras - Version 3.2 debug multi-caméras
window.cameraUtils = {
    // Obtenir le nombre de caméras disponibles
    getCameraCount: async function() {
        try {
            const devices = await navigator.mediaDevices.enumerateDevices();
            const videoDevices = devices.filter(device => device.kind === 'videoinput');
            return videoDevices.length;
        } catch (error) {
            console.error('Erreur lors de la détection des caméras:', error);
            return 0;
        }
    },

    // Obtenir la liste des caméras
    getCameraList: async function() {
        try {
            const devices = await navigator.mediaDevices.enumerateDevices();
            const videoDevices = devices.filter(device => device.kind === 'videoinput');
            return videoDevices.map(device => device.label || `Caméra ${device.deviceId.slice(0, 8)}`);
        } catch (error) {
            console.error('Erreur lors de la récupération de la liste des caméras:', error);
            return [];
        }
    },

    // Afficher une boîte de dialogue de sélection de caméra
    showCameraSelectionDialog: function(message, cameras) {
        return new Promise((resolve) => {
            // Créer une boîte de dialogue personnalisée
            const dialog = document.createElement('div');
            dialog.className = 'camera-selection-dialog';
            dialog.innerHTML = `
                <div class="camera-selection-overlay">
                    <div class="camera-selection-content">
                        <h3>Sélection de Caméra</h3>
                        <p>${message}</p>
                        <div class="camera-list">
                            ${cameras.map((camera, index) => `
                                <button class="camera-option" data-index="${index}">
                                    <i class="bi bi-camera-video"></i>
                                    ${camera}
                                </button>
                            `).join('')}
                        </div>
                        <div class="camera-selection-actions">
                            <button class="btn-modern btn-success" id="validateSelection" disabled style="font-weight: bold;">
                                ✅ Valider la sélection
                            </button>
                            <button class="btn-modern btn-primary" id="useFirstCamera">
                                Utiliser la première caméra
                            </button>
                            <button class="btn-modern btn-secondary" id="cancelSelection">
                                Annuler
                            </button>
                        </div>
                    </div>
                </div>
            `;

            // Ajouter les styles
            const style = document.createElement('style');
            style.textContent = `
                .camera-selection-dialog {
                    position: fixed;
                    top: 0;
                    left: 0;
                    width: 100%;
                    height: 100%;
                    z-index: 10000;
                }
                .camera-selection-overlay {
                    position: absolute;
                    top: 0;
                    left: 0;
                    width: 100%;
                    height: 100%;
                    background: rgba(0, 0, 0, 0.8);
                    display: flex;
                    align-items: center;
                    justify-content: center;
                }
                .camera-selection-content {
                    background: white;
                    border-radius: 15px;
                    padding: 30px;
                    max-width: 500px;
                    width: 90%;
                    max-height: 80vh;
                    overflow-y: auto;
                }
                .camera-selection-content h3 {
                    margin: 0 0 20px 0;
                    color: #2c3e50;
                    text-align: center;
                }
                .camera-selection-content p {
                    margin: 0 0 20px 0;
                    color: #6c757d;
                    line-height: 1.5;
                }
                .camera-list {
                    margin: 20px 0;
                    display: flex;
                    flex-direction: column;
                    gap: 10px;
                }
                .camera-option {
                    display: flex;
                    align-items: center;
                    gap: 10px;
                    padding: 15px;
                    border: 2px solid #e9ecef;
                    border-radius: 10px;
                    background: white;
                    cursor: pointer;
                    transition: all 0.3s ease;
                    text-align: left;
                    font-size: 14px;
                }
                .camera-option:hover {
                    border-color: #007bff;
                    background: #f8f9fa;
                }
                .camera-option.selected {
                    border-color: #007bff;
                    background: #e3f2fd;
                }
                .camera-selection-actions {
                    display: flex;
                    gap: 10px;
                    justify-content: center;
                    margin-top: 20px;
                }
                .btn-modern {
                    padding: 12px 20px;
                    border: none;
                    border-radius: 10px;
                    font-size: 14px;
                    font-weight: 500;
                    cursor: pointer;
                    transition: all 0.3s ease;
                }
                .btn-primary {
                    background: #007bff;
                    color: white;
                }
                .btn-primary:hover {
                    background: #0056b3;
                }
                .btn-secondary {
                    background: #6c757d;
                    color: white;
                }
                .btn-secondary:hover {
                    background: #5a6268;
                }
                .btn-success {
                    background: #28a745;
                    color: white;
                }
                .btn-success:hover:not(:disabled) {
                    background: #218838;
                }
                .btn-success:disabled {
                    background: #6c757d;
                    cursor: not-allowed;
                    opacity: 0.6;
                }
            `;

            document.head.appendChild(style);
            document.body.appendChild(dialog);

            // Gérer les événements
            let selectedIndex = -1;
            const validateBtn = dialog.querySelector('#validateSelection');

            // Sélection d'une caméra spécifique
            dialog.querySelectorAll('.camera-option').forEach(option => {
                option.addEventListener('click', () => {
                    dialog.querySelectorAll('.camera-option').forEach(opt => opt.classList.remove('selected'));
                    option.classList.add('selected');
                    selectedIndex = parseInt(option.dataset.index);
                    
                    // Activer le bouton OK quand une caméra est sélectionnée
                    validateBtn.disabled = false;
                });
            });

            // Valider la sélection
            validateBtn.addEventListener('click', () => {
                if (selectedIndex >= 0) {
                    resolve(selectedIndex);
                    cleanup();
                }
            });

            // Utiliser la première caméra
            dialog.querySelector('#useFirstCamera').addEventListener('click', () => {
                resolve(0);
                cleanup();
            });

            // Annuler
            dialog.querySelector('#cancelSelection').addEventListener('click', () => {
                resolve(-1);
                cleanup();
            });

            // Cliquer sur l'overlay pour fermer
            dialog.querySelector('.camera-selection-overlay').addEventListener('click', (e) => {
                if (e.target === e.currentTarget) {
                    resolve(-1);
                    cleanup();
                }
            });

            function cleanup() {
                document.body.removeChild(dialog);
                document.head.removeChild(style);
            }
        });
    },

    // Arrêter tous les flux de caméra actifs
    stopAllCameraStreams: async function() {
        console.log('🛑 Arrêt de tous les flux de caméra...');
        
        // 1. Arrêter le flux dans l'élément vidéo principal
        const video = document.getElementById('video');
        if (video && video.srcObject) {
            console.log('🛑 Arrêt du flux vidéo principal');
            const tracks = video.srcObject.getTracks();
            tracks.forEach(track => {
                track.stop();
                console.log('🛑 Track arrêtée:', track.kind, track.label);
            });
            video.srcObject = null;
        }
        
        // 2. Arrêter le flux global currentStream s'il existe
        if (typeof window.cleanupCamera === 'function') {
            console.log('🛑 Appel de cleanupCamera global');
            window.cleanupCamera();
        }
        
        // 3. Forcer l'arrêt de tous les flux MediaStream actifs
        try {
            const devices = await navigator.mediaDevices.enumerateDevices();
            console.log('🛑 Énumération des appareils pour nettoyage complet');
        } catch (error) {
            console.log('⚠️ Impossible d\'énumérer les appareils:', error);
        }
        
        console.log('✅ Nettoyage complet des flux terminé');
    },

    // Initialiser la caméra avec un index spécifique
    initSimpleCameraWithIndex: async function(cameraIndex) {
        try {
            console.log('🎥 Initialisation de la caméra avec index:', cameraIndex);
            
            const devices = await navigator.mediaDevices.enumerateDevices();
            const videoDevices = devices.filter(device => device.kind === 'videoinput');
            
            console.log('📹 Caméras disponibles:', videoDevices);
            
            if (cameraIndex >= 0 && cameraIndex < videoDevices.length) {
                const selectedDevice = videoDevices[cameraIndex];
                console.log('📷 Caméra sélectionnée:', selectedDevice);
                
                // Arrêter TOUS les flux existants (fonction globale)
                await this.stopAllCameraStreams();
                
                // Petite pause pour s'assurer que les flux sont bien arrêtés
                await new Promise(resolve => setTimeout(resolve, 100));
                
                // Contraintes très flexibles pour les caméras USB
                const constraints = {
                    video: {
                        deviceId: selectedDevice.deviceId ? { ideal: selectedDevice.deviceId } : undefined,
                        width: { ideal: 640, min: 320 },
                        height: { ideal: 480, min: 240 },
                        frameRate: { ideal: 15, max: 30 }
                    }
                };
                
                console.log('🔧 Contraintes utilisées:', constraints);
                
                const stream = await navigator.mediaDevices.getUserMedia(constraints);
                console.log('✅ Flux obtenu:', stream);
                
                if (video) {
                    video.srcObject = stream;
                    
                    // Configuration des événements vidéo
                    video.onloadedmetadata = () => {
                        console.log('📐 Dimensions vidéo:', {
                            width: video.videoWidth,
                            height: video.videoHeight
                        });
                    };
                    video.onerror = (error) => {
                        console.error('❌ Erreur vidéo:', error);
                    };
                    
                    // Retourner immédiatement true si le flux est assigné
                    console.log('✅ Flux assigné à l\'élément vidéo');
                    return true;
                }
            }
            return false;
        } catch (error) {
            console.error('❌ Erreur lors de l\'initialisation de la caméra:', error);
            
            // Essayer avec deviceId en mode ideal plutôt qu'exact
            try {
                console.log('🔄 Tentative avec deviceId en mode ideal...');
                
                await this.stopAllCameraStreams();
                await new Promise(resolve => setTimeout(resolve, 200));
                
                const devices = await navigator.mediaDevices.enumerateDevices();
                const videoDevices = devices.filter(device => device.kind === 'videoinput');
                const selectedDevice = videoDevices[cameraIndex];
                
                const idealConstraints = {
                    video: {
                        deviceId: selectedDevice.deviceId ? { ideal: selectedDevice.deviceId } : undefined
                        // Pas de contraintes de taille ou framerate
                    }
                };
                
                console.log('🔧 Contraintes ideales:', idealConstraints);
                const stream = await navigator.mediaDevices.getUserMedia(idealConstraints);
                
                const video = document.getElementById('video');
                if (video) {
                    video.srcObject = stream;
                    video.onloadedmetadata = () => console.log('📐 Caméra ideale prête');
                    video.onerror = (error) => console.error('❌ Erreur caméra ideale:', error);
                    console.log('✅ Flux ideal assigné');
                    return true;
                }
            } catch (idealError) {
                console.error('❌ Échec avec contraintes ideales:', idealError);
                
                // Dernière tentative : contraintes minimales
                try {
                    console.log('🔄 Dernière tentative avec contraintes minimales...');
                    
                    await this.stopAllCameraStreams();
                    await new Promise(resolve => setTimeout(resolve, 300));
                    
                    const video = document.getElementById('video');
                    const stream = await navigator.mediaDevices.getUserMedia({
                        video: true
                    });
                    
                    if (video) {
                        video.srcObject = stream;
                        return true;
                    }
                } catch (fallbackError) {
                    console.error('❌ Échec complet:', fallbackError);
                }
            }
            
            return false;
        }
    }
};

// Exposer les fonctions globalement pour Blazor
window.getCameraCount = async function() {
    console.log('🔍 getCameraCount appelée');
    try {
        const result = await window.cameraUtils.getCameraCount();
        console.log('📹 Nombre de caméras détectées:', result);
        return result;
    } catch (error) {
        console.error('❌ Erreur dans getCameraCount:', error);
        return 0;
    }
};

window.getCameraList = async function() {
    console.log('🔍 getCameraList appelée');
    try {
        const result = await window.cameraUtils.getCameraList();
        console.log('📹 Liste des caméras:', result);
        return result;
    } catch (error) {
        console.error('❌ Erreur dans getCameraList:', error);
        return [];
    }
};

window.showCameraSelectionDialog = function(message, cameras) {
    console.log('🔍 showCameraSelectionDialog appelée');
    try {
        return window.cameraUtils.showCameraSelectionDialog(message, cameras);
    } catch (error) {
        console.error('❌ Erreur dans showCameraSelectionDialog:', error);
        return Promise.resolve(-1);
    }
};

window.initSimpleCameraWithIndex = async function(index) {
    console.log('🔍 initSimpleCameraWithIndex appelée avec index:', index);
    try {
        const result = await window.cameraUtils.initSimpleCameraWithIndex(index);
        console.log('📹 Résultat initSimpleCameraWithIndex:', result);
        return result;
    } catch (error) {
        console.error('❌ Erreur dans initSimpleCameraWithIndex:', error);
        return false;
    }
};

window.stopAllCameraStreams = async function() {
    console.log('🔍 stopAllCameraStreams appelée');
    try {
        await window.cameraUtils.stopAllCameraStreams();
        return true;
    } catch (error) {
        console.error('❌ Erreur dans stopAllCameraStreams:', error);
        return false;
    }
};

// Vérification que tout est bien exposé
console.log('✅ camera-utils.js chargé avec succès');
console.log('📋 Fonctions disponibles:', {
    getCameraCount: typeof window.getCameraCount,
    getCameraList: typeof window.getCameraList,
    showCameraSelectionDialog: typeof window.showCameraSelectionDialog,
    initSimpleCameraWithIndex: typeof window.initSimpleCameraWithIndex,
    stopAllCameraStreams: typeof window.stopAllCameraStreams
});
