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
            return SalaryDetail.SalaryId > 0 ? "Update" : "Save";
        };

        $scope.SaveEmployeeDetails = function (emplyeeDetailScope, frmEmployees) {
            if (frmSalary.$valid) {
                debugger;
                SalaryService.SaveSalaryDetails(SalaryDetail).then(function (res) {
                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                            $scope.ClearFormData(frmEmployees);
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

    
  

    $scope.EditSalaryDetails = function (employeeId) {
        SalaryService.GetSalaryById(employeeId).then(function (res) {
            if (res) {
                var data = res.data;
                if (data.MessageType == messageTypes.Success) {
                    debugger;
                    $scope.SalaryDetail = data.Result;
                    $scope.SalaryDetail.JoiningDa = new Date($scope.SalaryDetail.JoiningDate);
                    $scope.SalaryDetail.BirthDate = new Date($scope.SalaryDetail.BirthDate);
                    $scope.SalaryDetail.CourseStartDate = new Date($scope.SalaryDetail.CourseStartDate);
                    $scope.SalaryDetail.CourseEndDate = new Date($scope.SalaryDetail.CourseEndDate);
                    $scope.SalaryDetail.FromPeriod = new Date($scope.SalaryDetail.FromPeriod);
                    $scope.SalaryDetail.ToPeriod = new Date($scope.SalaryDetail.ToPeriod);
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

            SalaryService.GetAllSalary(salaryDetailsParams.Paging).then(function (res) {

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
    //    $scope.designationScope();
    //    $scope.departmentsScope();
    //    $scope.salaryDetailScope.Gender;
    //}

    //$scope.designationScope = function () {
    //    EmployeeService.GetDesignationlist().then(function (res) {
    //        $scope.Designation = res.data.Result;
    //        console.log($scope.Designation);
    //    });
    //};


    //$scope.departmentsScope = function () {
    //    EmployeeService.GetDepartmentlist().then(function (res) {
    //        $scope.Departments = res.data.Result;
    //        console.log($scope.Departments);
    //    });
    //};

    ////Create Excel Report of Employees
    //$scope.createReport = function () {
    //    if (!$rootScope.permission.CanWrite) { return; }
    //    var filename = "Employee_" + $rootScope.fileDateName + ".xls";
    //    CommonFunctions.DownloadReport('/Employee/CreateEmployeeListReport', filename);
    //};
}
})();








