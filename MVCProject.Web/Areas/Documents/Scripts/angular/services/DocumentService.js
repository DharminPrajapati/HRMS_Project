angular.module("MVCApp")
    .service('DocumentService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        // Get Full Name 
        list.GetFullName = function (IsActive) {
            return $rootScope.apiURL + '/Document/GetFullName?isActive=' + IsActive + '&searchText=';
        };

        list.getEmployeebyId = function (Id) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Document/GetDeptDesiByEmployeeId?Id=' + Id
            });
        }

        //Get All Documents
        list.GetAllDocuments = function () {

            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Document/DocumentTypeDropDown'
            });
        }

        //Upload Document
        list.UploadFile = function (directoryPathEnumName) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Upload/UploadImage?directoryPathEnumName=' + directoryPathEnumName
            });
        };

        // Add File To DB
        list.AddFileToDB = function (filedata) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/FileUploadToDB',
                data: JSON.stringify(filedata)
            });
        };

        // Save Document Details
        list.SaveDocumentDetails = function (documentDetails) {
            debugger
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Document/SaveDocumentDetails/',
                data: JSON.stringify(documentDetails)
            });
        };
        return list;
    }]);