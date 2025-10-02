// Camera Debug JavaScript - Diagnostic des problèmes de caméra
console.log('🔍 Module de débogage caméra chargé');

// Fonctions de débogage de l'environnement
window.getBrowserInfo = function() {
    const userAgent = navigator.userAgent;
    let browser = "Inconnu";
    
    if (userAgent.includes("Chrome")) browser = "Chrome";
    else if (userAgent.includes("Firefox")) browser = "Firefox";
    else if (userAgent.includes("Safari")) browser = "Safari";
    else if (userAgent.includes("Edge")) browser = "Edge";
    
    const version = userAgent.match(/(Chrome|Firefox|Safari|Edge)\/(\d+)/);
    const versionNumber = version ? version[2] : "Inconnue";
    
    return `${browser} ${versionNumber}`;
};

window.isHttps = function() {
    return window.location.protocol === 'https:';
};

window.isMediaDevicesAvailable = function() {
    return !!(navigator.mediaDevices && navigator.mediaDevices.getUserMedia);
};

window.isPermissionsAvailable = function() {
    return !!(navigator.permissions && navigator.permissions.query);
};

// Test de la caméra avec logs détaillés
window.testCameraDebug = function() {
    console.log('🔍 Test de débogage de la caméra démarré...');
    
    const video = document.getElementById('debugVideo');
    if (!video) {
        console.error('❌ Élément vidéo de débogage non trouvé');
        return false;
    }
    
    console.log('📹 Élément vidéo trouvé, test des permissions...');
    
    // Vérifier les permissions
    if (navigator.permissions) {
        navigator.permissions.query({ name: 'camera' })
            .then(permission => {
                console.log('📋 Permission caméra:', permission.state);
                addDebugLog(`📋 Permission caméra: ${permission.state}`);
                
                if (permission.state === 'denied') {
                    addDebugLog('❌ Permission caméra refusée');
                    return false;
                }
                
                // Continuer avec le test
                testCameraAccess();
            })
            .catch(err => {
                console.warn('⚠️ Impossible de vérifier les permissions:', err);
                addDebugLog('⚠️ Impossible de vérifier les permissions');
                testCameraAccess();
            });
    } else {
        console.log('📋 Pas de support des permissions, test direct...');
        addDebugLog('📋 Pas de support des permissions, test direct...');
        testCameraAccess();
    }
    
    return true;
};

