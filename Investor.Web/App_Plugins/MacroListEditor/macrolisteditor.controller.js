/// <reference path="../../Umbraco/lib/angular/1.1.5/angular.js" />
/// <reference path="../../Umbraco/lib/jquery/jquery-2.0.3.min.js" />

function MacroListEditorController($scope, entityResource, assetsService) {
    var macroExists = function (macroList, macroId) {
        for (var i = 0; i < macroList.length; i++) {
            if (macroList[i].id === macroId) {
                return true;
            }
        }

        return false;
    };

    var getMacroById = function (macroId) {
        for (var i = 0; i < $scope.macros.length; i++) {
            if ($scope.macros[i].id === macroId) {
                return $scope.macros[i];
            }
        }

        return null;
    };

    $scope.macros = [];
    $scope.selectedMacros = [];

    $scope.toggleMacro = function (macro) {
        if ($scope.selectedMacros.indexOf(macro) === -1) {
            $scope.selectedMacros.push(macro);
        } else {
            $scope.selectedMacros.splice($scope.selectedMacros.indexOf(macro), 1);
        }
    };

    $scope.isSelected = function(macro) {
        var indexOf = macroExists($scope.selectedMacros, macro.id);

        return indexOf;
    };

    $scope.sync = function() {
        var macroList = function() {
            var tmpMacroList = [];
            
            for (var i = 0; i < $scope.selectedMacros.length; i++) {
                var item = $scope.selectedMacros[i];
                tmpMacroList.push({ id: item.id });
            }

            return tmpMacroList;
        };

        $scope.model.value = macroList();
    };

    //listen for formSubmitting event (the result is callback used to remove the event subscription)
    var unsubscribe = $scope.$on("formSubmitting", function () {
        $scope.sync();
    });
    
    //when the element is disposed we need to unsubscribe!
    // NOTE: this is very important otherwise if this is part of a modal, the listener still exists because the dom 
    // element might still be there even after the modal has been hidden.
    $scope.$on("$destroy", function () {
        unsubscribe();
    });

    assetsService.loadCss("/App_Plugins/MacroListEditor/macrolisteditor.css");

    //get the macro list
    entityResource.getAll("Macro", null)
        .then(function(data) {

            $scope.macros = data;
            
            if (angular.isArray($scope.model.value)) {
                // Select previusly saved macros.

                for (var i = 0; i < $scope.model.value.length; i++) {
                    var item = $scope.model.value[i];
                    
                    // We don't want to select macros that no longer exists
                    if (macroExists(data, item.id)) {
                        var macro = getMacroById(item.id);

                        $scope.selectedMacros.push(macro);
                    }
                }
            }
        });    
}

angular.module("umbraco").controller("MacroListEditorController", MacroListEditorController);