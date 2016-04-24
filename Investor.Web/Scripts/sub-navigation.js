$(document).ready(function () {
    $('.has-children').removeClass('.has-children--expanded');

    if ($('.sub-navigation-active').parents('.has-children').length > 0) {
      $('.sub-navigation-active').parents('.has-children').addClass('has-children--expanded');
    }

    if ($('.sub-navigation-active').children('ul').length > 0) {
      $('.sub-navigation-active').addClass('has-children--expanded');
    }

    $(".sub-navigation-mobile-btn").click(function (e) {
      e.preventDefault();

      if ($(".sub-navigation-nav").is(":visible")) {
        $(".sub-navigation-nav").removeClass("selected");
      } else {
        $(".sub-navigation-nav").addClass("selected");
      }

      $(".sub-navigation-nav").stop().slideToggle();
    });
});