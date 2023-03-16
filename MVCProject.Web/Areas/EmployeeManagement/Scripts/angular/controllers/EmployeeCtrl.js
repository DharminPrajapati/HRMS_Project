

(function () {
    'use strict';

    angular.module("MVCApp").controller('EmployeeCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'EmployeeService', EmployeeCtrl
    ]);

    function EmployeeCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, EmployeeService) {
        //Initial Declaration
        var employeeDetailsParams = {};



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
            InstitutionName: '',
            CourseName: '',
            CourseStartDate: null,
            CourseEndDate: null,
            Grade: '',
            Degree: '',
            CompanyName: '',
            LastJobLocation: '',
            JobPosition: '',
            FromPeriod: null,
            ToPeriod: null,
            IsActive: true,
            Attachment: null,

        };

        $scope.max = new Date();

        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return emplyeeDetailScope.EmployeeId > 0 ? "Update" : "Save";
        };




        $scope.SaveEmployeeDetails = function (emplyeeDetailScope, frmEmployees) {
            debugger;
           
            if (emplyeeDetailScope.FirstName == null || emplyeeDetailScope.FirstName == "") {
                toastr.warning(" First Name Required", warningTitle);
                $("#txtEmployee").focus();
                return;
            }
            else if (emplyeeDetailScope.LastName == null || emplyeeDetailScope.LastName == "" ) {
                toastr.warning("Last Name Required", warningTitle);
                $("#txtEmpln").focus();
            }
            else if (emplyeeDetailScope.Email == null || emplyeeDetailScope.Email == "") {
                toastr.warning("Email Required", warningTitle);
                $("#email").focus();
            }
            else if (emplyeeDetailScope.JoiningDate == null || emplyeeDetailScope.JoiningDate == "") {
                toastr.warning("Joining Date Required ", warningTitle);
                $("#joindate").focus();
            }
            else if (emplyeeDetailScope.PhoneNumber == null || emplyeeDetailScope.PhoneNumber == "") {
                toastr.warning("Phone Number Required", warningTitle);
                $("#phno").focus();
            }
            else if (emplyeeDetailScope.DesignationId == null || emplyeeDetailScope.DesignationId == "") {
                toastr.warning("Designation Required", warningTitle);
                $("#selectDesignation").focus();
            }
            //else if (!$("#selectDesignation").val()) {
            //    toastr.warning("Please fill Designation", warningTitle);
            //    $("#selectDesignation").focus();
            //}

            else if (emplyeeDetailScope.DepartmentId == null || emplyeeDetailScope.DepartmentId == "") {
                toastr.warning("Department Required", warningTitle);
                $("#selectDepartments").focus();
            }
            //else if (!$("#selectDepartments").val()) {
            //    toastr.warning("Please fill Department", warningTitle);
            //    $("#selectDepartments").focus();
            //}
            else if (emplyeeDetailScope.BirthDate == null || emplyeeDetailScope.BirthDate == "") {
                toastr.warning("Birth Date Required", warningTitle);
                $("#birthdate").focus();
            }

            else if (emplyeeDetailScope.Pincode == null || emplyeeDetailScope.Pincode == "") {
                toastr.warning("Pincode Required", warningTitle);
                $("#pincode").focus();
            }
            else if (emplyeeDetailScope.PermanentAddress == null || emplyeeDetailScope.PermanentAddress == "") {
                toastr.warning("Address Required", warningTitle);
                $("#permanentAddress").focus();
            }
            else if (emplyeeDetailScope.InstitutionName == null || emplyeeDetailScope.InstitutionName == "") {
                toastr.warning("Institution Name Required", warningTitle);
                $("#institutename").focus();
            }
            else if (emplyeeDetailScope.CourseName == null || emplyeeDetailScope.CourseName == "") {
                toastr.warning("Course Name Required", warningTitle);
                $("#CourseName").focus();
            }
            else if (emplyeeDetailScope.CourseStartDate == null || emplyeeDetailScope.CourseStartDate == "") {
                toastr.warning("Course Start-Date Required", warningTitle);
                $("#CourseStartDate").focus();
            }
            else if (emplyeeDetailScope.CourseEndDate == null || emplyeeDetailScope.CourseEndDate == "") {
                toastr.warning("Course End-Date Required", warningTitle);
                $("#CourseStartDate").focus();
            }
            else if (emplyeeDetailScope.CourseStartDate >= emplyeeDetailScope.CourseEndDate) {
                toastr.warning("Course Start date should be grater then end date", warningTitle);
                $("#CourseStartDate").focus();
                return;
            }
            else if (emplyeeDetailScope.Grade == null || emplyeeDetailScope.Grade == "") {
                toastr.warning("Grade Required", warningTitle);
                $("#grade").focus();
            }
            else if (emplyeeDetailScope.Degree == null || emplyeeDetailScope.Degree == "") {
                toastr.warning("Degree Required ", warningTitle);
                $("#degree").focus();
            }          
            else if (emplyeeDetailScope.FromPeriod >= emplyeeDetailScope.ToPeriod) {
                toastr.warning("FromPeriod date should be grater then ToPeriod ", warningTitle);
                $("#txtEmployee").focus();
                return;
            }
            if (frmEmployees.$valid) {                

                //emplyeeDetailScope.Attachment = [];
                //emplyeeDetailScope.Attachment = emplyeeDetailScope.Attachment.push($scope.FileData)
                debugger
                emplyeeDetailScope.Attachment = $scope.FileData;

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

        }

        $scope.EditEmployeeDetails = function (employeeId) {
           
            EmployeeService.GetEmployeeById(employeeId).then(function (res) {
                if (res) {
                   
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        $scope.emplyeeDetailScope = data.Result;
                        $scope.emplyeeDetailScope.JoiningDate = new Date($scope.emplyeeDetailScope.JoiningDate);
                        $scope.emplyeeDetailScope.BirthDate = new Date($scope.emplyeeDetailScope.BirthDate);
                        $scope.emplyeeDetailScope.CourseStartDate = new Date($scope.emplyeeDetailScope.CourseStartDate);
                        $scope.emplyeeDetailScope.CourseEndDate = new Date($scope.emplyeeDetailScope.CourseEndDate);
                        $scope.emplyeeDetailScope.FromPeriod = new Date($scope.emplyeeDetailScope.FromPeriod);
                        $scope.emplyeeDetailScope.ToPeriod = new Date($scope.emplyeeDetailScope.ToPeriod);


                        
                        ///$scope.emplyeeDetailScope = data.Result;
                        $scope.file = data.Result.Attachment[0];
                        var output = document.getElementById('output');
                        var binaryData = [];
                        binaryData.push($scope.file);
                        output.src = URL.createObjectURL(new Blob(binaryData, { type: "image /png/jpeg/jpg" }))
                        //output.src = URL.createObjectURL($scope.file);
                        output.onload = function () {
                            URL.revokeObjectURL(output.src)//free memory
                        }
                        $scope.lastStorageAudit = angular.copy(data.Result);
                        CommonFunctions.ScrollUpAndFocus("txtEmployee");
                    }
                    else if (data.MessageType == messageTypes.Error) {
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            });

            ////fileupload
            //$scope.uploadFile = function () {
            //    debugger;
            //    var fileInput = document.getElementById('file');
            //    //fileInput.click();

            //    //do nothing if there's no files
            //    if (fileInput.files.length === 0) return;

            //    var file = fileInput.files[0];

            //    var payload = new FormData();
            //    payload.append("file", file);

            //    var url = $rootScope.apiURL + '/Upload/UploadImage/'
            //    FileService.uploadFile(url, payload).then(function sucessCallback(response) {
            //        alert("Image uploaded");
            //        $scope.FileData = response.data.Result;
            //        $scope.emplyeeDetailScope.Attachment = response.data.Result;
            //    }).catch(function (response) {

            //        response
            //        //bummer
            //    });
            //    //$scope.FileDataTODB = function () {
            //    //    debugger
            //    //    EmployeeService.AddFileToDB($scope.FileData, $scope.emplyeeDetailScope)
            //    //        .then(function (res) {
            //    //            console.log(res.data.Result);
            //    //        })
            //    //}

            //}
        };

        //fileupload
        $scope.uploadFile = function () {
            
            var fileInput = document.getElementById('file');
            //fileInput.click();

            //do nothing if there's no files
            if (fileInput.files.length === 0) return;

            var file = fileInput.files[0];

            var payload = new FormData();
            payload.append("file", file);

            var url = $rootScope.apiURL + '/Upload/UploadImage/'
            FileService.uploadFile(url, payload).then(function sucessCallback(response) {
                alert("Image uploaded");
                $scope.FileData = response.data.Result;
                $scope.emplyeeDetailScope.Attachment = response.data.Result;
            }).catch(function (response) {

                response
                //bummer
            });
            //$scope.FileDataTODB = function () {
            //    debugger
            //    EmployeeService.AddFileToDB($scope.FileData, $scope.emplyeeDetailScope)
            //        .then(function (res) {
            //            console.log(res.data.Result);
            //        })
            //}

        }




        //}

        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sort: { FirstName: 'asc' }
        }, {
            getData: function ($defer, params) {

                if (employeeDetailsParams == null) {
                    employeeDetailsParams = {};
                }

                employeeDetailsParams.Paging = CommonFunctions.GetPagingParams(params);
                employeeDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';

                EmployeeService.GetEmployeeDetails(employeeDetailsParams.Paging).then(function (res) {
                    //debugger
                    if (res) {
                        var data = res.data;
                        if (res.data.MessageType == messageTypes.Success) {
                            $defer.resolve(res.data.Result.list);

                            params.total(res.data.Result.Total);

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
                InstitutionName: '',
                CourseName: '',
                CourseStartDate: null,
                CourseEndDate: null,
                Grade: '',
                Degree: '',
                CompanyName: '',
                LastJobLocation: '',
                JobPosition: '',
                FromPeriod: null,
                ToPeriod: null,
                IsActive: true,
                Attachment:null,
                file: null,
                output:''
            };
            //after save image is removed
            $scope.output = document.getElementById('output');//preview id
            $scope.file = document.getElementById('file');//image input id
            output.src = '';
            file.value = '';
            frmEmployees.$setPristine();
            $("#txtEmployee").focus();
            CommonFunctions.ScrollToTop();
        };

        $scope.Init = function () {
            $scope.designationScope();
            $scope.departmentsScope();
            $scope.emplyeeDetailScope.Gender;
        }

        $scope.designationScope = function () {
            EmployeeService.GetDesignationlist().then(function (res) {
                $scope.Designation = res.data.Result;               
            });
        };



        $scope.departmentsScope = function () {
            EmployeeService.GetDepartmentlist().then(function (res) {
                $scope.Departments = res.data.Result;               
            });
        };

    //    //Create Excel Report of Employees
    //    $scope.createReport = function () {
    //        if (!$rootScope.permission.CanWrite) { return; }
    //        var filename = "Employee_" + $rootScope.fileDateName + ".xls";
    //        CommonFunctions.DownloadReport('/Employee/CreateEmployeeListReport', filename);
    //    };
    //};
        $scope.createReport = function () {
            debugger
            EmployeeService.CreateExcelReport().then(function (res) {
                var data = res.data;
                debugger
                if (res.data.MessageType == messageTypes.Success) {

                    var fileName = res.data.Result;
                    var params = { fileName: fileName };
                    var form = document.createElement("form");
                    form.setAttribute("method", "POST");
                    form.setAttribute("action", "Employee/DownloadFile");
                    form.setAttribute("target", "_blank");

                    for (var key in params) {
                        if (params.hasOwnProperty(key)) {
                            var hiddenField = new document.createElement("input");
                            hiddenField.setAttribute("type", "hidden");
                            hiddenField.setAttribute("name", key);
                            hiddenField.setAttribute("value", params[key]);

                            form.appendChild(hiddenField);
                        }

                    }
                    document.body.appendChild(form);
                    form.submit();

                    $defer.resolve(res.data.Result);
                    if (res.data.Result.length == 0) { }
                    else {
                        params.total(res.data.Result[0].TotalRecords);
                    }
                }
                else if (res.data.MessageType == messageTypes.Error) {
                    toastr.error(res.data.Message, errorTitle);
                }
                $rootScope.isAjaxLoadingChild = false;
                CommonFunctions.SetFixHeader();
                debugger
            });
        };
    };



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


