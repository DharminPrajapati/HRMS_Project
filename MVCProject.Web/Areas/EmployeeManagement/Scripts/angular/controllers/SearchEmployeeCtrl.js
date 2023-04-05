﻿
(function () {
    'use strict';

    angular.module("MVCApp").controller('SearchEmployeeCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'SearchEmployeeService', SearchEmployeeCtrl
    ]);

    function SearchEmployeeCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, SearchEmployeeService) {
        //Initial Declaration
        var SearchEmployeeDetailsParams = {};
        var employeeDetailsParams = {};
        var SearchemployeeDetailScope = {};




        $scope.SearchemployeeDetailScope = {


            EmployeeId: null,
            FirstName: null,
            DepartmentId:null,
            DesignationId:null,
            // IsActive: true
        };

        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        //$scope.operationMode = function () {
        //    return SearchemployeeDetailScope.EmployeeId > 0 ? "Update" : "Save";
        //};



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
                employeeDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';

                SearchEmployeeService.GetAllEmployees(employeeDetailsParams.Paging).then(function (res) {

                    if (res) {
                        var data = res.data;
                        if (res.data.MessageType == messageTypes.Success) {
                            $defer.resolve(res.data.Result);
                            if (res.data.Result.length == 0) { }
                            else { params.total(res.data.Result[0].TotalRecords); }

                        }
                    }
                    else if (res.data.MessageType == messageTypes.Error) {// Error
                        toastr.error(res.data.Message, errorTitle);
                    }
                    $rootScope.isAjaxLoadingChild = false;
                    CommonFunctions.SetFixHeader();
                });
            }

        });

        //$scope.employeesScope = function () {
        //    debugger
        //    SearhEmployeeService.GetSalConfig().then(function (res) {
        //        var data = res.data.Result;
        //        $scope.SearchemployeeDetailScope = data;

        //        console.log(data);
        //    });
        //    // debugger
        //}
        $scope.Init = function () {
            debugger
            $scope.designationScope();
            $scope.departmentsScope();
           /* $scope.searchemployee();*/

        }

        //for designation dropdown
        $scope.designationScope = function () {

            debugger
            SearchEmployeeService.GetDesignationlist().then(function (res) {
                $scope.Designation = res.data.Result;
            });
        };

        $scope.departmentsScope = function () {
            SearchEmployeeService.GetDepartmentlist().then(function (res) {
                $scope.Departments = res.data.Result;
            });
        };


        // $scope.tableParams = new ngTableParams({
        //    page: 1,
        //    count: $rootScope.pageSize,
        //    sort: { FirstName: 'asc' }
        //}, {
        //    getData: function ($defer, params) {
        //        if (SearchemployeeDetailScope == null) {
        //            SearchemployeeDetailScope = {};
        //        }

        //        SearchemployeeDetailScope.Paging = CommonFunctions.GetPagingParams(params);
        //        SearchemployeeDetailScope.Paging.Search = $scope.isSearchClicked ? $scope.search : '';

        //        SearchEmployeeService.SearchEmp(SearchemployeeDetailScope.Paging).then(function (res) {

        //            if (res) {
        //                var data = res.data;
        //                if (res.data.MessageType == messageTypes.Success) {
        //                    $defer.resolve(res.data.Result);
        //                    if (res.data.Result.length == 0) { }
        //                    else { params.total(res.data.Result[0].TotalRecords); }

        //                }
        //            }
        //            else if (res.data.MessageType == messageTypes.Error) {// Error
        //                toastr.error(res.data.Message, errorTitle);
        //            }
        //            $rootScope.isAjaxLoadingChild = false;
        //            CommonFunctions.SetFixHeader();
        //        });
        //    }

        //});

        $scope.searchemployee = function (SearchemployeeDetailScope) {
            debugger
            console.log(SearchemployeeDetailScope);
            //SearchemployeeDetailScope = SearchemployeeDetailScope;
            SearchemployeeDetailScope.FirstName = SearchemployeeDetailScope.FirstName/*.angular(SearchemployeeDetailScope.FirstName)*/;
            SearchemployeeDetailScope.DepartmentId = SearchemployeeDetailScope.DepartmentId/*.angular(SearchemployeeDetailScope.DepartmentId)*/;
            SearchemployeeDetailScope.DesignationId = SearchemployeeDetailScope.DesignationId/*.angular(SearchemployeeDetailScope.DesignationId)*/;
            SearchEmployeeService.SearchEmp(SearchemployeeDetailScope).then(function (res) {
                $scope.data = res.data.Result;
                $scope.tableParams.reload();
            });
            
            
        };


        $scope.ClearFormData = function (frmSearchemployee) {
            $scope.SearchemployeeDetailScope = {

                EmployeeId: 0,
                FirstName: '',
                DepartmentName: '',
                DesignationName: '',
                //IsActive: true

            };

            frmSearchemployee.$setPristine();
            $("#FirstName").focus();
            CommonFunctions.ScrollToTop();
        };

    }
})();








