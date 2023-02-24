angular.module('MVCApp')
    .service('EmployeeService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        //Get All Employees
        list.GetAllEmployees = function (/*employeeDetailsParams*/)
        {
            return $http({
                method: 'GET',
                //method:'POST',
                url: $rootScope.apiURL + '/Employee/GetAllEmployees/',
                /*data:JSON.stringify(employeeDetailsParams)*/
            });
        }

        // Get All  list of Employees
        list.GetEmployeeList = function (isGetAll)
        {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Employee/GetEmployeeList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
            });
        }

        // Add/Update Employee Details
        list.SaveEmployeeDetails = function (employeeDetail) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/SaveEmployeeDetails',
                data: JSON.stringify(employeeDetail)
            });
        }

        // Get Employees By Id
        list.GetEmployeeById = function (employeeId) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Employee/GetEmployeeById?employeeId=' + employeeId
            });
        };

        return list;
    }]);