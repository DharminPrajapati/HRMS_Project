﻿angular.module('MVCApp')
    .service('SearchEmployeeService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        ////Get All Employees
        ////list.GetAllEmployees = function (/*employeeDetailsParams*/)
        ////{
        ////    return $http({
        ////        method: 'GET',
        ////        url: $rootScope.apiURL + '/Employee/GetAllEmployees/'
        ////    });
        ////}
        //list.GetAllEmployees = function (employeeDetailsParams) {
        //    return $http({
        //        method: 'POST',
        //        url: $rootScope.apiURL + '/Employee/GetAllEmployees/',
        //        data: JSON.stringify(employeeDetailsParams)
        //    });
        //}

        //list.GetAllConfigSalary = function (salaryDetailsParams) {
        //    return $http({
        //        method: 'POST',
        //        url: $rootScope.apiURL + '/Salary/GetAllConfigSalary/',
        //        data: JSON.stringify(salaryDetailsParams)
        //    });
        //}

        //// Get All  list of Employees
        //list.GetConfigSalaryList = function (isGetAll) {
        //    return $http({
        //        method: 'GET',
        //        url: $rootScope.apiURL + '/Salary/GetConfigSalaryList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
        //    });
        //}

        //// Add/Update Employee Details
        //list.SaveConfigSalaryDetails = function (configsalaryDetail) {
        //    debugger
        //    return $http({
        //        method: 'POST',
        //        url: $rootScope.apiURL + '/SalaryConfig/SaveConfigSalaryDetails',
        //        data: JSON.stringify(configsalaryDetail)
        //    });
        //}

        //// Get Employees By Id
        //list.GetConfigSalaryById = function (configsalaryId) {
        //    return $http({
        //        method: 'GET',
        //        url: $rootScope.apiURL + '/Salary/GetConfigSalaryById?GetConfigSalaryById=' + GetConfigSalaryById
        //    });
        //};

        //////Get employee DropDown
        ////list.GetEmployeelist = function () {
        ////    return $http({
        ////        methd: 'GET',
        ////        url: $rootScope.apiURL + '/Salary/GetEmployeeDropDown'
        ////    });
        ////};
        //list.GetSalConfig = function () {
        //    debugger
        //    return $http({
        //        methd: 'GET',
        //        url: $rootScope.apiURL + '/SalaryConfig/GetSalConfig'
        //    });
        //    debugger
        //};

        // Get Employee List using Sp
        list.GetEmployeeDetails = function (SearchEmployeeDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/GetEmployeeDetails/',
                data: JSON.stringify(SearchEmployeeDetailsParams)
            });
        }
        list.GetAllEmployees = function (employeeDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/GetAllEmployees/',
                data: JSON.stringify(employeeDetailsParams)
            });
        }

        //list.GetAllEmployees = function (/*employeeDetailsParams*/)
        //{
        //    return $http({
        //        method: 'GET',
        //        url: $rootScope.apiURL + '/Employee/GetAllEmployees/'
        //    });
        //}

        return list;
    }]);