// Test d'accès à la caméra
function testCameraAccess() {
    console.log('🎥 Test d\'accès à la caméra...');
    addDebugLog('🎥 Test d\'accès à la caméra...');
    
    const video = document.getElementById('debugVideo');
    
    // Essayer d'abord avec des contraintes élevées
    const constraints = {
        video: {
            width: { ideal: 640, min: 320 },
            height: { ideal: 480, min: 240 }
        }
    };
    
    console.log('📹 Contraintes vidéo:', constraints);
    addDebugLog('📹 Contraintes vidéo testées');
    
    navigator.mediaDevices.getUserMedia(constraints)
        .then(stream => {
            console.log('✅ Flux vidéo obtenu avec succès');
            addDebugLog('✅ Flux vidéo obtenu avec succès');
            
            // Afficher les informations de la caméra
            const videoTrack = stream.getVideoTracks()[0];
            if (videoTrack) {
                const settings = videoTrack.getSettings();
                console.log('📹 Paramètres de la caméra:', settings);
                addDebugLog(`📹 Caméra: ${videoTrack.label || 'Sans nom'}`);
                addDebugLog(`📹 Résolution: ${settings.width}x${settings.height}`);
            }
            
            // Assigner le flux à l'élément vidéo
            video.srcObject = stream;
            
            // Attendre que la vidéo soit prête
            video.onloadedmetadata = () => {
                console.log('🎬 Métadonnées vidéo chargées');
                addDebugLog('🎬 Métadonnées vidéo chargées');
                
                console.log('📹 Dimensions vidéo:', video.videoWidth, 'x', video.videoHeight);
                addDebugLog(`📹 Dimensions: ${video.videoWidth}x${video.videoHeight}`);
                
                // Démarrer la lecture
                video.play()
                    .then(() => {
                        console.log('▶️ Lecture vidéo démarrée avec succès');
                        addDebugLog('▶️ Lecture vidéo démarrée avec succès');
                        
                        // Vérifier que la vidéo est bien en cours de lecture
                        setTimeout(() => {
                            if (video.readyState >= 2) {
                                console.log('✅ Vidéo prête et en cours de lecture');
                                addDebugLog('✅ Vidéo prête et en cours de lecture');
                            } else {
                                console.warn('⚠️ Vidéo pas encore prête, readyState:', video.readyState);
                                addDebugLog(`⚠️ Vidéo pas prête, readyState: ${video.readyState}`);
                            }
                        }, 1000);
                    })
                    .catch(playErr => {
                        console.error('❌ Erreur lors du démarrage de la lecture:', playErr);
                        addDebugLog(`❌ Erreur lecture: ${playErr.message}`);
                    });
            };
            
            video.onerror = (err) => {
                console.error('❌ Erreur vidéo:', err);
                addDebugLog(`❌ Erreur vidéo: ${err.message}`);
            };
            
            video.oncanplay = () => {
                console.log('🎥 Vidéo prête à être lue');
                addDebugLog('🎥 Vidéo prête à être lue');
            };
            
            video.onplaying = () => {
                console.log('▶️ Vidéo en cours de lecture');
                addDebugLog('▶️ Vidéo en cours de lecture');
            };
        })
        .catch(err => {
            console.error('❌ Erreur lors de l\'activation de la caméra:', err);
            addDebugLog(`❌ Erreur caméra: ${err.name} - ${err.message}`);
            
            // Essayer avec des contraintes réduites
            console.log('🔄 Tentative avec contraintes réduites...');
            addDebugLog('🔄 Tentative avec contraintes réduites...');
            
            navigator.mediaDevices.getUserMedia({ video: true })
                .then(stream => {
                    console.log('✅ Caméra activée avec contraintes réduites');
                    addDebugLog('✅ Caméra activée avec contraintes réduites');
                    
                    video.srcObject = stream;
                    
                    video.onloadedmetadata = () => {
                        video.play()
                            .then(() => {
                                console.log('▶️ Lecture vidéo démarrée (contraintes réduites)');
                                addDebugLog('▶️ Lecture vidéo démarrée (contraintes réduites)');
                            })
                            .catch(playErr => {
                                console.error('❌ Erreur lecture (contraintes réduites):', playErr);
                                addDebugLog(`❌ Erreur lecture (contraintes réduites): ${playErr.message}`);
                            });
                    };
                })
                .catch(fallbackErr => {
                    console.error('❌ Échec même avec contraintes réduites:', fallbackErr);
                    addDebugLog(`❌ Échec total: ${fallbackErr.name} - ${fallbackErr.message}`);
                    
                    // Afficher un message d'erreur détaillé
                    const errorMessage = getDetailedErrorMessage(fallbackErr);
                    addDebugLog(`💡 Solution: ${errorMessage}`);
                });
        });
}

