(function () {
    'use strict';

    angular.module("MVCApp").controller('SalaryConfigCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'SalaryConfigService', SalaryConfigCtrl
    ]);

    function SalaryConfigCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, SalaryConfigService) {
        //Initial Declaration
        var configsalaryDetailsParams = {};




        $scope.salaryDetailScope = {

            SalaryConfigurationId: 0,
            DA: null,
            HRA: null,
            PF: null,
            IsActive: true
        };

        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return salaryDetailScope.SalaryConfigurationId > 0 ? "Update" : "Save";
        };

        $scope.SaveConfigSalaryDetails = function (salaryDetailScope, frmSalary) {
            if (frmSalary.$valid) {
                debugger;
                SalaryConfigService.SaveConfigSalaryDetails(salaryDetailScope).then(function (res) {
                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                            $scope.ClearFormData(frmSalary);
                            toastr.success(data.Message, successTitle);
                            $scope.tableParams.reload();
                        }
                        else if (data.MessageType == messageTypes.Error) {
                            toastr.error(data.Message, errorTitle);
                        }
                        else if (data.MessageType == messageTypes.Warning) {
                            toastr.warning(data.Message, warningTitle);
                        }
                    }

                });
            }
        }




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
                if (configsalaryDetailsParams == null) {
                    configsalaryDetailsParams = {};
                }

                configsalaryDetailsParams.Paging = CommonFunctions.GetPagingParams(params);
                configsalaryDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';

                SalaryConfigService.GetAllConfigSalary(configsalaryDetailsParams.Paging).then(function (res) {

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



        $scope.Init = function () {
            $scope.employeesScope();
            //$scope.departmentsScope();
            //$scope.emplyeeDetailScope.Gender;
        }

        //$scope.resetemployeeDetails = function (frmDesignations) {
        //    if ($scope.operationMode() == "Update") {
        //        $scope.frmDesignations = angular.copy($scope.lastStorageGroup);
        //        frmDesignations.$setPristine();
        //    } else {
        //        $scope.clearData(frmDesignations);
        //    }
        //};

        $scope.ClearFormData = function (frmSalary) {
            $scope.salaryDetailScope = {

                SalaryConfigurationId: 0,
                DA: '',
                HRA: '',
                PF: '',
                IsActive: true


            };

            frmSalary.$setPristine();
            $("#txtSalary").focus();
            CommonFunctions.ScrollToTop();
        };

        //$scope.Init = function () {
        //    $scope.employeesScope();
        //}



        ////Create Excel Report of Employees
        //$scope.createReport = function () {
        //    if (!$rootScope.permission.CanWrite) { return; }
        //    var filename = "Employee_" + $rootScope.fileDateName + ".xls";
        //    CommonFunctions.DownloadReport('/Employee/CreateEmployeeListReport', filename);
        //};
    }
})();








