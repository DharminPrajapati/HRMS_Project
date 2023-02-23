﻿angular.module("DorfKetalMVCApp")
.service("HWMService", ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];

    list.GetWasteTypeDDL = function () {
        return $http({
            method: "GET",
            url: $rootScope.apiURL + "/HWM/GetWasteCategotyForDropdown",
            params: null
        });
    };

    list.GetWasteCategotyDetails = function (CategoryId) {
        return $http({
            method: "GET",
            url: $rootScope.apiURL + "/HWM/GetWasteCategotyDetails?WasteCategoryId=" + CategoryId,
            params: null
        });
    };

    list.GetWasteGenerationDetails = function (WasteGenerationId) {
        return $http({
            method: "GET",
            url: $rootScope.apiURL + "/HWM/GetWasteDetails?WasteGenerationId=" + WasteGenerationId,
            params: null
        });
    };

    list.SaveWasteStorage = function (obj) {
        return $http({
            method: 'POST',
            dataType: 'json',
            url: $rootScope.apiURL + "/HWM/AddUpdateWasteStorage",
            data: JSON.stringify(obj)
        });
    };

    list.GetWasteStorageDDL = function () {
        return $http({
            method: "GET",
            url: $rootScope.apiURL + "/HWM/GetWasteStorageForDropdown",
            params: null
        });
    };

    return list;
} ]);