var version = 'v2.0::CacheFirstSafe';
var offlineUrl = "offline.html"; // <-- Offline/Index.cshtml
var urlsToCache = ['/', offlineUrl]; // <-- Add more URLs you would like to cache.
const staticCacheName = 'pages-cache-v2';

// Store core files in a cache (including a page to display when offline)
function updateStaticCache() {
    return caches.open(version)
        .then(function (cache) {
            return cache.addAll(urlsToCache);
        });
}

//LOADING FILES WHEN OFFLINE//
// self.addEventListener('fetch', event => {
//   console.log('Fetch event for ', event.request.url);
//   event.respondWith(
//     caches.match(event.request)
//     .then(response => {
//       if (response) {
//         console.log('Found ', event.request.url, ' in cache');
//         return response;
//       }
//       console.log('Network request for ', event.request.url);
//       return fetch(event.request)
//
//         .then(response => {
//           // TODO 5 - Respond with custom 404 page
//           return caches.open(staticCacheName).then(cache => {
//             cache.put(event.request.url, response.clone());
//             return response;
//           });
//         });
//
//
//     }).catch(error => {
//       console.log('Error, ', error);
//       return caches.match('./offline.html');
//     })
//   );
// });

self.addEventListener('fetch', (event) => {
    event.respondWith(async function () {
        try {
            return await fetch(event.request);
        } catch (err) {
            return caches.match(event.request);
        }
    }());
});

// TODO 6 - Respond with custom offline page

self.addEventListener('activate', event => {
    console.log('Activating new service worker...');

    const cacheWhitelist = [staticCacheName];

    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(cacheName => {
                    if (cacheWhitelist.indexOf(cacheName) === -1) {
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
});
