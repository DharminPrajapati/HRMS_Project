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

        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return emplyeeDetailScope.EmployeeId > 0 ? "Update" : "Save";
        };




        $scope.SaveEmployeeDetails = function (emplyeeDetailScope, frmEmployees) {
            if (!$("#txtEmployee").val()) {
                toastr.warning("Please fill the First Name", warningTitle);
                $("#txtEmployee").focus();
            }
            else if (!$("#txtEmpln").val()) {
                toastr.warning("Please fill the Last Name", warningTitle);
                $("#txtEmpln").focus();
            }
            else if (!$("#email").val()) {
                toastr.warning("Please fill Email", warningTitle);
                $("#email").focus();
            }
            else if (!$("#joindate").val()) {
                toastr.warning("Please fill the Joining Date", warningTitle);
                $("#joindate").focus();
            }
            else if (!$("#phno").val()) {
                toastr.warning("Please fill Phone Number", warningTitle);
                $("#phno").focus();
            }
            else if (!$("#selectDesignation").val()) {
                toastr.warning("Please fill Designation", warningTitle);
                $("#selectDesignation").focus();
            }
            else if (!$("#selectDepartments").val()) {
                toastr.warning("Please fill Department", warningTitle);
                $("#selectDepartments").focus();
            }
            else if (!$("#birthdate").val()) {
                toastr.warning("Please fill Birth Date", warningTitle);
                $("#birthdate").focus();
            }
            else if (!$("#pincode").val()) {
                toastr.warning("Please fill Pincode", warningTitle);
                $("#pincode").focus();
            }
            else if (!$("#permanentAddress").val()) {
                toastr.warning("Please fill Address", warningTitle);
                $("#permanentAddress").focus();
            }
            else if (!$("#institutename").val()) {
                toastr.warning("Please fill Institution Name", warningTitle);
                $("#institutename").focus();
            }
            else if (!$("#coursename").val()) {
                toastr.warning("Please fill Course Name", warningTitle);
                $("#coursename").focus();
            }
            else if (!$("#coursestartdate").val()) {
                toastr.warning("Please fill Course Start Date", warningTitle);
                $("#coursestartdate").focus();
            }
            else if (!$("#courseEnddate").val()) {
                toastr.warning("Please fill Course End Date", warningTitle);
                $("#courseEnddate").focus();
            }
            else if (!$("#grade").val()) {
                toastr.warning("Please fill Grade", warningTitle);
                $("#grade").focus();
            }
            else if (!$("#degree").val()) {
                toastr.warning("Please fill Degree", warningTitle);
                $("#degree").focus();
            }
            if (frmEmployees.$valid) {
                debugger;

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
            debugger
            EmployeeService.GetEmployeeById(employeeId).then(function (res) {
                if (res) {
                    debugger
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        $scope.emplyeeDetailScope = data.Result;
                        $scope.emplyeeDetailScope.JoiningDate = new Date($scope.emplyeeDetailScope.JoiningDate);
                        $scope.emplyeeDetailScope.BirthDate = new Date($scope.emplyeeDetailScope.BirthDate);
                        $scope.emplyeeDetailScope.CourseStartDate = new Date($scope.emplyeeDetailScope.CourseStartDate);
                        $scope.emplyeeDetailScope.CourseEndDate = new Date($scope.emplyeeDetailScope.CourseEndDate);
                        $scope.emplyeeDetailScope.FromPeriod = new Date($scope.emplyeeDetailScope.FromPeriod);
                        $scope.emplyeeDetailScope.ToPeriod = new Date($scope.emplyeeDetailScope.ToPeriod);


                        debugger;
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
            debugger;
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
                IsActive: true
            };

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
                console.log($scope.Designation);
            });
        };



        $scope.departmentsScope = function () {
            EmployeeService.GetDepartmentlist().then(function (res) {
                $scope.Departments = res.data.Result;
                console.log($scope.Departments);
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


