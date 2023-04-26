(function () {
    'use strict';

    angular.module("MVCApp").controller('CompanyMasterCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'CompanyMasterService', CompanyMasterCtrl
    ]);

    function CompanyMasterCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, CompanyMasterService) {
        // Initialize Default Department
        var CompanyMasterDetailsParams = {};


        $scope.CompanyMasterDetailsParams = {
            CompanyMasterId: 0,
            CompanyName: '',
            ShortCode: '',
            EntryById: null,
            EntryDate: null,
            UpdateBy: null,
            UpdateDate: null,
            IsActive : true
        };
        $scope.isSearchClicked = false;
        // For Edit
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return $scope.CompanyDetailScope.CompanyMasterId > 0 ? "Update" : "Save";
        };

        //Begin For Add /Update Department Details
        $scope.SaveCompanyMasterDetails = function (CompanyDetailScope, frmCompanyMaster) {
            if (CompanyDetailScope.CompanyName == null || CompanyDetailScope.CompanyName == "") {
                toastr.warning("Company Name is  Required", warningTitle);
                $("#txtCompanyName").focus();
                return;
            }
            else if (CompanyDetailScope.ShortCode == null || CompanyDetailScope.ShortCode == "") {
                toastr.warning("Short Code is  Required", warningTitle);
                $("#txtShortCode").focus();
                return;
            }
            if (frmCompanyMaster.$valid) {
                CompanyMasterService.SaveCompanyMasterDetails(CompanyDetailScope).then(function (res) {
                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                            $scope.ClearFormData(frmCompanyMaster);
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

        $scope.EditCompanyMasterDetails = function (CompanyMasterId) {

            CompanyMasterService.GetCompanyMasterById(CompanyMasterId).then(function (res) {
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {// Success
                        $scope.CompanyDetailScope = data.Result;
                        $scope.lastStorageAudit = angular.copy(data.Result);
                        CommonFunctions.ScrollUpAndFocus("txtCompanyName");
                    }
                    else if (data.MessageType == messageTypes.Error) {// Error
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            });
        };

        //Reset Details
        $scope.resetAllowanceMasterDetails = function (frmCompanyMaster) {
            if ($scope.operationMode() == "Update") {
                $scope.frmAllowanceMaster = angular.copy($scope.lastStorageGroup);
                frmCompanyMaster.$setPristine();
            } else {
                $scope.clearData(frmCompanyMaster);
            }
        };



        //Clear Data

        $scope.ClearFormData = function (frmCompanyMaster) {
            $scope.CompanyDetailScope = {
                CompanyMasterId: 0,
                CompanyName: '',
                ShortCode: '',
                Value: '',
                EntryById: null,
                EntryDate: null,
                UpdateBy: null,
                UpdateDate: null,
                IsActive: true
            };
            frmCompanyMaster.$setPristine();
            $("#CompanyName").focus();
            CommonFunctions.ScrollToTop();
        };

        //Load Department List

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { CompanyName: 'asc' }
        }, {
            getData: function ($defer, params) {
                if (CompanyMasterDetailsParams == null) {
                    CompanyMasterDetailsParams = {};
                }
                CompanyMasterDetailsParams.Paging = CommonFunctions.GetPagingParams(params);
                CompanyMasterDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';

                //Load Departments List

                CompanyMasterService.GetAllCompanyMaster(CompanyMasterDetailsParams.Paging).then(function (res) {
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


