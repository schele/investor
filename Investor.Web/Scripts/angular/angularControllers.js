(function ($) {

    angular.module("baseApp", ["baseApp.services", "baseApp.configuration"], function () {
    });
    var view = {
        controllers: {
            baseController: function () { },

            searchPanelController: function ($scope, $rootScope, searchDataService, configService) {
                $scope.searchTerm = "";
                $scope.possibleWordsResult = [];
                $scope.activeTab = "all";
                $scope.contentSearchTitle = "";
                $scope.imageSearchTitle = "";
                $scope.fileSearchTitle = "";

                configService.EnsureConfigs().then(function (configs) {
                    var searchQueryKey = configs["searchQueryKey"];

                    // Bind location -> searchQueryKey
                    $scope.searchTerm = $rootScope.GetQueryStringValue(searchQueryKey);
                    $scope.UpdateSearch($scope.searchTerm);
                });

                // constructs the suggestion engine
                var engine = new Bloodhound({
                    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
                    queryTokenizer: Bloodhound.tokenizers.whitespace,
                    dupDetector: function (left, right) {
                        return left.value == right.value;
                    },
                    local: $.map($scope.possibleWordsResult, function (word) { return { value: word }; })
                });

                // kicks off the loading/processing of `local` and `prefetch`
                engine.initialize();

                $scope.$watch("searchTerm", function (newValue, oldValue) {
                    if (!angular.isDefined(newValue) || !angular.isDefined(oldValue)) {
                        console.log("[SearchPanelController] => $watch(searchTerm): newValue or oldValu is not defined");
                        return;
                    }

                    if (newValue === oldValue) {
                        console.log("[SearchPanelController] => $watch(searchTerm): newValue is the same as oldValue");
                        return;
                    }

                    if (newValue == null || !angular.isUndefined(newValue.value)) {
                        return;
                    }

                    //$scope.UpdateSearch(newValue);

                    searchDataService.lookForPossibleWords(newValue, function (data) {
                        $scope.possibleWordsResult = data;
                        var map = $.map(data, function (word) { return { value: word }; });

                        _.each(map, function (remoteMatch) {
                            var isDuplicate;
                            isDuplicate = _.some(engine.index.datums, function (match) {
                                return engine.dupDetector(remoteMatch, match);
                            });
                            if (!isDuplicate) {
                                engine.add(remoteMatch);
                            }
                        });

                        //console.log(engine);
                    });
                });

                $scope.UpdateSearch = function (term) {
                    if (term.length >= 2 && term !== "") {
                        //searchDataService.lookForPossibleWords(term, function(data) {
                        //    $scope.possibleWordsResult = data;
                        //});

                        var getResultCount = function (data) {
                            var count = 0;

                            if (!angular.isUndefined(data.results)) {
                                count = data.results.length;
                            }

                            return count;
                        };

                        switch ($scope.activeTab) {
                            case "content":
                                searchDataService.lookForContent(term, function (data) {
                                    $scope.contentSearchTitle = "(" + getResultCount(data) + ")";
                                });

                                console.log("[SearchPanelController] => $scope.UpdateSearch(term): lookForContent");
                                break;
                            case "image":
                                searchDataService.lookForImages(term, function (data) {
                                    $scope.imageSearchTitle = "(" + getResultCount(data) + ")";
                                });

                                console.log("[SearchPanelController] => $scope.UpdateSearch(term): lookForImages");
                                break;
                            case "file":
                                searchDataService.lookForFiles(term, function (data) {
                                    $scope.fileSearchTitle = "(" + getResultCount(data) + ")";
                                });

                                console.log("[SearchPanelController] => $scope.UpdateSearch(term): lookForFiles");
                            case "all":
                                searchDataService.lookForContent(term, function (data) {
                                    $scope.contentSearchTitle = "(" + getResultCount(data) + ")";
                                });

                                searchDataService.lookForImages(term, function (data) {
                                    $scope.imageSearchTitle = "(" + getResultCount(data) + ")";
                                });

                                searchDataService.lookForFiles(term, function (data) {
                                    $scope.fileSearchTitle = "(" + getResultCount(data) + ")";
                                });

                                console.log("[SearchPanelController] => $scope.UpdateSearch(term): lookForContent|lookForImages|lookForFiles");
                                break;
                        }
                    } else {
                        $scope.possibleWordsResult = [];
                        $scope.contentSearch = "";
                        $scope.imageSearch = "";
                        $scope.fileSearch = "";
                        $scope.contentSearchTitle = "";
                        $scope.imageSearchTitle = "";
                        $scope.fileSearchTitle = "";
                        searchDataService.clearResults();
                    }
                };

                $scope.$on("typeahead:selected", function (event, suggestion, dataset) {
                    $scope.UpdateSearch(suggestion.value);
                });

                // Typeahead options object
                $scope.settings = {
                    hint: true,
                    highlight: true
                };

                $scope.dataset = {
                    displayKey: 'value',
                    source: engine.ttAdapter()
                };
            },

            searchController: function ($scope, searchDataService, configService) {
                $scope.submitUrl = "";
                $scope.searchTerm = "";
                $scope.possibleWordsResult = searchDataService.getPossibleWordsResult();

                var searchQueryKey;

                $scope.init = function (url) {
                    $scope.submitUrl = url;
                };

                configService.EnsureConfigs().then(function (configs) {
                    searchQueryKey = configs["searchQueryKey"];
                });

                // constructs the suggestion engine
                var engine = new Bloodhound({
                    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
                    queryTokenizer: Bloodhound.tokenizers.whitespace,
                    dupDetector: function (left, right) {
                        return left.value == right.value;
                    },
                    local: $.map($scope.possibleWordsResult, function (word) { return { value: word }; })
                });

                // kicks off the loading/processing of `local` and `prefetch`
                engine.initialize();

                searchDataService.onPossibleWordsResultUpdate($scope, function (context, data) {
                    // update the local scope model
                    $scope.possibleWordsResult = data;

                    var map = $.map(data, function (word) { return { value: word }; });

                    _.each(map, function (remoteMatch) {
                        var isDuplicate;
                        isDuplicate = _.some(engine.index.datums, function (match) {
                            return engine.dupDetector(remoteMatch, match);
                        });
                        if (!isDuplicate) {
                            engine.add(remoteMatch);
                        }
                    });
                });

                $scope.$on("typeahead:selected", function (event, suggestion, dataset) {
                    $scope.searchTerm = $scope.searchTerm.value;
                    $scope.submit();
                });

                $scope.$watch("searchTerm", function (newValue, oldValue) {
                    // Check if this is init of the watch

                    if (!angular.isDefined(newValue) || !angular.isDefined(oldValue)) {
                        console.log("[SearchController] => $watch(searchTerm): newValue or oldValu is not defined");

                        return;
                    }

                    if (newValue === oldValue) {
                        console.log("[SearchController] => $watch(searchTerm): newValue is the same as oldValue");

                        return;
                    }

                    if (newValue == null || !angular.isUndefined(newValue.value)) {
                        return;
                    }

                    if (newValue.length >= 2) {
                        searchDataService.lookForPossibleWords(newValue);
                    } else {
                        $scope.possibleWordsResult = [];
                    }

                    console.log("[SearchController] => $watch(searchTerm): Triggered with value: " + newValue);
                });

                //$scope.autoCompleteSelect = function(label, value) {
                //    $scope.searchTerm = value;
                //};

                $scope.submit = function () {
                    var url = $scope.submitUrl;
                    var newUrl = url + "?" + searchQueryKey + "=" + encodeURIComponent($scope.searchTerm);
                    location.replace(newUrl);
                };

                // Typeahead options object
                $scope.settings = {
                    hint: true,
                    highlight: true
                };

                $scope.dataset = {
                    displayKey: 'value',
                    source: engine.ttAdapter()
                };
            },

            contentSearchResultController: function ($scope, searchDataService) {
                $scope.searchModel = searchDataService.getLookForContentResult();
                $scope.searchCount = 0;
                $scope.searchTime = 0;

                searchDataService.onSearchTermUpdate($scope, function () {
                    // update the local scope model

                    var value = searchDataService.getSearchTerm();

                    if (value.length >= 2) {
                        searchDataService.lookForContent(value);

                        console.log("[ContentSearchResultController] => onSearchTermUpdate: LookForContent(" + value + ") LookForContent updated.");
                    } else {
                        // Clear previus results
                        $scope.searchModel.results = [];
                    }
                });

                // We update the model when new model data is available
                searchDataService.onLookForContentResultUpdate($scope, function (context, data) {
                    $scope.searchModel = data;

                    if (!angular.isUndefined(data.results)) {
                        $scope.searchCount = data.results.length;
                    }

                    if (!angular.isUndefined(data.searchTime)) {
                        $scope.searchTime = data.searchTime;
                    }
                });
            },

            imageSearchResultController: function ($scope, searchDataService) {
                $scope.searchTerm = searchDataService.getSearchTerm();
                $scope.searchModel = searchDataService.getLookForImagesResult();

                searchDataService.onSearchTermUpdate($scope, function () {
                    // update the local scope model

                    var value = searchDataService.getSearchTerm();

                    if (value.length >= 2) {
                        searchDataService.lookForImages(value);
                    } else {
                        // Clear previus results
                        $scope.searchModel.results = [];
                    }

                    console.log("[ImageSearchResultController] => onSearchTermUpdate: LookForImages(" + value + ") lookForImagesResult updated.");
                });

                // We update the model when new model data is available
                searchDataService.onLookForImagesResultUpdate($scope, function (context, data) {
                    $scope.searchModel = data;
                });
            },

            fileSearchResultController: function ($scope, searchDataService) {
                $scope.searchTerm = searchDataService.getSearchTerm();
                $scope.searchModel = searchDataService.getLookForFilesResult();

                searchDataService.onSearchTermUpdate($scope, function () {
                    // update the local scope model

                    var value = searchDataService.getSearchTerm();

                    if (value.length >= 2) {
                        searchDataService.lookForFiles(value);
                    } else {
                        // Clear previus results
                        $scope.searchModel.results = [];
                    }

                    console.log("[FileSearchResultController] => onSearchTermUpdate: LookForFiles(" + value + ") lookForFilesResult updated.");
                });

                // We update the model when new model data is available
                searchDataService.onLookForFilesResultUpdate($scope, function (context, data) {
                    $scope.searchModel = data;
                });
            },

            init: function () {
                angular.module("baseApp")
                    .controller("BaseController", [view.controllers.baseController])
                    .controller("SearchPanelController", ["$scope", "$rootScope", "searchDataService", "configService", view.controllers.searchPanelController])
                    .controller("SearchController", ["$scope", "searchDataService", "configService", view.controllers.searchController])
                    .controller("ContentSearchResultController", ["$scope", "searchDataService", view.controllers.contentSearchResultController])
                    .controller("ImageSearchResultController", ["$scope", "searchDataService", view.controllers.imageSearchResultController])
                    .controller("FileSearchResultController", ["$scope", "searchDataService", view.controllers.fileSearchResultController]);
            }
        },

        directives: {
            autocomplete: function ($timeout) {
                // ToDo: add a wait x until accepting new updates
                return {
                    restrict: "A",
                    scope: {
                        autocomplete: "=",
                        callback: "&autocompleteSelect"
                    },
                    link: function (scope, element, attrs) {
                        $timeout(function () {
                            scope.$watch("autocomplete", function (newValue, oldValue) {
                                if (scope.IsNullOrUndefined(newValue) && newValue !== oldValue) {
                                    console.log("[autocomplete] => $watch(autocompleteList): Init");
                                }

                                var options = {};

                                if (angular.isDefined(attrs["autocompleteMy"])) {
                                    options.my = attrs["autocompleteMy"];
                                }

                                if (angular.isDefined(attrs["autocompleteAt"])) {
                                    options.at = attrs["autocompleteAt"];
                                }

                                if (angular.isDefined(attrs["autocompleteCollision"])) {
                                    options.collision = attrs["autocompleteCollision"];
                                }

                                element.autocomplete({
                                    position: options, //{ my: scope.my, at: scope.at, collision: scope.collision },
                                    source: newValue,
                                    minLength: 2,
                                    select: function (event, ui) {
                                        if (ui.item == null) {
                                            return;
                                        }

                                        if (angular.isDefined(scope.callback)) {
                                            scope.callback({ label: ui.item.label, value: ui.item.value });
                                        }
                                    },
                                });

                            }, true);

                            console.log("[directive:ngWordsAutoComplete] => link");
                        });
                    }
                };
            },

            tabs: function () {
                return {
                    restrict: "A",
                    scope: {
                        beforeActivateCallback: "&tabsBeforeActivate"
                    },
                    link: function (scope, elm, attrs) {
                        var element = $(elm[0]);

                        $(element).tabs({
                            beforeActivate: function (event, ui) {
                                if (angular.isDefined(scope.beforeActivateCallback)) {
                                    scope.beforeActivateCallback({ event: event, ui: ui });
                                }
                            }
                        });
                    }
                };
            },

            watchRepeat: function () {
                return function (scope, element, attrs) {
                    //How to watch ngRepeat: http://stackoverflow.com/questions/13471129/angularjs-ng-repeat-finish-event
                    scope.$watch("$last + $first + $middle", function (value) {
                        if (value) {
                            scope.$emit("RepeatFinished");
                            scope.$broadcast("RepeatFinished");
                        }
                    });
                };
            },
            
            init: function () {
                angular.module("baseApp")
                    .directive("autocomplete", ["$timeout", view.directives.autocomplete])
                    .directive("tabs", view.directives.tabs)
                    .directive("watchRepeat", view.directives.watchRepeat);
            }
        },

        filters: {
            shortCapitalize: function () {
                return function (input, scope) {
                    if (input != null) {
                        return input.substring(0, 1).toUpperCase() + input.substring(1, 3);
                    }

                    return null;
                };
            },

            init: function () {
                angular.module("baseApp")
                    .filter("shortCapitalize", view.filters.shortCapitalize);
            }
        },
    };

    // initiate angular
    view.controllers.init();
    view.directives.init();
    view.filters.init();
})(jQuery);