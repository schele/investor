function iframeLoaded() {
  var iFrameId = document.getElementById('iframe-page-iframe');
  if (iFrameId) {
    // here you can make the height, I delete it first, then I make it again
    iFrameId.height = "";
    iFrameId.height = iFrameId.contentWindow.document.body.scrollHeight + "px";
  }
}