const ButtonLoadingToggle = async className => {
    var response = "";
    [...document.getElementsByClassName(className)].forEach(button => {
        var loading = button.dataset.loading;
        var reset = button.dataset.reset;

        if (button.classList.contains('loading')) {
            button.classList.remove('loading');
            button.innerHTML = reset;
            response ="reset";
        }
        else {
            button.classList.add('loading');
            button.innerHTML = loading;
            response ="loading";
        }
    });
    return response;
}
