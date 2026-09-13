(() => {
    window.Eterna = window.Eterna || {};

    const modules = [];
    let media = null;
    let scope = null;
    let booted = false;

    const tokens = {
        ease: "power2.out",
        easeInOut: "power1.inOut",
        easeStrong: "power3.out",
        easeNone: "none",
        micro: 0.18,
        fast: 0.32,
        standard: 0.48,
        slow: 0.64,
        reveal: 0.8,
        section: 1.0,
        emphasis: 0.56,
        chapter: 0.42,
        scrubFast: 0.52,
        scrubControlled: 0.82,
        scrubCinematic: 1.0
    };

    const context = () => ({
        env: window.Eterna.env,
        tokens,
        gsap: window.gsap,
        ScrollTrigger: window.ScrollTrigger,
        mm: media
    });

    const revert = () => {
        if (scope) {
            scope.revert();
            scope = null;
        }

        media = null;

        modules.forEach((entry) => {
            if (typeof entry.dispose === "function") {
                entry.dispose();
                entry.dispose = null;
            }
        });

        booted = false;
    };

    const run = (mode) => {
        modules.forEach((entry) => {
            const api = entry.factory(context());
            if (!api) {
                return;
            }

            if (mode === "static" && typeof api.initStatic === "function") {
                entry.dispose = api.initStatic() || null;
            }

            if (mode === "motion" && typeof api.init === "function") {
                entry.dispose = api.init() || null;
            }
        });
    };

    const boot = () => {
        const env = window.Eterna.env;
        if (!env || !env.home) {
            return;
        }

        revert();

        if (env.reducedMotion || !window.gsap || !window.ScrollTrigger) {
            document.documentElement.classList.add("is-static");
            document.documentElement.classList.remove("is-motion");
            run("static");
            return;
        }

        try {
            window.gsap.registerPlugin(window.ScrollTrigger);
            window.ScrollTrigger.config({ ignoreMobileResize: true });
            scope = window.gsap.context(() => {
                media = window.gsap.matchMedia();
                run("motion");
            });
            window.addEventListener("load", () => {
                window.ScrollTrigger?.refresh();
            }, { once: true });
            booted = true;
        } catch (error) {
            document.documentElement.classList.add("is-static");
            document.documentElement.classList.remove("is-motion");
            run("static");
        }
    };

    window.Eterna.motion = {
        tokens,
        use(name, factory) {
            modules.push({ name, factory, dispose: null });
        },
        boot,
        revert,
        get ready() {
            return booted;
        }
    };

    const start = () => window.Eterna.motion.boot();
    if (document.readyState === "complete") {
        start();
    } else {
        document.addEventListener("DOMContentLoaded", start, { once: true });
    }

    if (window.Eterna.env) {
        window.Eterna.env.onReducedChange(() => {
            start();
        });
    }
})();
