(function () {
    'use strict';

    angular.module("MVCApp").controller('DeductionMasterCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'DeductionMasterService', DeductionMasterCtrl
    ]);
    function DeductionMasterCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, DeductionMasterService) {
        // Initialize Default Department
        var DeductionDetailsParams = {};


        $scope.deductionDetailScope = {
            DeductionId: 0,
            Description: '',
            ShortCode: '',
            Value: '',
            EntryBy: null,
            EntryDate: null,
            UpdateBy: null,
            UpdatedDate: null,
            IsActive: true
        };
        $scope.isSearchClicked = false;
        // For Edit
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return $scope.deductionDetailScope.DeductionId > 0 ? "Update" : "Save";
        };

        //Begin For Add /Update Department Details
        $scope.SaveDeductionDetails = function (deductionDetailScope, frmDeduction) {
            if (deductionDetailScope.Description == null || deductionDetailScope.Description == "") {
                toastr.warning("Description is  Required", warningTitle);
                $("#txtDeduction").focus();
                return;
            }
            else if (deductionDetailScope.ShortCode == null || deductionDetailScope.ShortCode == "") {
                toastr.warning("Short Code is  Required", warningTitle);
                $("#txtShortCode").focus();
                return;
            }
            else if (deductionDetailScope.Value == null || deductionDetailScope.Value == "") {
                toastr.warning("Value is  Required", warningTitle);
                $("#txtValue").focus();
                return;
            }
            if (frmDeduction.$valid) {
                DeductionMasterService.SaveDeductionDetails(deductionDetailScope).then(function (res) {

                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                            $scope.ClearFormData(frmDeduction);
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

        $scope.EditDeductionDetails = function (deductionId) {
            DeductionMasterService.GetDeductionById(deductionId).then(function (res) {
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {// Success
                        $scope.deductionDetailScope = data.Result;
                        $scope.lastStorageAudit = angular.copy(data.Result);
                        CommonFunctions.ScrollUpAndFocus("txtDeduction");
                    }
                    else if (data.MessageType == messageTypes.Error) {// Error
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            });
        };

        //Reset Details
        $scope.resetDeductionDetails = function (frmDeduction) {
            if ($scope.operationMode() == "Update") {
                $scope.frmDeduction = angular.copy($scope.lastStorageGroup);
                frmDeduction.$setPristine();
            } else {
                $scope.clearData(frmDeduction);
            }
        };

        //Clear Data

        $scope.ClearFormData = function (frmDeduction) {
            $scope.deductionDetailScope = {
                DeductionId: 0,
                Description: '',
                ShortCode: '',
                Value: '',
                EntryBy: null,
                EntryDate: null,
                UpdateBy: null,
                UpdatedDate: null,
                IsActive: true
            };
            frmDeduction.$setPristine();
            $("#frmDeduction").focus();
            CommonFunctions.ScrollToTop();
        };

        //Load Department List

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            /*sorting: { DepartmentName: 'asc' }*/
        }, {
            getData: function ($defer, params) {
                if (DeductionDetailsParams == null) {
                    DeductionDetailsParams = {};
                }
                DeductionDetailsParams.Paging = CommonFunctions.GetPagingParams(params);
                DeductionDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';

                //Load Departments List

                DeductionMasterService.GetDeduction(DeductionDetailsParams.Paging).then(function (res) {
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
    }
})();