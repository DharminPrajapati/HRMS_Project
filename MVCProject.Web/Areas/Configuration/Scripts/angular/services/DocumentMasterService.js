angular.module('MVCApp')
    .service('DocumentMasterService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];



        // Add/Update Document Details
        list.SaveDocumentDetails = function (documentDetail) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/DocumentMaster/SaveDocumentDetails',
                data: JSON.stringify(documentDetail)
            });
        }

        // Get All list of Documents
        list.GetAllDocuments = function (DocumentpagingParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/DocumentMaster/GetAllDocuments/',
                data: JSON.stringify(DocumentpagingParams)
            });
        };

        // Get Document Items
        list.GetDocumentById = function (documentId) {

            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/DocumentMaster/GetDocumentById?documentId=' + documentId
            });
        };


        return list;
    }]);