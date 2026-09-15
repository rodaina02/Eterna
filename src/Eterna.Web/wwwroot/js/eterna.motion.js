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
        scrubFast: 0.48,
        scrubSnap: 0.34,
        scrubControlled: 0.7,
        scrubCinematic: 0.96,
        scrubTech: 0.52
    };

    const bindActive = (items, options = {}) => {
        const nodes = [...items];
        let scrollIndex = 0;
        let hoverIndex = null;
        const fine = window.matchMedia("(hover: hover) and (pointer: fine)").matches && !options.noHover;
        const abort = new AbortController();

        if (nodes[0]?._eternaActive) {
            nodes[0]._eternaActive.abort();
        }
        if (nodes[0]) {
            nodes[0]._eternaActive = abort;
        }

        const apply = (source) => {
            const index = hoverIndex ?? scrollIndex;
            nodes.forEach((node, i) => {
                node.classList.toggle("is-active", i === index);
            });
            if (typeof options.onChange === "function") {
                options.onChange(index, source);
            }
        };

        const api = {
            setScroll(index) {
                const next = Math.max(0, Math.min(nodes.length - 1, index | 0));
                if (next === scrollIndex) {
                    if (hoverIndex == null) {
                        return;
                    }
                    scrollIndex = next;
                    return;
                }
                scrollIndex = next;
                if (hoverIndex == null) {
                    apply("scroll");
                }
            },
            setHover(index) {
                hoverIndex = index;
                apply("hover");
            },
            clearHover() {
                if (hoverIndex == null) {
                    return;
                }
                hoverIndex = null;
                apply("scroll");
                if (typeof options.onHoverEnd === "function") {
                    options.onHoverEnd();
                }
            },
            dispose() {
                abort.abort();
            },
            get active() {
                return hoverIndex ?? scrollIndex;
            },
            get hover() {
                return hoverIndex;
            },
            get scroll() {
                return scrollIndex;
            }
        };

        if (fine) {
            nodes.forEach((node, index) => {
                node.addEventListener("pointerenter", () => api.setHover(index), { signal: abort.signal });
                node.addEventListener("pointerleave", (event) => {
                    const next = event.relatedTarget;
                    if (next && nodes.some((item) => item === next || item.contains(next))) {
                        return;
                    }
                    api.clearHover();
                }, { signal: abort.signal });
            });
        }

        if (nodes[0]) {
            nodes[0].classList.add("is-active");
        }

        return api;
    };

    const context = () => ({
        env: window.Eterna.env,
        tokens,
        bindActive,
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
            const settleTriggers = () => {
                if (!window.ScrollTrigger) {
                    return;
                }
                window.ScrollTrigger.sort();
                window.ScrollTrigger.refresh();
            };
            settleTriggers();
            window.addEventListener("load", settleTriggers, { once: true });
            booted = true;
        } catch (error) {
            document.documentElement.classList.add("is-static");
            document.documentElement.classList.remove("is-motion");
            run("static");
        }
    };

    window.Eterna.motion = {
        tokens,
        bindActive,
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
