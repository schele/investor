var global = (function() {

  var init = function() {
    campaignLightBox.init();
    cookies.init();
    mobileNavigation.init();
  }

  return {
    init: init
  }

})();