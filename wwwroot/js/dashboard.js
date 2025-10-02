// Modern Dashboard Charts and Interactions

// Initialize Modern Grade Distribution Chart
window.initializeModernGradeChart = function(labels, data) {
    const ctx = document.getElementById('gradeChart');
    if (!ctx) return;

    // Destroy existing chart if it exists
    if (window.gradeChartInstance) {
        window.gradeChartInstance.destroy();
    }

    // Modern color palette matching navmenu
    const colors = [
        '#1e1b4b', '#10b981', '#fbbf24', '#3b82f6', '#ef4444', '#8b5cf6'
    ];

    window.gradeChartInstance = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: colors,
                borderWidth: 0,
                hoverBorderWidth: 0,
                hoverOffset: 8
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'right',
                    labels: {
                        padding: 16,
                        usePointStyle: true,
                        pointStyle: 'circle',
                        font: {
                            size: 12,
                            family: "'Inter', sans-serif",
                            weight: '500'
                        },
                        color: '#374151'
                    }
                },
                tooltip: {
                    backgroundColor: 'rgba(17, 24, 39, 0.95)',
                    titleColor: '#ffffff',
                    bodyColor: '#ffffff',
                    borderColor: 'rgba(75, 85, 99, 0.2)',
                    borderWidth: 1,
                    cornerRadius: 8,
                    padding: 12,
                    displayColors: true,
                    titleFont: {
                        size: 13,
                        weight: '600'
                    },
                    bodyFont: {
                        size: 12
                    },
                    callbacks: {
                        label: function(context) {
                            const total = context.dataset.data.reduce((a, b) => a + b, 0);
                            const percentage = ((context.parsed / total) * 100).toFixed(1);
                            return `${context.parsed} policiers (${percentage}%)`;
                        }
                    }
                }
            },
            cutout: '65%',
            animation: {
                animateRotate: true,
                animateScale: false,
                duration: 800,
                easing: 'easeOutCubic'
            },
            hover: {
                animationDuration: 200
            },
            elements: {
                arc: {
                    borderRadius: 4
                }
            }
        }
    });
};

// Initialize Mini Charts
window.initializeMiniCharts = function(totalPoliciers, nouveauxCeMois) {
    // Chart for policiers evolution
    initializePoliciersMiniChart(totalPoliciers, nouveauxCeMois);
    
    // Chart for commissariats
    initializeCommissariatsMiniChart();
};

function initializePoliciersMiniChart(total, nouveaux) {
    const ctx = document.getElementById('policiersChart');
    if (!ctx) return;

    // Generate sample data for the last 12 months
    const months = [];
    const data = [];
    const currentMonth = new Date().getMonth();
    
    for (let i = 11; i >= 0; i--) {
        const date = new Date();
        date.setMonth(currentMonth - i);
        months.push(date.toLocaleDateString('fr-FR', { month: 'short' }));
        
        // Simulate growth data
        const baseValue = Math.max(0, total - nouveaux - (i * 5));
        const variation = Math.random() * 20 - 10;
        data.push(Math.max(0, baseValue + variation));
    }

    new Chart(ctx, {
        type: 'line',
        data: {
            labels: months,
            datasets: [{
                data: data,
                borderColor: '#1e1b4b',
                backgroundColor: 'rgba(30, 27, 75, 0.1)',
                borderWidth: 2,
                fill: true,
                tension: 0.4,
                pointRadius: 0,
                pointHoverRadius: 4,
                pointBackgroundColor: '#1e1b4b',
                pointBorderColor: '#ffffff',
                pointBorderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            interaction: {
                intersect: false,
                mode: 'index'
            },
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    enabled: false
                }
            },
            scales: {
                x: {
                    display: false
                },
                y: {
                    display: false
                }
            },
            elements: {
                point: {
                    hoverRadius: 0
                }
            },
            animation: {
                duration: 1000,
                easing: 'easeOutCubic'
            }
        }
    });
}

function initializeCommissariatsMiniChart() {
    const ctx = document.getElementById('commissariatsChart');
    if (!ctx) return;

    // Generate sample bar chart data
    const data = [65, 78, 85, 92, 88, 95, 89, 96, 82, 90, 87, 93];
    
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: Array.from({length: 12}, (_, i) => i + 1),
            datasets: [{
                data: data,
                backgroundColor: '#10b981',
                borderRadius: 2,
                borderSkipped: false
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    enabled: false
                }
            },
            scales: {
                x: {
                    display: false
                },
                y: {
                    display: false
                }
            },
            animation: {
                duration: 800,
                easing: 'easeOutCubic'
            }
        }
    });
}

// Load Chart.js from CDN
function loadChartJS() {
    return new Promise((resolve, reject) => {
        if (window.Chart) {
            resolve();
            return;
        }
        
        const script = document.createElement('script');
        script.src = 'https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.js';
        script.onload = () => resolve();
        script.onerror = () => reject(new Error('Failed to load Chart.js'));
        document.head.appendChild(script);
    });
}

// Initialize Chart.js when needed
window.ensureChartJS = async function() {
    try {
        await loadChartJS();
        return true;
    } catch (error) {
        console.error('Error loading Chart.js:', error);
        return false;
    }
};

// Enhanced UI interactions
document.addEventListener('DOMContentLoaded', function() {
    // Add smooth scroll behavior
    document.documentElement.style.scrollBehavior = 'smooth';
    
    // Add intersection observer for staggered animations
    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry, index) => {
            if (entry.isIntersecting) {
                setTimeout(() => {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }, index * 100);
            }
        });
    }, {
        threshold: 0.1
    });
    
    // Observe cards for animation
    document.querySelectorAll('.stat-card-modern, .card-modern').forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';
        card.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
        observer.observe(card);
    });
    
    // Enhanced hover effects for stat cards
    document.querySelectorAll('.stat-card-modern').forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-4px) scale(1.02)';
            this.style.transition = 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });
    
    // Add ripple effect to buttons
    document.querySelectorAll('.btn-modern').forEach(button => {
        button.addEventListener('click', function(e) {
            const ripple = document.createElement('span');
            const rect = this.getBoundingClientRect();
            const size = Math.max(rect.width, rect.height);
            const x = e.clientX - rect.left - size / 2;
            const y = e.clientY - rect.top - size / 2;
            
            ripple.style.cssText = `
                position: absolute;
                border-radius: 50%;
                background: rgba(255, 255, 255, 0.3);
                width: ${size}px;
                height: ${size}px;
                left: ${x}px;
                top: ${y}px;
                transform: scale(0);
                animation: ripple-animation 0.6s ease-out;
                pointer-events: none;
            `;
            
            this.style.position = 'relative';
            this.style.overflow = 'hidden';
            this.appendChild(ripple);
            
            setTimeout(() => {
                ripple.remove();
            }, 600);
        });
    });
    
    // Add hover effects to table rows
    document.querySelectorAll('.table-modern tbody tr').forEach(row => {
        row.addEventListener('mouseenter', function() {
            this.style.backgroundColor = '#f8fafc';
            this.style.transform = 'scale(1.01)';
            this.style.transition = 'all 0.2s ease';
        });
        
        row.addEventListener('mouseleave', function() {
            this.style.backgroundColor = '';
            this.style.transform = 'scale(1)';
        });
    });
});

// Add the ripple animation CSS
const style = document.createElement('style');
style.textContent = `
    @keyframes ripple-animation {
        to {
            transform: scale(2);
            opacity: 0;
        }
    }
`;
document.head.appendChild(style);

// Call ensureChartJS immediately
window.ensureChartJS();