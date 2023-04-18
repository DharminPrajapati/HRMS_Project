angular.module('MVCApp')
    .service('CompanyMasterService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        // Get All list of company Details
        list.GetAllCompanyMaster = function (CompanyMasterDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/CompanyMaster/GetAllCompanyMaster/',
                data: JSON.stringify(CompanyMasterDetailsParams)
            });
        };
        // Get All list of Department Details
        list.GetCompanyList = function (isGetAll) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/CompanyMaster/GetCompanyList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
            });
        };



        // Add/Update Department Details
        list.SaveCompanyMasterDetails = function (CompanyDetail) {
            debugger
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/CompanyMaster/SaveCompanyMasterDetails',
                data: JSON.stringify(CompanyDetail)
            });
        }

        // Get Department Items
        list.GetCompanyMasterById = function (CompanyMasterId) {

            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/CompanyMaster/GetCompanyMasterById?CompanyMasterId=' + CompanyMasterId
            });
        };

        return list;
    }]);