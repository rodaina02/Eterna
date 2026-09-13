(() => {
    const root = document.documentElement;
    const reducedQuery = window.matchMedia("(prefers-reduced-motion: reduce)");
    const coarseQuery = window.matchMedia("(pointer: coarse)");
    const page = document.querySelector(".site-main--home") ? "home" : "inner";

    const breakpoint = () => {
        const width = window.innerWidth;
        if (width >= 1024) {
            return "desktop";
        }
        if (width >= 768) {
            return "tablet";
        }
        return "mobile";
    };

    const apply = () => {
        const reduced = reducedQuery.matches;
        root.classList.toggle("is-static", reduced);
        root.classList.toggle("is-motion", !reduced);
        root.classList.toggle("is-coarse", coarseQuery.matches);
        root.classList.toggle("is-fine", !coarseQuery.matches);
        root.dataset.page = page;
        root.dataset.breakpoint = breakpoint();
    };

    apply();
    reducedQuery.addEventListener("change", apply);
    coarseQuery.addEventListener("change", apply);
    window.addEventListener("resize", apply, { passive: true });

    window.Eterna = window.Eterna || {};
    window.Eterna.env = {
        get reducedMotion() {
            return reducedQuery.matches;
        },
        get coarse() {
            return coarseQuery.matches;
        },
        get fine() {
            return !coarseQuery.matches;
        },
        get page() {
            return page;
        },
        get home() {
            return page === "home";
        },
        get breakpoint() {
            return breakpoint();
        },
        get desktop() {
            return window.innerWidth >= 1024;
        },
        get tablet() {
            return window.innerWidth >= 768 && window.innerWidth < 1024;
        },
        get mobile() {
            return window.innerWidth < 768;
        },
        onReducedChange(handler) {
            reducedQuery.addEventListener("change", handler);
        }
    };
})();
