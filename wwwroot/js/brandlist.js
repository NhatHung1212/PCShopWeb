document.addEventListener('DOMContentLoaded', function() {
    const brands = document.querySelector('.brands-row');
    if (!brands) return;

    // Prevent double-init
    if (brands.dataset.marqueeInitialized) return;
    brands.dataset.marqueeInitialized = '1';

    // Duplicate content for seamless loop
    brands.innerHTML = brands.innerHTML + brands.innerHTML;

    const speed = 0.06; // pixels per millisecond
    let rafId = null;
    let lastTime = performance.now();

    function step(time) {
        const dt = time - lastTime;
        lastTime = time;
        brands.scrollLeft += speed * dt;

        // When scrolled past the first half, reset to start
        const half = brands.scrollWidth / 2;
        if (brands.scrollLeft >= half) {
            brands.scrollLeft -= half;
        }

        rafId = requestAnimationFrame(step);
    }

    // Pause on hover
    brands.addEventListener('mouseenter', function() {
        if (rafId) cancelAnimationFrame(rafId);
        rafId = null;
    });
    brands.addEventListener('mouseleave', function() {
        lastTime = performance.now();
        if (!rafId) rafId = requestAnimationFrame(step);
    });

    // Pause when tab hidden
    document.addEventListener('visibilitychange', function() {
        if (document.hidden) {
            if (rafId) cancelAnimationFrame(rafId);
            rafId = null;
        } else {
            lastTime = performance.now();
            if (!rafId) rafId = requestAnimationFrame(step);
        }
    });

    // Start
    rafId = requestAnimationFrame(step);
});
