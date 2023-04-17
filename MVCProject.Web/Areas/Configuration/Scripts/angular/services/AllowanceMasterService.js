angular.module('MVCApp')
    .service('AllowanceMasterService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        // Get All list of Department Details
        list.GetAllowanceMaster = function (AllowanceMasterDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/AllowanceMaster/GetAllowanceMaster/',
                data: JSON.stringify(AllowanceMasterDetailsParams)
            });
        };
        //// Get All list of Department Details
        //list.GetDepartmentList = function (isGetAll) {
        //    return $http({
        //        method: 'GET',
        //        url: $rootScope.apiURL + '/Department/GetDepartmentList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
        //    });
        //};



        // Add/Update Department Details
        list.SaveAllowanceDetails = function (AllowanceDetail) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/AllowanceMaster/SaveAllowanceMasterDetails',
                data: JSON.stringify(AllowanceDetail)
            });
        }

        // Get Department Items
        list.GetAllowanceMasterById = function (AllowanceId) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/AllowanceMaster/GetAllowanceMasterById?AllowanceId=' + AllowanceId
            });
        };

        return list;
    }]);