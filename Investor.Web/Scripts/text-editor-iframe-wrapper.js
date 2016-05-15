var textEditorIframeWrapper = (function() {
  var wrapIframe = function() {
    $('.text-editor-content iframe[src*="youtube.com"]').wrap('<div class="text-editor-iframe"></div>');
  }

  var init = function() {
    wrapIframe();
  }

  return {
    init: init
  }
})();