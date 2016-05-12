var slideShow = (function() {
  var initSlideShow = function () {
    if ($('.slide-show-item:first-child') != null && $('.slide-show-dot:first-child') != null) {
      $('.slide-show-item:first-child').addClass('active');
      $('.slide-show-dot:first-child').addClass('active');
      return true;
    }
    return false;
  }

  var addActiveClass = function (activeElement, firstItem) {
    if (!activeElement.is(":last-child")) {
      $('.slide-show-item-list').animate({
        "margin-left": (-(activeElement.index() + 1) * slideItemWidth)
      }, 400);
      activeElement.next().addClass("active");
    } else {
      $('.slide-show-item-list').animate({ 'margin-left': (0) }, 400);
      firstItem.addClass('active');
    }
  }

  var autoSlide = function () {
    var currentActiveSlide = $('.slide-show-item.active');
    var currentActiveDot = $('.slide-show-dot.active');
    var firstSlide = $('.slide-show-item:first-child');
    var firstDot = $('.slide-show-dot:first-child');
    $('.slide-show-item.active, .slide-show-dot.active').removeAttr('style').removeClass('active');
    addActiveClass(currentActiveSlide, firstSlide);
    addActiveClass(currentActiveDot, firstDot);
  }

  var selectedIsBefore = function (activeElement) {
    var prevItems = activeElement.prevAll();
    var numberOfSlides = 0;

    prevItems.each(function () {
      numberOfSlides += 1;
      if ($(this).hasClass('active')) {
        return false;
      }
    });

    return numberOfSlides;
  }

  var selectedIsAfter = function (activeElement) {
    var nextItems = activeElement.nextAll();
    var numberOfSlides = 0;

    nextItems.each(function () {
      numberOfSlides += 1;
      if ($(this).hasClass('active')) {
        return false;
      }
    });
    return numberOfSlides;
  }

  var selectSlideAnimation = function (activeElement) {

    var numberOfSlides;

    if (activeElement.nextAll('.active').length > 0) {
      numberOfSlides = selectedIsAfter(activeElement);
      $('.slide-show-item-list').animate({
        "margin-left": (-(activeElement.index() + numberOfSlides) * slideItemWidth)
      }, 400);
    } else {
      numberOfSlides = selectedIsBefore(activeElement);
      $('.slide-show-item-list').animate({
        "margin-left": (-(activeElement.index() - numberOfSlides) * slideItemWidth)
      }, 400);
    }
  }

  var selectSlide = function (setSlideInterval) {
    $('.slide-show-dot').click(function () {
      if (!$(this).hasClass('active')) {
        var currentActiveSlide = $('.slide-show-item.active');
        $('.slide-show-item.active, .slide-show-dot.active').removeClass('active');
        var index = $(this).attr('name');
        $('[name="' + index + '"]').addClass('active');
        selectSlideAnimation(currentActiveSlide);
      }
      clearTimeout(setSlideInterval);
      setSlideInterval = setInterval('autoSlide()', 14000);
    });
  }

  var slideItem = $('.slide-show-item');
  var slideItemWidth = slideItem.width();

  var setSlideListWidth = function () {
    $('.slide-show-item-list').css({
      width: function () {
        return slideItem.length * slideItemWidth;
      }
    });
  }

  var startSlideInterval = function () {
    if (initSlideShow()) {
      setSlideListWidth();
      initSlideShow();
      var setSlideInterval = setInterval('autoSlide()', 14000);
      selectSlide(setSlideInterval);
    }
  }

  $(document).ready(function () {
    
  });

  (function ($, sr) {

    // debouncing function from John Hann
    // http://unscriptable.com/index.php/2009/03/20/debouncing-javascript-methods/
    var debounce = function (func, threshold, execAsap) {
      var timeout;

      return function debounced() {
        var obj = this, args = arguments;
        function delayed() {
          if (!execAsap)
            func.apply(obj, args);
          timeout = null;
        };

        if (timeout)
          clearTimeout(timeout);
        else if (execAsap)
          func.apply(obj, args);

        timeout = setTimeout(delayed, threshold || 500);
      };
    }
    // smartresize 
    jQuery.fn[sr] = function (fn) { return fn ? this.bind('resize', debounce(fn)) : this.trigger(sr); };

  })(jQuery, 'smartresize');


  var mediaMax = matchMedia('(max-width: 1200px)');
  var mediaMin = matchMedia('(min-width: 1200px)');

  var resizeFunction = function () {
    slideItem = $('.slide-show-item');
    slideItemWidth = slideItem.width();
    setSlideListWidth();
    $('.slide-show-item.active, .slide-show-dot.active').removeClass('active');
    $('.slide-show-item-list').css({ 'margin-left': (0) });
    $('.slide-show-item:first-child').addClass('active');
    $('.slide-show-dot:first-child').addClass('active');
  }

  var onResize = $(window).smartresize(function () {
    if (mediaMin.matches || mediaMax.matches) {
      resizeFunction();
    }
  });

  var init = function() {
    startSlideInterval();
    onResize();
  }

  return {
    init: init
  }
});