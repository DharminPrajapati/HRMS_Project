(function () {
    'use strict';

    angular.module("MVCApp").controller('SearchEmployeeCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'SearchEmployeeService', SearchEmployeeCtrl
    ]);

    function SearchEmployeeCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, SearchEmployeeService) {
        //Initial Declaration
        var SearchEmployeeDetailsParams = {};
        var employeeDetailsParams = {};




        $scope.SearchemployeeDetailScope = {


            EmployeeId: 0,
            FirstName: null,
            DepartmentName: null,
            DesignationName: null,
            // IsActive: true
        };

        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return SearchemployeeDetailScope.EmployeeId > 0 ? "Update" : "Save";
        };

        //$scope.SearchemployeeDetailScope = function (SearchemployeeDetailScope, frmSearchemployee) {
        //    if (frmSearchemployee.$valid) {
        //        debugger;
        //        SearhEmployeeService.configsalaryDetails(SearchemployeeDetailScope).then(function (res) {
        //            if (res) {
        //                var data = res.data;
        //                if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
        //                    $scope.ClearFormData(frmSalary);
        //                    toastr.success(data.Message, successTitle);
        //                    $scope.tableParams.reload();
        //                }
        //                else if (data.MessageType == messageTypes.Error) {
        //                    toastr.error(data.Message, errorTitle);
        //                }
        //                else if (data.MessageType == messageTypes.Warning) {
        //                    toastr.warning(data.Message, warningTitle);
        //                }
        //            }

        //        });
        //    }
        //}


        //$scope.EditSalaryDetails = function (salaryconfigId) {
        //    debugger
        //    SalaryConfigService.GetConfigSalaryById(salaryconfigId).then(function (res) {
        //        if (res) {
        //            var data = res.data;
        //            if (data.MessageType == messageTypes.Success) {
        //                debugger;
        //                $scope.salaryDetailScope = data.Result;
        //                $scope.lastStorageAudit = angular.copy(data.Result);
        //                CommonFunctions.ScrollUpAndFocus("txtSalary");
        //            }
        //            else if (data.MessageType == messageTypes.Error) {
        //                toastr.error(data.Message, errorTitle);
        //            }
        //        }
        //        $rootScope.isAjaxLoadingChild = false;
        //    });
        //}

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


        //$scope.Init = function () {
        //    //  debugger
        //    $scope.employeesScope();
        //    // debugger
        //    //$scope.departmentsScope();
        //    //$scope.emplyeeDetailScope.Gender;
        //}


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








