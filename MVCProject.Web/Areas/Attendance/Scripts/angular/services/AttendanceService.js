angular.module('MVCApp')
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
        //list.GetAllAttendance = function () {
        //    debugger
        //    return $http({
        //        method: 'GET',
        //        url: $rootScope.apiURL + '/Attendance/GetAllAttendance/',
        //        data: JSON.stringify(Attendanelist)
        //    });
        //}

        return list;
    }]);