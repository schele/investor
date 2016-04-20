$(document).ready(function() {
  $("#mobile-main-menu .ui").click(function (e) {
    e.preventDefault();

    if ($("#mobile-main-menu .options").is(":visible")) {
      $("#mobile-main-menu").removeClass("selected");
    } else {
      $("#mobile-main-menu").addClass("selected");
    }

    $("#mobile-main-menu .options").stop().slideToggle();
  });
});