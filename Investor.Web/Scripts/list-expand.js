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

var commentsClasses = {
  button: '.newsroom-page-comments-expand-btn',
  container: '.newsroom-page-comments-expandable',
  closed: '.newsroom-page-comments-expandable-closed',
  open: '.newsroom-page-comments-expandable-open'
}

var noticesClasses = {
  button: '.newsroom-page-notices-expand-btn',
  container: '.newsroom-page-notices-expandable',
  closed: '.newsroom-page-notices-expandable-closed',
  open: '.newsroom-page-notices-expandable-open'
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
expandLinkList(commentsClasses);
expandLinkList(noticesClasses);