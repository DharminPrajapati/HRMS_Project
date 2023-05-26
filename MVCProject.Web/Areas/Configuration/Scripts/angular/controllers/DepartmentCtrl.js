(function () {
    'use strict';

    angular.module("MVCApp").controller('DepartmentCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'DepartmentService', DepartmentCtrl
    ]);

    function DepartmentCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, DepartmentService) {
        // Initialize Default Department
        var departmentDetailsParams = {};


        $scope.departmentDetailScope = {
            DepartmentId: 0,
            CompanyMasterId: 0,
            DepartmentName: '',
            EntryById: null,
            EntryDate: null,
            UpdateBy: null,
            UpdatedDate: null,
            IsActive: true
        };
        $scope.isSearchClicked = false;
        // For Edit
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return $scope.departmentDetailScope.DepartmentId > 0 ? "Update" : "Save";
        };

        //Begin For Add /Update Department Details
        $scope.SaveDepartmentDetails = function (departmentDetailScope, frmDepartments) {
            if (frmDepartments.$valid) {
                DepartmentService.SaveDepartmentDetails(departmentDetailScope).then(function (res) {
                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                            $scope.ClearFormData(frmDepartments);
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

        //Binding Data to Edit Details

        $scope.EditDepartmentDetails = function (departmentId) {
            debugger
            DepartmentService.GetDepartmentById(departmentId).then(function (res) {
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {// Success
                        $scope.departmentDetailScope = data.Result;
                        $scope.lastStorageAudit = angular.copy(data.Result);
                        CommonFunctions.ScrollUpAndFocus("txtDepartments");
                    }
                    else if (data.MessageType == messageTypes.Error) {// Error
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            });



         };

        //Reset Details
        $scope.resetDepartmentDetails = function (frmDesignations) {
            if ($scope.operationMode() == "Update") {
                $scope.frmDepartments = angular.copy($scope.lastStorageGroup);
                frmDepartments.$setPristine();
            } else {
                $scope.clearData(frmDepartments);
            }
        };

        //Clear Data

        $scope.ClearFormData = function (frmDepartments) {
            $scope.departmentDetailScope = {
                DepartmentId: 0,
                DepartmentName: '',
                EntryById: null,
                EntryDate: null,
                UpdateBy: null,
                UpdatedDate: null,
                IsActive: true
            };
            frmDepartments.$setPristine();
            $("#txtDepartments").focus();
            CommonFunctions.ScrollToTop();
        };

        //Load Department List

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { DepartmentName: 'asc' }
        }, {
            getData: function ($defer, params) {
                if (departmentDetailsParams == null) {
                    departmentDetailsParams = {};
                }
                departmentDetailsParams.Paging = CommonFunctions.GetPagingParams(params);
                departmentDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';

                //Load Departments List

                DepartmentService.GetAllDepartments(departmentDetailsParams.Paging).then(function (res) {
                    var data = res.data;
                    if (res.data.MessageType == messageTypes.Success) {// Success
                        $defer.resolve(res.data.Result);
                        if (res.data.Result.length == 0) {
                        } else {
                            params.total(res.data.Result[0].TotalRecords);
                        }
                    } else if (res.data.MessageType == messageTypes.Error) {// Error
                        toastr.error(res.data.Message, errorTitle);
                    }
                    $rootScope.isAjaxLoadingChild = false;
                    CommonFunctions.SetFixHeader();
                });
            }
        });

        $scope.Init = function () {
            $scope.companyScope();

        }

        $scope.companyScope = function () {

            DepartmentService.GetCompanyList().then(function (res) {
                $scope.company = res.data.Result;
            });
        };

        $scope.Export = function () {

            DepartmentService.CreateExcelReport().then(function (res) {
                var data = res.data;

                if (data.MessageType == messageTypes.Success) {

                    var fileName = res.data.Result;
                    var params = { fileName: fileName };
                    var form = document.createElement("form");
                    form.setAttribute("method", "POST");
                    form.setAttribute("action", "/Department/DownloadFile");
                    form.setAttribute("target", "_blank");

                    for (var key in params) {
                        if (params.hasOwnProperty(key)) {
                            var hiddenField = document.createElement("input");
                            hiddenField.setAttribute("type", "hidden");
                            hiddenField.setAttribute("name", key);
                            hiddenField.setAttribute("value", params[key]);

                            form.appendChild(hiddenField);
                        }

                    }

                    document.body.appendChild(form);
                    form.submit();

                    $defer.resolve(res.data.Result);
                    if (res.data.Result.length == 0) { }
                    else {
                        params.total(res.data.Result[0].TotalRecords);
                    }
                }

                $rootScope.isAjaxLoadingChild = false;
                CommonFunctions.SetFixHeader();


            });

        };
    }
})();