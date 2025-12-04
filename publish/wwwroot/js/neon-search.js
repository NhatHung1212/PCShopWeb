/**
 * Neon Search Bar Animation
 * Manages the animated border effects based on user interaction
 */

document.addEventListener('DOMContentLoaded', function() {
    const searchForm = document.getElementById('neonSearchForm');
    const searchInput = document.getElementById('neonSearchInput');
    const searchBtn = document.getElementById('neonSearchBtn');
    
    // Check if elements exist
    if (!searchForm || !searchInput || !searchBtn) {
        console.warn('Neon search elements not found');
        return;
    }
    
    let currentDuration = 4000; 
    let targetDuration = 4000;
    let animationFrame = null;
    let isTyping = false;
    let isSearching = false;

    /**
     * Smooth transition between animation speeds
     */
    function smoothTransition() {
        const diff = targetDuration - currentDuration;
        
        if (Math.abs(diff) > 10) {
            currentDuration += diff * 0.015;
            searchForm.style.setProperty('--spin-duration', `${currentDuration}ms`);
            animationFrame = requestAnimationFrame(smoothTransition);
        } else {
            currentDuration = targetDuration;
            searchForm.style.setProperty('--spin-duration', `${currentDuration}ms`);
            animationFrame = null;
        }
    }

    /**
     * Handle input typing - slow down animation
     */
    searchInput.addEventListener('input', function() {
        if (!isTyping && !isSearching) {
            isTyping = true;
            targetDuration = 60000; // Slow animation when typing
            if (!animationFrame) {
                smoothTransition();
            }
        }
    });

    /**
     * Handle focus - reset animation if no value
     */
    searchInput.addEventListener('focus', function() {
        if (!isTyping && !searchInput.value) {
            targetDuration = 4000;
            if (animationFrame) {
                cancelAnimationFrame(animationFrame);
                animationFrame = null;
            }
            currentDuration = 4000;
            searchForm.style.setProperty('--spin-duration', `${currentDuration}ms`);
        }
    });

    /**
     * Handle blur - return to normal speed
     */
    searchInput.addEventListener('blur', function() {
        if (!isSearching) {
            isTyping = false;
            targetDuration = 4000;

            if (animationFrame) {
                cancelAnimationFrame(animationFrame);
                animationFrame = null;
            }

            smoothTransition();
        }
    });

    /**
     * Handle search button click - speed up animation
     */
    searchBtn.addEventListener('click', function() {
        isSearching = true;

        if (animationFrame) {
            cancelAnimationFrame(animationFrame);
            animationFrame = null;
        }

        targetDuration = 2500; // Speed up on search
        smoothTransition();

        // Return to normal after search animation
        setTimeout(function() {
            isSearching = false;
            isTyping = false;
            targetDuration = 4000;
            
            if (animationFrame) {
                cancelAnimationFrame(animationFrame);
                animationFrame = null;
            }
            
            smoothTransition();
        }, 1500);
    });

    /**
     * Handle form submission
     */
    searchForm.addEventListener('submit', function(e) {
        // Validate input
        if (!searchInput.value.trim()) {
            e.preventDefault();
            searchInput.focus();
            return false;
        }
    });

    /**
     * Keyboard shortcuts
     */
    document.addEventListener('keydown', function(e) {
        // Focus search on Ctrl/Cmd + K
        if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
            e.preventDefault();
            searchInput.focus();
        }
        
        // Submit on Enter when focused
        if (e.key === 'Enter' && document.activeElement === searchInput) {
            if (searchInput.value.trim()) {
                searchForm.submit();
            }
        }
        
        // Clear on Escape
        if (e.key === 'Escape' && document.activeElement === searchInput) {
            searchInput.value = '';
            searchInput.blur();
        }
    });
});