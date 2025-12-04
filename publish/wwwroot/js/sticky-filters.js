// Calculate header height and adjust filters sticky position
document.addEventListener('DOMContentLoaded', function() {
    const header = document.querySelector('.header-wrapper');
    const filters = document.querySelector('.products-filters');
    
    if (header && filters) {
        function updateFiltersPosition() {
            const headerHeight = header.offsetHeight;
            filters.style.top = `${headerHeight}px`;
        }
        
        // Initial update
        updateFiltersPosition();
        
        // Update on resize
        window.addEventListener('resize', updateFiltersPosition);
        
        // Update after fonts load (affects header height)
        if (document.fonts) {
            document.fonts.ready.then(updateFiltersPosition);
        }
    }
});
