////if ("serviceWorker" in navigator) {
////    navigator.serviceWorker
////        .register("/serviceworker.js")
////        .then(function (registration) {
////            console.log("success load");
////        })
////        .catch(function (err) {
////            console.log(err);
////        });
////}

// 註冊 service worker
if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('./serviceworker.js', { scope: '/' }).then(function (registration) {
        // 註冊成功
        console.log('ServiceWorker registration successful with scope: ', registration.scope);
    }).catch(function (err) {
        // 註冊失败 :(
        console.log('ServiceWorker registration failed: ', err);
    });
}