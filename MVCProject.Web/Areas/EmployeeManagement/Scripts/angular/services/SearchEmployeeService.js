﻿angular.module('MVCApp')
    .service('SearchEmployeeService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        //Get designationlist 
        list.GetDesignationlist = function () {
            debugger
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