var cookies = (function () {

  var moduleContainer = $(".cookie-information");

   var openMessage = function () {
      var cookieValue = '';

      if (document.cookie) {
        cookieValue = document.cookie.replace(/(?:(?:^|.*;\s*)acceptCookie\s*\=\s*([^;]*).*$)|^.*$/, '$1');
      }

      if (cookieValue !== '1') {
        moduleContainer.addClass('cookie-information--visible');
      }
   }

  var closeMessage = function () {
      $('.cookie-information__inner__button').click(function (e) {
        e.preventDefault();
        var acceptDate = new Date(2020, 1, 1);
        document.cookie = 'acceptCookie=1; path=/; expires=' + acceptDate.toUTCString();
        moduleContainer.removeClass('cookie-information--visible');
      });
    }

  var init = function () {
    openMessage();
    closeMessage();
  }
  
  return {
    init : init
  }

})();