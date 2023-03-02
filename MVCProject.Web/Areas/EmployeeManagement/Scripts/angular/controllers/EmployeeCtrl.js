﻿(function () {
    'use strict';

    angular.module("MVCApp").controller('EmployeeCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'EmployeeService', EmployeeCtrl
    ]);

    function EmployeeCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService , EmployeeService) {
          //Initial Declaration
        var employeeDetailsParams = {};
      


        $scope.emplyeeDetailScope = {
            EmployeeId : 0,
            FirstName : '',
            LastName : '',
            Email : '',
            JoiningDate : '',
            PhoneNumber : null,
            AlternatePhoneNumber: null,
            DesignationId: 0,
            DepartmentId: 0,
            BirthDate: null,
            Gender: 1,
            PermanentAddress :'',
            TemporaryAddress :'',
            Pincode: null,
            InstitutionName: '',
            CourseName: '',
            CourseStartDate: null,
            CourseEndDate: null,
            Grade: '',
            Degree: '',
            CompanyName:'',
            LastJobLocation:'',
            JobPosition:'',
            FromPeriod:null,
            ToPeriod:null,
            IsActive : true
        };

        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function ()
        {
            return emplyeeDetailScope.EmployeeId > 0 ? "Update" : "Save";
        };

        // Add/Update Employee Details

        $scope.SaveEmployeeDetails = function (emplyeeDetailScope, frmEmployees)
        {
            
            if (frmEmployees.$valid) {
                //$("#txtEmployee").focus();
                //CommonFunctions.ScrollToTop();
                //toastr.error("Please fill All Fields", errorTitle);
                debugger;
                EmployeeService.SaveEmployeeDetails(emplyeeDetailScope).then(function (res) {
                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                            $scope.ClearFormData(frmEmployees);
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
            //else {
            //    toastr.error("Please fill All Fields", errorTitle);
            //}
        }

        $scope.EditEmployeeDetails = function (employeeId)
        {
            EmployeeService.GetEmployeeById(employeeId).then(function (res) {
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        debugger;
                        $scope.emplyeeDetailScope = data.Result;
                        $scope.emplyeeDetailScope.JoiningDate = new Date($scope.emplyeeDetailScope.JoiningDate);
                        $scope.emplyeeDetailScope.BirthDate = new Date($scope.emplyeeDetailScope.BirthDate);
                        $scope.emplyeeDetailScope.CourseStartDate = new Date($scope.emplyeeDetailScope.CourseStartDate);
                        $scope.emplyeeDetailScope.CourseEndDate = new Date($scope.emplyeeDetailScope.CourseEndDate);
                        $scope.emplyeeDetailScope.FromPeriod = new Date($scope.emplyeeDetailScope.FromPeriod);
                        $scope.emplyeeDetailScope.ToPeriod = new Date($scope.emplyeeDetailScope.ToPeriod);
                        $scope.lastStorageAudit = angular.copy(data.Result);
                        CommonFunctions.ScrollUpAndFocus("txtEmployee");
                    }
                    else if (data.MessageType == messageTypes.Error) {
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            });
        }

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sort: {FirstName:'asc'}
        }, {
            getData: function ($defer,params)
            {
                if (employeeDetailsParams == null) {
                    employeeDetailsParams = {};
                }

                employeeDetailsParams.Paging = CommonFunctions.GetPagingParams(params);
                employeeDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : ''; 

                EmployeeService.GetAllEmployees(employeeDetailsParams.Paging).then(function (res)
                {
                    
                    if (res) {
                        var data = res.data;
                        if (res.data.MessageType == messageTypes.Success) {
                            $defer.resolve(res.data.Result);
                            if (res.data.Result.length == 0) { }
                            else { params.total(50); }
                            /*else { params.total(res.data.Result[0].TotalRecords); }*/
                           
                        }
                    }
                    else if (res.data.MessageType == messageTypes.Error) {// Error
                        toastr.error(res.data.Message, errorTitle);
                    }
                    $rootScope.isAjaxLoadingChild = false;
                    CommonFunctions.SetFixHeader();
                });
            }
            
            });
        //$scope.resetemployeeDetails = function (frmDesignations) {
        //    if ($scope.operationMode() == "Update") {
        //        $scope.frmDesignations = angular.copy($scope.lastStorageGroup);
        //        frmDesignations.$setPristine();
        //    } else {
        //        $scope.clearData(frmDesignations);
        //    }
        //};

        $scope.ClearFormData = function (frmEmployees) {
            $scope.emplyeeDetailScope = {
                EmployeeId: 0,
                FirstName: '',
                LastName: '',
                Email: '',
                JoiningDate: '',
                PhoneNumber: null,
                AlternatePhoneNumber: null,
                DesignationId: 0,
                DepartmentId: 0,
                BirthDate: null,
                Gender: 1,
                PermanentAddress: '',
                TemporaryAddress: '',
                Pincode: null,
                InstitutionName:'',
                CourseName:'',
                CourseStartDate :null,
                CourseEndDate:null,
                Grade:'',
                Degree: '',
                CompanyName: '',
                LastJobLocation: '',
                JobPosition: '',
                FromPeriod: null,
                ToPeriod: null,
                IsActive: true
            };

            frmEmployees.$setPristine();
            $("#txtEmployee").focus();
            CommonFunctions.ScrollToTop();
        };

        $scope.Init = function (){
            $scope.designationScope();
            $scope.departmentsScope();
            $scope.emplyeeDetailScope.Gender;
        }

        $scope.designationScope = function () {
            EmployeeService.GetDesignationlist().then(function (res) {
                $scope.Designation = res.data.Result;
                console.log($scope.Designation);
            });
        };
        $scope.uploadFile = function () {
            debugger;
            var fileInput = document.getElementById('file');
            //fileInput.click();

            //do nothing if there's no files
            if (fileInput.files.length === 0) return;

            var file = fileInput.files[0];

            var payload = new FormData();
            payload.append("stuff", "some string");
            payload.append("file", file);
           var url = $rootScope.apiURL + '/Upload/UploadImage'

            //var url = $rootScope.apiURL +'/UploadPrec/UploadFile'
            //use the service to upload the file
            FileService.uploadFile(url, payload).then(function (response) {

                response
                //success, file uploaded
            }).catch(function (response) {

                response
                //bummer
            });
        }

        $scope.departmentsScope = function () {
            EmployeeService.GetDepartmentlist().then(function (res) {
                $scope.Departments = res.data.Result;
                console.log($scope.Departments);
            });
        };

        //Create Excel Report of Employees
        $scope.createReport = function () {
            if (!$rootScope.permission.CanWrite) { return; }
            var filename = "Employee_" + $rootScope.fileDateName + ".xls";
            CommonFunctions.DownloadReport('/Employee/CreateEmployeeListReport', filename);
        };
    }
    angular.module("MVCApp").factory('FileService', ['$http', function ($http) {
      /*  debugger;*/
        return {
            uploadFile: function (url, file) {
                return $http({
                    url: url,
                    method: 'POST',
                    data: file,
                    headers: { 'Content-Type': undefined }, //this is important
                    transformRequest: angular.identity //also important
                });
            },
            otherFunctionHere: function (url, stuff) {
                return $http.get(url);
            }
        };
    }]);


      
})();


