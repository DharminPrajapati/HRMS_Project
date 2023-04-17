(function () {
    'use strict';

    angular.module("MVCApp").controller('AllowanceMasterCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'AllowanceMasterService', AllowanceMasterCtrl
    ]);

    function AllowanceMasterCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, AllowanceMasterService) {
        // Initialize Default Department
        var AllowaneMasterDetailsParams = {};


        $scope.AllowanceDetailScope = {
            AllowanceId: 0,
            Description: '',
            ShortCode: '',
            Value: '',
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
            return $scope.AllowanceDetailScope.AllowanceId > 0 ? "Update" : "Save";
        };

        //Begin For Add /Update Department Details
        $scope.SaveAllowanceDetails = function (AllowanceDetailScope, frmAllowanceMaster) {
            if (frmAllowanceMaster.$valid) {
                AllowanceMasterService.SaveAllowanceDetails(AllowanceDetailScope).then(function (res) {
                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                            $scope.ClearFormData(frmAllowanceMaster);
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

        $scope.EditAllowanceMasterDetails = function (AllowanceId) {
            AllowanceMasterService.GetAllowanceMasterById(AllowanceId).then(function (res) {
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {// Success
                        $scope.AllowanceDetailScope = data.Result;
                        $scope.lastStorageAudit = angular.copy(data.Result);
                        CommonFunctions.ScrollUpAndFocus("txtDescription");
                    }
                    else if (data.MessageType == messageTypes.Error) {// Error
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            });
        };

        //Reset Details
        $scope.resetAllowanceMasterDetails = function (frmAllowanceMaster) {
            if ($scope.operationMode() == "Update") {
                $scope.frmAllowanceMaster = angular.copy($scope.lastStorageGroup);
                frmAllowanceMaster.$setPristine();
            } else {
                $scope.clearData(frmAllowanceMaster);
            }
        };

        //Clear Data

        $scope.ClearFormData = function (frmAllowanceMaster) {
            $scope.AllowanceDetailScope = {
                AllowanceId: 0,
                Description: '',
                ShortCode: '',
                Value: '',
                EntryById: null,
                EntryDate: null,
                UpdateBy: null,
                UpdatedDate: null,
                IsActive: true
            };
            frmAllowanceMaster.$setPristine();
            $("#txtDescription").focus();
            CommonFunctions.ScrollToTop();
        };

        //Load Department List

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { Description: 'asc' }
        }, {
            getData: function ($defer, params) {
                if (AllowaneMasterDetailsParams == null) {
                    AllowaneMasterDetailsParams = {};
                }
                AllowaneMasterDetailsParams.Paging = CommonFunctions.GetPagingParams(params);
                AllowaneMasterDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';

                //Load Departments List

                AllowanceMasterService.GetAllowanceMaster(AllowaneMasterDetailsParams.Paging).then(function (res) {
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