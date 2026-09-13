(() => {
    const root = document.documentElement;
    const reducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

    root.classList.toggle("is-static", reducedMotion);
    root.classList.toggle("is-motion", !reducedMotion);
})();
