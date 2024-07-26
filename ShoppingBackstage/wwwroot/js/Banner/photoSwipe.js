// photoSwipe.js

import PhotoSwipeLightbox from '/lib/photoswipe-5.4.4/photoswipe-lightbox.esm.min.js';

const leftArrowSVGString = '<svg aria-hidden="true" class="pswp__icn" viewBox="0 0 100 125" width="100" height="125"><path d="M5,50L50,5l3,3L11,50l42,42l-3,3L5,50z M92,95l3-3L53,50L95,8l-3-3L47,50L92,95z"/></svg>';
function LoadPhotoSwipe() {
    console.log("Initializing PhotoSwipe");
    const options = {
        /* element pointer */
        gallery: '.table',
        children: 'a',
        
        /* animation */
        showHideAnimationType: 'zoom',
        showAnimationDuration: 1000,
        hideAnimationDuration: 1000,
        
        /* style */
        arrowPrevSVG: leftArrowSVGString,
        arrowNextSVG: leftArrowSVGString,
        mainClass: 'pswp--custom-icon-colors',
        
        /* core */
        pswpModule: () => import('/lib/photoswipe-5.4.4/photoswipe.esm.min.js')
    };

    const lightbox = new PhotoSwipeLightbox(options);
    lightbox.on('uiRegister', function() {
        lightbox.pswp.ui.registerElement({
            name: 'zoom-level-indicator',
            order: 9,
            onInit: (el, pswp) => {
                pswp.on('zoomPanUpdate', (e) => {
                    if (e.slide === pswp.currSlide) {
                        el.innerText = '當前縮放比例為 ' + Math.round(pswp.currSlide.currZoomLevel * 100) + '%';
                    }
                });
            }
        });
    });

    lightbox.init();
}

window.LoadPhotoSwipe = LoadPhotoSwipe;