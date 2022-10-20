var isApp = matchMedia('(display-mode: standalone)')
    .matches;
var head = document.querySelector("head");
if (isApp) {
    head.innerHTML +=
        '<meta name=google content=notranslate>' +
        '<meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0">';
}