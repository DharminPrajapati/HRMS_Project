﻿var app = angular.module('myApp', ['ui.calendar']);
app.controller('AttendanceCtrl', ['$scope', '$http', 'uiCalendarConfig', function ($scope, $http, uiCalendarConfig) {

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

}])