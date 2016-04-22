$(document).ready(function () {

  $('#fancy_box').magnificPopup({
    type: 'inline',
    closeBtnInside: false,
    closeBtnOutside: false
    // other options
  }).trigger("click");;

  $('#campaign-custom-close, #campaign-custom-close-mobile').click(function () {
    $.magnificPopup.close();
  });

});