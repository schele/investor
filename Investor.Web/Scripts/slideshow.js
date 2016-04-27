var initSlideShow = function() {
  if ($('.slide-show-item:first-child') != null && $('.slide-show-dot:first-child') != null) {
      $('.slide-show-item:first-child').addClass('active');
      $('.slide-show-dot:first-child').addClass('active');
      return true;
  }
  return false;
}

var addActiveClass = function(activeElement, firstItem) {
  if (!activeElement.is(":last-child")) {
    activeElement.next().addClass("active");
  } else {
    firstItem.addClass('active');
  }
}

var autoSlide = function () {
  var currentActiveSlide = $('.slide-show-item.active');
  var currentActiveDot = $('.slide-show-dot.active');
  var firstSlide = $('.slide-show-item:first-child');
  var firstDot = $('.slide-show-dot:first-child');
  $('.slide-show-item.active, .slide-show-dot.active').removeClass('active');
  addActiveClass(currentActiveSlide, firstSlide);
  addActiveClass(currentActiveDot, firstDot);
}

var selectSlide = function () {
    $('.slide-show-dot').click(function () {
    $('.slide-show-item.active, .slide-show-dot.active').removeClass('active');
    var index = $(this).attr('name');
    $('[name="' + index + '"]').addClass('active');
      clearTimeout(setSlideInterval);
      setSlideInterval = setInterval('autoSlide()', 6000);
    });
}

var setSlideInterval = setInterval('autoSlide()', 6000);

var startSlideInterval = function () {
  if (initSlideShow()) {
    initSlideShow();
  }
}



startSlideInterval();
selectSlide();