(function () {
    'use strict';

    angular.module("MVCApp").controller('AttendanceCtrl', [
        '$scope', '$rootScope', '$http', 'ngTableParams', 'CommonFunctions', 'FileService', 'uiCalendarConfig', 'AttendanceService', AttendanceCtrl
    ]);

    //BEGIN DesignationCtrl
    function AttendanceCtrl($scope, $rootScope, $http, ngTableParams, CommonFunctions, FileService, uiCalendarConfig, AttendanceService) {
        var employeeDetailsParams = {};

        $scope.EmployeeDetails = {
            EmployeeId: 0,
            FirstName: '',
            LastName: '',
            Date: '',
            InTime: '',
            outTime: '',
            InDiscription: '',
            OutDiscription: ''

        };
        $scope.headers = [];

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

        
        $scope.getAttendance = function () {
           
            AttendanceService.GetHRAttendance($scope.selectedMonth, $scope.selectedYear).then(function (res) {
                $scope.headers = [];
                $scope.employees = res.data.Result;
                $scope.dates = res.data.Result[0];
                console.log($scope.employees);

                angular.forEach(Object.keys($scope.dates), function (selected) {
                    if (selected != 'FirstName' && selected != 'LastName' && selected != 'EmployeeId' && selected != 'EmployeeId1') {
                        $scope.headers.push({ Date: selected });
                    }
                });

            }, function (error) {
                console.log(error);
            });
        };

        $scope.getAttendance();

        //  $scope.getAttendance = function () {
        //    var pagingParams = {
        //        CurrentPageNumber: 1,
        //        PageSize: 10,
        //        Search: '' 
        //    };
        //    AttendanceService.GetHRAttendance($scope.selectedMonth, $scope.selectedYear, pagingParams).then(function (res) {
        //        $scope.headers = [];
        //        $scope.employees = res.data.Result.list;
        //        $scope.dates = res.data.Result.list[0];
        //        console.log($scope.employees);

        //        angular.forEach(Object.keys($scope.dates), function (selected) {
        //            if (selected != 'FirstName' && selected != 'LastName' && selected != 'EmployeeId' && selected != 'EmployeeId1') {
        //                $scope.headers.push({ Date: selected });
        //            }
        //        });

        //    }, function (error) {
        //        console.log(error);
        //    });
        //};

        //$scope.getAttendance();



        //$scope.tableParams = new ngTableParams({
        //    page: 1,
        //    count: $rootScope.pageSize,
        //    sort: { FirstName: 'asc' }
        //}, {
        //    getData: function ($defer, params) {
        //        if (employeeDetailsParams == null) {
        //            employeeDetailsParams = {};
        //        }

        //        employeeDetailsParams.Paging = CommonFunctions.GetPagingParams(params);
        //        /*    debugger*/
        //        employeeDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
        //        //debugger
        //        AttendanceService.GetAllEmployees(employeeDetailsParams.Paging).then(function (res) {

        //            if (res) {
        //                var data = res.data;
        //                if (res.data.MessageType == messageTypes.Success) {
        //                    $defer.resolve(res.data.Result);
        //                    if (res.data.Result.length == 0) { }
        //                    //else { params.total(50); }
        //                    else { params.total(res.data.Result[0].TotalRecords); }

        //                }
        //            }
        //            else if (res.data.MessageType == messageTypes.Error) {// Error
        //                toastr.error(res.data.Message, errorTitle);
        //            }
        //            //  debugger
        //            $rootScope.isAjaxLoadingChild = false;
        //            CommonFunctions.SetFixHeader();
        //        });
        //    }

        //});

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

                

                 //format time in AM/PM format
                function formatTime(timeString, isOutTime) {
                    var time = new Date('2000-01-01T' + timeString + 'Z');
                    var timeZoneOffset = time.getTimezoneOffset(); // get timezone offset in minutes
                    time.setTime(time.getTime() + timeZoneOffset * 60 * 1000); // adjust time object based on timezone offset
                    if (isNaN(time.getTime())) {
                        return '';
                    }
                    var hours = time.getHours();
                    var minutes = time.getMinutes();
                    var suffix = isOutTime ? 'PM' : 'AM';
                    hours = hours % 24;
                    hours = hours ? hours : 24; // the hour '0' should be '12'
                    minutes = minutes < 10 ? '0' + minutes : minutes;
                    var strTime = hours + ':' + minutes + ' ' + suffix;
                    return strTime;
                }
                
                




                //format date
                $scope.formatDate = function (dateString) {
                    var date = new Date(dateString);
                    var year = date.getFullYear();
                    var month = date.getMonth() + 1;
                    var day = date.getDate();
                    return year + '-' + (month < 10 ? '0' + month : month) + '-' + (day < 10 ? '0' + day : day);
                }
                // push event to events array
                $scope.events.push({
                    title: '',
                    intimedescription:value.InTime,// In Time in AM format
                    outtimetdescription:value.OutTime, // Out Time in PM format
                    indescription: value.InDiscription,
                    outdescription: value.OutDiscription,
                    start: $scope.formatDate(value.Date),
                    end: $scope.formatDate(value.Date),
                    IsActive: true,
                    stick: true
                });
            });
        });

        /* Render Tooltip */

        //configure calendar
        $scope.uiConfig = {

            calendar: {
                height: 700,
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
                    $('#eventModal').modal('show');

                },


                eventAfterAllRender: function () {
                    if ($scope.events.length > 0 && isFirstTime) {
                        //Focus first event
                        uiCalendarConfig.calendars.myCalendar.fullCalendar('gotoDate', $scope.events[0].start);
                        isFirstTime = false;

                    }
                },
                //eventRender: function (event, element) {
                //    $(element).tooltip({ title: event.title});                 
                //},



                eventMouseover: function (event, jsEvent) {
                    var tooltip = '<div class="tooltipevent" style="width:auto;height:auto;background:#f2f2f2;position:absolute;z-index:10001;border:1px solid  #ddd;border-radius:3px;padding:5px 10px; font-size:11px;font-weight:600">' + 'In Time: ' + event.intimedescription + '<br>' + 'Out Time: '  + event.outtimetdescription + '</div>';
                    $("body").append(tooltip);
                    $(this).mouseover(function (e) {
                        $(this).css('z-index', 10000);
                        $('.tooltipevent').fadeIn('500');
                        $('.tooltipevent').fadeTo('10', 1.9);
                    }).mousemove(function (e) {
                        $('.tooltipevent').css('top', e.pageY + 10);
                        $('.tooltipevent').css('left', e.pageX + 20);
                    });
                },

                eventMouseout: function (calEvent, jsEvent) {
                    $(this).css('z-index', 8);
                    $('.tooltipevent').remove();
                },
            }
        };
    }
})();