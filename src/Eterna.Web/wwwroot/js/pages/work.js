(() => {
    const page = document.querySelector("[data-inner-page='work']");
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
                const copy = section.querySelector("[data-reveal-copy]");
                const logo = section.querySelector("[data-reveal-logo]");
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

                if (logo) {
                    timeline.from(logo, {
                        autoAlpha: 0,
                        y: 18,
                        duration: 0.56,
                        ease: "power2.out"
                    }, 0.06);
                }

                if (copy) {
                    timeline.from(copy, {
                        autoAlpha: 0,
                        y: 16,
                        duration: 0.42,
                        ease: "power2.out"
                    }, 0.08);
                }

                if (rows.length > 0) {
                    timeline.from(rows, {
                        autoAlpha: 0,
                        y: 14,
                        duration: 0.36,
                        stagger: 0.05,
                        ease: "power2.out"
                    }, 0.1);
                }

                if (rules.length > 0) {
                    timeline.from(rules, {
                        scaleX: 0,
                        duration: 0.4,
                        ease: "power2.out",
                        transformOrigin: "left center"
                    }, 0.12);
                }

                if (section.getBoundingClientRect().top < window.innerHeight * 0.86) {
                    timeline.progress(1);
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
        window.addEventListener("hashchange", () => {
            if (window.ScrollTrigger) {
                window.ScrollTrigger.refresh();
            }
        });
    };

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", start, { once: true });
    } else {
        start();
    }
})();
