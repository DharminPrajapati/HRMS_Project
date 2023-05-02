(function () {
    'use strict';

    angular.module("MVCApp").controller('DocumentMasterCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'DocumentMasterService', DocumentMasterCtrl
    ]);

    function DocumentMasterCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, DocumentMasterService) {
        // Initialize Default Department
        var DocumentpagingParams = {};


        $scope.DocumentDetail = {
            DocumentId: 0,
            DocumentName: '',
            IsActive: true
        };
        $scope.isSearchClicked = false;
        // For Edit
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return $scope.DocumentDetail.DocumentId > 0 ? "Update" : "Save";
        };

        //Begin For Add /Update Document Details
        $scope.SaveDocumentDetails = function (DocumentDetail, frmDocument) {
            debugger
            if (frmDocument.$valid) {
                DocumentMasterService.SaveDocumentDetails(DocumentDetail).then(function (res) {
                    if (res) {
                        debugger
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                            debugger
                            $scope.ClearFormData(frmDocument);
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

        $scope.EditDocumentDetails = function (documentId) {
            DocumentMasterService.GetDocumentById(documentId).then(function (res) {
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {// Success
                        $scope.DocumentDetail = data.Result;
                        $scope.lastStorageAudit = angular.copy(data.Result);
                        CommonFunctions.ScrollUpAndFocus("txtDocument");
                    }
                    else if (data.MessageType == messageTypes.Error) {// Error
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            });
        };

        //Clear Data

        $scope.ClearFormData = function (frmDocument) {
            $scope.DocumentDetail = {
                DocumentId: 0,
                DocumentName: '',
                IsActive: true
            };
            frmDocument.$setPristine();
            $("#txtDocument").focus();
            CommonFunctions.ScrollToTop();
        };

        //Load Document List

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { DocumentName: 'asc' }
        }, {
            getData: function ($defer, params) {
                if (DocumentpagingParams == null) {
                    DocumentpagingParams = {};
                }
                DocumentpagingParams.Paging = CommonFunctions.GetPagingParams(params);
                DocumentpagingParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';

                //Load Document List

                DocumentMasterService.GetAllDocuments(DocumentpagingParams.Paging).then(function (res) {
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