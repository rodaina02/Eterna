(() => {
    if (!window.Eterna?.motion) {
        return;
    }

    const themes = ["dark", "black", "paper", "lime"];
    const lineScale = {
        hidden: { stroke: 0, cross: 0, branch: 0, node: 0 },
        emerge: { stroke: 0.22, cross: 0, branch: 0, node: 0 },
        flow: { stroke: 0.48, cross: 0.18, branch: 0, node: 0 },
        branch: { stroke: 0.36, cross: 0, branch: 1, node: 0 },
        cross: { stroke: 0.28, cross: 0.78, branch: 0, node: 0 },
        route: { stroke: 0.1, cross: 0.92, branch: 0, node: 0 },
        frame: { stroke: 0.18, cross: 0.42, branch: 0, node: 0 },
        pause: { stroke: 0.58, cross: 0.22, branch: 0, node: 1 },
        resume: { stroke: 0.74, cross: 0.28, branch: 0, node: 0 },
        vanish: { stroke: 0, cross: 0, branch: 0, node: 0 }
    };

    const headerOffset = () => {
        const header = document.querySelector("[data-header-root]");
        return header ? header.offsetHeight : 72;
    };

    const applyHeaderTheme = (theme) => {
        const header = document.querySelector("[data-header-root]");
        if (!header || !themes.includes(theme) || header.dataset.activeTheme === theme) {
            return;
        }

        themes.forEach((name) => {
            header.classList.remove(`site-header--${name}`, `section-${name}`);
        });
        header.classList.add(`site-header--${theme}`, `section-${theme}`);
        header.dataset.activeTheme = theme;

        header.querySelectorAll("[data-header-brand]").forEach((mark) => {
            const active = mark.getAttribute("data-header-brand") === theme;
            mark.classList.toggle("is-active", active);
            mark.alt = active ? "Eterna" : "";
        });

        const overlay = document.querySelector("[data-nav-overlay]");
        if (overlay) {
            const overlayTheme = theme === "dark" ? "black" : theme;
            themes.forEach((name) => {
                overlay.classList.remove(`nav-overlay--${name}`, `section-${name}`);
            });
            overlay.classList.add(`nav-overlay--${overlayTheme}`, `section-${overlayTheme}`);
        }

        const ink = theme === "paper" || theme === "lime";
        const system = document.querySelector("[data-system]");
        if (system) {
            system.classList.toggle("is-ink", ink);
        }
        document.querySelectorAll("[data-chapter]").forEach((el) => {
            el.classList.toggle("is-ink", ink);
        });
    };

    const createSystem = (ctx, { motion }) => {
        const { gsap, tokens } = ctx;
        const header = document.querySelector("[data-header-root]");
        const surfaces = [...document.querySelectorAll("[data-surface][data-chapter-index]")];
        const chapterNow = [...document.querySelectorAll("[data-chapter-now]")];
        const chapterRest = [...document.querySelectorAll("[data-chapter-rest]")];
        const stroke = document.querySelector("[data-system-stroke]");
        const cross = document.querySelector("[data-system-cross]");
        const branches = [...document.querySelectorAll("[data-system-branch]")];
        const node = document.querySelector("[data-system-node]");
        const formA = document.querySelector("[data-geo='a']");
        const formB = document.querySelector("[data-geo='b']");
        const geometry = document.querySelector("[data-system-geometry]");

        let chapter = "";
        let lineState = "hidden";
        let geoScene = "";
        let geometryOwner = "system";
        let lineOwner = "system";
        const disposers = [];

        const tween = (el, vars) => {
            if (!el) {
                return;
            }
            if (motion && gsap) {
                gsap.to(el, { ...vars, overwrite: "auto" });
            } else {
                if (vars.scaleY !== undefined) {
                    el.style.transform = `scaleY(${vars.scaleY})`;
                }
                if (vars.scaleX !== undefined) {
                    el.style.transform = `scaleX(${vars.scaleX})`;
                }
                if (vars.opacity !== undefined) {
                    el.style.opacity = String(vars.opacity);
                }
            }
        };

        const setChapter = (value, ending = false) => {
            if (value === chapter) {
                return;
            }
            chapter = value;
            const next = ending ? "ETERNA" : value;
            const rest = ending ? " / END" : " / 14";

            const apply = () => {
                chapterNow.forEach((el) => {
                    el.textContent = next;
                });
                chapterRest.forEach((el) => {
                    el.textContent = rest;
                });
            };

            if (!motion || !gsap) {
                apply();
                return;
            }

            gsap.timeline()
                .to(chapterNow.concat(chapterRest), {
                    y: -5,
                    opacity: 0,
                    clipPath: "inset(0 0 100% 0)",
                    duration: tokens.chapter * 0.46,
                    ease: tokens.ease
                })
                .add(apply)
                .fromTo(chapterNow.concat(chapterRest), {
                    y: 6,
                    opacity: 0,
                    clipPath: "inset(100% 0 0 0)"
                }, {
                    y: 0,
                    opacity: 1,
                    clipPath: "inset(0 0 0 0)",
                    duration: tokens.chapter * 0.54,
                    ease: tokens.ease
                });

            if (stroke && lineOwner === "system") {
                gsap.fromTo(stroke, { opacity: 0.55 }, {
                    opacity: 1,
                    duration: tokens.chapter,
                    ease: tokens.ease
                });
            }
        };

        const setLine = (state) => {
            if (lineOwner !== "system" || state === lineState) {
                return;
            }
            lineState = state;
            const next = lineScale[state] ?? lineScale.hidden;
            const duration = motion ? tokens.slow : 0;
            const ease = tokens.easeInOut;

            tween(stroke, { scaleY: next.stroke, opacity: next.stroke > 0 ? 1 : 0, duration, ease });
            tween(cross, { scaleX: next.cross, opacity: next.cross > 0 ? 1 : 0, duration, ease });
            branches.forEach((branch, index) => {
                tween(branch, {
                    scaleX: next.branch,
                    opacity: next.branch > 0 ? 0.7 - index * 0.12 : 0,
                    duration,
                    ease
                });
            });
            tween(node, {
                opacity: next.node,
                scale: next.node ? 1 : 0.6,
                duration: motion ? tokens.fast : 0,
                ease: tokens.ease
            });
        };

        const geometryScenes = {
            hidden: { a: { xPercent: -18, yPercent: 8, opacity: 0, scale: 0.92 }, b: { xPercent: 22, yPercent: 14, opacity: 0, scale: 0.92 } },
            split: { a: { xPercent: -36, yPercent: -12, opacity: 0.2, scale: 1.18 }, b: { xPercent: 38, yPercent: 18, opacity: 0.16, scale: 1.05 } },
            statement: { a: { xPercent: -14, yPercent: 2, opacity: 0.18, scale: 1.08 }, b: { xPercent: 16, yPercent: 10, opacity: 0.14, scale: 1 } },
            human: { a: { xPercent: -10, yPercent: -2, opacity: 0.42, scale: 1.32 }, b: { xPercent: 12, yPercent: 6, opacity: 0.36, scale: 1.18 } },
            echo: { a: { xPercent: -24, yPercent: 10, opacity: 0.08, scale: 0.86 }, b: { xPercent: 22, yPercent: 14, opacity: 0.07, scale: 0.86 } },
            cta: { a: { xPercent: -4, yPercent: 2, opacity: 0.16, scale: 0.96 }, b: { xPercent: 6, yPercent: 6, opacity: 0.14, scale: 0.96 } }
        };

        const setGeometry = (scene) => {
            if (geometryOwner !== "system" || !formA || !formB || scene === geoScene) {
                return;
            }
            geoScene = scene;
            const next = geometryScenes[scene] || geometryScenes.hidden;
            const apply = (el, vars) => {
                if (motion && gsap) {
                    gsap.to(el, { ...vars, duration: tokens.section, ease: tokens.easeInOut, overwrite: "auto" });
                }
            };
            apply(formA, next.a);
            apply(formB, next.b);
            if (geometry) {
                geometry.dataset.scene = scene;
            }
        };

        const applyLocation = (section) => {
            if (!section) {
                return;
            }
            applyHeaderTheme(section.dataset.surface);
            setChapter(section.dataset.chapterIndex, section.dataset.chapterEnd === "true");
            if (lineOwner === "system") {
                setLine(section.dataset.lineState || "flow");
            }
            if (geometryOwner === "system") {
                setGeometry(section.dataset.geoScene || "echo");
            }
        };

        applyHeaderTheme(header?.dataset.headerTheme || "black");
        setChapter("01");
        setLine("hidden");

        if (motion && gsap && ctx.ScrollTrigger && surfaces.length) {
            let cache = [];
            const measure = () => {
                cache = surfaces.map((section) => {
                    const box = section.parentElement?.classList.contains("pin-spacer")
                        ? section.parentElement
                        : section;
                    const top = box.getBoundingClientRect().top + window.scrollY;
                    return { section, top, bottom: top + box.offsetHeight };
                });
            };
            const sync = () => {
                const line = window.scrollY + headerOffset();
                for (let index = cache.length - 1; index >= 0; index -= 1) {
                    const item = cache[index];
                    if (line >= item.top && line < item.bottom) {
                        applyLocation(item.section);
                        return;
                    }
                }
            };
            ctx.ScrollTrigger.addEventListener("refreshInit", measure);
            measure();
            const trigger = ctx.ScrollTrigger.create({
                start: 0,
                end: "max",
                onRefresh: sync,
                onUpdate: sync
            });
            const compact = ctx.ScrollTrigger.create({
                start: 48,
                onToggle: (self) => header?.classList.toggle("is-compact", self.isActive)
            });
            disposers.push(() => {
                ctx.ScrollTrigger.removeEventListener("refreshInit", measure);
                trigger.kill();
                compact.kill();
            });
        } else if (surfaces.length) {
            const ratios = new Map();
            const sync = () => {
                let next = surfaces[0];
                let best = -1;
                surfaces.forEach((section) => {
                    const ratio = ratios.get(section) ?? 0;
                    if (ratio > best) {
                        best = ratio;
                        next = section;
                    }
                });
                applyLocation(next);
            };
            const band = Math.max(48, headerOffset());
            const rest = Math.max(80, window.innerHeight - band - 96);
            const io = new IntersectionObserver((entries) => {
                entries.forEach((entry) => {
                    ratios.set(entry.target, entry.isIntersecting ? entry.intersectionRatio : 0);
                });
                sync();
            }, { root: null, threshold: [0, 0.25, 0.5, 1], rootMargin: `-${band}px 0px -${rest}px 0px` });
            surfaces.forEach((section) => io.observe(section));
            disposers.push(() => io.disconnect());
            if (header) {
                const onScroll = () => header.classList.toggle("is-compact", window.scrollY > 48);
                window.addEventListener("scroll", onScroll, { passive: true });
                onScroll();
                disposers.push(() => window.removeEventListener("scroll", onScroll));
            }
        }

        window.Eterna.system = {
            headerOffset,
            forms: () => ({ a: formA, b: formB }),
            line: () => ({ stroke, cross, branches, node }),
            claimGeometry() {
                geometryOwner = "section";
            },
            releaseGeometry(scene) {
                geometryOwner = "system";
                geoScene = "";
                setGeometry(scene || "echo");
            },
            claimLine() {
                lineOwner = "section";
            },
            releaseLine(state) {
                lineOwner = "system";
                lineState = "";
                setLine(state || "flow");
            },
            setGeometry,
            setLine,
            setChapter
        };

        return () => {
            disposers.forEach((dispose) => dispose());
            header?.classList.remove("is-compact");
            window.Eterna.system = null;
        };
    };

    window.Eterna.motion.use("system", (ctx) => ({
        initStatic() {
            return createSystem(ctx, { motion: false });
        },
        init() {
            return createSystem(ctx, { motion: true });
        }
    }));
})();
