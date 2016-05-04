/// <reference path="../../lib/jquery-1.10.2.js" />
/// <reference path="../../lib/angular.js" />

(function ($) {

angular.module("baseApp.configuration", ["ngCookies"]);

function configService($http, $q, $cookies) {
    // Private
    var siteConstants = null;
    var localStore = {};
    var url = window.location.protocol + "//" + window.location.host + "/" + "umbraco/surface/configsurface/index";
    var cookieKey = "configsKey";
    var useCookieStore = true;
    var loadingInProgress = false;
    var deferred = $q.defer();

    var getCookieConfigs = function () {
        if (useCookieStore) {
            var cookieValue = $cookies[cookieKey];

            if (cookieValue) {
                return cookieValue;
            }
        }

        return null;
    };

    var setCookieConfigs = function (data) {
        if (useCookieStore) {
            $cookies[cookieKey] = data;
        }
    };

    var ensureConfigs = function () {
        var tryGetCookie = getCookieConfigs();

        if (tryGetCookie) {
            deferred.resolve(tryGetCookie);

            return deferred.promise;
        }

        if (siteConstants) {
            deferred.resolve(siteConstants);

            return deferred.promise;
        }

        // No siteConstants found, load them.
        if (!loadingInProgress) {
            loadingInProgress = true;

            $http.post(url).success(function (data) {

                siteConstants = {};

                // Assign the configuration keys with the correct properties from the server.
                angular.forEach(data, function (value, key) {
                    siteConstants[key] = value;
                });

                setCookieConfigs(siteConstants);

                deferred.resolve(siteConstants);

                loadingInProgress = false;
            });
        }

        return deferred.promise;
    };

    var getLocalConfig = function (key) {
        return localStore[key];
    };

    var addLocalConfig = function (key, value) {
        var tryGetValue = getLocalConfig(key);

        if (angular.isUndefined(tryGetValue) && !angular.isDefined(tryGetValue)) {
            localStore[key] = value;
        }
    };

    var setLocalConfig = function (key, value) {
        localStore[key] = value;
    };

    // Public
    return {
        EnsureConfigs: ensureConfigs,
        getLocalConfig: getLocalConfig,
        addLocalConfig: addLocalConfig,
        setLocalConfig: setLocalConfig
    };
}
angular.module("baseApp.configuration").factory("configService", ["$http", "$q", "$cookies", configService]);

})(jQuery);