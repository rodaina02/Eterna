(() => {
    if (!window.Eterna?.motion) {
        return;
    }

    const bindForm = (form) => {
        if (form.dataset.eternaBound === "1") {
            return;
        }
        form.dataset.eternaBound = "1";
        const fields = [...form.querySelectorAll(".field")].filter((field) => !field.classList.contains("sr-only"));

        const setInvalid = (field, invalid) => {
            const control = field.querySelector("input, select, textarea");
            const error = field.querySelector(".field__error");
            if (!control) {
                return;
            }
            field.classList.toggle("is-invalid", invalid);
            control.setAttribute("aria-invalid", invalid ? "true" : "false");
            if (error) {
                error.hidden = !invalid;
                if (invalid) {
                    control.setAttribute("aria-describedby", error.id);
                } else {
                    control.removeAttribute("aria-describedby");
                }
            }
        };

        const validateField = (field) => {
            const control = field.querySelector("input, select, textarea");
            if (!control) {
                return true;
            }
            const valid = control.checkValidity();
            setInvalid(field, !valid);
            return valid;
        };

        fields.forEach((field) => {
            const control = field.querySelector("input, select, textarea");
            control?.addEventListener("blur", () => validateField(field));
            control?.addEventListener("input", () => {
                if (field.classList.contains("is-invalid")) {
                    validateField(field);
                }
            });
        });

        form.addEventListener("submit", (event) => {
            const honeypot = form.querySelector("[name='Website']");
            if (honeypot?.value) {
                event.preventDefault();
                return;
            }
            const valid = fields.every((field) => validateField(field));
            if (!valid) {
                event.preventDefault();
                const first = form.querySelector(".field.is-invalid input, .field.is-invalid select, .field.is-invalid textarea");
                first?.focus();
            }
        });
    };

    window.Eterna.motion.use("forms", () => ({
        initStatic() {
            document.querySelectorAll("[data-contact-form]").forEach(bindForm);
        },
        init() {
            document.querySelectorAll("[data-contact-form]").forEach(bindForm);
        }
    }));
})();
