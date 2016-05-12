var iframeSize = (function() {
  var getSize = function() {
     $('iframe').iFrameResize([{}]);
  }

  var init = function() {
    getSize();
  }

  return {
    init: init
  }
})();
