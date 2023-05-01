angular.module('MVCApp')
    .service('ReportsService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        list.GetAllEmployees = function (employeeDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/GetAllEmployees/',
                data: JSON.stringify(employeeDetailsParams)
            });
        }

        // Get Employee List using Sp
        list.GetEmployeeDetails = function (employeeDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/GetEmployeeDetails/',
                data: JSON.stringify(employeeDetailsParams)
            });
        }
        //Create Excel Report For Employees
        list.CreateExcelReport = function () {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Reports/CreateEmployeeListReport'
            });
        };

        list.ExportPDF = function () {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Reports/ExportPDF'
            });
        }

        return list;
    }]);