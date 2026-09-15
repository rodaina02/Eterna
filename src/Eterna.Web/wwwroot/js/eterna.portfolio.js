(() => {
    if (!window.Eterna?.motion) {
        return;
    }

    window.Eterna.motion.use("portfolio", (ctx) => {
        const init = () => {
            const { gsap, tokens, mm, ScrollTrigger, bindActive } = ctx;
            const section = document.querySelector("[data-work]");
            if (!section || !gsap || !mm) {
                return;
            }

            const runway = section.querySelector("[data-work-runway]");
            const frame = section.querySelector(".home-work__viewport");
            const cards = gsap.utils.toArray(section.querySelectorAll("[data-work-card]"));
            if (!cards.length) {
                return;
            }

            const travelX = () => {
                if (!runway || !frame) {
                    return 0;
                }
                const last = cards[cards.length - 1];
                if (!last) {
                    return 0;
                }
                const styles = window.getComputedStyle(runway);
                const gap = Number.parseFloat(styles.columnGap) || Number.parseFloat(styles.gap) || 0;
                const padEnd = Number.parseFloat(styles.paddingRight) || 0;
                const breathing = Math.max(gap, 48);
                const currentX = Number.parseFloat(gsap.getProperty(runway, "x")) || 0;
                const lastRight = last.getBoundingClientRect().right - currentX;
                const frameRight = frame.getBoundingClientRect().right;
                const overflow = runway.scrollWidth - frame.clientWidth;
                const edge = lastRight - frameRight;
                const needed = Math.max(overflow, edge) + Math.max(0, breathing - padEnd);
                return -Math.max(0, needed);
            };

            mm.add("(min-width: 1024px)", () => {
                const active = bindActive(cards);
                const timeline = gsap.timeline({
                    defaults: { ease: tokens.easeNone }
                });

                if (runway) {
                    timeline.to(runway, {
                        x: () => travelX(),
                        ease: tokens.easeNone,
                        duration: 0.8
                    }, 0);
                    timeline.to(runway, {
                        x: () => travelX(),
                        duration: 0.2
                    }, 0.8);
                }

                const trigger = ScrollTrigger.create({
                    trigger: section,
                    start: () => `top ${window.Eterna.system?.headerOffset() || 72}`,
                    end: () => `+=${Math.round(window.innerHeight * 1.22)}`,
                    pin: true,
                    pinSpacing: true,
                    scrub: tokens.scrubCinematic,
                    animation: timeline,
                    anticipatePin: 1,
                    invalidateOnRefresh: true,
                    refreshPriority: 1,
                    onUpdate: (self) => {
                        const travel = 0.8;
                        const index = cards.length < 2
                            ? 0
                            : Math.min(
                                cards.length - 1,
                                Math.floor((Math.min(self.progress, travel) / travel) * cards.length)
                            );
                        active.setScroll(index);
                    }
                });

                return () => {
                    active.dispose();
                    trigger.kill();
                    gsap.set(cards.concat(runway || []), { clearProps: "transform,clipPath" });
                };
            });

            mm.add("(max-width: 1023px)", () => {
                const active = bindActive(cards);
                return () => active.dispose();
            });
        };

        return { initStatic() {}, init };
    });
})();
