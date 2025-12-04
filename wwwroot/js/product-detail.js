// Product Detail Page JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Quantity Controls
    const decreaseBtn = document.getElementById('decreaseQty');
    const increaseBtn = document.getElementById('increaseQty');
    const quantityInput = document.getElementById('quantityInput');
    
    if (decreaseBtn && increaseBtn && quantityInput) {
        decreaseBtn.addEventListener('click', function() {
            let currentValue = parseInt(quantityInput.value);
            if (currentValue > 1) {
                quantityInput.value = currentValue - 1;
                animateQuantity();
            }
        });
        
        increaseBtn.addEventListener('click', function() {
            let currentValue = parseInt(quantityInput.value);
            if (currentValue < 99) {
                quantityInput.value = currentValue + 1;
                animateQuantity();
            }
        });
    }
    
    function animateQuantity() {
        quantityInput.style.transform = 'scale(1.2)';
        quantityInput.style.color = '#a855f7';
        setTimeout(() => {
            quantityInput.style.transform = 'scale(1)';
            quantityInput.style.color = '#fff';
        }, 200);
    }
    
    // Image Gallery
    const mainImage = document.getElementById('mainImage');
    const thumbnails = document.querySelectorAll('.thumbnail');
    
    thumbnails.forEach(thumbnail => {
        thumbnail.addEventListener('click', function() {
            // Remove active class from all thumbnails
            thumbnails.forEach(t => t.classList.remove('active'));
            
            // Add active class to clicked thumbnail
            this.classList.add('active');
            
            // Update main image
            const newImageSrc = this.querySelector('img').src;
            mainImage.style.opacity = '0';
            setTimeout(() => {
                mainImage.src = newImageSrc;
                mainImage.style.opacity = '1';
            }, 200);
        });
    });
    
    // Image Zoom Effect
    const mainImageWrapper = document.getElementById('mainImageWrapper');
    
    if (mainImageWrapper && mainImage) {
        mainImageWrapper.addEventListener('mousemove', function(e) {
            const rect = this.getBoundingClientRect();
            const x = ((e.clientX - rect.left) / rect.width) * 100;
            const y = ((e.clientY - rect.top) / rect.height) * 100;
            
            mainImage.style.transformOrigin = `${x}% ${y}%`;
        });
        
        mainImageWrapper.addEventListener('mouseenter', function() {
            mainImage.style.transform = 'scale(1.5)';
        });
        
        mainImageWrapper.addEventListener('mouseleave', function() {
            mainImage.style.transform = 'scale(1)';
        });
    }
    
    // Add to Cart Animation
    const addToCartBtn = document.querySelector('.btn-add-to-cart');
    
    if (addToCartBtn) {
        addToCartBtn.addEventListener('click', function(e) {
            // Create success ripple effect
            const ripple = document.createElement('span');
            ripple.style.cssText = `
                position: absolute;
                border-radius: 50%;
                background: rgba(255, 255, 255, 0.6);
                width: 100px;
                height: 100px;
                margin-top: -50px;
                margin-left: -50px;
                animation: ripple 0.6s;
                pointer-events: none;
            `;
            
            const rect = this.getBoundingClientRect();
            ripple.style.left = (e.clientX - rect.left) + 'px';
            ripple.style.top = (e.clientY - rect.top) + 'px';
            
            this.appendChild(ripple);
            
            setTimeout(() => {
                ripple.remove();
            }, 600);
        });
    }
    
    // Smooth scroll for breadcrumb
    document.querySelectorAll('.breadcrumb a').forEach(link => {
        link.addEventListener('click', function(e) {
            if (this.getAttribute('href').startsWith('#')) {
                e.preventDefault();
                const target = document.querySelector(this.getAttribute('href'));
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }
        });
    });
    
    // Scroll animations
    const animateOnScroll = () => {
        const elements = document.querySelectorAll('.product-specs, .related-products');
        
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }
            });
        }, {
            threshold: 0.1
        });
        
        elements.forEach(element => {
            element.style.opacity = '0';
            element.style.transform = 'translateY(30px)';
            element.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
            observer.observe(element);
        });
    };
    
    animateOnScroll();
    
    // Sticky price on scroll (mobile)
    if (window.innerWidth <= 768) {
        const productPrice = document.querySelector('.product-detail-price');
        const productActions = document.querySelector('.product-actions');
        
        if (productPrice && productActions) {
            window.addEventListener('scroll', function() {
                const actionsRect = productActions.getBoundingClientRect();
                
                if (actionsRect.top < 0) {
                    // Create sticky bar
                    if (!document.getElementById('stickyBar')) {
                        const stickyBar = document.createElement('div');
                        stickyBar.id = 'stickyBar';
                        stickyBar.style.cssText = `
                            position: fixed;
                            bottom: 0;
                            left: 0;
                            right: 0;
                            background: linear-gradient(180deg, #1a1a1a 0%, #0a0a0a 100%);
                            border-top: 1px solid #2a2a2a;
                            padding: 15px 20px;
                            display: flex;
                            justify-content: space-between;
                            align-items: center;
                            z-index: 1000;
                            box-shadow: 0 -5px 20px rgba(0, 0, 0, 0.5);
                        `;
                        
                        stickyBar.innerHTML = `
                            <span style="font-size: 20px; font-weight: 800; background: linear-gradient(135deg, #a855f7 0%, #ec4899 100%); -webkit-background-clip: text; -webkit-text-fill-color: transparent;">
                                ${productPrice.textContent}
                            </span>
                            ${addToCartBtn.outerHTML}
                        `;
                        
                        document.body.appendChild(stickyBar);
                    }
                } else {
                    const stickyBar = document.getElementById('stickyBar');
                    if (stickyBar) {
                        stickyBar.remove();
                    }
                }
            });
        }
    }
});

// Ripple animation
const style = document.createElement('style');
style.textContent = `
    @keyframes ripple {
        from {
            opacity: 1;
            transform: scale(0);
        }
        to {
            opacity: 0;
            transform: scale(2);
        }
    }
`;
document.head.appendChild(style);
