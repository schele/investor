﻿var app = angular.module("umbraco")
.controller("DropdownForInvestmentsEditorController", function ($scope) {
    if ($scope.model.value === undefined || $scope.model.value === null || $scope.model.value === "") {
        // set default value
        $scope.model.value = "index, follow";
    }
});