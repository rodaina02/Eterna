(() => {
    if (!window.Eterna?.motion) {
        return;
    }

    window.Eterna.motion.use("hero", (ctx) => {
        const init = () => {
            const { gsap, tokens, mm } = ctx;
            const hero = document.querySelector("[data-hero]");
            if (!hero || !gsap) {
                return;
            }

            const mark = hero.querySelector("[data-hero-mark]");
            const point = hero.querySelector("[data-hero-point]");
            const wipeA = hero.querySelector("[data-draw-a]");
            const wipeB = hero.querySelector("[data-draw-b]");
            const lock = hero.querySelector(".home-hero__logo");
            const lines = hero.querySelectorAll("[data-hero-title] .home-hero__line > span");
            const meta = hero.querySelector("[data-hero-meta]");
            const lede = hero.querySelector("[data-hero-lede]");
            const cue = hero.querySelector("[data-hero-scroll]");
            const canvas = hero.querySelector("[data-hero-canvas]");
            const forms = window.Eterna.system?.forms() || {};
            const line = window.Eterna.system?.line() || {};

            const intro = gsap.timeline({
                defaults: { ease: tokens.ease }
            });

            if (point) {
                intro.fromTo(point, { opacity: 0, scale: 0.4 }, {
                    opacity: 1,
                    scale: 1,
                    duration: 0.22,
                    ease: tokens.easeStrong
                }, 0);
            }

            if (wipeA) {
                intro.fromTo(wipeA, { scale: 0, opacity: 1 }, {
                    scale: 1,
                    duration: 0.77,
                    ease: tokens.easeInOut
                }, 0.12);
            }

            if (wipeB) {
                intro.fromTo(wipeB, { scale: 0, opacity: 1 }, {
                    scale: 1,
                    duration: 0.77,
                    ease: tokens.easeInOut
                }, 0.77);
            }

            if (wipeA && wipeB) {
                intro.to([wipeA, wipeB], { scale: 1, duration: 0.32, ease: tokens.ease }, 1.54);
            }

            if (lock) {
                intro.to(lock, {
                    opacity: 1,
                    duration: 0.28,
                    ease: tokens.ease,
                    onStart: () => lock.classList.add("is-ready")
                }, 1.72);
            }

            if (point) {
                intro.to(point, { opacity: 0, duration: 0.2 }, 1.72);
            }

            if (wipeA && wipeB) {
                intro.to([wipeA, wipeB], { opacity: 0, duration: 0.2 }, 1.86);
            }

            if (lines.length) {
                intro.fromTo(lines, { yPercent: 110 }, {
                    yPercent: 0,
                    duration: 0.48,
                    stagger: 0.14,
                    ease: tokens.easeStrong
                }, 1.54);
            }

            if (meta) {
                intro.fromTo(meta, { opacity: 0, y: 8 }, { opacity: 1, y: 0, duration: 0.32 }, 1.78);
            }
            if (lede) {
                intro.fromTo(lede, { opacity: 0, y: 8 }, { opacity: 1, y: 0, duration: 0.32 }, 1.9);
            }
            if (cue) {
                intro.fromTo(cue, { opacity: 0, y: 6 }, { opacity: 1, y: 0, duration: 0.28 }, 2.04);
            }

            mm.add("(min-width: 768px)", () => {
                window.Eterna.system?.claimGeometry();
                window.Eterna.system?.claimLine();

                const exit = gsap.timeline({
                    defaults: { ease: tokens.easeNone },
                    scrollTrigger: {
                        trigger: hero,
                        start: "top top",
                        end: "bottom top",
                        scrub: tokens.scrubCinematic,
                        invalidateOnRefresh: true,
                        onUpdate: (self) => {
                            if (self.progress > 0.18) {
                                window.Eterna.system?.releaseLine("emerge");
                            }
                        },
                        onLeave: () => {
                            window.Eterna.system?.releaseGeometry("statement");
                            window.Eterna.system?.releaseLine("emerge");
                        },
                        onLeaveBack: () => {
                            window.Eterna.system?.claimGeometry();
                            window.Eterna.system?.claimLine();
                        }
                    }
                });

                if (canvas) {
                    exit.to(canvas, { y: -36, opacity: 0.86 }, 0.08);
                }
                if (lock) {
                    exit.to(lock, { y: -28, opacity: 0 }, 0.12);
                }
                if (forms.a) {
                    exit.fromTo(forms.a, {
                        xPercent: -6,
                        yPercent: 4,
                        opacity: 0,
                        scale: 1.05
                    }, {
                        xPercent: -34,
                        yPercent: -14,
                        opacity: 0.22,
                        scale: 1.2
                    }, 0.18);
                }
                if (forms.b) {
                    exit.fromTo(forms.b, {
                        xPercent: 8,
                        yPercent: 6,
                        opacity: 0,
                        scale: 1
                    }, {
                        xPercent: 36,
                        yPercent: 20,
                        opacity: 0.18,
                        scale: 1.08
                    }, 0.24);
                }
                if (line.stroke) {
                    exit.fromTo(line.stroke, { scaleY: 0, opacity: 0 }, { scaleY: 0.22, opacity: 1 }, 0.32);
                }

                return () => {
                    exit.scrollTrigger?.kill();
                    window.Eterna.system?.releaseGeometry("echo");
                    window.Eterna.system?.releaseLine("flow");
                };
            });
        };

        return {
            initStatic() {
                document.querySelector(".home-hero__logo")?.classList.add("is-ready");
            },
            init
        };
    });
})();
