(function($) {

    "use strict";

    // init jarallax parallax
    var initJarallax = function() {
      jarallax(document.querySelectorAll(".jarallax"));

      jarallax(document.querySelectorAll(".jarallax-img"), {
        keepImg: true,
      });
    }
    
    $(document).ready(function() {
        
      var swiper = new Swiper(".main-swiper", {
        loop: true,
        speed: 800,
        autoplay: {
          delay: 6000,
        },
        effect: "creative",
        creativeEffect: {
          prev: {
            shadow: true,
            translate: ["-20%", 0, -1],
          },
          next: {
            translate: ["100%", 0, 0],
          },
        },
        pagination: {
          el: ".main-slider-pagination",
          clickable: true,
        },
      });
      
      var swiper = new Swiper(".product-swiper", {
        speed: 1000,
        spaceBetween: 20,
        navigation: {
          nextEl: ".product-carousel-next",
          prevEl: ".product-carousel-prev",
        },
        breakpoints: {
          0: {
            slidesPerView: 1,
          },
          480: {
            slidesPerView: 2,
          },
          900: {
            slidesPerView: 3,
            spaceBetween: 20,
          },
          1200: {
            slidesPerView: 5,
            spaceBetween: 20,
          }
        },
      }); 

      var swiper = new Swiper(".testimonial-swiper", {
        speed: 1000,
        navigation: {
          nextEl: ".testimonial-arrow-next",
          prevEl: ".testimonial-arrow-prev",
        },
      });

      var thumb_slider = new Swiper(".thumb-swiper", {
        slidesPerView: 1,
      });
      var large_slider = new Swiper(".large-swiper", {
        spaceBetween: 10,
        effect: 'fade',
        thumbs: {
          swiper: thumb_slider,
        },
      });
      
      initJarallax();
      AOS.init();
      
    }); // End of a document ready
    
})(jQuery);