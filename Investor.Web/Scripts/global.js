var globalFunctions = (function () {
    var init = function () {
      campaignLightBox.init();
      cookies.init();
      mobileNavigation.init();
      subNavigation.init();
    }

    return {
      init: init
    }
 })();