(() => {
    if (!window.Eterna?.motion) {
        return;
    }

    const pinStart = () => `top ${window.Eterna.system?.headerOffset() || 72}`;

    const initStatement = (ctx) => {
        const { gsap, tokens } = ctx;
        const section = document.querySelector("[data-statement]");
        if (!section) {
            return;
        }
        const beats = section.querySelectorAll("[data-statement-beat]");
        const note = section.querySelector("[data-statement-note]");
        const timeline = gsap.timeline({
            defaults: { ease: tokens.easeNone },
            scrollTrigger: {
                trigger: section,
                start: "top 88%",
                end: "bottom 42%",
                scrub: tokens.scrubControlled
            }
        });
        if (beats[0]) {
            timeline.fromTo(beats[0], { opacity: 0.28, y: 16 }, { opacity: 0.7, y: 0 }, 0);
        }
        if (beats[1]) {
            timeline.fromTo(beats[1], { opacity: 0.2, y: 18 }, { opacity: 1, y: 0 }, 0.28);
        }
        if (beats[2]) {
            timeline.fromTo(beats[2], { opacity: 0.2, y: 16 }, { opacity: 1, y: 0 }, 0.55);
        }
        if (note) {
            timeline.fromTo(note, { opacity: 0.2 }, { opacity: 1 }, 0.72);
        }
    };

    const initHuman = (ctx) => {
        const { gsap, tokens, mm, ScrollTrigger } = ctx;
        const section = document.querySelector("[data-human]");
        if (!section) {
            return;
        }
        const title = section.querySelector(".section-header__title");
        const items = gsap.utils.toArray(section.querySelectorAll("[data-human-item]"));
        const image = section.querySelector("[data-human-image]");
        const frame = section.querySelector("[data-human-frame]");
        const forms = window.Eterna.system?.forms() || {};

        mm.add("(min-width: 1024px)", () => {
            window.Eterna.system?.claimGeometry();
            const timeline = gsap.timeline({ defaults: { ease: tokens.easeNone } });

            if (frame) {
                timeline.fromTo(frame, {
                    clipPath: "inset(12% 28% 14% 10%)",
                    xPercent: 14,
                    scale: 1.04
                }, {
                    clipPath: "inset(4% 10% 6% 4%)",
                    xPercent: 6,
                    scale: 1.02
                }, 0);
                timeline.to(frame, {
                    clipPath: "inset(0% 0% 0% 0%)",
                    xPercent: -8,
                    scale: 1
                }, 0.22);
            }
            if (image) {
                timeline.fromTo(image, { opacity: 0.4 }, { opacity: 1 }, 0);
                timeline.to(image, { xPercent: -12, opacity: 0.92 }, 0.38);
            }
            if (forms.a) {
                timeline.fromTo(forms.a, {
                    xPercent: -40,
                    yPercent: -8,
                    opacity: 0.22,
                    scale: 1.18
                }, {
                    xPercent: -8,
                    yPercent: 0,
                    opacity: 0.5,
                    scale: 1.34,
                    immediateRender: false
                }, 0);
            }
            if (forms.b) {
                timeline.fromTo(forms.b, {
                    xPercent: 44,
                    yPercent: 12,
                    opacity: 0.16,
                    scale: 1.04
                }, {
                    xPercent: 10,
                    yPercent: 6,
                    opacity: 0.42,
                    scale: 1.2,
                    immediateRender: false
                }, 0.08);
            }
            if (forms.a && forms.b) {
                timeline.to(forms.a, { xPercent: -2, yPercent: 2, scale: 1.28, opacity: 0.58 }, 0.48);
                timeline.to(forms.b, { xPercent: 4, yPercent: 6, scale: 1.16, opacity: 0.5 }, 0.48);
            }
            if (title) {
                timeline.fromTo(title, { opacity: 0.72, y: 12 }, { opacity: 1, y: 0 }, 0.48);
            }
            items.forEach((item, index) => {
                timeline.fromTo(item, { opacity: 0.18 }, { opacity: 1 }, 0.62 + index * 0.05);
                timeline.to(items.filter((_, itemIndex) => itemIndex !== index), { opacity: 0.34 }, 0.62 + index * 0.05);
            });
            if (frame) {
                timeline.to(frame, { xPercent: -22, clipPath: "inset(8% 28% 12% 4%)", opacity: 0.78 }, 0.86);
            }
            if (forms.a && forms.b) {
                timeline.to(forms.a, { xPercent: -16, yPercent: -4, opacity: 0.22, scale: 1.1 }, 0.9);
                timeline.to(forms.b, { xPercent: 18, yPercent: 10, opacity: 0.18, scale: 1.05 }, 0.9);
            }

            const trigger = ScrollTrigger.create({
                trigger: section,
                start: pinStart,
                end: "+=118%",
                pin: true,
                pinSpacing: true,
                scrub: tokens.scrubCinematic,
                animation: timeline,
                anticipatePin: 1,
                invalidateOnRefresh: true,
                onEnter: () => window.Eterna.system?.claimGeometry(),
                onEnterBack: () => window.Eterna.system?.claimGeometry(),
                onLeave: () => window.Eterna.system?.releaseGeometry("echo"),
                onLeaveBack: () => window.Eterna.system?.releaseGeometry("statement")
            });

            return () => {
                trigger.kill();
                window.Eterna.system?.releaseGeometry("echo");
            };
        });

        mm.add("(min-width: 768px) and (max-width: 1023px)", () => {
            const timeline = gsap.timeline({
                defaults: { ease: tokens.easeNone },
                scrollTrigger: {
                    trigger: section,
                    start: "top 82%",
                    end: "bottom 38%",
                    scrub: tokens.scrubControlled
                }
            });
            if (frame) {
                timeline.fromTo(frame, { clipPath: "inset(12% 22% 12% 8%)" }, { clipPath: "inset(0% 0% 0% 0%)" }, 0);
            }
            if (title) {
                timeline.fromTo(title, { opacity: 0.4, y: 10 }, { opacity: 1, y: 0 }, 0.12);
            }
            if (items.length) {
                timeline.fromTo(items, { opacity: 0.3 }, { opacity: 1, stagger: 0.05 }, 0.28);
            }
            return () => timeline.scrollTrigger?.kill();
        });

        mm.add("(max-width: 767px)", () => {
            const timeline = gsap.timeline({
                defaults: { ease: tokens.ease },
                scrollTrigger: {
                    trigger: section,
                    start: "top 78%",
                    end: "bottom 48%",
                    scrub: tokens.scrubFast
                }
            });
            if (frame) {
                timeline.fromTo(frame, { clipPath: "inset(10% 16% 10% 8%)" }, { clipPath: "inset(0% 0% 0% 0%)" }, 0);
            }
            if (title) {
                timeline.fromTo(title, { opacity: 0.4, y: 8 }, { opacity: 1, y: 0 }, 0.1);
            }
            if (items.length) {
                timeline.fromTo(items, { opacity: 0.32 }, { opacity: 1, stagger: 0.04 }, 0.2);
            }
            return () => timeline.scrollTrigger?.kill();
        });
    };

    const initPillars = (ctx) => {
        const { gsap, tokens, mm, ScrollTrigger } = ctx;
        const section = document.querySelector("[data-pillars]");
        if (!section) {
            return;
        }
        const origin = section.querySelector("[data-pillars-origin]");
        const streams = section.querySelectorAll("[data-pillars-stream]");
        const branches = section.querySelectorAll("[data-pillar-branch]");
        const pillars = gsap.utils.toArray(section.querySelectorAll("[data-pillar]"));

        mm.add("(min-width: 1024px)", () => {
            gsap.set(pillars, { opacity: 0.28, scale: 0.97, transformOrigin: "left top" });
            const timeline = gsap.timeline({ defaults: { ease: tokens.easeInOut, duration: 0.2 } });
            if (origin) {
                timeline.fromTo(origin, { opacity: 0, y: 12 }, { opacity: 1, y: 0 }, 0);
                timeline.to(origin, { opacity: 0.18, y: -8 }, 0.14);
            }
            if (streams.length) {
                timeline.fromTo(streams, { opacity: 0, y: 10 }, { opacity: 0.78, y: 0, stagger: 0.05 }, 0.14);
            }
            if (branches.length) {
                timeline.fromTo(branches, { scaleX: 0 }, { scaleX: 1, stagger: 0.04, ease: tokens.easeNone }, 0.18);
            }
            if (streams.length) {
                timeline.to(streams, { opacity: 0.22 }, 0.34);
            }
            pillars.forEach((pillar, index) => {
                timeline.to(pillars, { opacity: 0.28, scale: 0.97 }, 0.36 + index * 0.18);
                timeline.to(pillar, { opacity: 1, scale: 1 }, 0.36 + index * 0.18);
            });

            const trigger = ScrollTrigger.create({
                trigger: section,
                start: pinStart,
                end: "+=120%",
                pin: true,
                pinSpacing: true,
                scrub: tokens.scrubCinematic,
                animation: timeline,
                anticipatePin: 1,
                invalidateOnRefresh: true
            });

            return () => {
                trigger.kill();
                gsap.set(pillars, { clearProps: "opacity,transform" });
            };
        });

        mm.add("(max-width: 1023px)", () => {
            const timeline = gsap.timeline({
                scrollTrigger: {
                    trigger: section,
                    start: "top 80%",
                    once: true
                }
            });
            timeline.from(pillars, { opacity: 0.35, y: 12, stagger: 0.08, ease: tokens.ease, duration: tokens.standard });
            return () => timeline.scrollTrigger?.kill();
        });
    };

    const initServices = (ctx) => {
        const { gsap, tokens } = ctx;
        const section = document.querySelector("[data-services]");
        if (!section) {
            return;
        }
        const artifact = section.querySelector("[data-services-artifact]");
        const rule = section.querySelector("[data-services-rule]");
        const groups = gsap.utils.toArray(section.querySelectorAll("[data-service-group]"));
        const title = section.querySelector(".section-header__title");
        const timeline = gsap.timeline({
            defaults: { ease: tokens.easeNone },
            scrollTrigger: {
                trigger: section,
                start: "top 84%",
                end: "bottom 32%",
                scrub: tokens.scrubControlled
            }
        });
        if (title) {
            timeline.fromTo(title, { opacity: 0.35, y: 12 }, { opacity: 1, y: 0 }, 0);
        }
        if (artifact) {
            timeline.fromTo(artifact, { y: 24, opacity: 0.2, rotation: -0.6 }, { y: 0, opacity: 1, rotation: -0.6 }, 0.08);
            timeline.to(artifact, { xPercent: -6, opacity: 0.92 }, 0.42);
        }
        if (rule) {
            timeline.fromTo(rule, { scaleX: 0 }, { scaleX: 1 }, 0.18);
        }
        groups.forEach((group, index) => {
            const arch = group.querySelector("[data-service-arch]");
            const body = group.querySelector(".service-group, .arrow-link");
            timeline.fromTo(group, { opacity: 0.22 }, { opacity: 1 }, 0.38 + index * 0.2);
            if (arch) {
                timeline.fromTo(arch.children, { opacity: 0.12, y: 8 }, { opacity: 1, y: 0, stagger: 0.04 }, 0.38 + index * 0.2);
            }
            if (body) {
                timeline.fromTo(body, { opacity: 0.35 }, { opacity: 1 }, 0.44 + index * 0.2);
            }
        });
    };

    const initWhy = (ctx) => {
        const { gsap, tokens, mm, ScrollTrigger } = ctx;
        const section = document.querySelector("[data-why]");
        if (!section) {
            return;
        }
        const items = gsap.utils.toArray(section.querySelectorAll("[data-why-item]"));
        if (!items.length) {
            return;
        }

        mm.add("(min-width: 768px)", () => {
            gsap.set(items, { opacity: 0.32 });
            let current = -1;
            const setActive = (index) => {
                if (index === current) {
                    return;
                }
                current = index;
                items.forEach((item, itemIndex) => {
                    const active = itemIndex === index;
                    item.classList.toggle("is-active", active);
                    gsap.to(item, {
                        opacity: active ? 1 : 0.32,
                        duration: tokens.fast,
                        ease: tokens.ease,
                        overwrite: "auto"
                    });
                });
            };
            const trigger = ScrollTrigger.create({
                trigger: section.querySelector("[data-why-sequence]"),
                start: "top 62%",
                end: "bottom 40%",
                scrub: tokens.scrubControlled,
                onUpdate: (self) => setActive(Math.min(items.length - 1, Math.floor(self.progress * items.length))),
                onEnter: () => setActive(0)
            });
            return () => {
                trigger.kill();
                gsap.set(items, { clearProps: "opacity" });
                items.forEach((item) => item.classList.remove("is-active"));
            };
        });

        mm.add("(max-width: 767px)", () => {
            const timeline = gsap.timeline({
                scrollTrigger: { trigger: section, start: "top 80%", once: true }
            });
            timeline.from(items, { opacity: 0.35, y: 10, stagger: 0.06, duration: tokens.standard, ease: tokens.ease });
            return () => timeline.scrollTrigger?.kill();
        });
    };

    const initProcess = (ctx) => {
        const { gsap, tokens, mm, ScrollTrigger } = ctx;
        const section = document.querySelector("[data-process]");
        if (!section) {
            return;
        }
        const stages = gsap.utils.toArray(section.querySelectorAll("[data-process-stage]"));
        const official = stages.filter((stage) => stage.dataset.processStage !== "continue");
        const evolve = stages.find((stage) => stage.dataset.processStage === "continue");
        const spine = section.querySelector("[data-process-spine]");
        const nodes = gsap.utils.toArray(section.querySelectorAll("[data-process-node]"));

        mm.add("(min-width: 1024px)", () => {
            gsap.set(stages, { opacity: 0.3 });
            const timeline = gsap.timeline({ defaults: { ease: tokens.easeInOut, duration: 0.16 } });
            if (spine) {
                timeline.fromTo(spine, { scaleX: 0 }, { scaleX: 1, ease: tokens.easeNone, duration: 1 }, 0);
            }
            official.forEach((stage, index) => {
                const at = 0.12 + index * 0.14;
                timeline.to(stages, { opacity: 0.3 }, at);
                timeline.to(stage, { opacity: 1 }, at);
                if (nodes[index]) {
                    timeline.to(nodes[index], { opacity: 1 }, at);
                }
            });
            if (evolve) {
                timeline.to(official, { opacity: 0.3 }, 0.88);
                timeline.to(evolve, { opacity: 1 }, 0.88);
            }
            const trigger = ScrollTrigger.create({
                trigger: section,
                start: pinStart,
                end: "+=132%",
                pin: true,
                pinSpacing: true,
                scrub: tokens.scrubCinematic,
                animation: timeline,
                anticipatePin: 1,
                invalidateOnRefresh: true
            });
            return () => {
                trigger.kill();
                gsap.set(stages, { clearProps: "opacity" });
            };
        });

        mm.add("(max-width: 1023px)", () => {
            const timeline = gsap.timeline({
                scrollTrigger: { trigger: section, start: "top 80%", once: true }
            });
            timeline.from(official, { opacity: 0.35, y: 10, stagger: 0.06, duration: tokens.standard, ease: tokens.ease });
            if (evolve) {
                timeline.from(evolve, { opacity: 0.4, duration: tokens.fast }, "-=0.1");
            }
            return () => timeline.scrollTrigger?.kill();
        });
    };

    const initTech = (ctx) => {
        const { gsap, tokens } = ctx;
        const section = document.querySelector("[data-tech]");
        if (!section) {
            return;
        }
        const clusters = gsap.utils.toArray(section.querySelectorAll("[data-tech-cluster]"));
        const lattice = section.querySelector("[data-tech-lattice]");
        const core = section.querySelector("[data-tech-core]");
        const timeline = gsap.timeline({
            defaults: { ease: tokens.easeNone },
            scrollTrigger: {
                trigger: section,
                start: "top 78%",
                end: "bottom 28%",
                scrub: tokens.scrubFast
            }
        });
        clusters.forEach((cluster, index) => {
            timeline.to(clusters, { opacity: 0.32 }, index * 0.1);
            timeline.to(cluster, { opacity: 1 }, index * 0.1);
        });
        if (lattice) {
            timeline.to(lattice, { scale: 0.42, opacity: 0, transformOrigin: "center center" }, 0.78);
        }
        timeline.to(clusters, { opacity: 0.18, scale: 0.92, transformOrigin: "center center" }, 0.84);
        if (core) {
            timeline.fromTo(core, { scale: 0.4, opacity: 0 }, { scale: 1, opacity: 1 }, 0.86);
        }
    };

    const initIndustries = (ctx) => {
        const { gsap, tokens } = ctx;
        const section = document.querySelector("[data-industries]");
        if (!section) {
            return;
        }
        const items = gsap.utils.toArray(section.querySelectorAll("[data-industry]"));
        const rule = section.querySelector("[data-industries-rule]");
        const timeline = gsap.timeline({
            defaults: { ease: tokens.easeNone },
            scrollTrigger: {
                trigger: section,
                start: "top 82%",
                end: "bottom 42%",
                scrub: tokens.scrubFast
            }
        });
        if (rule) {
            timeline.fromTo(rule, { scaleX: 0 }, { scaleX: 1 }, 0);
        }
        items.forEach((item, index) => {
            timeline.to(items, { opacity: 0.34 }, 0.08 + index * 0.1);
            timeline.to(item, { opacity: 1 }, 0.08 + index * 0.1);
        });
        timeline.to(items, { opacity: 1 }, 0.92);
    };

    const initControl = (ctx) => {
        const { gsap, tokens } = ctx;
        const section = document.querySelector("[data-control]");
        if (!section) {
            return;
        }
        const lines = section.querySelectorAll("[data-control-line]");
        const principles = section.querySelectorAll("[data-control-primary] li");
        const support = section.querySelector("[data-control-support]");
        const timeline = gsap.timeline({
            defaults: { ease: tokens.easeNone },
            scrollTrigger: {
                trigger: section,
                start: "top 78%",
                end: "bottom 36%",
                scrub: tokens.scrubControlled
            }
        });
        if (lines[0]) {
            timeline.fromTo(lines[0], { opacity: 0.2, y: 12 }, { opacity: 1, y: 0 }, 0);
        }
        if (lines[1]) {
            timeline.fromTo(lines[1], { opacity: 0, y: 12 }, { opacity: 1, y: 0 }, 0.28);
        }
        if (principles.length) {
            timeline.fromTo(principles, { opacity: 0.2 }, { opacity: 1, stagger: 0.05 }, 0.46);
        }
        if (support) {
            timeline.fromTo(support, { opacity: 0.2 }, { opacity: 1 }, 0.72);
        }
    };

    const initAbout = (ctx) => {
        const { gsap, tokens } = ctx;
        const section = document.querySelector("[data-about]");
        if (!section) {
            return;
        }
        const scene = section.querySelector("[data-about-scene]");
        const photo = scene?.querySelector("img");
        const lockup = section.querySelector("[data-about-lockup]");
        const copy = section.querySelector("[data-about-copy]");
        const timeline = gsap.timeline({
            defaults: { ease: tokens.easeNone },
            scrollTrigger: {
                trigger: section,
                start: "top 86%",
                end: "bottom 40%",
                scrub: tokens.scrubControlled
            }
        });
        if (scene) {
            timeline.fromTo(scene, { opacity: 0.28, y: 16 }, { opacity: 1, y: 0 }, 0);
        }
        if (photo) {
            timeline.fromTo(photo, { scale: 1.04, opacity: 0.72 }, { scale: 1, opacity: 1 }, 0.08);
        }
        if (copy) {
            timeline.fromTo(copy, { opacity: 0.28, y: 10 }, { opacity: 1, y: 0 }, 0.22);
        }
        if (lockup) {
            timeline.fromTo(lockup, { opacity: 0.2, y: 8 }, { opacity: 1, y: 0 }, 0.34);
        }
    };

    const initCta = (ctx) => {
        const { gsap, tokens, mm, ScrollTrigger } = ctx;
        const section = document.querySelector("[data-cta]");
        if (!section) {
            return;
        }
        const lines = section.querySelectorAll("[data-cta-line]");
        const resolve = section.querySelector("[data-cta-resolve]");
        const lockup = section.querySelector("[data-cta-lockup]");
        const action = section.querySelector("[data-motion-cta], .btn");
        const mark = section.querySelector("[data-cta-mark]");

        mm.add("(min-width: 1024px)", () => {
            const timeline = gsap.timeline({ defaults: { ease: tokens.easeNone } });
            if (mark) {
                timeline.fromTo(mark, { scale: 1.35, opacity: 0.18 }, { scale: 1, opacity: 0.28 }, 0);
            }
            if (lines[0]) {
                timeline.fromTo(lines[0], { opacity: 0, y: 22, clipPath: "inset(100% 0 0 0)" }, { opacity: 1, y: 0, clipPath: "inset(0 0 0 0)" }, 0.08);
            }
            if (lines[1]) {
                timeline.fromTo(lines[1], { opacity: 0, y: 22, clipPath: "inset(100% 0 0 0)" }, { opacity: 1, y: 0, clipPath: "inset(0 0 0 0)" }, 0.2);
            }
            if (lines[2]) {
                timeline.fromTo(lines[2], { opacity: 0, y: 22, clipPath: "inset(100% 0 0 0)" }, { opacity: 1, y: 0, clipPath: "inset(0 0 0 0)" }, 0.32);
            }
            if (lines.length) {
                timeline.to(lines, { y: -10, scale: 0.9, opacity: 0.18, transformOrigin: "left top" }, 0.52);
            }
            if (resolve) {
                timeline.fromTo(resolve, { opacity: 0, y: 10, scale: 1.04 }, { opacity: 1, y: 0, scale: 1 }, 0.58);
                timeline.to(resolve, { opacity: 0, y: -8, scale: 0.96 }, 0.72);
            }
            if (lockup) {
                timeline.fromTo(lockup, { opacity: 0, y: 12 }, { opacity: 1, y: 0 }, 0.74);
            }
            if (mark) {
                timeline.to(mark, { opacity: 0.12, scale: 0.92 }, 0.74);
            }
            if (action) {
                timeline.fromTo(action, { opacity: 0, y: 8 }, { opacity: 1, y: 0, ease: tokens.ease }, 0.84);
            }
            const trigger = ScrollTrigger.create({
                trigger: section,
                start: "top top",
                end: "+=100%",
                pin: true,
                pinSpacing: true,
                scrub: tokens.scrubControlled,
                animation: timeline,
                anticipatePin: 1,
                invalidateOnRefresh: true
            });
            return () => trigger.kill();
        });

        mm.add("(max-width: 1023px)", () => {
            const timeline = gsap.timeline({
                scrollTrigger: { trigger: section, start: "top 80%", once: true }
            });
            if (lines.length) {
                timeline.from(lines, { opacity: 0.35, y: 10, stagger: 0.08, duration: tokens.emphasis, ease: tokens.ease });
            }
            if (lockup) {
                timeline.fromTo(lockup, { opacity: 0 }, { opacity: 1, duration: tokens.fast }, "-=0.1");
            }
            if (action) {
                timeline.from(action, { opacity: 0, y: 8, duration: tokens.fast }, "-=0.08");
            }
            return () => timeline.scrollTrigger?.kill();
        });
    };

    window.Eterna.motion.use("scroll", (ctx) => ({
        initStatic() {},
        init() {
            initStatement(ctx);
            initHuman(ctx);
            initPillars(ctx);
            initServices(ctx);
            initWhy(ctx);
            initProcess(ctx);
            initTech(ctx);
            initIndustries(ctx);
            initControl(ctx);
            initAbout(ctx);
            initCta(ctx);
        }
    }));
})();
