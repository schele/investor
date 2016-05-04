(function ($) {
    angular.module("baseApp.services", ["baseApp.configuration", "ngCookies", "siyfion.sfTypeahead"], function () {
    })
.run(["configService", "$rootScope", function (configService, $rootScope) {
    // Module Init

    // Global local configs
    var currentCulture = $("html").data("culture");
    configService.addLocalConfig("currentCulture", currentCulture);

    // Prototypes
    $rootScope.constructor.prototype.IsNullOrUndefined = function (object) {
        return !(object !== null && object !== undefined);
    };

    $rootScope.constructor.prototype.GetQueryStringValue = function (param) {

        var sPageUrl = window.location.search.substring(1);
        var sUrlVariables = sPageUrl.split('&');

        for (var i = 0; i < sUrlVariables.length; i++) {
            var sParameterName = sUrlVariables[i].split('=');

            if (sParameterName[0] == param) {
                return decodeURIComponent(sParameterName[1]);
            }
        }

        return null;
    };
}]);

    var view = {
        services: {
            umbracoHelper: function ($q, $http) {
                function transform(data) {
                    return $.param(data);
                }

                function getUrl() {
                    return window.location.protocol + "//" + window.location.host + "/";
                }

                function getEncryptedRoute(controller, action, area, additionalRouteVals) {
                    var deferredData = $q.defer();

                    var url = getUrl();

                    url += "umbraco/Surface/AjaxHelper/GetEncryptedRoute";

                    $http.post(url, {
                        controllerName: controller,
                        controllerAction: action,
                        area: area,
                        additionalRouteVals: additionalRouteVals
                    }).success(function (data) {
                        //success, resolve your promise here
                        deferredData.resolve(data);
                    }).error(function (err) {
                        //error, use reject here
                        deferredData.reject(err);
                    });

                    return deferredData.promise;
                }

                function getUrlToSurfaceController(controller, action) {
                    var url = getUrl();

                    return url + "umbraco/Surface/" + controller + "/" + action;
                }

                function getUrlToApiController(controller, action) {
                    var url = getUrl();

                    return url + "umbraco/api/" + controller + "api" + "/" + action;
                }

                // New way of calling a surface controller but with UmbracoContext support
                function surfaceControllerPost(controller, action, parameters) {
                    var deferredData = $q.defer();

                    getEncryptedRoute(controller, action, "", parameters).then(function (response) {
                        $http.post(location.href, response, { headers: { "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8" }, transformRequest: transform })
                            .success(function (data) {
                                deferredData.resolve(data);
                            }).error(function (err) {
                                deferredData.reject(err);
                            });
                    });

                    return deferredData.promise;
                }

                function surfaceControllerGet(controller, action, parameters) {
                    var deferredData = $q.defer();

                    getEncryptedRoute(controller, action, "", parameters).then(function (response) {
                        $http({ url: location.href, method: "GET", params: response, headers: { "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8" } })
                            .success(function (data, status, headers, config) {
                                deferredData.resolve(data);
                            })
                            .error(function (data, status, headers, config) {
                                deferredData.reject(data);
                            });
                    });

                    return deferredData.promise;
                }

                return {
                    getUrlToSurfaceController: getUrlToSurfaceController,
                    getUrlToApiController: getUrlToApiController,
                    surfaceControllerPost: surfaceControllerPost,
                    surfaceControllerGet: surfaceControllerGet
                };
            },

            searchDataService: function ($http, $rootScope, umbracoHelper, configService) {
                // Private
                var searchTerm = "";
                var possibleWordsResult = [];
                var lookForResult = [];
                var lookForContentResult = [];
                var lookForImagesResult = [];
                var lookForFilesResult = [];

                var eventHandlers = {
                    broadcastSearchTerm: function (newValue) {
                        $rootScope.$broadcast("searchDataService.searchTermUpdate", newValue);
                    },
                    broadcastPossibleWordsResult: function (newValue) {
                        $rootScope.$broadcast("searchDataService.possibleWordsResultUpdate", newValue);
                    },
                    broadcastLookForResult: function (newValue) {
                        $rootScope.$broadcast("searchDataService.lookForResultUpdate", newValue);
                    },
                    broadcastLookForContentResult: function (newValue) {
                        $rootScope.$broadcast("searchDataService.lookForContentResultUpdate", newValue);
                    },
                    broadcastLookForImagesResult: function (newValue) {
                        $rootScope.$broadcast("searchDataService.lookForImagesResultUpdate", newValue);
                    },
                    broadcastLookForFilesResult: function (newValue) {
                        $rootScope.$broadcast("searchDataService.lookForFilesResultUpdate", newValue);
                    },
                    broadcastClearResults: function () {
                        $rootScope.$broadcast("searchDataService.clearResults");
                    },
                };

                var eventSubscribers = {
                    onSearchTermUpdate: function ($scope, callback) {
                        $scope.$on("searchDataService.searchTermUpdate", function (context, data) {
                            callback(context, data);
                        });
                    },
                    onPossibleWordsResultUpdate: function ($scope, callback) {
                        $scope.$on("searchDataService.possibleWordsResultUpdate", function (context, data) {
                            callback(context, data);
                        });
                    },
                    onLookForResultUpdate: function ($scope, callback) {
                        $scope.$on("searchDataService.lookForResultUpdate", function (context, data) {
                            callback(context, data);
                        });
                    },
                    onLookForContentResultUpdate: function ($scope, callback) {
                        $scope.$on("searchDataService.lookForContentResultUpdate", function (context, data) {
                            callback(context, data);
                        });
                    },
                    onLookForImagesResultUpdate: function ($scope, callback) {
                        $scope.$on("searchDataService.lookForImagesResultUpdate", function (context, data) {
                            callback(context, data);
                        });
                    },
                    onLookForFilesResultUpdate: function ($scope, callback) {
                        $scope.$on("searchDataService.lookForFilesResultUpdate", function (context, data) {
                            callback(context, data);
                        });
                    },
                    onClearResultsUpdate: function ($scope, callback) {
                        $scope.$on("searchDataService.clearResults", function (context) {
                            callback(context);
                        });
                    },
                };

                var setLookForPossibleWords = function (data) {
                    angular.copy(data, possibleWordsResult);
                    eventHandlers.broadcastPossibleWordsResult(data);
                };

                var setLookForResult = function (data) {
                    angular.copy(data, lookForResult);
                    eventHandlers.broadcastLookForResult(data);
                };

                var setLookForContentResult = function (data) {
                    angular.copy(data, lookForContentResult);
                    eventHandlers.broadcastLookForContentResult(data);
                };

                var setLookForImagesResult = function (data) {
                    angular.copy(data, lookForImagesResult);
                    eventHandlers.broadcastLookForImagesResult(data);
                };

                var setLookForFilesResult = function (data) {
                    angular.copy(data, lookForFilesResult);
                    eventHandlers.broadcastLookForFilesResult(data);
                };

                var clearResults = function () {
                    setLookForContentResult([]);
                    setLookForImagesResult([]);
                    setLookForFilesResult([]);
                    eventHandlers.broadcastClearResults();
                };

                // Public
                return {
                    getSearchTerm: function () {
                        return searchTerm;
                    },
                    setSearchTerm: function (value) {
                        searchTerm = value;
                        eventHandlers.broadcastSearchTerm(value);
                    },
                    getPossibleWordsResult: function () {
                        return possibleWordsResult;
                    },
                    getLookForResult: function () {
                        return lookForResult;
                    },
                    getLookForContentResult: function () {
                        return lookForContentResult;
                    },
                    getLookForImagesResult: function () {
                        return lookForImagesResult;
                    },
                    getLookForFilesResult: function () {
                        return lookForFilesResult;
                    },
                    clearResults: clearResults,
                    onSearchTermUpdate: eventSubscribers.onSearchTermUpdate,
                    onPossibleWordsResultUpdate: eventSubscribers.onPossibleWordsResultUpdate,
                    onLookForResultUpdate: eventSubscribers.onLookForResultUpdate,
                    onLookForContentResultUpdate: eventSubscribers.onLookForContentResultUpdate,
                    onLookForImagesResultUpdate: eventSubscribers.onLookForImagesResultUpdate,
                    onLookForFilesResultUpdate: eventSubscribers.onLookForFilesResultUpdate,
                    onClearResultsUpdate: eventSubscribers.onClearResultsUpdate,
                    lookForPossibleWords: function (term, callback) {
                        var url = umbracoHelper.getUrlToSurfaceController("search", "lookforpossiblewords");

                        if (term === "") {
                            setLookForPossibleWords([]);
                            return;
                        }

                        $http.post(url, {
                            searchTerm: term,
                            culture: configService.getLocalConfig("currentCulture")
                        }).success(function (data, status, headers, config) {
                            if (angular.isDefined(data.error)) {
                                console.log("[lookForPossibleWords] Error: " + data.errorMessage);

                                setLookForPossibleWords([]);

                                return;
                            } else {
                                setLookForPossibleWords(data);
                            }

                            if (angular.isDefined(callback)) {
                                callback(possibleWordsResult);
                            }
                        }).error(function (data, status, headers, config) {
                        });
                    },
                    lookFor: function (term, callback) {
                        //if (term === "") {
                        //    setLookForResult([]);
                        //    return;
                        //}

                        //umbracoHelper.surfaceControllerRequest("Search", "LookFor", { searchTerm: term }).then(function (response) {
                        //    if (angular.isDefined(response.error)) {
                        //        console.log("[lookFor] Error: " + response.errorMessage);

                        //        setLookForResult([]);
                        //    } else {
                        //        setLookForResult(response);
                        //    }

                        //    if (angular.isDefined(callback)) {
                        //        callback(lookForResult);
                        //    }
                        //});

                        var url = umbracoHelper.getUrlToSurfaceController("search", "lookfor");

                        if (term === "") {
                            setLookForResult([]);
                            return;
                        }

                        $http.post(url, {
                            searchTerm: term,
                            culture: configService.getLocalConfig("currentCulture")
                        }).success(function (data, status, headers, config) {
                            if (angular.isDefined(data.error)) {
                                console.log("[lookFor] Error: " + data.errorMessage);

                                setLookForResult([]);

                                return;
                            } else {
                                setLookForResult(data);
                            }

                            if (callback == "function") {
                                callback(lookForResult);
                            }
                        }).error(function (data, status, headers, config) {
                        });
                    },
                    lookForContent: function (term, callback) {
                        //if (term === "") {
                        //    setLookForContentResult([]);
                        //    return;
                        //}

                        //umbracoHelper.surfaceControllerRequest("Search", "LookForContent", { searchTerm: term }).then(function (response) {
                        //    if (angular.isDefined(response.error)) {
                        //        console.log("[lookForContent] Error: " + response.errorMessage);

                        //        setLookForContentResult([]);
                        //    } else {
                        //        setLookForContentResult(response);
                        //    }

                        //    if (angular.isDefined(callback)) {
                        //        callback(lookForContentResult);
                        //    }
                        //});

                        //var url = umbracoHelper.getUrlToSurfaceController("search", "lookforcontent");

                        if (term === "") {

                            return;
                        }

                        umbracoHelper.surfaceControllerPost("search", "lookforcontent", { searchTerm: term })
                            .then(function(data) {
                                if (angular.isDefined(data.error)) {
                                    console.log("[lookForContent] Error: " + data.errorMessage);

                                    setLookForContentResult([]);
                                } else {
                                    setLookForContentResult(data);
                                }

                                if (angular.isDefined(callback)) {
                                    callback(lookForContentResult);
                                }
                            }, function(error) {
                                // error
                            console.log(error);
                        });


                        //$http.post(url, {
                        //    searchTerm: term,
                        //    culture: configService.getLocalConfig("currentCulture")
                        //}).success(function (data) {
                        //    if (angular.isDefined(data.error)) {
                        //        console.log("[lookForContent] Error: " + data.errorMessage);

                        //        setLookForContentResult([]);
                        //    } else {
                        //        setLookForContentResult(data);
                        //    }

                        //    if (angular.isDefined(callback)) {
                        //        callback(lookForContentResult);
                        //    }
                        //}).error(function (data, status, headers, config) {
                        //});
                    },
                    lookForImages: function (term, callback) {
                        //if (term === "") {
                        //    setLookForImagesResult([]);
                        //    return;
                        //}

                        //umbracoHelper.surfaceControllerRequest("Search", "LookForImages", { searchTerm: term }).then(function (response) {
                        //    if (angular.isDefined(response.error)) {
                        //        console.log("[lookForImages] Error: " + response.errorMessage);

                        //        setLookForImagesResult([]);
                        //    } else {
                        //        setLookForImagesResult(response);
                        //    }

                        //    if (angular.isDefined(callback)) {
                        //        callback(lookForImagesResult);
                        //    }
                        //});

                        var url = umbracoHelper.getUrlToSurfaceController("search", "lookforimages");

                        if (term === "") {
                            setLookForImagesResult([]);
                            return;
                        }

                        $http.post(url, {
                            searchTerm: term,
                            culture: configService.getLocalConfig("currentCulture")
                        }).success(function (data, status, headers, config) {
                            if (angular.isDefined(data.error)) {
                                console.log("[lookForImages] Error: " + data.errorMessage);

                                setLookForImagesResult([]);

                                return;
                            } else {
                                setLookForImagesResult(data);
                            }

                            if (angular.isDefined(callback)) {
                                callback(lookForImagesResult);
                            }
                        }).error(function (data, status, headers, config) {
                        });
                    },
                    lookForFiles: function (term, callback) {
                        //if (term === "") {
                        //    setLookForFilesResult([]);
                        //    return;
                        //}

                        //umbracoHelper.surfaceControllerRequest("Search", "LookForFiles", { searchTerm: term }).then(function (response) {
                        //    if (angular.isDefined(response.error)) {
                        //        console.log("[lookForImages] Error: " + response.errorMessage);

                        //        setLookForFilesResult([]);
                        //    } else {
                        //        setLookForFilesResult(response);
                        //    }

                        //    if (angular.isDefined(callback)) {
                        //        callback(lookForFilesResult);
                        //    }
                        //});

                        var url = umbracoHelper.getUrlToSurfaceController("search", "lookforfiles");

                        if (term === "") {
                            setLookForFilesResult([]);
                            return;
                        }

                        $http.post(url, {
                            searchTerm: term,
                            culture: configService.getLocalConfig("currentCulture")
                        }).success(function (data, status, headers, config) {
                            if (angular.isDefined(data.error)) {
                                console.log("[lookForFiles] Error: " + data.errorMessage);

                                setLookForFilesResult([]);

                                return;
                            } else {
                                setLookForFilesResult(data);
                            }

                            if (angular.isDefined(callback)) {
                                callback(lookForFilesResult);
                            }
                        }).error(function (data, status, headers, config) {
                        });
                    }
                };
            },

            newsArchiveDataService: function ($http, $rootScope, umbracoHelper) {
                var newsList = [];
                var years = [];
                var currentCulture = "en";

                var eventHandlers = {
                    broadcastNewsList: function (newValue) {
                        $rootScope.$broadcast("newsArchiveDataService.newsListUpdate", newValue);
                    },
                    broadcastYears: function (newValue) {
                        $rootScope.$broadcast("newsArchiveDataService.yearsUpdate", newValue);
                    },

                };

                var eventSubscribers = {
                    onNewsListUpdate: function ($scope, callback) {
                        $scope.$on("newsArchiveDataService.newsListUpdate", function (context, data) {
                            callback(context, data);
                        });
                    },
                    onYearsUpdate: function ($scope, callback) {
                        $scope.$on("newsArchiveDataService.yearsUpdate", function (context, data) {
                            callback(context, data);
                        });
                    }
                };

                var setNewsList = function (data) {
                    angular.copy(data, newsList);
                    eventHandlers.broadcastNewsList(data);
                };

                var setYears = function (data) {
                    years = data;
                    eventHandlers.broadcastYears(data);
                };

                var setCulture = function (culture) {
                    currentCulture = culture;
                };

                var getCulture = function () {
                    return currentCulture;
                };

                // Public
                return {
                    onNewsListUpdate: eventSubscribers.onNewsListUpdate,
                    onYearsUpdate: eventSubscribers.onYearsUpdate,
                    getCurrentCulture: getCulture,
                    setCulture: setCulture,
                    getNews: function (year, month, callback) {
                        var url = umbracoHelper.getUrlToSurfaceController("newsarchive", "getnews");

                        $http.post(url, {
                            culture: currentCulture,
                            year: year,
                            month: month
                        }).success(function (data, status, headers, config) {
                            if (angular.isDefined(data.error)) {
                                console.log("[getNews(year)] Error: " + data.errorMessage);
                                setNewsList([]);

                                return;
                            } else {
                                setNewsList(data);
                            }

                            if (angular.isDefined(callback)) {
                                callback(newsList);
                            }
                        });
                    },
                    getYears: function (callback) {
                        var url = umbracoHelper.getUrlToSurfaceController("newsarchive", "getyears");

                        $http.post(url, {
                            culture: currentCulture
                        }).success(function (data, status, headers, config) {
                            if (angular.isDefined(data.error)) {
                                console.log("[getNews] Error: " + data.errorMessage);
                                setYears([]);

                                return;
                            } else {
                                setYears(data);
                            }

                            if (angular.isDefined(callback)) {
                                callback(data);
                            }
                        });
                    },
                };
            },

            calculatorService: function () {
                var m = {
                    isIncMoms: false,
                    momssats: 0.25,
                    skattReduction: 0.50,
                    materialMoms: 0.04,
                    resekostnad: 0
                };

                function prisCalc(styckpris, isIncMoms, momssats, skattReduction, materialMoms, resekostnad) {
                    this.isIncMoms = isIncMoms;
                    this.Moms = parseInt("1.0") + momssats;
                    this.skattReduction = skattReduction;
                    this.materialMoms = materialMoms;
                    this.Resekostnad = resekostnad;
                    this.CostPerKvm = 0;
                    var pris = new prisModel();

                    this.init = function () {
                        this.ChangePrice(styckpris);
                    };

                    this.ChangePrice = function (price) {
                        var a = pris.styckPris;
                        if (this.isIncMoms) {
                            a.exMoms = price / this.Moms;
                            a.incMoms = price;

                        } else {
                            a.exMoms = price;
                            a.incMoms = price * this.Moms;
                        }
                    };

                    this.Calculate = function (kvadratmeter) {

                        //beräkningar börjar här
                        var c = pris;
                        var cost1 = c.UtanReduction;
                        var cost2 = c.MedReduction;
                        var costMat = c.Material;

                        // beräkningar av 1 kvm = pris * värdet av kvadratmeter.
                        cost1.exMoms = parseFloat(kvadratmeter) * c.styckPris.exMoms;

                        // beräkna material kostnaden
                        costMat.exMoms = cost1.exMoms - (cost1.exMoms / (parseInt("1.0") + this.materialMoms));

                        // lägg till moms i material kostnaden
                        costMat.incMoms = costMat.exMoms * this.Moms;

                        // beräkna skattereductionen genom kostnaden av cost1 ex moms.
                        cost2.exMoms = (cost1.exMoms - costMat.exMoms) * this.skattReduction;

                        // lägg till övriga kostnader
                        cost1.exMoms += (/*costMat.exMoms + */this.Resekostnad);
                        cost2.exMoms += (costMat.exMoms + this.Resekostnad);

                        // beräkna kostnaden inc moms.
                        cost1.incMoms = cost1.exMoms * this.Moms;
                        cost2.incMoms = cost2.exMoms * this.Moms;

                        // avrunda upot till närmaster heltal.
                        cost1.exMoms = avrundaPengar(cost1.exMoms);
                        cost2.exMoms = avrundaPengar(cost2.exMoms);

                        cost1.incMoms = avrundaPengar(cost1.incMoms);
                        cost2.incMoms = avrundaPengar(cost2.incMoms);

                        return pris;
                    };

                    function avrundaPengar(p) {
                        //avrunda upp till närmaste krona.
                        return (Math.ceil(p / 1)) * 1;
                    }

                    //Iniciate
                    this.init();

                    // Models

                    function prisModel() {
                        this.styckPris = new function () {
                            this.exMoms = 0;
                            this.incMoms = 0;
                        };
                        this.UtanReduction = new function () {
                            this.exMoms = 0;
                            this.incMoms = 0;
                        };
                        this.MedReduction = new function () {
                            this.exMoms = 0;
                            this.incMoms = 0;
                        };
                        this.Material = new function () {
                            this.exMoms = 0;
                            this.incMoms = 0;
                        };
                    }
                }

                function create(pris) {
                    return new prisCalc(pris, m.isIncMoms, m.momssats, m.skattReduction, m.materialMoms, m.resekostnad);
                }

                function setIsIncMoms(isIncMoms) {
                    m.isIncMoms = isIncMoms;

                    return this;
                }

                function setMomssats(momssats) {
                    m.momssats = momssats;

                    return this;
                }

                function setSkattReduction(skattReduction) {
                    m.skattReduction = skattReduction;

                    return this;
                }

                function setMaterialMoms(materialMoms) {
                    m.materialMoms = materialMoms;

                    return this;
                }

                function setResekostnad(resekostnad) {
                    m.resekostnad = resekostnad;

                    return this;
                }

                return {
                    SetIncVAT: setIsIncMoms,
                    SetVATRate: setMomssats,
                    SetTaxReduction: setSkattReduction,
                    SetMaterialVAT: setMaterialMoms,
                    SetTravelCost: setResekostnad,
                    Create: create
                };
            },

            calculatorServiceHour: function (umbracoHelper, configService) {
                return {
                    CalculatePriceHemstadning: function (houseSize, visitsPerMonth, discountRate) {
                        return umbracoHelper.surfaceControllerGet("PriceService", "CalculatePriceHemstadning", { houseSize: houseSize, visitsPerMonth: visitsPerMonth, discountRate: discountRate });
                    }
                };
            },



            init: function () {
                angular.module("baseApp.services")
                       .factory("umbracoHelper", ["$q", "$http", view.services.umbracoHelper])
                       .factory("searchDataService", ["$http", "$rootScope", "umbracoHelper", "configService", view.services.searchDataService])
                       .factory("newsArchiveDataService", ["$http", "$rootScope", "umbracoHelper", view.services.newsArchiveDataService])
                       .factory("calculatorService", [view.services.calculatorService])
                       .factory("calculatorServiceHour", ["umbracoHelper", "configService", view.services.calculatorServiceHour]);
            }
        }
    };
    // initiate angular services
    view.services.init();

})(jQuery);