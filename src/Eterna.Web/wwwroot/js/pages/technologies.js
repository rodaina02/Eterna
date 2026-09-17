(() => {
    const page = document.querySelector("[data-inner-page='technologies']");
    if (!page) {
        return;
    }

    const sections = [...page.querySelectorAll("[data-inner-reveal]")];
    let context = null;
    let listening = false;

    const kill = () => {
        if (context) {
            context.revert();
            context = null;
        }
    };

    const boot = () => {
        kill();

        const env = window.Eterna && window.Eterna.env;
        if (!env || env.home || env.reducedMotion || !window.gsap || !window.ScrollTrigger || sections.length === 0) {
            return;
        }

        window.gsap.registerPlugin(window.ScrollTrigger);

        context = window.gsap.context(() => {
            sections.forEach((section) => {
                const rules = section.querySelectorAll("[data-reveal-rule]");

                const timeline = window.gsap.timeline({
                    scrollTrigger: {
                        trigger: section,
                        start: "top 86%",
                        toggleActions: "play reverse play reverse"
                    }
                });

                timeline.from(section, {
                    autoAlpha: 0,
                    y: 28,
                    duration: 0.56,
                    ease: "power2.out"
                }, 0);

                if (rules.length > 0) {
                    timeline.from(rules, {
                        scaleX: 0,
                        duration: 0.48,
                        ease: "power2.out",
                        transformOrigin: "left center"
                    }, 0.12);
                }
            });
        }, page);
    };

    const start = () => {
        boot();
        if (!listening && window.Eterna && window.Eterna.env && typeof window.Eterna.env.onReducedChange === "function") {
            window.Eterna.env.onReducedChange(boot);
            listening = true;
        }
    };

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", start, { once: true });
    } else {
        start();
    }
})();
