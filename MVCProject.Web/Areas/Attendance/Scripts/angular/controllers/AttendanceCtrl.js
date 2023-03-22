(function () {
    'use strict';

    angular.module("MVCApp").controller('AttendanceCtrl', [
        '$scope', '$rootScope','$http', 'ngTableParams', 'CommonFunctions', 'FileService','uiCalendarConfig', 'AttendanceService', AttendanceCtrl
    ]);

    //BEGIN DesignationCtrl
    function AttendanceCtrl($scope, $rootScope, $http,ngTableParams, CommonFunctions, FileService, uiCalendarConfig, AttendanceService) {
        var employeeDetailsParams = {};

        $scope.EmployeeDetails = {
            EmployeeId: 0,
            FirstName: '',
            LastName: '',
            Date: '',
            InTime: '',
            outTime: '',
            InDiscription: '',
            OutDiscription:''

        };

        
        $scope.isSearchClicked = false;


        var currentDate = new Date();
        $scope.months = [
            { name: 'January', value: 1 },
            { name: 'February', value: 2 },
            { name: 'March', value: 3 },
            { name: 'April', value: 4 },
            { name: 'May', value: 5 },
            { name: 'June', value: 6 },
            { name: 'July', value: 7 },
            { name: 'August', value: 8 },
            { name: 'September', value: 9 },
            { name: 'October', value: 10 },
            { name: 'November', value: 11 },
            { name: 'December', value: 12 }
        ];
        $scope.years = [];
        for (var i = currentDate.getFullYear() - 10; i <= currentDate.getFullYear() + 5; i++) {
            $scope.years.push(i);
        }
        $scope.selectedMonth = currentDate.getMonth() + 1;
        $scope.selectedYear = currentDate.getFullYear();


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
                            if (res.data.Result.length == 0) { }
                            //else { params.total(50); }
                            else { params.total(res.data.Result[0].TotalRecords); }

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
        debugger

        


        $scope.SelectedEvent = null;
        var isFirstTime = true;


        $scope.events = [];
        $scope.eventSources = [$scope.events];

       


        //Load events from server
        $http.get($rootScope.apiURL + '/Attendance/GetAllAttendance/', {
            cache: true,
            params: {}
        }).then(function (data) {
            $scope.events.length = 0;
            angular.forEach(data.data.Result, function (value) {
            debugger
            $scope.events.push({
                title: 'present',
                description: value.InTime,
                start: value.Date,
                end: value.Date,
                allDay: true,
                stick: true

            });
            //$scope.events.push({
            //    title: '2ND',
            //    description: 'DESC',
            //    start: new Date(),
            //    end: new Date(),
            //    allDay: true,
            //    stick: true

            //})
            debugger
            //angular.forEach(data.data.Result, function (value) {
            //    debugger
            //    $scope.events.push({
            //        title: value.InDiscription,
            //        description: value.OutDiscription,
            //        start: new Date(parseInt(value.InTime.substr(0,7))),
            //        end: new Date(parseInt(value.OutTime.substr(0,7))),
            //        allDay: value.IsActive,
            //        stick: true
                   
            //    });             
            //});
            });
        });

        //configure calendar
        $scope.uiConfig = {
            calendar: {
                height: 450,
                editable: true,
                displayEventTime: false,
                header: {
                    left: 'month basicWeek ',
                    center: 'title',
                    right: 'today prev,next'
                },
                eventSources: $scope.eventSources,
                eventClick: function (event) {
                    $scope.SelectedEvent = event;
                },
                eventAfterAllRender: function () {
                    if ($scope.events.length > 0 && isFirstTime) {
                        //Focus first event
                        uiCalendarConfig.calendars.myCalendar.fullCalendar('gotoDate', $scope.events[0].start);
                        isFirstTime = false;
                    }
                }
            }
        };
    }
})();