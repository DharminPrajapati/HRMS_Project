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

        //Get Designation DropDown
        list.GetDesignationlist = function () {
            
            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/Employee/GetDesignationDropDown'
            });
        };

        //Get Department DropDown
        list.GetDepartmentlist = function () {

            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/Employee/GetDepartmentDropDown'
            });
        };

        //Create Excel Report For Employees
        list.CreateExcelReport = function () {
            return $http({
                method: 'GET',
                responseType: 'blob',
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
            debugger
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Employee/FileUploadTODB',
                data: JSON.stringify(filedata)
            });
        }


        return list;
    }]);