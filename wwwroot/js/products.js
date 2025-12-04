// Products Page JavaScript
document.addEventListener('DOMContentLoaded', function() {
    const productsGrid = document.getElementById('productsGrid');
    const sortSelect = document.getElementById('sortSelect');
    
    if (!productsGrid) return;
    
    let products = Array.from(productsGrid.querySelectorAll('.product-card'));
    
    // Sort functionality
    if (sortSelect) {
        sortSelect.addEventListener('change', function() {
            const sortValue = this.value;
            sortProducts(sortValue);
        });
    }
    
    // Sort products
    function sortProducts(sortBy) {
        const sortedProducts = [...products];
        
        switch(sortBy) {
            case 'price-asc':
                sortedProducts.sort((a, b) => {
                    const priceA = parseFloat(a.dataset.price);
                    const priceB = parseFloat(b.dataset.price);
                    return priceA - priceB;
                });
                break;
            case 'price-desc':
                sortedProducts.sort((a, b) => {
                    const priceA = parseFloat(a.dataset.price);
                    const priceB = parseFloat(b.dataset.price);
                    return priceB - priceA;
                });
                break;
            case 'name':
                sortedProducts.sort((a, b) => {
                    const nameA = a.dataset.name;
                    const nameB = b.dataset.name;
                    return nameA.localeCompare(nameB);
                });
                break;
            case 'newest':
            default:
                // Keep original order
                break;
        }
        
        // Re-append products in new order
        productsGrid.innerHTML = '';
        sortedProducts.forEach(product => {
            productsGrid.appendChild(product);
        });
        
        // Re-trigger animations
        animateProducts();
    }
    
    // Animate products on scroll
    function animateProducts() {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach((entry, index) => {
                if (entry.isIntersecting) {
                    setTimeout(() => {
                        entry.target.style.opacity = '1';
                        entry.target.style.transform = 'translateY(0)';
                    }, index * 50);
                    observer.unobserve(entry.target);
                }
            });
        }, {
            threshold: 0.1
        });
        
        products.forEach(product => {
            product.style.opacity = '0';
            product.style.transform = 'translateY(20px)';
            product.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
            observer.observe(product);
        });
    }
    
    // Initial animation
    animateProducts();
    
    // Smooth scroll to top button
    const scrollToTopBtn = document.createElement('button');
    scrollToTopBtn.innerHTML = '<i class="fas fa-arrow-up"></i>';
    scrollToTopBtn.className = 'scroll-to-top';
    scrollToTopBtn.style.cssText = `
        position: fixed;
        bottom: 30px;
        right: 30px;
        width: 50px;
        height: 50px;
        background: linear-gradient(135deg, #a855f7 0%, #7c3aed 100%);
        border: none;
        border-radius: 50%;
        color: #fff;
        font-size: 20px;
        cursor: pointer;
        opacity: 0;
        visibility: hidden;
        transition: all 0.3s;
        z-index: 1000;
        box-shadow: 0 5px 15px rgba(168, 85, 247, 0.3);
    `;
    document.body.appendChild(scrollToTopBtn);
    
    window.addEventListener('scroll', function() {
        if (window.pageYOffset > 300) {
            scrollToTopBtn.style.opacity = '1';
            scrollToTopBtn.style.visibility = 'visible';
        } else {
            scrollToTopBtn.style.opacity = '0';
            scrollToTopBtn.style.visibility = 'hidden';
        }
    });
    
    scrollToTopBtn.addEventListener('click', function() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
    
    scrollToTopBtn.addEventListener('mouseenter', function() {
        this.style.transform = 'translateY(-5px)';
    });
    
    scrollToTopBtn.addEventListener('mouseleave', function() {
        this.style.transform = 'translateY(0)';
    });
    
    // Add to cart animation
    document.querySelectorAll('.add-to-cart-btn').forEach(btn => {
        btn.addEventListener('click', function(e) {
            // Create floating animation
            const icon = this.querySelector('i');
            if (icon) {
                icon.style.animation = 'bounce 0.5s ease';
                setTimeout(() => {
                    icon.style.animation = '';
                }, 500);
            }
        });
    });
});

// Bounce animation
const style = document.createElement('style');
style.textContent = `
    @keyframes bounce {
        0%, 100% { transform: translateY(0); }
        50% { transform: translateY(-10px); }
    }
`;
document.head.appendChild(style);
