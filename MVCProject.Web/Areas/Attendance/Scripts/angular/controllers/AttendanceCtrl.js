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
            LastName: ''
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

        //var date = new Date();
        //var d = date.getDate();
        //var m = date.getMonth();
        //var y = date.getFullYear();

        //$scope.changeTo = 'Hungarian';
        ///* event source that pulls from google.com */
        //$scope.eventSource = {
        //    url: "http://www.google.com/calendar/feeds/usa__en%40holiday.calendar.google.com/public/basic",
        //    className: 'gcal-event',           // an option!
        //    currentTimezone: 'America/Chicago' // an option!
        //};
        ///* event source that contains custom events on the scope */
        //$scope.events = [
        //    { title: 'All Day Event', start: new Date(y, m, 1) },
        //    { title: 'Long Event', start: new Date(y, m, d - 5), end: new Date(y, m, d - 2) },
        //    { id: 999, title: 'Repeating Event', start: new Date(y, m, d - 3, 16, 0), allDay: false },
        //    { id: 999, title: 'Repeating Event', start: new Date(y, m, d + 4, 16, 0), allDay: false },
        //    { title: 'Birthday Party', start: new Date(y, m, d + 1, 19, 0), end: new Date(y, m, d + 1, 22, 30), allDay: false },
        //    { title: 'Click for Google', start: new Date(y, m, 28), end: new Date(y, m, 29), url: 'http://google.com/' }
        //];
        ///* event source that calls a function on every view switch */
        //$scope.eventsF = function (start, end, timezone, callback) {
        //    var s = new Date(start).getTime() / 1000;
        //    var e = new Date(end).getTime() / 1000;
        //    var m = new Date(start).getMonth();
        //    var events = [{ title: 'Feed Me ' + m, start: s + (50000), end: s + (100000), allDay: false, className: ['customFeed'] }];
        //    callback(events);
        //};

        //$scope.calEventsExt = {
        //    color: '#f00',
        //    textColor: 'yellow',
        //    events: [
        //        { type: 'party', title: 'Lunch', start: new Date(y, m, d, 12, 0), end: new Date(y, m, d, 14, 0), allDay: false },
        //        { type: 'party', title: 'Lunch 2', start: new Date(y, m, d, 12, 0), end: new Date(y, m, d, 14, 0), allDay: false },
        //        { type: 'party', title: 'Click for Google', start: new Date(y, m, 28), end: new Date(y, m, 29), url: 'http://google.com/' }
        //    ]
        //};
        ///* alert on eventClick */
        //$scope.alertOnEventClick = function (date, jsEvent, view) {
        //    $scope.alertMessage = (date.title + ' was clicked ');
        //};
        ///* alert on Drop */
        //$scope.alertOnDrop = function (event, delta, revertFunc, jsEvent, ui, view) {
        //    $scope.alertMessage = ('Event Droped to make dayDelta ' + delta);
        //};
        /////* alert on Resize */
        ////$scope.alertOnResize = function (event, delta, revertFunc, jsEvent, ui, view) {
        ////    $scope.alertMessage = ('Event Resized to make dayDelta ' + delta);
        ////};
        ///* add and removes an event source of choice */
        //$scope.addRemoveEventSource = function (sources, source) {
        //    var canAdd = 0;
        //    angular.forEach(sources, function (value, key) {
        //        if (sources[key] === source) {
        //            sources.splice(key, 1);
        //            canAdd = 1;
        //        }
        //    });
        //    if (canAdd === 0) {
        //        sources.push(source);
        //    }
        //};
        ///* add custom event*/
        //$scope.addEvent = function () {
        //    $scope.events.push({
        //        title: 'Open Sesame',
        //        start: new Date(y, m, 28),
        //        end: new Date(y, m, 29),
        //        className: ['openSesame']
        //    });
        //};
        ///* remove event */
        //$scope.remove = function (index) {
        //    $scope.events.splice(index, 1);
        //};
        ///* Change View */
        //$scope.changeView = function (view, calendar) {
        //    uiCalendarConfig.calendars[calendar].fullCalendar('changeView', view);
        //};
        ///* Change View */
        //$scope.renderCalender = function (calendar) {
        //    if (uiCalendarConfig.calendars[calendar]) {
        //        uiCalendarConfig.calendars[calendar].fullCalendar('render');
        //    }
        //};
        ///* Render Tooltip */
        //$scope.eventRender = function (event, element, view) {
        //    element.attr({
        //        'tooltip': event.title,
        //        'tooltip-append-to-body': true
        //    });
        //    $compile(element)($scope);
        //};
        ///* config object */
        //$scope.uiConfig = {
        //    calendar: {
        //        height: 450,
        //        editable: true,
        //        header: {
        //            left: 'title',
        //            center: '',
        //            right: 'today prev,next'
        //        },
        //        eventClick: $scope.alertOnEventClick,
        //        eventDrop: $scope.alertOnDrop,
        //        eventResize: $scope.alertOnResize,
        //        eventRender: $scope.eventRender
        //    }
        //};

        //$scope.changeLang = function () {
        //    if ($scope.changeTo === 'Hungarian') {
        //        $scope.uiConfig.calendar.dayNames = ["Vasárnap", "Hétfő", "Kedd", "Szerda", "Csütörtök", "Péntek", "Szombat"];
        //        $scope.uiConfig.calendar.dayNamesShort = ["Vas", "Hét", "Kedd", "Sze", "Csüt", "Pén", "Szo"];
        //        $scope.changeTo = 'English';
        //    } else {
        //        $scope.uiConfig.calendar.dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        //        $scope.uiConfig.calendar.dayNamesShort = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        //        $scope.changeTo = 'Hungarian';
        //    }
        //};
        ///* event sources array*/
        //$scope.eventSources = [$scope.events, $scope.eventSource, $scope.eventsF];
        //$scope.eventSources2 = [$scope.calEventsExt, $scope.eventsF, $scope.events];


        $scope.SelectedEvent = null;
        var isFirstTime = true;

        $scope.events = [];
        $scope.eventSources = [$scope.events];


        //Load events from server
        $http.get('/home/getevents', {
            cache: true,
            params: {}
        }).then(function (data) {
            $scope.events.slice(0, $scope.events.length);
            debugger
            angular.forEach(data.data, function (value) {
                $scope.events.push({
                    Title: value.Title,
                    Description: value.Description,
                    StartAt: new Date(parseInt(value.StartAt.substr(6))),
                    EndAt: new Date(parseInt(value.EndAt.substr(6))),
                    IsallDay: value.IsFullDay,
                    stick: true
                });
            });
        });

        //configure calendar
        $scope.uiConfig = {
            calendar: {
                height: 450,
                editable: true,
                displayEventTime: false,
                header: {
                    left: 'month basicWeek basicDay agendaWeek agendaDay',
                    center: 'title',
                    right: 'today prev,next'
                },
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