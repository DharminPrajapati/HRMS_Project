angular.module('MVCApp')
    .service('DepartmentService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        // Get All list of Department Details
        list.GetAllDepartments = function (departmentDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Department/GetAllDepartments/',
                data: JSON.stringify(departmentDetailsParams)
            });
        };
        // Get All list of Department Details
        list.GetDepartmentList = function (isGetAll) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Department/GetDepartmentList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
            });
        };



        // Add/Update Department Details
        list.SaveDepartmentDetails = function (departmentDetail) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Department/SaveDepartmentDetails',
                data: JSON.stringify(departmentDetail)
            });
        }

        // Get Department Items
        list.GetDepartmentById = function (departmentId) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Department/GetDepartmentById?departmentId=' + departmentId
            });
        };

        // Excel
        list.CreateExcelReport = function () {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Department/CreateEmployeeListReport'
            });
        };
        return list;
    }]);