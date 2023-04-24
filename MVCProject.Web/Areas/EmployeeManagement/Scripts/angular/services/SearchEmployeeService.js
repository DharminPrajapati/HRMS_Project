/*const { search } = require("modernizr");*/

angular.module('MVCApp')
    .service('SearchEmployeeService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        //Get designationlist 
        list.GetDesignationlist = function () {
            
            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/SearchEmployee/GetDesignationDropDown'
            });
        };

        //Get Department DropDown
        list.GetDepartmentlist = function () {
            
            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/SearchEmployee/GetDepartmentDropDown'
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

        list.SearchEmp = function (SearchEmployeeDetailsParams, searchDetail) {
    
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + "/SearchEmployee/AdvancedSearchEmployee?FirstName=" + searchDetail.FirstName + "&DepartmentId=" + searchDetail.DepartmentId + "&DesignationId=" + searchDetail.DesignationId,
                data: JSON.stringify(SearchEmployeeDetailsParams)
            })
        }

        //list.GetAllEmployees = function (/*employeeDetailsParams*/)
        //{
        //    return $http({
        //        method: 'GET',0
        //        url: $rootScope.apiURL + '/Employee/GetAllEmployees/'
        //    });
        //}

        return list;
    }]);