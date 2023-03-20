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
        list.GetAllEmployees = function (employeeDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/GetAllEmployees/',
                data: JSON.stringify(employeeDetailsParams)
            });
        }

        //Get Salary With Employees
        list.GetEmployeeSalary = function (salaryDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Salary/GetEmployeeSalary/',
                data: JSON.stringify(salaryDetailsParams)
            });
        }
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

        //Get Designation DropDown
        list.GetDesignationlist = function () {

            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/Employee/GetDesignationDropDown'
            });
        };

        //Get Department DropDown
        list.GetDepartmentlist = function () {

            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/Employee/GetDepartmentDropDown'
            });
        };

        list.GetSalConfig = function () {
            debugger
            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/SalaryConfig/GetSalConfig'
            });
            debugger
        };

        return list;
    }]);