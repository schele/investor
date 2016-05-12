var mobileNavigation = (function() {

  var expandNav = function() {
    $("#mobile-main-menu .ui").click(function(e) {
      e.preventDefault();

      if ($("#mobile-main-menu .options").is(":visible")) {
        $("#mobile-main-menu").removeClass("selected");
      } else {
        $("#mobile-main-menu").addClass("selected");
      }

      $("#mobile-main-menu .options").stop().slideToggle();
    });
  }

  var expandSearch = function() {
    $("#search .ui").click(function(e) {
      e.preventDefault();

      if ($("#search .options").is(":visible")) {
        $("#search").removeClass("selected");
      } else {
        $("#search").addClass("selected");
      }

      $("#search .options").stop().slideToggle();
    });
  }

  var init = function() {
    expandNav();
    expandSearch();
  }

  return {
    init: init
  }

})();