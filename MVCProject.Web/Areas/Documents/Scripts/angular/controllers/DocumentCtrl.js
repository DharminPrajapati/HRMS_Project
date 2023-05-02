(function () {
    'use strict';

    angular.module("MVCApp").controller('DocumentCtrl', [
        '$scope', '$rootScope', '$window','ngTableParams', 'CommonFunctions', 'FileService', 'DocumentService', DocumentCtrl
    ]);

    function DocumentCtrl($scope, $rootScope,$window,ngTableParams, CommonFunctions, FileService, DocumentService) {
        var documentDetailsParams = {};

        $scope.documentDetails = {

            EmployeeId: 0,
            FullnameURL: '',
            selectedProject: '',
            Id: 0,
            Name: '',
            IsActive: true

        };

        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return documentDetails.EmpDocumentId > 0 ? "Update" : "Save";
        };

        //DropDown For Document Type
        $scope.DocumentDrop = function () {

            DocumentService.GetAllDocuments().then(function (res) {
                $scope.documents = res.data.Result;
                console.log($scope.documents);
            });

        }

        $scope.Init = function () {
            $scope.DocumentDrop();
        }


        //Angu-Complete For Employee Details
        $scope.FullnameURL = DocumentService.GetFullName(true);

        $scope.employee = [];
        $scope.selectedProject = function (selected) {
            if (angular.isDefined(selected)) {
                $scope.employee = selected.originalObject
                $scope.getEmployeebyId($scope.employee.Id)
            }
        }

        $scope.getEmployeebyId = function (Id) {

            DocumentService.getEmployeebyId(Id).then(function (res) {

                var data = res.data;
                if (!angular.isUndefined(data.Result) && data.Result != '') {
                    $scope.documentDetails = res.data.Result;
                    $scope.files = res.data.Result.Attachments;
                    console.log(data);


                }
                else {

                    $scope.documentDetails.EmployeeId = $scope.employee.Id;
                    $scope.documentDetails.DesignationName = $scope.employee.DesignationName;
                    $scope.documentDetails.DepartmentName = $scope.employee.DepartmentName;
                    $scope.documentDetails.files = $scope.employee.Attachments;
                    console.log($scope.employee.Id);
                }
            })
        }

        //Begin For Add /Update Department Details
        $scope.SaveDocumentDetails = function (documentDetails, frmDocuments) {
            
            if (frmDocuments.$valid) {

                documentDetails.Attachments = $scope.files;
                documentDetails.DeleteAttachments = $scope.DeletePoints;
                
                DocumentService.SaveDocumentDetails(documentDetails).then(function (res) {
                    
                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                            toastr.success(data.Message, successTitle);
                            $scope.ClearFormData(frmDocuments);
                            //$scope.tableParams.reload();  
                            location.reload();
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


        $scope.uploadFiles = function (fileObj, index) {
            var fileInput = document.getElementById('file_' + index);
            if (fileInput.files.length === 0) return;
            var file = fileInput.files[0];
            var payload = new FormData();
            payload.append("file", file);
            var url = $rootScope.apiURL + '/Upload/UploadFile?databaseName=' + $rootScope.userContext.CompanyDB;
            FileService.uploadFiles(url, payload).then(function successCallback(response) {
                //alert("File Uploaded");

                //console.log(response);
                $scope.FileData = response.data.Result;
                fileObj.FileName = response.data.Result.FileName;
                fileObj.FilePath = response.data.Result.FilePath;
                fileObj.OriginalName = response.data.Result.OriginalFileName;
                $scope.saveBtnIsDisable = false;
                //fileInput.files = [];
                angular.element(document.getElementById('file')).val('');
                //$scope.AddFileToDB($scope.FileData);

                //     console.log($scope.EmployeeMasterDetailScope.RefId);
            }).catch(function (response) {
                response
            });
            $scope.AddFileToDB = function () {
                EmployeeMasterService.AddFileToDB($scope.FileData)
                    .then(function (res) {
                        console.log(res.data.Result);
                    })
            }
        }

        $scope.fileSelected = function () {

            // Disable file input
            angular.element('input[type="file"]').attr('disabled', true);

            // Get the filename from the selected file
            var filename = $scope.selectedFile.name;

            // Display the filename
            $scope.selectedFilename = filename;
            $scope.$apply(); // Update the view
        };
        $scope.files = [];

        $scope.addFile = function () {

            $scope.files.push({
                Description: '',
                IsEdit: true,
                FileName: '',
                FilePath: '',
                OriginalName: '',
                DocumentType: '',
                EmpDocumentId: 0
            });
            $scope.saveBtnIsDisable = true;
            console.log($scope.files);
        };


        $scope.savefile = function (file) {

            file.IsEdit = false;
            $scope.selectedFile = null;
        }
        $scope.cancel = function (file) {
            // file.IsEdit = false;
            var index = $scope.files.indexOf(file);
            $scope.files.splice(index, 1);

            $scope.selectedFile = null;
        }
        $scope.DeletePoints = [];

        $scope.delete = function (file) {
            
            $scope.DeletePoints.push({
                Description: file.Description,
                IsEdit: true,
                FileName: '',
                FilePath: '',
                OriginalName: '',
                DocumentType: '',
                EmpDocumentId: file.EmpDocumentId
            });

            var index = $scope.files.indexOf(file);
            $scope.files.splice(index, 1);
            toastr.success("Delete Successfully", successTitle);
        };




        //Clear Form Data
        $scope.ClearFormData = function (frmDocuments) {
            $scope.documentDetails = {

                EmployeeId: 0,
                FullnameURL: '',
                selectedProject: '',
                Id: 0,
                Name: '',
                IsActive: true

            };
            $scope.files = null;
            $scope.$broadcast('angucomplete-alt:clearInput');
            $window.location.reload();
            frmDocuments.$setPristine();
            $("#txtEmp").focus();
            CommonFunctions.ScrollToTop();
        };
    }
    angular.module("MVCApp").factory('FileService', ['$http', function ($http) {
        return {
            uploadFiles: function (url, payload) {

                return $http({
                    url: url,
                    method: 'POST',
                    data: payload,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity

                });
            },
        };
    }]);

})();