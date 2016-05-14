/// <reference path="../../Umbraco/lib/angular/1.1.5/angular.js" />
/// <reference path="../../Umbraco/lib/jquery/jquery-2.0.3.min.js" />
/// <reference path="../../Umbraco/Js/umbraco.services.js" />

function RGRelatedLinksWithMediaController($rootScope, $scope, $element, $timeout, dialogService, assetsService, contentResource, mediaResource, iconHelper) {
    // Support as a macro parameter we need to prefill the config.
    var config = {
        max: null, // Lägg till hur många det ska vara null = default
        hideCaption: false,
        hideNewWindow: false
    };

    if (!angular.isUndefined($scope.model.config) && $scope.model.config !== null) {
        //map the user config
        angular.extend(config, $scope.model.config);
    }
    
    //map back to the model
    $scope.model.config = config;

    if ($scope.model.value == null || !$scope.model.value) {
        $scope.model.value = [];
    }
    
    $scope.model.config.max = isNumeric($scope.model.config.max) && $scope.model.config.max !== 0 ? $scope.model.config.max : Number.MAX_VALUE;

    $scope.Mode = {
        ExternalLink: 1,
        InternalLink: 2,
        InternalMedia: 3
    };

    $scope.newCaption = '';
    $scope.newLink = 'http://';
    $scope.newNewWindow = false;
    $scope.newInternal = null;
    $scope.newInternalName = '';
    $scope.newInternalIcon = null;
    $scope.currentEditLink = null;
    $scope.hasError = false;
    $scope.CurrentMode = $scope.Mode.ExternalLink;

    if ($scope.model.value) {
        // Update any changes for internal items.
        angular.forEach($scope.model.value, function (item) {
            if (item.isInternal) {
                var id = item.link;
                var provider = contentResource;

                if (item.mode == $scope.Mode.InternalMedia) {
                    provider = mediaResource;
                }

                provider
                .getById(id)
                .then(function (data) {
                    item.internalName = data.name;
                    item.internalIcon = iconHelper.convertFromLegacyIcon(data.icon);
                });
            }
        });
    }

    $scope.internal = function ($event) {
        $scope.currentEditLink = null;

        $scope.contentPickerOverlay = {};
        $scope.contentPickerOverlay.view = "contentpicker";
        $scope.contentPickerOverlay.title = "Select content";
        $scope.contentPickerOverlay.multiPicker = false;
        $scope.contentPickerOverlay.show = true;

        $scope.contentPickerOverlay.submit = function (model) {

            select(model.selection[0]);

            $scope.contentPickerOverlay.show = false;
            $scope.contentPickerOverlay = null;
        };

        $scope.contentPickerOverlay.close = function (oldModel) {
            $scope.contentPickerOverlay.show = false;
            $scope.contentPickerOverlay = null;
        };

        $event.preventDefault();
    };

    $scope.selectInternal = function ($event, link) {
        $scope.currentEditLink = link;

        $scope.contentPickerOverlay = {};
        $scope.contentPickerOverlay.view = "contentpicker";
        $scope.contentPickerOverlay.title = "Select content";
        $scope.contentPickerOverlay.multiPicker = false;
        $scope.contentPickerOverlay.show = true;

        $scope.contentPickerOverlay.submit = function (model) {

            select(model.selection[0]);

            $scope.contentPickerOverlay.show = false;
            $scope.contentPickerOverlay = null;
        };

        $scope.contentPickerOverlay.close = function (oldModel) {
            $scope.contentPickerOverlay.show = false;
            $scope.contentPickerOverlay = null;
        };

        $event.preventDefault();
    };

    // ToDo
    $scope.internalMedia = function ($event) {
        $scope.currentEditLink = null;
        //dialogService.mediaPicker({
        //    multipicker: false,
        //    callback: select
        //});

        //$scope.mediaPickerOverlay = {
        //    view: "mediapicker",
        //    title: "Select media",
        //    //startNodeId: $scope.model.config.startNodeId,
        //    multiPicker: false,
        //    show: true,
        //    submit: function(model) {
        //        _.each(model.selectedImages, function(media, i) {

        //            if (!media.thumbnail) {
        //                media.thumbnail = mediaHelper.resolveFileFromEntity(media, true);
        //                $scope.images.push(media);
        //                $scope.ids.push(media.id);
        //            }
        //        });

        //        $scope.mediaPickerOverlay.show = false;
        //        $scope.mediaPickerOverlay = null;
        //    }
        //};

        $scope.contentPickerOverlay = {};
        $scope.contentPickerOverlay.view = "mediapicker";
        $scope.contentPickerOverlay.title = "Select media";
        $scope.contentPickerOverlay.multiPicker = false;
        $scope.contentPickerOverlay.show = true;

        $scope.contentPickerOverlay.submit = function (model) {
            select(model.selectedImages[0]);

            $scope.contentPickerOverlay.show = false;
            $scope.contentPickerOverlay = null;
        };

        $scope.contentPickerOverlay.close = function (oldModel) {
            $scope.contentPickerOverlay.show = false;
            $scope.contentPickerOverlay = null;
        };

        $event.preventDefault();
    };
    
    // ToDo
    $scope.selectInternalMedia = function($event, link) {
        //$scope.currentEditLink = link;
        //dialogService.mediaPicker({
        //    callback: select
        //});
        //$event.preventDefault();

        $scope.currentEditLink = link;
        
        $scope.contentPickerOverlay = {};
        $scope.contentPickerOverlay.view = "mediapicker";
        $scope.contentPickerOverlay.title = "Select media";
        $scope.contentPickerOverlay.multiPicker = false;
        $scope.contentPickerOverlay.show = true;

        $scope.contentPickerOverlay.submit = function (model) {
            select(model.selectedImages[0]);

            $scope.contentPickerOverlay.show = false;
            $scope.contentPickerOverlay = null;
        };

        $scope.contentPickerOverlay.close = function (oldModel) {
            $scope.contentPickerOverlay.show = false;
            $scope.contentPickerOverlay = null;
        };

        $event.preventDefault();
    };
    
    $scope.edit = function (idx) {
        for (var i = 0; i < $scope.model.value.length; i++) {
            $scope.model.value[i].edit = false;
        }
        $scope.model.value[idx].edit = true;
    };

    $scope.saveEdit = function (idx) {
        $scope.model.value[idx].title = $scope.model.value[idx].caption;
        $scope.model.value[idx].edit = false;
    };

    $scope.delete = function (idx) {
        $scope.model.value.splice(idx, 1);
    };

    $scope.isExternalLinkMode = function (mode) {
        if (angular.isUndefined(mode) || mode == null) {
            return $scope.CurrentMode == $scope.Mode.ExternalLink;
        }

        return mode == $scope.Mode.ExternalLink;
    };
    
    $scope.isInternalLinkMode = function (mode) {
        if (angular.isUndefined(mode) || mode == null) {
            return $scope.CurrentMode == $scope.Mode.InternalLink;
        }
        
        return mode == $scope.Mode.InternalLink;
    };

    $scope.isInternalMediaMode = function (mode) {
        if (angular.isUndefined(mode) || mode == null) {
            return $scope.CurrentMode == $scope.Mode.InternalMedia;
        }

        return mode == $scope.Mode.InternalMedia;
    };

    $scope.add = function ($event) {
        if ($scope.newCaption == "" && !$scope.model.config.hideCaption) {
            $scope.hasError = true;
        } else {
            if ($scope.CurrentMode == $scope.Mode.ExternalLink) {
                var newExtLink = new function () {
                    this.caption = $scope.newCaption;
                    this.link = $scope.newLink;
                    this.newWindow = $scope.newNewWindow;
                    this.edit = false;
                    this.isInternal = false;
                    this.mode = $scope.Mode.ExternalLink;
                    this.type = "external";
                    this.title = $scope.newCaption;
                };
                $scope.model.value.push(newExtLink);
            } else if($scope.CurrentMode == $scope.Mode.InternalLink) {
                var newIntLink = new function () {
                    this.caption = $scope.newCaption;
                    this.link = $scope.newInternal;
                    this.newWindow = $scope.newNewWindow;
                    this.internal = $scope.newInternal;
                    this.edit = false;
                    this.isInternal = true;
                    this.mode = $scope.Mode.InternalLink;
                    this.internalName = $scope.newInternalName;
                    this.internalIcon = $scope.newInternalIcon;
                    this.type = "internal";
                    this.title = $scope.newCaption;
                };
                $scope.model.value.push(newIntLink);
            } else if ($scope.CurrentMode == $scope.Mode.InternalMedia) {
                var newIntMediaLink = new function () {
                    this.caption = $scope.newCaption;
                    this.link = $scope.newInternal;
                    this.newWindow = $scope.newNewWindow;
                    this.internal = $scope.newInternal;
                    this.edit = false;
                    this.isInternal = true;
                    this.mode = $scope.Mode.InternalMedia;
                    this.internalName = $scope.newInternalName;
                    this.internalIcon = $scope.newInternalIcon;
                    this.type = "internalMedia";
                    this.title = $scope.newCaption;
                };
                $scope.model.value.push(newIntMediaLink);
            }
            $scope.newCaption = '';
            $scope.newLink = 'http://';
            $scope.newNewWindow = false;
            $scope.newInternal = null;
            $scope.newInternalName = '';
            $scope.newInternalIcon = null;
        }
        $event.preventDefault();
    };

    $scope.switch = function ($event, mode) {
        $scope.CurrentMode = mode;
        $event.preventDefault();
    };

    $scope.switchLinkType = function ($event, link, mode) {
        if (mode == $scope.Mode.ExternalLink) {
            link.isInternal = false;
            link.type = "external";
        }
        else if (mode == $scope.Mode.InternalLink) {
            link.isInternal = true;
            link.type = "internal";
        }
        else if (mode == $scope.Mode.InternalMedia) {
            link.isInternal = true;
            link.type = "internal";
        }

        if (!link.isInternal) {
            link.link = $scope.newLink;
        }

        link.mode = mode;

        $event.preventDefault();
    };

    $scope.move = function (index, direction) {
        var temp = $scope.model.value[index];
        $scope.model.value[index] = $scope.model.value[index + direction];
        $scope.model.value[index + direction] = temp;
    };

    //helper for determining if a user can add items
    $scope.canAdd = function () {
        return $scope.model.config.max <= 0 || $scope.model.config.max > countVisible();
    }

    //helper that returns if an item can be sorted
    $scope.canSort = function () {
        return countVisible() > 1;
    }

    $scope.sortableOptions = {
        axis: 'y',
        handle: '.handle',
        cursor: 'move',
        cancel: '.no-drag',
        containment: 'parent',
        placeholder: 'sortable-placeholder',
        forcePlaceholderSize: true,
        helper: function (e, ui) {
            // When sorting table rows, the cells collapse. This helper fixes that: http://www.foliotek.com/devblog/make-table-rows-sortable-using-jquery-ui-sortable/
            ui.children().each(function () {
                $(this).width($(this).width());
            });
            return ui;
        },
        items: '> tr:not(.unsortable)',
        tolerance: 'pointer',
        update: function (e, ui) {
            // Get the new and old index for the moved element (using the URL as the identifier)
            var newIndex = ui.item.index();
            var movedLinkUrl = ui.item.attr('data-link');
            var originalIndex = getElementIndexByUrl(movedLinkUrl);

            // Move the element in the model
            var movedElement = $scope.model.value[originalIndex];
            $scope.model.value.splice(originalIndex, 1);
            $scope.model.value.splice(newIndex, 0, movedElement);
        },
        start: function (e, ui) {
            //ui.placeholder.html("<td colspan='5'></td>");

            // Build a placeholder cell that spans all the cells in the row: http://stackoverflow.com/questions/25845310/jquery-ui-sortable-and-table-cell-size
            var cellCount = 0;
            $('td, th', ui.helper).each(function () {
                // For each td or th try and get it's colspan attribute, and add that or 1 to the total
                var colspan = 1;
                var colspanAttr = $(this).attr('colspan');
                if (colspanAttr > 1) {
                    colspan = colspanAttr;
                }
                cellCount += colspan;
            });

            // Add the placeholder UI - note that this is the item's content, so td rather than tr - and set height of tr
            ui.placeholder.html('<td colspan="' + cellCount + '"></td>').height(ui.item.height());
        }
    };
    
    $scope.isSortVisible = function () {
        return $scope.model.config.max !== "1";
    }

    $scope.isCaptionVisible = function () {
        return $scope.model.config.hideCaption !== "1";
    }

    $scope.isNewWindowVisible = function () {
        return $scope.model.config.hideNewWindow !== "1";
    }

    //helper to count what is visible
    function countVisible() {
        return $scope.model.value.length;
    }

    function isNumeric(n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    }

    function getElementIndexByUrl(url) {
        for (var i = 0; i < $scope.model.value.length; i++) {
            if ($scope.model.value[i].link == url) {
                return i;
            }
        }

        return -1;
    }

    function select(data) {
        if ($scope.currentEditLink != null) {
            $scope.currentEditLink.internal = data.id;
            $scope.currentEditLink.internalName = data.name;
            $scope.currentEditLink.internalIcon = iconHelper.convertFromLegacyIcon(data.icon);
            $scope.currentEditLink.link = data.id;
        } else {
            $scope.newInternal = data.id;
            $scope.newInternalName = data.name;
            $scope.newInternalIcon = iconHelper.convertFromLegacyIcon(data.icon);
        }
    }
    angular.element(document).ready(function () {
        var element = $($element).closest("[ng-controller='Umbraco.PropertyEditors.Grid.MacroController']");
        // Check if this is used in a macro.
        if (element.length) {
            //// Get the controller
            //var scope = element.scope();
            //var overlay = scope.macroPickerOverlay;
            //overlay.position = "center";
            var overlay = element.find(".umb-overlay");
            overlay.width(726);
        }
    });

    $scope.$on("formSubmitting", function () {
        if (!_.isArray($scope.model.value)) {
            $scope.model.value = [];
        }
    });
}

angular.module("umbraco").controller("RG.RelatedLinksWithMediaController", RGRelatedLinksWithMediaController);