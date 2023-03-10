angular.module('MVCApp')
    .service('SalaryService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        //Get All Employees
        //list.GetAllEmployees = function (/*employeeDetailsParams*/)
        //{
        //    return $http({
        //        method: 'GET',
        //        url: $rootScope.apiURL + '/Employee/GetAllEmployees/'
        //    });
        //}
        list.GetAllSalary = function (salaryDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Salary/GetAllSalary/',
                data: JSON.stringify(salaryDetailsParams)
            });
        }

        // Get All  list of Employees
        list.GetSalaryList = function (isGetAll) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Salary/GetSalaryList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
            });
        }

        // Add/Update Employee Details
        list.SaveSalaryDetails = function (SalaryDetail) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Salary/SaveSalaryDetails',
                data: JSON.stringify(SalaryDetail)
            });
        }

        // Get Employees By Id
        list.GetSalaryById = function (salaryId) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Salary/GetSalaryById?salaryId=' + salaryId
            });
        };

        //Get employee DropDown
        list.GetEmployeelist = function () {
            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/Salary/GetEmployeeDropDown'
            });
        };


        return list;
    }]);