// Lister les caméras disponibles
window.listCamerasDebug = function() {
    console.log('📋 Récupération de la liste des caméras...');
    addDebugLog('📋 Récupération de la liste des caméras...');
    
    if (!navigator.mediaDevices || !navigator.mediaDevices.enumerateDevices) {
        addDebugLog('❌ API enumerateDevices non disponible');
        return null;
    }
    
    navigator.mediaDevices.enumerateDevices()
        .then(devices => {
            const videoDevices = devices.filter(device => device.kind === 'videoinput');
            
            console.log('📹 Caméras trouvées:', videoDevices.length);
            addDebugLog(`📹 Caméras trouvées: ${videoDevices.length}`);
            
            if (videoDevices.length === 0) {
                addDebugLog('❌ Aucune caméra détectée');
                return null;
            }
            
            videoDevices.forEach((device, index) => {
                const label = device.label || `Caméra ${index + 1} (sans nom)`;
                const isUSB = label.toLowerCase().includes('usb') || 
                             label.toLowerCase().includes('camera') ||
                             label.toLowerCase().includes('webcam');
                const icon = isUSB ? '🔌' : '💻';
                
                console.log(`${icon} ${index + 1}. ${label}`);
                addDebugLog(`${icon} ${index + 1}. ${label}`);
            });
            
            return videoDevices.length.toString();
        })
        .catch(err => {
            console.error('❌ Erreur lors de l\'énumération des caméras:', err);
            addDebugLog(`❌ Erreur énumération: ${err.message}`);
            return null;
        });
};

// Obtenir des informations système
window.getSystemInfo = function() {
    const info = {
        userAgent: navigator.userAgent,
        platform: navigator.platform,
        language: navigator.language,
        cookieEnabled: navigator.cookieEnabled,
        onLine: navigator.onLine,
        doNotTrack: navigator.doNotTrack,
        hardwareConcurrency: navigator.hardwareConcurrency || 'Non disponible',
        maxTouchPoints: navigator.maxTouchPoints || 'Non disponible',
        vendor: navigator.vendor,
        product: navigator.product
    };
    
    let infoText = '';
    for (const [key, value] of Object.entries(info)) {
        infoText += `<strong>${key}:</strong> ${value}<br>`;
    }
    
    return infoText;
};

// Fonctions d'interface
window.updateDebugInfo = function(elementId, value) {
    const element = document.getElementById(elementId);
    if (element) {
        element.textContent = value;
    }
};

window.updateSystemInfo = function(html) {
    const element = document.getElementById('systemInfo');
    if (element) {
        element.innerHTML = html;
    }
};

window.addDebugLog = function(message) {
    const logsContainer = document.getElementById('debugLogs');
    if (logsContainer) {
        const logEntry = document.createElement('p');
        logEntry.className = 'log-entry';
        logEntry.textContent = `[${new Date().toLocaleTimeString()}] ${message}`;
        logsContainer.appendChild(logEntry);
        
        // Auto-scroll vers le bas
        logsContainer.scrollTop = logsContainer.scrollHeight;
    }
};

window.clearDebugLogs = function() {
    const logsContainer = document.getElementById('debugLogs');
    if (logsContainer) {
        logsContainer.innerHTML = '';
    }
};

// Fonction pour obtenir un message d'erreur détaillé
function getDetailedErrorMessage(error) {
    switch (error.name) {
        case 'NotAllowedError':
            return 'Accès refusé. Vérifiez les permissions de votre navigateur et autorisez l\'accès à la caméra.';
        case 'NotFoundError':
            return 'Aucune caméra trouvée. Vérifiez que votre caméra est bien connectée et reconnue par le système.';
        case 'NotReadableError':
            return 'Caméra déjà utilisée par une autre application. Fermez les autres applications qui utilisent la caméra.';
        case 'OverconstrainedError':
            return 'Contraintes vidéo non supportées par votre caméra. Essayez de réduire la résolution.';
        case 'SecurityError':
            return 'Erreur de sécurité. Assurez-vous que vous êtes sur HTTPS ou localhost.';
        case 'AbortError':
            return 'Accès à la caméra interrompu. Vérifiez que votre caméra fonctionne correctement.';
        case 'NotSupportedError':
            return 'Format vidéo non supporté. Vérifiez que votre navigateur supporte le format vidéo de votre caméra.';
        default:
            return `Erreur inconnue: ${error.message}. Vérifiez la console pour plus de détails.`;
    }
}

console.log('🔍 Module de débogage caméra prêt');
