(() => {
    const page = document.querySelector("[data-inner-page='services']");
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
                const heading = section.querySelector("[data-reveal-heading]");
                const rules = section.querySelectorAll("[data-reveal-rule]");
                const rows = section.querySelectorAll("[data-reveal-row]");

                const timeline = window.gsap.timeline({
                    scrollTrigger: {
                        trigger: section,
                        start: "top 86%",
                        toggleActions: "play reverse play reverse"
                    }
                });

                timeline.from(heading || section, {
                    autoAlpha: 0,
                    y: 24,
                    duration: 0.48,
                    ease: "power2.out"
                }, 0);

                if (rows.length > 0) {
                    timeline.from(rows, {
                        autoAlpha: 0,
                        y: 14,
                        duration: 0.36,
                        stagger: 0.05,
                        ease: "power2.out"
                    }, 0.08);
                }

                if (rules.length > 0) {
                    timeline.from(rules, {
                        scaleX: 0,
                        duration: 0.4,
                        ease: "power2.out",
                        transformOrigin: "left center"
                    }, 0.1);
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
