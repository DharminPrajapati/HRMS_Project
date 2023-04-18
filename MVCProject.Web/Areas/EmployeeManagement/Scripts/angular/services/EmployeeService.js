angular.module('MVCApp')
    .service('EmployeeService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        //Get All Employees
        //list.GetAllEmployees = function (/*employeeDetailsParams*/)
        //{
        //    return $http({
        //        method: 'GET',
        //        url: $rootScope.apiURL + '/Employee/GetAllEmployees/'
        //    });
        //}
        list.GetAllEmployees = function (employeeDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/GetAllEmployees/',
                data: JSON.stringify(employeeDetailsParams)
            });
        }
        // Get Employee List using Sp
        list.GetEmployeeDetails = function (employeeDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/GetEmployeeDetails/',
                data: JSON.stringify(employeeDetailsParams)
            });
        }

        // Get All  list of Employees
        list.GetEmployeeList = function (isGetAll)
        {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Employee/GetEmployeeList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
            });
        }

        // Add/Update Employee Details
        list.SaveEmployeeDetails = function (employeeDetail) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/SaveEmployeeDetails',
                data: JSON.stringify(employeeDetail)
            });
        }

        // Get Employees By Id
        list.GetEmployeeById = function (employeeId) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Employee/GetEmployeeById?employeeId=' + employeeId
            });
        };

         //Get company DropDown
        list.GetCompanyList = function () {
            
            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/Employee/GetcompanyDropDown'
            });
        };

        //Get Designation DropDown
        list.GetDesignationlist = function (id) {
           
            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/Employee/GetDesignationDropDown?id=' + id
            });
        };

        //Get Department DropDown
        list.GetDepartmentlist = function (id) {

            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/Employee/GetDepartmentDropDown?id=' + id
            });
        };

        //Create Excel Report For Employees
        list.CreateExcelReport = function () {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Employee/CreateEmployeeListReport'
            });
        };


     
        //Image uploading
        list.uploadFile = function (directoryPathEnumName) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + 'Upload/UploadImage?directoryPathEnumName' + directoryPathEnumName
            });
        };
        list.AddFileToDB = function (filedata) {
           
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/FileUploadTODB',
                data: JSON.stringify(filedata)
            });
        }
        list.GeneratePdf = function () {
           
            return $http({
                method: 'GET',                
                url: $rootScope.apiURL + '/Employee/GeneratePdf'
            });
            
        }

        list.ExportPDF = function () {
            
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Employee/ExportPDF'
            });
        }

        return list;
    }]);