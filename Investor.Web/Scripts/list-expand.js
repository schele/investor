var listExpand = (function() {

  var expandLinkList = function () {
    $('.expandable-list-btn').click(function (e) {
      e.preventDefault();

      var toggleBtn = $(this);
      $(toggleBtn).find('.expandable-list-closed, .expandable-list-open').removeClass('active');

      $(this).siblings('.expandable-list-container').stop().slideToggle(300, function () {
        if ($(this).is(":visible")) {
          $(toggleBtn).find('.expandable-list-open').addClass("active");
        } else {
          $(toggleBtn).find('.expandable-list-closed').addClass("active");
        }
      });
    });
  }

  var init = function() {
    expandLinkList();
  }

  return {
    init: init
  }

})();
