(function () {
    'use strict';

    angular.module("MVCApp").controller('ReportsCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'ReportsService', ReportsCtrl
    ]);

    function ReportsCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, ReportsService) {
        var employeeDetailsParams = {};

        //$scope.emplyeeDetailScope = {
        //    EmployeeId: 0,
        //    CompanyMasterId: 0,
        //    FirstName: '',
        //    LastName: '',
        //    Email: '',
        //    JoiningDate: '',
        //    PhoneNumber: null,
        //    AlternatePhoneNumber: null,
        //    DesignationId: 0,
        //    DepartmentId: 0,
        //    BirthDate: null,
        //    Gender: 1,
        //    PermanentAddress: '',
        //    TemporaryAddress: '',
        //    Pincode: null,
        //    InstitutionName: '',
        //    CourseName: '',
        //    CourseStartDate: null,
        //    CourseEndDate: null,
        //    Grade: '',
        //    Degree: '',
        //    CompanyName: '',
        //    LastJobLocation: '',
        //    JobPosition: '',
        //    FromPeriod: null,
        //    ToPeriod: null,
        //    IsActive: true,
        //    Attachment: null,
        //    file: '',
        //    output: '',

        //};
        $scope.Export = function () {

            ReportsService.CreateExcelReport().then(function (res) {
                var data = res.data;

                if (res.data.MessageType == messageTypes.Success) {

                    var fileName = res.data.Result;
                    var params = { fileName: fileName };
                    var form = document.createElement("form");
                    form.setAttribute("method", "POST");
                    form.setAttribute("action", "/Reports/Reports/DownloadFile");
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
        $scope.ExportPDF = function () {

            ReportsService.ExportPDF().then(function (res) {

                var data = res.data;
                if (res.data.MessageType == messageTypes.Success) {
                    var filename = res.data.Result;
                    var params = { filename: filename };
                    var form = document.createElement("form");
                    form.setAttribute("method", "POST");
                    form.setAttribute("action", "/Reports/Reports/DownloadPDF");
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

                ReportsService.GetEmployeeDetails(employeeDetailsParams.Paging).then(function (res) {
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
    }
})();
