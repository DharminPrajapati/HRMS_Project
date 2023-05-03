(function () {
    'use strict';

    angular.module("MVCApp").controller('DashboardCtrl', [
        '$scope', '$rootScope', '$http', '$timeout', 'ngTableParams', 'CommonFunctions', 'FileService', 'DashboardService', DashboardCtrl
    ]);

    //BEGIN DesignationCtrl
    function DashboardCtrl($scope, $rootScope, $http, $timeout, ngTableParams, CommonFunctions, FileService, DashboardService) {

        $scope.countemployee = function () {

            DashboardService.CountEmployees().then(function (res) {
                $scope.employee = res.data.Result[0].EmployeeCount;

                $scope.department = res.data.Result[0].DepartmentCount;

                $scope.active_employee = res.data.Result[0].ActiveEmployeeCount;
            });
        };



        // Load gender chart data
        $scope.genderChartData = [];
        $scope.genderChartLabels = [];
        $scope.genderChartOptions = {
            legend: {
                display: true,
                position: 'bottom'
            }
        };
        $scope.genderChartColors = ['#FF6384', '#36A2EB'];

        $scope.loadGenderChart = function () {

            DashboardService.GenderChart().then(function (res) {

                var data = res.data.Result;
                var chartData = [];
                for (var i = 0; i < data.length; i++) {
                    chartData.push({ label: data[i].Gender, value: data[i].Count });
                }

                nv.addGraph(function () {
                    var chart = nv.models.pieChart()
                        .x(function (d) { return d.label; })
                        .y(function (d) { return d.value; })
                        .showLabels(true)
                        .width(400)
                        .height(400); // set width and height here;
                    d3.select('#gender-chart svg')
                        .datum(chartData)
                        /*.transition().duration(350)*/
                        .call(chart);
                    return chart;
                });
            });
        };

        // Load role chart data
        $scope.roleChartData = [];
        $scope.roleChartLabels = [];
        $scope.roleChartOptions = {
            legend: {
                display: true,
                position: 'bottom'
            }
        };
        $scope.roleChartColors = ['#FF6384', '#36A2EB'];

        $scope.roleChart = function () {
       
            DashboardService.RoleChart().then(function (res) {

                var data = res.data.Result;
                var chartData = [];
                for (var i = 0; i < data.length; i++) {
                    var label = data[i].UserRoleName === "HR" ? "HR" : "Employee";
                    chartData.push({ label: label, value: data[i].Count });
                }
            
                nv.addGraph(function () {
                    var chart = nv.models.pieChart()
                        .x(function (d) { return d.label; })
                        .y(function (d) { return d.value; })
                        .showLabels(true)
                        .width(400)
                        .height(400)
                        .color(['#FF6384', '#36A2EB', '#FFCE56', '#33FF99'])

                    d3.select('#role-chart svg')
                        .datum(chartData)
                        /*.transition().duration(350)*/
                        .call(chart);
                    return chart;
                });
            });
        };



        $scope.Init = function () {
            $scope.countemployee();
            $scope.loadGenderChart();
            $scope.roleChart();

        }


    }

})();