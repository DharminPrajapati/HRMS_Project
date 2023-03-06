(function () {
    'use strict';

    angular.module("MVCApp").controller('AttendanceCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'AttendanceService', AttendanceCtrl
    ]);

    //BEGIN DesignationCtrl
    function AttendanceCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, AttendanceService) {
        var employeeDetailsParams = {};

        $scope.EmployeeDetails = {
            EmployeeId: 0,
            FirstName: '',
            LastName: ''
        };

        $scope.isSearchClicked = false;

        $scope.selectedMonth = moment().month() + 1;
        $scope.selectedYear = moment().year();
        $scope.generateDates = function () {
            //debugger
            var year = $scope.selectedYear;
            var month = $scope.selectedMonth;
            var numDays = new Date(year, month, 0).getDate();
            var dates = [];

            for (var i = 1; i <= numDays; i++) {
                var date = new Date(year, month - 1, i);
                dates.push(date);
            }

            $scope.dates = dates;
        };

        $scope.generateDates();



        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sort: { FirstName: 'asc' }
        }, {
            getData: function ($defer, params) {
                if (employeeDetailsParams == null) {
                    employeeDetailsParams = {};
                }

                employeeDetailsParams.Paging = CommonFunctions.GetPagingParams(params);
                /*    debugger*/
                employeeDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //debugger
                AttendanceService.GetAllEmployees(employeeDetailsParams.Paging).then(function (res) {

                    if (res) {
                        var data = res.data;
                        if (res.data.MessageType == messageTypes.Success) {
                            $defer.resolve(res.data.Result);
                            //if (res.data.Result.length == 0) { }
                            //else { params.total(50); }
                            /*else { params.total(res.data.Result[0].TotalRecords); }*/

                        }
                    }
                    else if (res.data.MessageType == messageTypes.Error) {// Error
                        toastr.error(res.data.Message, errorTitle);
                    }
                    //  debugger
                    $rootScope.isAjaxLoadingChild = false;
                    CommonFunctions.SetFixHeader();
                });
            }

        });

    }
})();