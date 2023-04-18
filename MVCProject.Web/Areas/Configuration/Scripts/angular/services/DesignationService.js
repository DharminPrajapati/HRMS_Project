angular.module('MVCApp')
    .service('DesignationService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        // Get All list of Designation Details
        list.GetAllDesignations = function (designationDetailParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Designations/GetAllDesignations/',
                data: JSON.stringify(designationDetailParams)
            });
        };
        // Get All list of Designation Details
        list.GetDesignationList = function (isGetAll) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Designations/GetDesignationList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
            });
        };



        // Add/Update Designation Details
        list.SaveDesignationDetails = function (designationDetail) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Designations/SaveDesignationDetails',
                data: JSON.stringify(designationDetail)
            });
        }

        // Get Designation Items
        list.GetDesignationById = function (designationId) {
            
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Designations/GetDesignationById?designationId=' + designationId
            });
        };
        //Get company DropDown
        list.GetCompanyList = function () {

            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/Department/GetcompanyDropDown'
            });
        };

        //Get Department DropDown
        list.GetDepartmentlist = function (id) {
          
            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/Designations/GetDepartmentDropDown?id=' + id
            });
        };

        // Create Excel Report
        list.CreateExcelReport = function () {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Designations/CreateEmployeeListReport'
            });
        };

        return list;
    }]);