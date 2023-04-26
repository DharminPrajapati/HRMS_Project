(function () {
    'use strict';

    angular.module("MVCApp").controller('DocumentCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'DocumentService', DocumentCtrl
    ]);

    function DocumentCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, EmployeeService) {
    }
})();
