/**
 * Hero Video Slide Carousel
 * Fast-moving slide groups with pagination indicators.
 */

document.addEventListener('DOMContentLoaded', () => {
    const carousel = document.querySelector('.video-carousel');
    const wrapper = document.querySelector('.video-slides-wrapper');
    const dots = document.querySelectorAll('.carousel-pagination .pagination-dot');

    if (!carousel || !wrapper) return;

    const slideCount = wrapper.children.length;
    if (slideCount === 0) return;

    const transitionValue = 'transform 0.65s cubic-bezier(0.7, 0.05, 0.4, 1)';
    const autoDelay = Number(carousel.dataset.autoDelay) || 3200;

    let currentIndex = 0;
    let isAnimating = false;
    let autoTimer = null;
    let pendingSteps = 0;
    let resumeAfterManual = false;
    let isHovering = false;
    let queuedTarget = null;

    const setTransition = enabled => {
        wrapper.style.transition = enabled ? transitionValue : 'none';
    };

    const setActiveDot = index => {
        if (!dots.length) return;
        dots.forEach(dot => dot.classList.remove('active'));
        const targetDot = dots[index % dots.length];
        if (targetDot) targetDot.classList.add('active');
    };

    const playAllVideos = () => {
        carousel.querySelectorAll('video').forEach(video => {
            const promise = video.play();
            if (promise && typeof promise.catch === 'function') {
                promise.catch(() => {
                    /* Autoplay might be blocked; ignore */
                });
            }
        });
    };

    const goToNext = (manual = false) => {
        if (isAnimating) return;
        isAnimating = true;

        if (manual) {
            resumeAfterManual = true;
            stopAuto();
        }

        setTransition(true);
        requestAnimationFrame(() => {
            wrapper.style.transform = 'translateX(-100%)';
        });
    };

    function handleTransitionEnd(event) {
        if (event.target !== wrapper) return;

        const firstSlide = wrapper.firstElementChild;
        if (firstSlide) {
            wrapper.appendChild(firstSlide);
        }

        currentIndex = (currentIndex + 1) % slideCount;
        setActiveDot(currentIndex);

        setTransition(false);
        wrapper.style.transform = 'translateX(0)';

        requestAnimationFrame(() => {
            isAnimating = false;

            if (pendingSteps > 0) {
                pendingSteps--;
                goToNext(false);
                return;
            }

            if (queuedTarget !== null) {
                processQueuedTarget();
                return;
            }

            if (resumeAfterManual) {
                resumeAfterManual = false;
                if (!isHovering) startAuto();
            }
        });

        playAllVideos();
    }

    function processQueuedTarget() {
        if (queuedTarget === null) return;
        const target = queuedTarget;
        queuedTarget = null;

        if (target === currentIndex) {
            if (!resumeAfterManual && !isHovering) startAuto();
            return;
        }

        const stepsForward = (target - currentIndex + slideCount) % slideCount;
        pendingSteps = Math.max(stepsForward - 1, 0);
        goToNext(true);
    }

    function queueGoToSlide(index) {
        if (!Number.isFinite(index)) return;
        const normalized = (index % slideCount + slideCount) % slideCount;
        queuedTarget = normalized;
        resumeAfterManual = true;
        stopAuto();

        if (!isAnimating) {
            processQueuedTarget();
        }
    }

    const startAuto = () => {
        if (autoTimer || slideCount <= 1) return;
        autoTimer = setInterval(() => goToNext(false), autoDelay);
    };

    const stopAuto = () => {
        if (!autoTimer) return;
        clearInterval(autoTimer);
        autoTimer = null;
    };

    dots.forEach((dot, index) => {
        dot.addEventListener('click', () => {
            queueGoToSlide(index);
        });
    });

    carousel.addEventListener('mouseenter', () => {
        isHovering = true;
        stopAuto();
    });

    carousel.addEventListener('mouseleave', () => {
        isHovering = false;
        if (!resumeAfterManual) startAuto();
    });

    document.addEventListener('keydown', event => {
        const rect = carousel.getBoundingClientRect();
        const inView = rect.top < window.innerHeight && rect.bottom > 0;
        if (!inView) return;

        if (event.key === 'ArrowRight') {
            goToNext(true);
        }
    });

    document.addEventListener('visibilitychange', () => {
        if (document.hidden) {
            stopAuto();
        } else if (!isHovering && !resumeAfterManual) {
            startAuto();
            playAllVideos();
        }
    });

    setTransition(false);
    wrapper.style.transform = 'translateX(0)';
    setActiveDot(0);
    playAllVideos();
    startAuto();

    wrapper.addEventListener('transitionend', handleTransitionEnd);
});