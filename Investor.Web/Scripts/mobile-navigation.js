$(document).ready(function() {
  $("#mobile-main-menu .ui").click(function (e) {
    e.preventDefault();
    $("#mobile-main-menu .options").slideToggle();
  });
});