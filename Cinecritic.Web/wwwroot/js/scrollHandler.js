function addScrollHandler(dotNetHelper) {
    let html = document.querySelector('html');

    window.addEventListener('scroll', () => {
        dotNetHelper.invokeMethodAsync('OnScrollAsync', {
            scrollTop: html.scrollTop,
            scrollHeight: html.scrollHeight,
            clientHeight: html.clientHeight,
        });
    })
}