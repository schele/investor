$(document).ready(function () {
    $('.has-children').removeClass('.has-children--expanded');

    if ($('.sub-navigation-active').parents('.has-children').length > 0) {
      $('.sub-navigation-active').parents('.has-children').addClass('has-children--expanded');
    }

    if ($('.sub-navigation-active').children('ul').length > 0) {
      $('.sub-navigation-active').addClass('has-children--expanded');
    }
});