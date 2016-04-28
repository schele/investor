var relatedDocumentsClasses = {
  button: '.article-page-documents-expand-btn',
  container: '.article-page-documents-expandable',
  closed: '.article-page-documents-expandable-closed',
  open: '.article-page-documents-expandable-open'
}

var relatedLinksClasses = {
  button: '.article-page-links-expand-btn',
  container: '.article-page-links-expandable',
  closed: '.article-page-links-expandable-closed',
  open: '.article-page-links-expandable-open'
}

var expandLinkList = function(classObj) {
  $(classObj.button).click(function (e) {
    e.preventDefault();
    $(classObj.open + ',' + classObj.closed).removeClass('active');

    $(classObj.container).stop().slideToggle(300, function() {
      if ($(classObj.container).is(":visible")) {
        $(classObj.open).addClass("active");
      } else {
        $(classObj.closed).addClass("active");
      }
    });
  });
}

expandLinkList(relatedDocumentsClasses);
expandLinkList(relatedLinksClasses);