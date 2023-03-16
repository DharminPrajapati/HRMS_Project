(function () {
    'use strict';

    angular.module("MVCApp").controller('SalaryCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'SalaryService', SalaryCtrl
    ]);

    function SalaryCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, SalaryService) {
        //Initial Declaration
        var salaryDetailsParams = {};
        



        $scope.salaryDetailScope = {

            SalaryId: 0,
            EmployeeId: 0,
            BasicSalary: '',
            DA: '',
            HRA: '',
            PF: '',
            IsActive: true
        };

        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return salaryDetailScope.SalaryId > 0 ? "Update" : "Save";
        };

        $scope.SaveSalaryDetails = function (salaryDetailScope , frmSalary) {
            if (frmSalary.$valid) {
                debugger;
                SalaryService.SaveSalaryDetails(salaryDetailScope).then(function (res) {
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

    
  

        $scope.EditSalaryDetails = function (salaryId) {
        debugger
        SalaryService.GetSalaryById(salaryId).then(function (res) {
            if (res) {
                var data = res.data;
                if (data.MessageType == messageTypes.Success) {
                    debugger;
                    $scope.salaryDetailScope = data.Result;
                    $scope.lastStorageAudit = angular.copy(data.Result);
                    CommonFunctions.ScrollUpAndFocus("txtSalary");
                }
                else if (data.MessageType == messageTypes.Error) {
                    toastr.error(data.Message, errorTitle);
                }
            }
            $rootScope.isAjaxLoadingChild = false;
        });
    }

    $scope.tableParams = new ngTableParams({
        page: 1,
        count: $rootScope.pageSize,
        sort: { FirstName: 'asc' }
    }, {
        getData: function ($defer, params) {
            if (salaryDetailsParams == null) {
                salaryDetailsParams = {};
            }

            salaryDetailsParams.Paging = CommonFunctions.GetPagingParams(params);
            salaryDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';

            SalaryService.GetEmployeeSalary(salaryDetailsParams.Paging).then(function (res) {

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
       
        $scope.employeesScope = function () {
            //debugger
            SalaryService.GetEmployeelist().then(function (res) {
                $scope.Employees = res.data.Result;
                console.log($scope.Employees);
            });
        };

        $scope.designationScope = function () {
            EmployeeService.GetDesignationlist().then(function (res) {
                $scope.Designation = res.data.Result;
                console.log($scope.Designation);
            });
        };



        $scope.departmentsScope = function () {
            EmployeeService.GetDepartmentlist().then(function (res) {
                $scope.Departments = res.data.Result;
                console.log($scope.Departments);
            });
        };
        debugger
        $scope.Init = function () {
            debugger
            $scope.employeesScope();
            $scope.designationScope(); 
            $scope.departmentsScope();
            $scope.salaryDetailScope.DA=20
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
            SalaryId: 0,
            EmployeeId: 0,
            BasicSalary: '',
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








