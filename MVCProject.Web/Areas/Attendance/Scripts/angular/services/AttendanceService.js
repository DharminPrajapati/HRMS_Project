﻿angular.module('MVCApp')
    .service('AttendanceService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        list.GetAllEmployees = function (employeeDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/GetAllEmployees/',
                data: JSON.stringify(employeeDetailsParams)
            });
        }

        list.GetEmployeeList = function (isGetAll) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Employee/GetEmployeeList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
            });
        }

        list.GetHRAttendance = function (month, year, pagesize, pageNumber, search = '') {
            
            var url = $rootScope.apiURL + '/Attendance/GetHRAttendanceByMonthYear?month=' + month + '&year=' + year + '&pagesize=' + pagesize + '&pageNumber=' + pageNumber;
            if (search) {
                url += '&search=' + search;
            }

            return $http.get(url);
            debugger
        }
        // list.GetHRAttendance = function (month, year, pagingParams) {
        //    return $http({
        //        method: 'GET',               
        //        url: $rootScope.apiURL + '/Attendance/GetHRAttendanceByMonthYear?month=' + month + '&year=' + year,
        //        data: { month: month, year: year, pagingParams }
        //    });
        //}

        return list;
    }]);