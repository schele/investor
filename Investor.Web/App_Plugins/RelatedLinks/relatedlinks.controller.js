/// <reference path="../../Umbraco/lib/angular/1.1.5/angular.js" />
/// <reference path="../../Umbraco/lib/jquery/jquery-2.0.3.min.js" />
/// <reference path="../../Umbraco/Js/umbraco.services.js" />

function RelatedLinksController($rootScope, $scope, $timeout, dialogService, assetsService, contentResource, mediaResource) {
    assetsService.loadCss("/App_Plugins/RelatedLinks/relatedlinks.css", $scope).then(function () {
        handleCss(false);
    });

    var config = {
        maxItems: null // Lägg till hur många det ska vara null = default
    };

    if (!angular.isUndefined($scope.model.config) && $scope.model.config !== null) {
        //map the user config
        angular.extend(config, $scope.model.config);
    }

    //map back to the model
    $scope.model.config = config;

    if (!$scope.model.value) {
        $scope.model.value = [];
    }
    
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
                });
            }
        });
    }

    $scope.IsMaxItems = function () {
        if ($scope.model.config.maxItems != null && $scope.model.config.maxItems != 0) {
            if ($scope.model.value.length < $scope.model.config.maxItems) {
                return false;
            }
        } else if ($scope.model.config.maxItems == null) {
            return false;
        }

        return true;
    }

    $scope.internal = function ($event) {
        $scope.currentEditLink = null;
        dialogService.contentPicker({
            //scope: $scope,
            multipicker: false,
            callback: select
        });
        $event.preventDefault();
    };

    $scope.internalMedia = function ($event) {
        $scope.currentEditLink = null;
        dialogService.mediaPicker({
            //scope: $scope,
            multipicker: false,
            callback: select
        });
        $event.preventDefault();
    };

    $scope.selectInternal = function ($event, link) {
        $scope.currentEditLink = link;
        dialogService.contentPicker({
            //scope: $scope,
            callback: select
        });
        $event.preventDefault();
    };
    
    $scope.selectInternalMedia = function ($event, link) {
            $scope.currentEditLink = link;
            dialogService.mediaPicker({
                //scope: $scope,
                callback: select
            });
            $event.preventDefault();
    };
    
    $scope.edit = function (idx) {
        for (var i = 0; i < $scope.model.value.length; i++) {
            $scope.model.value[i].edit = false;
        }
        $scope.model.value[idx].edit = true;
    };

    $scope.cancelEdit = function (idx) {
        var item = $scope.model.value[idx];
        item.edit = false;

        if ($scope.currentEditLink) {
            item.internalName = $scope.currentEditLink.internalName;
        }
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
        if ($scope.newCaption == "") {
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

        }
        $event.preventDefault();
    };

    $scope.switch = function ($event, mode) {
        $scope.CurrentMode = mode;
        $event.preventDefault();
    };

    $scope.switchLinkType = function ($event, link, mode) {
        if (mode == Mode.ExternalLink) {
            link.isInternal = false;
            link.type = "external";
        }
        else if (mode == Mode.InternalLink) {
            link.isInternal = true;
            link.type = "internal";
        }
        else if (mode == Mode.InternalMedia) {
            link.isInternal = true;
            link.type = "internal";
        }

        if (!link.isInternal) {
            link.link = $scope.newLink;
        }
        
        $event.preventDefault();
    };

    function select(data) {
        if ($scope.currentEditLink != null) {
            $scope.currentEditLink.internal = data.id;
            $scope.currentEditLink.internalName = data.name;
            $scope.currentEditLink.link = data.id;
        } else {
            $scope.newInternal = data.id;
            $scope.newInternalName = data.name;
        }
    }

    function handleCss(disable)
    {
        //var thisDialog = $(".umb-overlay umb-overlay-right").last();
        //var controller = angular.element(thisDialog).controller();

        var element = angular.element("[ng-controller='Umbraco.PropertyEditors.Grid.MacroController']:last");

        var dialogScope = element.scope();

        $(".umb-overlay").each(function () {
            $(this).width(726);
            console.log(this);
        });

        //if (!disable) {
        //    thisDialog.width(726);
        //} else {
        //    thisDialog.width(500); // Reset the dialog
        //}

        var lookfor = '/App_Plugins/RelatedLinks/relatedlinks.css';
        [].forEach.call(document.styleSheets, function (styleSheet) {
            if (styleSheet.href && styleSheet.href.indexOf(lookfor) != -1) {
                styleSheet.disabled = disable;

                return false;
            }

            return true;
        });
    }
    
    $scope.$on('$destroy', function () {
        handleCss(true);

        // We need to remove the stylesheet that we added since it changes the style for all dialogs.
        //var toRemove = '/App_Plugins/RelatedLinks/relatedlinks.css';
        //[].forEach.call(document.styleSheets, function (styleSheet) {
        //    if (styleSheet.href && styleSheet.href.indexOf(toRemove) != -1) {
        //        styleSheet.disabled = true;
        //        //$(styleSheet).remove();

        //        return false;
        //    }

        //    return true;
        //});
    });
}

angular.module("umbraco").controller("RelatedLinksController", RelatedLinksController);