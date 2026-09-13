(() => {
    if (!window.Eterna?.motion) {
        return;
    }

    window.Eterna.motion.use("portfolio", (ctx) => {
        const init = () => {
            const { gsap, tokens, mm, ScrollTrigger } = ctx;
            const section = document.querySelector("[data-work]");
            if (!section || !gsap || !mm) {
                return;
            }

            const viewport = section.querySelector("[data-work-runway]");
            const cards = gsap.utils.toArray(section.querySelectorAll("[data-work-card]"));
            if (!cards.length) {
                return;
            }

            mm.add("(min-width: 1024px)", () => {
                gsap.set(cards, { opacity: 0.34, scale: 0.97 });
                gsap.set(cards[0], { opacity: 1, scale: 1 });
                const timeline = gsap.timeline({
                    defaults: { ease: tokens.easeNone }
                });

                cards.forEach((card, index) => {
                    const at = index / Math.max(cards.length - 1, 1);
                    timeline.to(cards, { opacity: 0.34, scale: 0.97, duration: 0.18 }, at);
                    timeline.to(card, { opacity: 1, scale: 1, duration: 0.18 }, at);
                    const rule = card.querySelector("[data-work-rule]");
                    const frame = card.querySelector("[data-work-frame]");
                    const type = card.querySelector(".work-card__type");
                    if (rule) {
                        timeline.fromTo(rule, { scaleX: 0 }, { scaleX: 1, duration: 0.18, ease: tokens.ease }, at);
                    }
                    if (frame) {
                        timeline.fromTo(frame, { clipPath: "inset(8% 12% 8% 12%)" }, { clipPath: "inset(0% 0% 0% 0%)", duration: 0.18 }, at);
                    }
                    if (type) {
                        timeline.fromTo(type, { yPercent: 12, opacity: 0.4 }, { yPercent: 0, opacity: 1, duration: 0.18 }, at);
                    }
                });

                if (viewport) {
                    timeline.to(viewport, {
                        x: () => {
                            const max = Math.max(0, viewport.scrollWidth - section.clientWidth + 48);
                            return -max;
                        },
                        ease: tokens.easeNone,
                        duration: 1
                    }, 0);
                }

                const trigger = ScrollTrigger.create({
                    trigger: section,
                    start: () => `top ${window.Eterna.system?.headerOffset() || 72}`,
                    end: "+=118%",
                    pin: true,
                    pinSpacing: true,
                    scrub: tokens.scrubCinematic,
                    animation: timeline,
                    anticipatePin: 1,
                    invalidateOnRefresh: true
                });

                return () => {
                    trigger.kill();
                    gsap.set(cards.concat(viewport || []), { clearProps: "transform,opacity,clipPath" });
                };
            });

            mm.add("(max-width: 1023px)", () => {
                const timeline = gsap.timeline({
                    defaults: { ease: tokens.ease, duration: tokens.standard },
                    scrollTrigger: {
                        trigger: section,
                        start: "top 80%",
                        once: true
                    }
                });
                cards.forEach((card, index) => {
                    const title = card.querySelector(".work-card__title");
                    const meta = card.querySelector(".work-card__meta");
                    const frame = card.querySelector("[data-work-frame]");
                    if (frame) {
                        timeline.from(frame, { opacity: 0.4, y: 10 }, index * 0.06);
                    }
                    if (title) {
                        timeline.from(title, { opacity: 0.4, y: 8 }, index * 0.06 + 0.02);
                    }
                    if (meta) {
                        timeline.from(meta, { opacity: 0.4 }, index * 0.06 + 0.04);
                    }
                });
                return () => timeline.scrollTrigger?.kill();
            });
        };

        return { initStatic() {}, init };
    });
})();
