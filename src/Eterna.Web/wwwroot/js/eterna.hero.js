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
                intro.fromTo(wipeA, {
                    clipPath: "inset(86% 74% 2% 6%)"
                }, {
                    clipPath: "inset(0% 8% 10% 0%)",
                    duration: 0.72,
                    ease: tokens.easeInOut
                }, 0.12);
            }

            if (wipeB) {
                intro.fromTo(wipeB, {
                    clipPath: "inset(38% 100% 8% 28%)"
                }, {
                    clipPath: "inset(30% 0% 0% 16%)",
                    duration: 0.78,
                    ease: tokens.easeInOut
                }, 0.7);
            }

            if (lines.length) {
                intro.fromTo(lines, { yPercent: 110 }, {
                    yPercent: 0,
                    duration: 0.5,
                    stagger: 0.12,
                    ease: tokens.easeStrong
                }, 0.92);
            }

            if (lock) {
                intro.to(lock, {
                    opacity: 1,
                    duration: 0.32,
                    ease: tokens.ease,
                    onStart: () => lock.classList.add("is-ready")
                }, 1.58);
            }

            if (point) {
                intro.to(point, { opacity: 0, duration: 0.2 }, 1.62);
            }

            if (wipeA && wipeB) {
                intro.to([wipeA, wipeB], { opacity: 0, duration: 0.28 }, 1.78);
            }

            if (meta) {
                intro.fromTo(meta, { opacity: 0, y: 8 }, { opacity: 1, y: 0, duration: 0.3 }, 1.18);
            }
            if (lede) {
                intro.fromTo(lede, { opacity: 0, y: 8 }, { opacity: 1, y: 0, duration: 0.3 }, 1.72);
            }
            if (cue) {
                intro.fromTo(cue, { opacity: 0, y: 6 }, { opacity: 1, y: 0, duration: 0.26 }, 1.96);
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
                            if (self.progress > 0.16) {
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
                    exit.to(canvas, { y: -28, opacity: 0.9 }, 0.04);
                }
                if (lock) {
                    exit.fromTo(lock, {
                        clipPath: "inset(0% 0% 0% 0%)",
                        xPercent: 0,
                        yPercent: 0,
                        scale: 1,
                        opacity: 1
                    }, {
                        clipPath: "inset(6% 18% 14% 6%)",
                        xPercent: -12,
                        yPercent: -10,
                        scale: 0.78,
                        opacity: 0,
                        transformOrigin: "36% 52%",
                        immediateRender: false
                    }, 0.06);
                }
                if (mark) {
                    exit.to(mark, { y: -20, opacity: 0.2 }, 0.1);
                }
                if (forms.a) {
                    exit.fromTo(forms.a, {
                        xPercent: -10,
                        yPercent: 2,
                        opacity: 0,
                        scale: 0.9
                    }, {
                        xPercent: -32,
                        yPercent: -16,
                        opacity: 0.28,
                        scale: 1.22,
                        immediateRender: false
                    }, 0.14);
                }
                if (forms.b) {
                    exit.fromTo(forms.b, {
                        xPercent: 8,
                        yPercent: 6,
                        opacity: 0,
                        scale: 0.92
                    }, {
                        xPercent: 34,
                        yPercent: 18,
                        opacity: 0.2,
                        scale: 1.08,
                        immediateRender: false
                    }, 0.2);
                }
                if (line.stroke) {
                    exit.fromTo(line.stroke, {
                        scaleY: 0,
                        opacity: 0
                    }, {
                        scaleY: 0.24,
                        opacity: 0.42,
                        immediateRender: false
                    }, 0.28);
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
