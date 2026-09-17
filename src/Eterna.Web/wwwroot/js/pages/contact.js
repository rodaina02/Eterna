(() => {
    const form = document.querySelector("[data-inner-page='contact'] [data-contact-form]");
    if (!form || form.dataset.eternaBound === "1") {
        return;
    }

    form.dataset.eternaBound = "1";

    const submit = form.querySelector("[data-contact-submit]");
    const live = form.querySelector("[data-contact-live]");
    const fields = [...form.querySelectorAll(".field")];

    const setInvalid = (field, invalid) => {
        const control = field.querySelector("input, select, textarea");
        const error = field.querySelector(".field__error");
        if (!control) {
            return;
        }

        field.classList.toggle("is-invalid", invalid);
        control.setAttribute("aria-invalid", invalid ? "true" : "false");

        if (error) {
            if (invalid) {
                control.setAttribute("aria-describedby", error.id);
            } else if (control.getAttribute("aria-describedby") === error.id) {
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
        if (form.dataset.submitting === "1") {
            event.preventDefault();
            return;
        }

        const valid = fields.map((field) => validateField(field)).every(Boolean);
        if (!valid) {
            event.preventDefault();
            const first = form.querySelector(".field.is-invalid input, .field.is-invalid select, .field.is-invalid textarea");
            first?.focus();
            return;
        }

        form.dataset.submitting = "1";
        form.classList.add("is-submitting");
        form.setAttribute("aria-busy", "true");
        if (submit) {
            submit.disabled = true;
            submit.textContent = "Sending";
        }
        if (live) {
            live.textContent = "Sending your project details.";
        }
    });
})();
