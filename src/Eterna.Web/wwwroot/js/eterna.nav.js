(() => {
    const trigger = document.querySelector("[data-nav-trigger]");
    const overlay = document.querySelector("[data-nav-overlay]");
    const main = document.getElementById("main");
    const footer = document.querySelector(".site-footer");

    if (!trigger || !overlay) {
        return;
    }

    const focusableSelector = 'a[href], button:not([disabled]), [tabindex]:not([tabindex="-1"])';
    let lastFocus = null;

    const getFocusable = () => {
        const inOverlay = [...overlay.querySelectorAll(focusableSelector)]
            .filter((element) => !element.hasAttribute("hidden") && !element.closest("[hidden]"));
        return [trigger, ...inOverlay];
    };

    const isOpen = () => trigger.getAttribute("aria-expanded") === "true";

    const setOpen = (open, { restoreToTrigger = false } = {}) => {
        trigger.setAttribute("aria-expanded", String(open));
        trigger.setAttribute("aria-label", open ? "Close menu" : "Open menu");
        trigger.classList.toggle("is-open", open);
        overlay.hidden = !open;
        overlay.classList.toggle("is-open", open);
        document.documentElement.classList.toggle("is-nav-open", open);

        if (main) {
            main.toggleAttribute("inert", open);
        }
        if (footer) {
            footer.toggleAttribute("inert", open);
        }

        if (open) {
            lastFocus = document.activeElement;
            const firstLink = overlay.querySelector(focusableSelector);
            (firstLink || overlay).focus();
            return;
        }

        const restoreTarget = restoreToTrigger || !lastFocus || typeof lastFocus.focus !== "function"
            ? trigger
            : lastFocus;
        restoreTarget.focus();
    };

    trigger.addEventListener("click", () => {
        setOpen(!isOpen());
    });

    document.addEventListener("keydown", (event) => {
        if (!isOpen()) {
            return;
        }

        if (event.key === "Escape") {
            event.preventDefault();
            setOpen(false);
            return;
        }

        if (event.key !== "Tab") {
            return;
        }

        const focusable = getFocusable();
        if (focusable.length === 0) {
            return;
        }

        const first = focusable[0];
        const last = focusable[focusable.length - 1];
        const active = document.activeElement;

        if (event.shiftKey && active === first) {
            event.preventDefault();
            last.focus();
        } else if (!event.shiftKey && active === last) {
            event.preventDefault();
            first.focus();
        }
    });

    window.addEventListener("resize", () => {
        if (window.matchMedia("(min-width: 1024px)").matches && isOpen()) {
            setOpen(false, { restoreToTrigger: true });
        }
    });
})();
