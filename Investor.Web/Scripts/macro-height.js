var macroHeight = (function() {
  var containerRows = function() {
    var rows = [];

    $(".row").each(function() {
      if ($(this).find('.col-md-6').length === 2) {
        rows.push(this);
      }
    });

    return rows;
  }

  var macroContainers = function(firstContainer, lastContainer) {
    var largestContainer;
    var smallestContainer;

    if (firstContainer.length <= lastContainer.length) {
      smallestContainer = firstContainer;
      largestContainer = lastContainer;
    } else {
      smallestContainer = lastContainer;
      largestContainer = firstContainer;
    }

    return {
      largest: largestContainer,
      smallest: smallestContainer
    }
  }

  var setMacroHeight = function(rows) {
    $(rows).each(function() {
      var firstMacroContainer = $(this).find('.col-md-6:first-child > div').children();
      var lastMacroContainer = $(this).find('.col-md-6:last-child > div').children();

      var containers = macroContainers(firstMacroContainer, lastMacroContainer);
      
      if (containers.smallest.length <= 0) {
        return false;
      }

      for (var i = 0; i < containers.smallest.length; i++) {
        var largestHeight;

        if ($(containers.smallest[i]).height() > $(containers.largest[i]).height()) {
          largestHeight = $(containers.smallest[i]).height();
          $(containers.largest[i]).height(largestHeight);
        } else {
          largestHeight = $(containers.largest[i]).height();
          $(containers.smallest[i]).height(largestHeight);
        }
      }
    });
  }

  var init = function() {
    var rows = containerRows();
    setMacroHeight(rows);
  }

  return {
    init: init
  }
})();