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

        gsap.fromTo(section, { yPercent: 8 }, {
            yPercent: 0,
            ease: tokens.easeNone,
            scrollTrigger: {
                trigger: section,
                start: "top 98%",
                end: "top 52%",
                scrub: tokens.scrubControlled
            }
        });

        const timeline = gsap.timeline({
            defaults: { ease: tokens.easeNone },
            scrollTrigger: {
                trigger: section,
                start: "top 86%",
                end: "top 18%",
                scrub: tokens.scrubControlled
            }
        });
        if (beats[0]) {
            timeline.fromTo(beats[0], { opacity: 0.5, y: 16 }, { opacity: 0.86, y: 0 }, 0);
        }
        if (beats[1]) {
            timeline.fromTo(beats[1], { opacity: 0.46, y: 18 }, { opacity: 1, y: 0 }, 0.18);
        }
        if (beats[2]) {
            timeline.fromTo(beats[2], { opacity: 0.46, y: 16 }, { opacity: 1, y: 0 }, 0.36);
        }
        if (note) {
            timeline.fromTo(note, { opacity: 0.62, y: 8 }, { opacity: 1, y: 0 }, 0.52);
        }
    };

    const initHuman = (ctx) => {
        const { gsap, tokens, mm, ScrollTrigger, bindActive } = ctx;
        const section = document.querySelector("[data-human]");
        if (!section) {
            return;
        }
        const title = section.querySelector(".section-header__title");
        const items = gsap.utils.toArray(section.querySelectorAll("[data-human-item]"));
        const image = section.querySelector("[data-human-image]");
        const frame = section.querySelector("[data-human-frame]");
        const marker = section.querySelector("[data-human-marker]");
        const forms = window.Eterna.system?.forms() || {};

        mm.add("(min-width: 1024px)", () => {
            window.Eterna.system?.claimGeometry();
            const timeline = gsap.timeline({ defaults: { ease: tokens.easeNone } });
            let trigger = null;

            const active = bindActive(items, {
                onChange(index, source) {
                    if (source !== "hover" || !timeline || items.length < 2) {
                        return;
                    }
                    gsap.to(timeline, {
                        progress: index / (items.length - 1),
                        duration: 0.42,
                        ease: tokens.ease,
                        overwrite: "auto"
                    });
                },
                onHoverEnd() {
                    if (timeline && trigger) {
                        gsap.to(timeline, {
                            progress: trigger.progress,
                            duration: 0.32,
                            ease: tokens.ease,
                            overwrite: "auto"
                        });
                    }
                }
            });

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
                timeline.fromTo(title, { opacity: 0.72, y: 12 }, { opacity: 1, y: 0 }, 0.18);
            }
            if (marker && items.length) {
                timeline.fromTo(marker, { y: 0 }, {
                    y: () => {
                        const last = items[items.length - 1];
                        const first = items[0];
                        return Math.max(0, last.offsetTop - first.offsetTop);
                    },
                    duration: 1,
                    ease: tokens.easeNone
                }, 0);
            }
            if (frame) {
                timeline.to(frame, { xPercent: -22, clipPath: "inset(8% 28% 12% 4%)", opacity: 0.78 }, 0.86);
            }
            if (forms.a && forms.b) {
                timeline.to(forms.a, { xPercent: -16, yPercent: -4, opacity: 0.22, scale: 1.1 }, 0.9);
                timeline.to(forms.b, { xPercent: 18, yPercent: 10, opacity: 0.18, scale: 1.05 }, 0.9);
            }

            trigger = ScrollTrigger.create({
                trigger: section,
                start: pinStart,
                end: () => `+=${Math.round(window.innerHeight * 1.16)}`,
                pin: true,
                pinSpacing: true,
                scrub: tokens.scrubCinematic,
                animation: timeline,
                anticipatePin: 1,
                invalidateOnRefresh: true,
                onUpdate: (self) => {
                    if (!items.length) {
                        return;
                    }
                    active.setScroll(Math.min(items.length - 1, Math.floor(self.progress * items.length)));
                    if (active.hover != null && items.length > 1) {
                        timeline.progress(active.hover / (items.length - 1));
                    }
                },
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
            const active = items.length ? bindActive(items) : null;
            const timeline = gsap.timeline({
                defaults: { ease: tokens.easeNone },
                scrollTrigger: {
                    trigger: section,
                    start: "top 72%",
                    end: "bottom 32%",
                    scrub: tokens.scrubControlled,
                    onUpdate: (self) => {
                        if (active && items.length) {
                            active.setScroll(Math.min(items.length - 1, Math.floor(self.progress * items.length)));
                        }
                    }
                }
            });
            if (frame) {
                timeline.fromTo(frame, { clipPath: "inset(12% 22% 12% 8%)" }, { clipPath: "inset(0% 0% 0% 0%)" }, 0);
            }
            if (title) {
                timeline.fromTo(title, { opacity: 0.7, y: 10 }, { opacity: 1, y: 0 }, 0.12);
            }
            return () => timeline.scrollTrigger?.kill();
        });

        mm.add("(max-width: 767px)", () => {
            const active = items.length ? bindActive(items, { noHover: true }) : null;
            const timeline = gsap.timeline({
                defaults: { ease: tokens.ease },
                scrollTrigger: {
                    trigger: section,
                    start: "top 70%",
                    end: "bottom 42%",
                    scrub: tokens.scrubFast,
                    onUpdate: (self) => {
                        if (active && items.length) {
                            active.setScroll(Math.min(items.length - 1, Math.floor(self.progress * items.length)));
                        }
                    }
                }
            });
            if (frame) {
                timeline.fromTo(frame, { clipPath: "inset(10% 16% 10% 8%)" }, { clipPath: "inset(0% 0% 0% 0%)" }, 0);
            }
            if (title) {
                timeline.fromTo(title, { opacity: 0.7, y: 8 }, { opacity: 1, y: 0 }, 0.1);
            }
            if (marker && items.length > 1) {
                timeline.fromTo(marker, { y: 0 }, {
                    y: () => Math.max(0, items[items.length - 1].offsetTop - items[0].offsetTop),
                    duration: 1,
                    ease: tokens.easeNone
                }, 0);
            }
            return () => timeline.scrollTrigger?.kill();
        });
    };

    const initPillars = (ctx) => {
        const { gsap, tokens, mm, ScrollTrigger, bindActive } = ctx;
        const section = document.querySelector("[data-pillars]");
        if (!section) {
            return;
        }
        const origin = section.querySelector("[data-pillars-origin]");
        const streams = section.querySelectorAll("[data-pillars-stream]");
        const branches = section.querySelectorAll("[data-pillar-branch]");
        const pillars = gsap.utils.toArray(section.querySelectorAll("[data-pillar]"));

        gsap.fromTo(section, { yPercent: 14 }, {
            yPercent: 0,
            ease: tokens.easeNone,
            scrollTrigger: {
                trigger: section,
                start: "top 98%",
                end: "top 42%",
                scrub: tokens.scrubCinematic
            }
        });

        mm.add("(min-width: 1024px)", () => {
            const active = bindActive(pillars);
            const timeline = gsap.timeline({ defaults: { ease: tokens.easeInOut, duration: 0.2 } });
            if (origin) {
                timeline.fromTo(origin, { opacity: 0.4, y: 12 }, { opacity: 1, y: 0 }, 0);
                timeline.to(origin, { opacity: 0.42, y: -8 }, 0.14);
            }
            if (streams.length) {
                timeline.fromTo(streams, { opacity: 0.4, y: 10 }, { opacity: 0.78, y: 0, stagger: 0.05 }, 0.14);
            }
            if (branches.length) {
                timeline.fromTo(branches, { scaleX: 0 }, { scaleX: 1, stagger: 0.04, ease: tokens.easeNone }, 0.18);
            }
            const trigger = ScrollTrigger.create({
                trigger: section,
                start: pinStart,
                end: () => `+=${Math.round(window.innerHeight * 1.2)}`,
                pin: true,
                pinSpacing: true,
                scrub: tokens.scrubCinematic,
                animation: timeline,
                anticipatePin: 1,
                invalidateOnRefresh: true,
                onUpdate: (self) => {
                    if (pillars.length) {
                        active.setScroll(Math.min(pillars.length - 1, Math.floor(self.progress * pillars.length)));
                    }
                }
            });

            return () => {
                trigger.kill();
            };
        });

        mm.add("(max-width: 1023px)", () => {
            bindActive(pillars);
            return () => {};
        });
    };

    const initServices = (ctx) => {
        const { gsap, tokens, bindActive } = ctx;
        const section = document.querySelector("[data-services]");
        if (!section) {
            return;
        }
        const artifact = section.querySelector("[data-services-artifact]");
        const rule = section.querySelector("[data-services-rule]");
        const groups = gsap.utils.toArray(section.querySelectorAll("[data-service-group]"));
        const title = section.querySelector(".section-header__title");
        const active = groups.length ? bindActive(groups) : null;
        const timeline = gsap.timeline({
            defaults: { ease: tokens.easeNone },
            scrollTrigger: {
                trigger: section,
                start: "top 88%",
                end: "bottom 36%",
                scrub: tokens.scrubControlled,
                onUpdate: (self) => {
                    if (active && groups.length) {
                        active.setScroll(Math.min(groups.length - 1, Math.floor(self.progress * groups.length)));
                    }
                }
            }
        });
        if (title) {
            timeline.fromTo(title, { opacity: 0.86, y: 10 }, { opacity: 1, y: 0 }, 0);
        }
        if (artifact) {
            timeline.fromTo(artifact, { y: 28, opacity: 0.72, rotation: -0.6 }, { y: 0, opacity: 1, rotation: -0.6 }, 0.06);
            timeline.to(artifact, { xPercent: -8, y: -6 }, 0.48);
        }
        if (rule) {
            timeline.fromTo(rule, { scaleX: 0 }, { scaleX: 1 }, 0.16);
        }
        groups.forEach((group, index) => {
            const arch = group.querySelector("[data-service-arch]");
            const body = group.querySelector(".service-group, .arrow-link");
            timeline.fromTo(group, { y: 12, opacity: 0.78 }, { y: 0, opacity: 1 }, 0.28 + index * 0.16);
            if (arch) {
                timeline.fromTo(arch.children, { opacity: 0.58, y: 8 }, { opacity: 0.78, y: 0, stagger: 0.03 }, 0.3 + index * 0.16);
            }
            if (body) {
                timeline.fromTo(body, { opacity: 0.78 }, { opacity: 1 }, 0.36 + index * 0.16);
            }
        });
        if (rule) {
            timeline.to(rule, { scaleX: 0.22, xPercent: -12 }, 0.88);
        }
        if (artifact) {
            timeline.to(artifact, { y: -12, opacity: 0.86 }, 0.9);
        }
    };

    const initWhy = (ctx) => {
        const { mm, ScrollTrigger, bindActive, tokens } = ctx;
        const section = document.querySelector("[data-why]");
        if (!section) {
            return;
        }
        const items = [...section.querySelectorAll("[data-why-item]")];
        if (!items.length) {
            return;
        }

        mm.add("(min-width: 768px)", () => {
            const active = bindActive(items);
            const trigger = ScrollTrigger.create({
                trigger: section,
                start: "top 46%",
                end: () => `+=${Math.round(window.innerHeight * 0.72)}`,
                scrub: tokens.scrubSnap,
                onUpdate: (self) => active.setScroll(Math.min(items.length - 1, Math.floor(self.progress * items.length)))
            });
            return () => trigger.kill();
        });

        mm.add("(max-width: 767px)", () => {
            const active = bindActive(items, { noHover: true });
            const trigger = ScrollTrigger.create({
                trigger: section,
                start: "top 62%",
                end: () => `+=${Math.round(window.innerHeight * 0.85)}`,
                scrub: tokens.scrubSnap,
                onUpdate: (self) => active.setScroll(Math.min(items.length - 1, Math.floor(self.progress * items.length)))
            });
            return () => trigger.kill();
        });
    };

    const initProcess = (ctx) => {
        const { gsap, tokens, mm, ScrollTrigger, bindActive } = ctx;
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
            const active = bindActive(stages);
            const timeline = gsap.timeline({ defaults: { ease: tokens.easeInOut, duration: 0.16 } });
            if (spine) {
                timeline.fromTo(spine, { scaleX: 0 }, { scaleX: 1, ease: tokens.easeNone, duration: 1 }, 0);
            }
            nodes.forEach((node, index) => {
                timeline.to(node, { opacity: 1 }, 0.12 + index * 0.14);
            });
            const trigger = ScrollTrigger.create({
                trigger: section,
                start: pinStart,
                end: () => `+=${Math.round(window.innerHeight * 1.32)}`,
                pin: true,
                pinSpacing: true,
                scrub: tokens.scrubCinematic,
                animation: timeline,
                anticipatePin: 1,
                invalidateOnRefresh: true,
                onUpdate: (self) => {
                    const count = evolve ? stages.length : official.length;
                    active.setScroll(Math.min(count - 1, Math.floor(self.progress * count)));
                }
            });
            return () => trigger.kill();
        });

        mm.add("(max-width: 1023px)", () => {
            bindActive(stages);
            return () => {};
        });
    };

    const initTech = (ctx) => {
        const { gsap, tokens, mm, ScrollTrigger, bindActive } = ctx;
        const section = document.querySelector("[data-tech]");
        if (!section) {
            return;
        }
        const clusters = [...section.querySelectorAll("[data-tech-cluster]")];
        const panels = [...section.querySelectorAll("[data-tech-panel]")];
        const now = section.querySelector("[data-tech-now]");
        const core = section.querySelector("[data-tech-core]");
        if (!clusters.length) {
            return;
        }

        const sync = (index) => {
            panels.forEach((panel, i) => panel.classList.toggle("is-active", i === index));
            clusters.forEach((item, i) => {
                item.classList.toggle("is-active", i === index);
                if (i === index) {
                    item.setAttribute("aria-current", "true");
                } else {
                    item.removeAttribute("aria-current");
                }
            });
            if (now) {
                now.textContent = String(index + 1).padStart(2, "0");
            }
        };

        const bindClusterHover = (active) => {
            const nav = section.querySelector("[data-tech-nav]");
            if (!nav) {
                return () => {};
            }

            const onOver = (event) => {
                if (event.pointerType === "touch") {
                    return;
                }
                const item = event.target.closest("[data-tech-cluster]");
                if (!item || !nav.contains(item)) {
                    return;
                }
                const index = clusters.indexOf(item);
                if (index < 0) {
                    return;
                }
                active.setHover(index);
            };
            const onLeave = (event) => {
                if (event.relatedTarget && nav.contains(event.relatedTarget)) {
                    return;
                }
                active.clearHover();
            };
            const onFocus = (event) => {
                const item = event.target.closest("[data-tech-cluster]");
                if (!item) {
                    return;
                }
                const index = clusters.indexOf(item);
                if (index >= 0) {
                    active.setHover(index);
                }
            };
            const onBlur = (event) => {
                if (event.relatedTarget && nav.contains(event.relatedTarget)) {
                    return;
                }
                active.clearHover();
            };

            nav.addEventListener("pointerover", onOver);
            nav.addEventListener("pointerleave", onLeave);
            nav.addEventListener("focusin", onFocus);
            nav.addEventListener("focusout", onBlur);
            clusters.forEach((item) => {
                if (!item.hasAttribute("tabindex")) {
                    item.setAttribute("tabindex", "0");
                }
            });

            return () => {
                nav.removeEventListener("pointerover", onOver);
                nav.removeEventListener("pointerleave", onLeave);
                nav.removeEventListener("focusin", onFocus);
                nav.removeEventListener("focusout", onBlur);
            };
        };

        mm.add("(min-width: 1024px)", () => {
            const active = bindActive(clusters, { onChange: sync, noHover: true });
            sync(0);
            const unhover = bindClusterHover(active);
            const timeline = gsap.timeline({ defaults: { ease: tokens.easeNone } });
            clusters.forEach((_, index) => {
                timeline.to({}, { duration: 0.12 }, index * 0.12);
            });
            if (core) {
                timeline.fromTo(core, { scale: 0.45, opacity: 0.2 }, { scale: 1, opacity: 1, duration: 0.18 }, 0.28);
                timeline.to(core, { scale: 0.82, opacity: 0.55, duration: 0.16 }, 0.86);
            }
            const trigger = ScrollTrigger.create({
                trigger: section,
                start: pinStart,
                end: () => `+=${Math.round(window.innerHeight * 0.58)}`,
                pin: true,
                pinSpacing: true,
                scrub: tokens.scrubTech,
                animation: timeline,
                anticipatePin: 1,
                invalidateOnRefresh: true,
                onUpdate: (self) => {
                    const index = Math.min(clusters.length - 1, Math.floor(self.progress * clusters.length));
                    active.setScroll(index);
                }
            });
            return () => {
                unhover();
                trigger.kill();
            };
        });

        mm.add("(max-width: 1023px)", () => {
            const active = bindActive(clusters, { onChange: sync, noHover: true });
            sync(0);
            const unhover = bindClusterHover(active);
            const trigger = ScrollTrigger.create({
                trigger: section,
                start: "top 56%",
                end: () => `+=${Math.round(window.innerHeight * 0.7)}`,
                scrub: tokens.scrubTech,
                onUpdate: (self) => {
                    active.setScroll(Math.min(clusters.length - 1, Math.floor(self.progress * clusters.length)));
                }
            });
            return () => {
                unhover();
                trigger.kill();
            };
        });
    };

    const initIndustries = (ctx) => {
        const { gsap, tokens, ScrollTrigger, bindActive } = ctx;
        const section = document.querySelector("[data-industries]");
        if (!section) {
            return;
        }
        const items = [...section.querySelectorAll("[data-industry]")];
        const rule = section.querySelector("[data-industries-rule]");
        const veil = section.querySelector("[data-industries-veil]");
        const lede = section.querySelector(".lede");
        const title = section.querySelector(".section-header__title");
        const active = bindActive(items);

        gsap.fromTo(section, { yPercent: 6 }, {
            yPercent: 0,
            ease: tokens.easeNone,
            scrollTrigger: {
                trigger: section,
                start: "top 92%",
                end: "top 50%",
                scrub: tokens.scrubFast
            }
        });

        if (rule) {
            gsap.fromTo(rule, { scaleX: 0 }, {
                scaleX: 1,
                ease: tokens.easeNone,
                scrollTrigger: {
                    trigger: section,
                    start: "top 72%",
                    end: "top 42%",
                    scrub: tokens.scrubFast
                }
            });
        }

        ScrollTrigger.create({
            trigger: section,
            start: "top 52%",
            end: "bottom 38%",
            scrub: tokens.scrubFast,
            onUpdate: (self) => {
                if (items.length) {
                    active.setScroll(Math.min(items.length - 1, Math.floor(self.progress * items.length)));
                }
            }
        });

        const exit = gsap.timeline({
            defaults: { ease: tokens.easeNone },
            scrollTrigger: {
                trigger: section,
                start: "bottom 30%",
                end: "bottom top",
                scrub: tokens.scrubControlled
            }
        });
        if (title) {
            exit.to(title, { opacity: 0.55, y: -10, scale: 0.99 }, 0);
        }
        if (lede) {
            exit.to(lede, { opacity: 0.5, y: -8 }, 0.04);
        }
        if (items.length) {
            exit.to(items, { opacity: 0.55, y: -8, stagger: 0.01 }, 0.06);
        }
        if (rule) {
            exit.to(rule, { scaleX: 0.55, backgroundColor: "var(--e-lime)" }, 0.16);
        }
        if (veil) {
            exit.fromTo(veil, { scaleY: 0 }, { scaleY: 1 }, 0.22);
        }
    };

    const initControl = (ctx) => {
        const { gsap, tokens, mm, ScrollTrigger } = ctx;
        const section = document.querySelector("[data-control]");
        if (!section) {
            return;
        }
        const track = section.querySelector("[data-control-track]") || section;
        const stage = section.querySelector("[data-control-stage]") || section;
        const lines = section.querySelectorAll("[data-control-line]");
        const lede = section.querySelector("[data-control-lede]");
        const principles = section.querySelectorAll("[data-control-primary] li");
        const support = section.querySelector("[data-control-support]");
        const inner = stage.querySelector(".container");
        const line = window.Eterna.system?.line() || {};

        const setState = (name) => {
            section.dataset.sceneState = name;
        };

        const buildTimeline = (withSurface) => {
            const timeline = gsap.timeline({ defaults: { ease: tokens.easeNone } });
            window.Eterna.system?.claimLine();

            if (withSurface && inner) {
                timeline.fromTo(inner, { y: 20, opacity: 0.92 }, { y: 0, opacity: 1, duration: 0.1 }, 0);
            }
            if (line.stroke) {
                timeline.fromTo(line.stroke, {
                    scaleY: 0.28,
                    opacity: 0.5
                }, {
                    scaleY: 0.62,
                    opacity: 1,
                    duration: 0.1,
                    immediateRender: false
                }, 0.08);
            }
            if (line.node) {
                timeline.fromTo(line.node, { opacity: 0, scale: 0.6 }, { opacity: 1, scale: 1, duration: 0.08, immediateRender: false }, 0.14);
            }
            if (lines[0]) {
                timeline.fromTo(lines[0], { opacity: 0, y: 18 }, { opacity: 1, y: 0, duration: 0.08 }, 0.22);
            }
            if (lines[1]) {
                timeline.fromTo(lines[1], { opacity: 0, y: 20 }, { opacity: 1, y: 0, duration: 0.08 }, 0.32);
            }
            if (lede) {
                timeline.fromTo(lede, { opacity: 0, y: 12 }, { opacity: 1, y: 0, duration: 0.08 }, 0.42);
            }
            if (principles.length) {
                timeline.fromTo(principles, { opacity: 0.22, y: 10 }, { opacity: 1, y: 0, stagger: 0.03, duration: 0.1 }, 0.52);
            }
            if (support) {
                timeline.fromTo(support, { opacity: 0.22, y: 8 }, { opacity: 1, y: 0, duration: 0.08 }, 0.66);
            }
            if (line.stroke) {
                timeline.to(line.stroke, { scaleY: 0.62, opacity: 1, duration: 0.12 }, 0.76);
            }
            if (line.node) {
                timeline.to(line.node, { opacity: 1, scale: 1, duration: 0.12 }, 0.76);
            }
            if (line.stroke) {
                timeline.to(line.stroke, { scaleY: 0.82, opacity: 1, duration: 0.1 }, 0.9);
            }
            if (line.node) {
                timeline.to(line.node, { opacity: 0.9, scale: 1, duration: 0.1 }, 0.9);
            }

            return timeline;
        };

        const bindState = (self) => {
            if (self.progress < 0.14) {
                setState("GOVERNANCE_ENTER");
            } else if (self.progress < 0.84) {
                setState("GOVERNANCE_ACTIVE");
            } else {
                setState("GOVERNANCE_EXIT");
            }
        };

        mm.add("(min-width: 1024px)", () => {
            const timeline = buildTimeline(true);
            const trigger = ScrollTrigger.create({
                trigger: track,
                start: pinStart,
                endTrigger: document.querySelector("[data-about]") || track,
                end: "top 24%",
                pin: stage,
                pinSpacing: false,
                scrub: tokens.scrubControlled,
                animation: timeline,
                anticipatePin: 0,
                invalidateOnRefresh: true,
                onEnter: () => {
                    window.Eterna.system?.claimLine();
                    setState("GOVERNANCE_ENTER");
                },
                onEnterBack: () => {
                    window.Eterna.system?.claimLine();
                    setState("GOVERNANCE_ACTIVE");
                },
                onLeave: () => {
                    window.Eterna.system?.releaseLine("resume");
                    setState("GOVERNANCE_EXIT");
                },
                onLeaveBack: () => {
                    window.Eterna.system?.releaseLine("cross");
                    setState("CONTEXT");
                },
                onUpdate: bindState
            });
            return () => {
                trigger.kill();
                window.Eterna.system?.releaseLine("flow");
            };
        });

        mm.add("(max-width: 1023px)", () => {
            const timeline = buildTimeline(false);
            const trigger = ScrollTrigger.create({
                trigger: section,
                start: "top 78%",
                end: "bottom 24%",
                pin: false,
                scrub: tokens.scrubControlled,
                animation: timeline,
                invalidateOnRefresh: true,
                onEnter: () => window.Eterna.system?.claimLine(),
                onEnterBack: () => window.Eterna.system?.claimLine(),
                onLeave: () => window.Eterna.system?.releaseLine("resume"),
                onLeaveBack: () => window.Eterna.system?.releaseLine("cross"),
                onUpdate: bindState
            });
            return () => {
                trigger.kill();
                window.Eterna.system?.releaseLine("flow");
            };
        });
    };

    const initAbout = (ctx) => {
        const { gsap, tokens, mm } = ctx;
        const section = document.querySelector("[data-about]");
        if (!section) {
            return;
        }
        const scene = section.querySelector("[data-about-scene]");
        const photo = section.querySelector("[data-about-photo]");
        const lockup = section.querySelector("[data-about-lockup]");
        const copy = section.querySelector("[data-about-copy]");
        const title = section.querySelector(".section-header__title");
        const link = section.querySelector(".arrow-link, .text-link");
        const veil = section.querySelector("[data-about-veil]");
        const cta = document.querySelector("[data-cta]");

        const play = (start, end, endTrigger) => {
            const timeline = gsap.timeline({
                defaults: { ease: tokens.easeNone },
                scrollTrigger: {
                    trigger: section,
                    start,
                    end,
                    endTrigger: endTrigger || section,
                    scrub: tokens.scrubControlled,
                    invalidateOnRefresh: true,
                    onUpdate: (self) => {
                        if (self.progress < 0.18) {
                            section.dataset.sceneState = "STUDIO_ENTER";
                        } else if (self.progress < 0.7) {
                            section.dataset.sceneState = "STUDIO_ACTIVE";
                        } else {
                            section.dataset.sceneState = "STUDIO_EXIT";
                        }
                    }
                }
            });
            if (scene) {
                timeline.fromTo(scene, { opacity: 0.42, y: 16, scale: 1.02 }, { opacity: 1, y: 0, scale: 1 }, 0);
            }
            if (photo) {
                timeline.fromTo(photo, {
                    scale: 1.05,
                    opacity: 0.5
                }, {
                    scale: 1,
                    opacity: 1
                }, 0.03);
            }
            if (title) {
                timeline.fromTo(title, { opacity: 0, y: 12 }, { opacity: 1, y: 0 }, 0.08);
            }
            if (copy) {
                timeline.fromTo(copy, { opacity: 0, y: 10 }, { opacity: 1, y: 0 }, 0.12);
            }
            if (lockup) {
                timeline.fromTo(lockup, { opacity: 0.55, scale: 0.97 }, { opacity: 1, scale: 1 }, 0.14);
            }
            if (link) {
                timeline.fromTo(link, { opacity: 0.7, y: 6 }, { opacity: 1, y: 0 }, 0.16);
            }
            if (title) {
                timeline.to(title, { opacity: 1, y: 0 }, 0.62);
            }
            if (copy) {
                timeline.to(copy, { opacity: 1, y: 0 }, 0.62);
            }
            if (lockup) {
                timeline.to(lockup, { scale: 1.14, opacity: 1 }, 0.7);
            }
            if (photo) {
                timeline.to(photo, { scale: 0.94, yPercent: -6, opacity: 0.92 }, 0.74);
            }
            if (scene) {
                timeline.to(scene, { scale: 0.96, yPercent: -8 }, 0.76);
            }
            if (title) {
                timeline.to(title, { opacity: 0.82, y: -8 }, 0.8);
            }
            if (copy) {
                timeline.to(copy, { opacity: 0.78, y: -8 }, 0.82);
            }
            if (link) {
                timeline.to(link, { opacity: 0.62, y: -6 }, 0.84);
            }
            if (veil) {
                timeline.fromTo(veil, { scaleY: 0 }, { scaleY: 1 }, 0.78);
            }
            return () => timeline.scrollTrigger?.kill();
        };

        mm.add("(min-width: 1024px)", () => play("top 24%", pinStart, cta));
        mm.add("(max-width: 1023px)", () => play("top 78%", "bottom 22%"));
    };

    const initCta = (ctx) => {
        const { gsap, tokens, mm, ScrollTrigger } = ctx;
        const section = document.querySelector("[data-cta]");
        const contact = section?.querySelector("[data-contact]") || document.querySelector("[data-contact]");
        if (!section) {
            return;
        }
        const track = section.querySelector("[data-cta-track]") || section;
        const canvas = section.querySelector("[data-cta-scene]") || section;
        const lines = section.querySelectorAll("[data-cta-line]");
        const resolve = section.querySelector("[data-cta-resolve]");
        const lockup = section.querySelector("[data-cta-lockup]");
        const mark = section.querySelector("[data-cta-mark]");
        const title = section.querySelector("[data-cta-title]");
        const panel = contact?.querySelector("[data-contact-panel]");
        const form = contact?.querySelector("[data-contact-form]");

        const setState = (name) => {
            section.dataset.sceneState = name;
        };

        mm.add("(min-width: 1024px)", () => {
            const timeline = gsap.timeline({ defaults: { ease: tokens.easeNone } });
            if (mark) {
                timeline.fromTo(mark, { scale: 1.28, opacity: 0.22 }, { scale: 1, opacity: 1 }, 0);
            }
            lines.forEach((line, index) => {
                timeline.fromTo(line, { yPercent: 110 }, { yPercent: 0, duration: 0.1 }, 0.08 + index * 0.1);
            });
            if (lines.length) {
                timeline.to(lines, { yPercent: 0 }, 0.38);
            }
            if (mark) {
                timeline.to(mark, { scale: 0.72, opacity: 0, y: -20 }, 0.42);
            }
            if (title) {
                timeline.to(title, { scale: 0.78, y: -28, opacity: 0 }, 0.44);
            }
            if (resolve) {
                timeline.fromTo(resolve, { opacity: 0, y: 18 }, { opacity: 1, y: 0 }, 0.52);
            }
            if (resolve) {
                timeline.to(resolve, { opacity: 0, y: -14 }, 0.68);
            }
            if (lockup) {
                timeline.fromTo(lockup, { opacity: 0, y: 20, scale: 0.92 }, { opacity: 1, y: 0, scale: 1 }, 0.66);
            }
            if (lockup) {
                timeline.to(lockup, { scale: 1.04, opacity: 1 }, 0.82);
            }
            timeline.to({}, {}, 0.98);

            const trigger = ScrollTrigger.create({
                trigger: track,
                start: pinStart,
                end: "bottom top",
                pin: canvas,
                pinSpacing: false,
                scrub: tokens.scrubCinematic,
                animation: timeline,
                anticipatePin: 0,
                invalidateOnRefresh: true,
                onUpdate: (self) => {
                    if (self.progress < 0.12) {
                        setState("CTA_ENTER");
                    } else if (self.progress < 0.62) {
                        setState("CTA_LEGACY");
                    } else {
                        setState("CTA_RESOLUTION");
                    }
                }
            });
            return () => trigger.kill();
        });

        mm.add("(max-width: 1023px)", () => {
            const timeline = gsap.timeline({
                defaults: { ease: tokens.easeNone },
                scrollTrigger: {
                    trigger: track,
                    start: "top 82%",
                    end: "bottom 28%",
                    scrub: tokens.scrubControlled
                }
            });
            if (mark) {
                timeline.fromTo(mark, { scale: 1.12, opacity: 0.4 }, { scale: 1, opacity: 1 }, 0);
            }
            lines.forEach((line, index) => {
                timeline.fromTo(line, { yPercent: 108 }, { yPercent: 0 }, 0.06 + index * 0.12);
            });
            if (mark) {
                timeline.to(mark, { opacity: 0, y: -10 }, 0.38);
            }
            if (title) {
                timeline.to(title, { opacity: 0, y: -12 }, 0.4);
            }
            if (resolve) {
                timeline.fromTo(resolve, { opacity: 0, y: 10 }, { opacity: 1, y: 0 }, 0.46);
            }
            if (resolve) {
                timeline.to(resolve, { opacity: 0, y: -8 }, 0.62);
            }
            if (lockup) {
                timeline.fromTo(lockup, { opacity: 0, y: 12 }, { opacity: 1, y: 0 }, 0.6);
            }
            return () => timeline.scrollTrigger?.kill();
        });

        if (contact && panel) {
            const rise = gsap.timeline({
                defaults: { ease: tokens.easeNone },
                scrollTrigger: {
                    trigger: contact,
                    start: "top 94%",
                    end: "top 68%",
                    scrub: tokens.scrubControlled,
                    invalidateOnRefresh: true,
                    onEnter: () => {
                        contact.dataset.sceneState = "CONTACT";
                    }
                }
            });
            rise.fromTo(panel, { y: 32 }, { y: 0 }, 0);
            if (form) {
                rise.fromTo(form, { y: 14, opacity: 0.9 }, { y: 0, opacity: 1 }, 0.08);
            }
        }
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
