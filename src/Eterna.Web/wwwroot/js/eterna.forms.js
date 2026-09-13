(() => {
    if (!window.Eterna?.motion) {
        return;
    }

    window.Eterna.motion.use("forms", () => ({
        initStatic() {},
        init() {}
    }));
})();
