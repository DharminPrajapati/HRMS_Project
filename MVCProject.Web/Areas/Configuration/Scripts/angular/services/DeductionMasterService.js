angular.module('MVCApp')
    .service('DeductionMasterService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        // Get All list of Department Details
        list.GetDeduction = function (DeductionDetailsParams) {

            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/DeductionMaster/GetDeduction/',
                data: JSON.stringify(DeductionDetailsParams)
            });
        };

        //// Get All list of Department Details
        //list.GetDepartmentList = function (isGetAll) {
        //    return $http({
        //        method: 'GET',
        //        url: $rootScope.apiURL + '/DeductionMaster/GetDepartmentList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
        //    });
        //};

        // Add/Update Department Details
        list.SaveDeductionDetails = function (deductionDetail) {

            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/DeductionMaster/SaveDeductionDetails',
                data: JSON.stringify(deductionDetail)
            });
        }

        // Get Department Items
        list.GetDeductionById = function (deductionId) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/DeductionMaster/GetDeductionById?deductionId=' + deductionId
            });
        };

        return list;
    }]);