function addScrollHandler(dotNetHelper) {
    let main = document.querySelector('main');

    main.addEventListener('scroll', () => {
        dotNetHelper.invokeMethodAsync('OnScrollAsync', {
            scrollTop: main.scrollTop,
            scrollHeight: main.scrollHeight,
            clientHeight: main.clientHeight,
        });
    })
}