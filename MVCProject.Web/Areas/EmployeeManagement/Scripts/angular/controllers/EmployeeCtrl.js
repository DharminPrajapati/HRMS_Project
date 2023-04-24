
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
            CompanyMasterId : 0,
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
            file: '',
            output: '',

        };

        $scope.max = new Date();

        $scope.IsImg = false;
        $scope.AllReadyUpload = false;

        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return emplyeeDetailScope.EmployeeId > 0 ? "Update" : "Save";
        };




        $scope.SaveEmployeeDetails = function (emplyeeDetailScope, frmEmployees) {
          

            if (emplyeeDetailScope.FirstName == null || emplyeeDetailScope.FirstName == "") {
                toastr.warning(" First Name Required", warningTitle);
                $("#txtEmployee").focus();
                return;
            }
            else if (emplyeeDetailScope.LastName == null || emplyeeDetailScope.LastName == "") {
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
            else if (emplyeeDetailScope.AlternatePhoneNumber == null || emplyeeDetailScope.AlternatePhoneNumber == "") {
                toastr.warning("Alternate Phone Number Required", warningTitle);
                $("#alphno").focus();
            }
            else if (emplyeeDetailScope.DesignationId == null || emplyeeDetailScope.DesignationId == "") {
                toastr.warning("Designation Required", warningTitle);
                $("#selectDesignation").focus();
            }
            

            else if (emplyeeDetailScope.DepartmentId == null || emplyeeDetailScope.DepartmentId == "") {
                toastr.warning("Department Required", warningTitle);
                $("#selectDepartments").focus();
            }
            
            else if (emplyeeDetailScope.BirthDate == null || emplyeeDetailScope.BirthDate == "") {
                toastr.warning("Birth Date Required", warningTitle);
                $("#birthdate").focus();
            }

            //else if (emplyeeDetailScope.Pincode == null || emplyeeDetailScope.Pincode == "") {
            //    toastr.warning("Pincode Required", warningTitle);
            //    $("#pincode").focus();
            //}
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
            //else if (emplyeeDetailScope.CourseStartDate == null || emplyeeDetailScope.CourseStartDate == "") {
            //    toastr.warning("Course Start-Date Required", warningTitle);
            //    $("#CourseStartDate").focus();
            //}
            //else if (emplyeeDetailScope.CourseEndDate == null || emplyeeDetailScope.CourseEndDate == "") {
            //    toastr.warning("Course End-Date Required", warningTitle);
            //    $("#CourseStartDate").focus();
            //}
            //else if (emplyeeDetailScope.CourseStartDate >= emplyeeDetailScope.CourseEndDate) {
            //    toastr.warning("Course Start date should be grater then end date", warningTitle);
            //    $("#CourseStartDate").focus();
            //    return;
            //}
            //else if (emplyeeDetailScope.Grade == null || emplyeeDetailScope.Grade == "") {
            //    toastr.warning("Grade Required", warningTitle);
            //    $("#grade").focus();
            //}
            else if (emplyeeDetailScope.Degree == null || emplyeeDetailScope.Degree == "") {
                toastr.warning("Degree Required ", warningTitle);
                $("#degree").focus();
            }
            //else if (emplyeeDetailScope.FromPeriod >= emplyeeDetailScope.ToPeriod) {
            //    toastr.warning("FromPeriod date should be grater then ToPeriod ", warningTitle);
            //    $("#txtEmployee").focus();
            //    return;
            //}

            if (output.src) {
                $scope.AllReadyUpload = true;
            }
            if ($scope.AllReadyUpload) {

                if (frmEmployees.$valid) {
                    if ($scope.FileData) {

                        //emplyeeDetailScope.Attachment = [];
                        //emplyeeDetailScope.Attachment = emplyeeDetailScope.Attachment.push($scope.FileData)

                        emplyeeDetailScope.Attachment = $scope.FileData;
                    }

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
                } else {
                    toastr.warning("click upload Image");
                }
            }
            else {
                toastr.warning("Select Image.", warningTitle);
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



                        ////$scope.emplyeeDetailScope = data.Result;
                        //$scope.file = data.Result.Attachment[0];
                        //var output = document.getElementById('output');
                        //var binaryData = [];
                        //binaryData.push($scope.file);
                        //output.src = URL.createObjectURL(new Blob(binaryData, { type: "image /png/jpeg/jpg" }))
                        ////output.src = URL.createObjectURL($scope.file);
                        //output.onload = function () {
                        //    URL.revokeObjectURL(output.src)//free memory
                        //}
                        $scope.emplyeeDetailScope = data.Result;
                        var attachmentData = data.Result.Attachment;
                        output.src = attachmentData.RelativePath;

                        $scope.departmentsScope($scope.emplyeeDetailScope.CompanyMasterId);
                        $scope.designationScope($scope.emplyeeDetailScope.DepartmentId);
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

        //Display Image onload and validation
        $scope.loadFile = function (event) {
            var input = event.target;
            if (input.files && input.files[0]) {
                var fileName = input.files[0].name;
                var extension = fileName.split('.').pop().toLowerCase();
                if (extension !== 'jpg' && extension !== 'jepg' && extension !== 'png' && extension !== 'gif') {
                    toastr.warning("Select Valid Image File", warningTitle);
                    $("#file").focus();
                    input.value = '';
                } else {
                    var output = document.getElementById('output');
                    output.src = URL.createObjectURL(event.target.files[0]);
                    output.onload = function () {
                        URL.revokeObjectURL(output.src)//free memory
                        $scope.emplyeeDetailScope.Attachment = null
                    }
                }
            }
        };

        //fileupload
        $scope.uploadFile = function (fileobj) {
            var fileInput = document.getElementById('file');
            if (fileInput.files.length > 0) {
                $scope.IsImg = true;
                var fileInput = document.getElementById('file');
                if (fileInput.files.length === 0) return; //do nothing if there's no files
                var file = fileInput.files[0];
                var payload = new FormData();
                payload.append("file", file);

                var url = $rootScope.apiURL + '/Upload/UploadImage/'

                FileService.uploadFile(url, payload).then(function sucessCallback(response) {
                    toastr.success("Image uploaded", successTitle);
                    $scope.FileData = response.data.Result;
                }).catch(function (response) {
                    response
                });
            } else {
                toster.warning("Select image to upload", warningTitle);
                $("#file").focus();
            }
        }



        ////fileupload
        //$scope.uploadFile = function () {

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
        //$scope.FileDataTODB = function () {
        //    debugger
        //    EmployeeService.AddFileToDB($scope.FileData, $scope.emplyeeDetailScope)
        //        .then(function (res) {
        //            console.log(res.data.Result);
        //        })
        //}
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
                Attachment: null,
                file: '',
                output: ''
            };
            //after save image is removed
            $scope.IsImg = false;
            $scope.OnclickUpload = false;
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
            $scope.companyScope();
            $scope.emplyeeDetailScope.Gender;
        }

        $scope.designationScope = function (id) {
         
            if (id == undefined) {
                return
            }
            debugger
            EmployeeService.GetDesignationlist(id).then(function (res) {
                $scope.Designation = res.data.Result;
            });
        };

        $scope.companyScope = function () {
          
            EmployeeService.GetCompanyList().then(function (res) {
                $scope.company = res.data.Result;
            });
        };

        $scope.departmentsScope = function (id) {
            
            if (id == undefined) {
                return
            }
            EmployeeService.GetDepartmentlist(id).then(function (res) {
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
        $scope.Export = function () {
            
            EmployeeService.CreateExcelReport().then(function (res) {
                var data = res.data;

                if (res.data.MessageType == messageTypes.Success) {

                    var fileName = res.data.Result;
                    var params = { fileName: fileName };
                    var form = document.createElement("form");
                    form.setAttribute("method", "POST");
                    form.setAttribute("action", "/EmployeeManagement/Employee/DownloadFile");
                    form.setAttribute("target", "_blank");

                    for (var key in params) {
                        if (params.hasOwnProperty(key)) {
                            var hiddenField = document.createElement("input");
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

                $rootScope.isAjaxLoadingChild = false;
                CommonFunctions.SetFixHeader();

            });

        };

        $scope.ExportPDF = function()
        {
            
            EmployeeService.ExportPDF().then(function (res)
            {
               
                var data = res.data;
                if (res.data.MessageType == messageTypes.Success) {
                    var filename = res.data.Result;
                    var params = { filename: filename };
                    var form = document.createElement("form");
                    form.setAttribute("method", "POST");
                    form.setAttribute("action", "/EmployeeManagement/Employee/DownloadPDF");
                    form.setAttribute("target", "_blank");

                    for (var dt in params) {

                        if (params.hasOwnProperty(dt)) {
                            var hiddenField = document.createElement("input");
                            hiddenField.setAttribute("type", "hidden");
                            hiddenField.setAttribute("name", dt);
                            hiddenField.setAttribute("value", params[dt]);

                            form.appendChild(hiddenField);
                        }
                    }
                    document.body.appendChild(form);
                    form.submit();

                    $defer.resolve(res.data.Result);
                    if (data.Result.length == 0) {
                    }
                    else {
                        params.total(data.Result[0].TotalRecords);
                    }
                }

                $rootScope.isAjaxLoadingChild = false;
                CommonFunctions.SetFixHeader();
            });
        }

        $scope.totalExp = {};

        $scope.calExper = function (fromPeriod, toPeriod) {
          
            var start = new Date(fromPeriod);
            var end = new Date(toPeriod);
            var diff = end.getTime() - start.getTime();
            var days = Math.floor(diff / (1000 * 60 * 60 * 24));
            var months = Math.floor(days / 30);
            var years = Math.floor(months / 12);
            var remainingDays = days - (months * 30);
            $scope.totalExp.years = years + ' years';
            $scope.totalExp.months = months + ' months';
            $scope.totalExp.remainingDays = remainingDays + ' days';
           
        }
        //$scope.generatePdf = function () {
        //    debugger
        //    EmployeeService.GeneratePdf().then(function (res) {
        //        debugger
        //        if (res) {
        //            var data = res.data.Result;
        //            var doc = new jsPDF();
        //            var pos = 20;


        //            /*doc.setFontsize(16);*/
        //            doc.setFont('helvetica', 'bold')
        //            doc.text('Employee List', 85, 10);

        //            angular.forEach(data, function (record) {
        //                doc.setFontSize(12);
        //                doc.setFont('helvetica', 'normal');
        //                doc.text(20, pos, record.BatchNo);
        //                doc.text(50, pos, record.FirstName);
        //                doc.text(80, pos, record.LastName);
        //                doc.text(110, pos, record.Gender == 1 ? "Male" : "Female");
        //                doc.text(140, pos, record.PermanentAddress);
        //                pos += 5;


        //            });
        //              doc.save('Employees.pdf');

        //        }
        //    });
        //}

        $scope.generatePdf = function () {
            EmployeeService.GeneratePdf().then(function (res) {
                if (res) {
                    var data = res.data.Result;
                    var doc = new jsPDF();
                    var pos = 30;
                    var currentPage = 0; // initialize currentPage to 0

                    doc.setFont('helvetica', 'bold');
                    doc.text('Employee List', 80, 20);


                    // Add current date and time
                    var now = new Date();
                    var dateTime = now.toLocaleDateString() + ' ' + now.toLocaleTimeString();
                    doc.setFontSize(10);
                    doc.text('Generated on: ' + dateTime, 135, 20);

                    //// Load the image from a URL or file path
                    //var imgURL = '~/Content/images/company-logo.png';

                    angular.forEach(data, function (record, index) {
                        if (pos > 280) {
                            currentPage++;
                            pos = 20;
                            doc.addPage(); // add new page
                            doc.setFont('helvetica', 'bold');
                            doc.text('Employee List - Page ' + currentPage, 80, 20);
                            doc.setFontSize(10); // set font size for page number
                            doc.text("Page: " + currentPage, 135, 15 /*+ " of " + data.length, 150, 290*/); // add page number

                            //// Add the image to the page
                            //doc.addImage(imgURL, 'PNG', 10, 280, 40, 40);
                        }
                        doc.setFontSize(12);
                        doc.setFont('helvetica', 'normal');
                        doc.text(20, pos, record.BatchNo);
                        doc.text(50, pos, record.FirstName);
                        doc.text(80, pos, record.LastName);
                        doc.text(110, pos, record.Gender == 1 ? "Male" : "Female");
                        doc.text(140, pos, record.PermanentAddress);
                        pos += 5;
                    });

                    // add page number to last page
                    currentPage++;
                    doc.setFontSize(10);
                    doc.setFont('helvetica', 'bold');
                    doc.text("Page: " + currentPage, 135, 15/*+ " of " + data.length, 150, 290*/);

                    doc.save('Employees.pdf');
                }
            });
        };

    };



    angular.module("MVCApp").factory('FileService', ['$http', function ($http) {

        return {
            uploadFile: function (url, payload) {
                return $http({
                    url: url,
                    method: 'POST',
                    data: payload,
                    headers: { 'Content-Type': undefined }, //this is important
                    transformRequest: angular.identity //also important
                });
            },
            //otherFunctionHere: function (url, stuff) {
            //    return $http.get(url);
            //}
        };
    }]);

})();

