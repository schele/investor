var globalFunctions = (function () {
  var init = function () {
    campaignLightBox.init();
    cookies.init();
    mobileNavigation.init();
    subNavigation.init();
    textEditorIframeWrapper.init();
  }

  return {
    init: init
  }
})();

var startPageFunctions = (function() {
  var init = function() {
    slideShow.init();
    macroHeight.init();
  }

  return {
    init: init
  }
})();