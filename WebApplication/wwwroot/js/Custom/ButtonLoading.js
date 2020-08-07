const ButtonLoadingToggle = async (className) =>
    [...document.getElementsByClassName(className)].forEach(button => {
        var loading = button.dataset.loading;
        var reset = button.dataset.reset;

        if (button.classList.contains('loading')) {
            button.classList.remove('loading');
            button.innerHTML = reset;
            return "reset";
        }
        else {
            button.classList.add('loading');
            button.innerHTML = loading;
            return "loading";
        }
    });
