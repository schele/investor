/// <reference path="../../Umbraco/lib/angular/1.1.5/angular.js" />
/// <reference path="../../Umbraco/lib/jquery/jquery-2.0.3.min.js" />

function MacroContainerEditor($scope, $timeout, dialogService, entityResource, macroService, macroResource, assetsService, notificationsService) {
    $scope.renderModel = [];
    $scope.isEditMode = false;
    assetsService.loadCss("/App_Plugins/MacroContainerEditor/macrocontainereditor.css", $scope);

    //setup the default config
    var config = {
        allowedMacros: []
    };

    //map the user config
    angular.extend(config, $scope.model.config);

    //map back to the model
    $scope.model.config = config;

    if ($scope.model.value) {
        var macros = $scope.model.value;
        
        angular.forEach(macros, function (syntax, key) {
            if (syntax && syntax.length > 10) {
                // Backward Compatible
                syntax = syntax.replace("[Module]", "Module");

                var parsed = macroService.parseMacroSyntax(syntax);
                
                if (!parsed) {
                    parsed = {};
                }

                parsed = decodeParsedMacro(parsed);
                parsed.syntax = syntax;
                collectDetails(parsed);
                $scope.renderModel.push(parsed);
            }
        });
    }
    
    function decodeParsedMacro(parsed) {
        if (parsed.macroParamsDictionary) {
            var key;
            for (key in parsed.macroParamsDictionary) {
                if (parsed.macroParamsDictionary.hasOwnProperty(key)) {
                    var value = decodeURIComponent(parsed.macroParamsDictionary[key]);
                    value = _.unescape(value);
                    
                    if (value.detectIsJson()) {
                        var tmpValue = value;
                        try {
                            value = angular.fromJson(value);
                        } catch (e) {
                            // not json
                            value = tmpValue;
                        }
                    }

                    parsed.macroParamsDictionary[key] = value;
                }
            }
        }

        return parsed;
    }

    function isAllowedMacro(macroId) {
        for (var i = 0; i < $scope.model.config.allowedMacros.length; i++) {
            var allowedMacro = $scope.model.config.allowedMacros[i];

            if (allowedMacro.id === macroId) {
                return true;
            }
        }

        return false;
    }

    function collectDetails(macro) {
        macro.details = "";
        macro.displayName = macro.macroAlias.replace(/([a-z])([A-Z])/g, '$1 $2');

        if (macro.macroParamsDictionary) {
            angular.forEach((macro.macroParamsDictionary), function (value, key) {
                if (!_.isString(value)) {
                    try {
                        value = angular.toJson(value);
                    } catch (e) {
                        // not json
                    }
                }
                
                var tmpValue = "";
                var parsedHtml = $.parseHTML(decodeURIComponent(value));
                var element = $(parsedHtml);
                
                // Fix images
                element.find('img').each(function () {
                    $(this).removeAttr('style');
                });

                $.each(element, function() {
                    if (!angular.isUndefined(this.outerHTML)) {
                        tmpValue += this.outerHTML;
                    }
                    else if (!angular.isUndefined(this.wholeText)) {
                        tmpValue += this.wholeText;
                    }
                });

                macro.details += key + ": " + tmpValue + " ";
            });
        }
    }

    function openDialog(index) {
        var dialogData = {};

        if (!angular.isUndefined(index)) {
            var macro = $scope.renderModel[index];
            dialogData = { macroData: macro };
        }

        dialogService.macroPicker({
            scope: $scope,
            dialogData: dialogData,
            callback: function (data) {
                collectDetails(data);

                //update the raw syntax and the list...
                if (!angular.isUndefined(index)) {
                    $scope.renderModel[index] = data;
                } else {
                    $scope.renderModel.push(data);
                }
                
                //set the isEditMode to false
                $scope.isEditMode = false;
            },
            closeCallback: function () {
                //set the isEditMode to false
                $scope.isEditMode = false;
            }
        });

        // We need to get the insertMacroForm and strip out all macros that are not allowed if allowedMacros is not empty.
        if ($scope.model.config.allowedMacros.length !== 0) {
            $timeout(function () {
                var element = angular.element("[ng-controller='Umbraco.Dialogs.InsertMacroController']:last");
                var dialogScope = element.scope();
                
                $(".umb-overlay").each(function () {
                    $(this).width(726);
                    console.log("yoooo");
                });


                var validMacros = [];

                for (var i = 0; i < dialogScope.macros.length; i++) {
                    var dialogMacro = dialogScope.macros[i];

                    if (isAllowedMacro(dialogMacro.id)) {
                        validMacros.push(dialogMacro);
                    }
                }

                angular.copy(validMacros, dialogScope.macros);
            }, 500);
        }
    }

    $scope.edit = function (index) {
        if ($scope.isEditMode) {
            notificationsService.warning("Warning", "You are already editing an item.");

            return;
        }

        $scope.isEditMode = true;

        openDialog(index);
    };

    $scope.add = function () {
        openDialog();
    };

    $scope.remove = function (index) {
        //if ($scope.isEditMode) {
        //    notificationsService.warning("Warning", "You must first stop editing before you can remove items.");

        //    return;
        //}

        $scope.renderModel.splice(index, 1);
        $scope.macros.splice(index, 1);
    };

    $scope.clear = function () {
        $scope.model.value = [];
        $scope.renderModel = [];
        $scope.macros = [];
    };

    $scope.$on("formSubmitting", function (ev, args) {
        $scope.model.value = [];

        angular.forEach($scope.renderModel, function (value, key) {
            $scope.model.value.push(value.syntax);
        });
    });
}
angular.module("umbraco").controller("MacroContainerEditor", MacroContainerEditor);