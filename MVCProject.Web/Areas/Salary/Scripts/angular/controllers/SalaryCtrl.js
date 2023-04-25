(function () {
    'use strict';

    angular.module("MVCApp").controller('SalaryCtrl', [
        '$scope', '$rootScope', '$location', '$window', 'ngTableParams', 'CommonFunctions', 'FileService', 'SalaryService', SalaryCtrl
    ]);

    function SalaryCtrl($scope, $rootScope, $location, $window, ngTableParams, CommonFunctions, FileService, SalaryService) {
        //Initial Declaration
        var salaryDetailsParams = {};

        $scope.AllowancesDetailScope = {};


        $scope.salaryDetailScope = {

            SalaryId: 0,
            EmployeeId: 0,
            BasicSalary: '',
            FullnameURL: '',
            selectedProject: '',
            Id: 0,
            Name: '',
            netSalary: '',
            IsActive: true,
            TotalAllowance: 0,
            TotalDeductoin: 0
        };

        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return salaryDetailScope.SalaryId > 0 ? "Update" : "Save";
        };



        $scope.SaveSalaryDetails = function (salaryDetailScope, frmSalary) {
            if (frmSalary.$valid) {
                
                SalaryService.SaveSalaryDetails(salaryDetailScope).then(function (res) {
                    
                    if (res) {
                        // Save Salary Allowance
                        for (var i = 0; i < $scope.AllowancesDetailScope.length; i++) {
                            $scope.AllowancesDetailScope[i].EmployeeId = salaryDetailScope.EmployeeId;
                            $scope.AllowancesDetailScope[i].SalaryId = res.data.Result;
                            if (salaryDetailScope.AllowanceAmount && salaryDetailScope.AllowanceAmount[i]) {
                                $scope.AllowancesDetailScope[i].SalaryAllowanceId = salaryDetailScope.AllowanceAmount[i].SalaryAllowanceId;
                            } else {
                                $scope.AllowancesDetailScope[i].SalaryAllowanceId = 0;
                            }

                            delete $scope.AllowancesDetailScope[i].EntityKey;
                        }
                        // Save Salary Deduction
                        for (var i = 0; i < $scope.DeductionDetailScope.length; i++) {
                            $scope.DeductionDetailScope[i].EmployeeId = salaryDetailScope.EmployeeId;
                            $scope.DeductionDetailScope[i].SalaryId = res.data.Result;
                            if (salaryDetailScope.DeductionAmount && salaryDetailScope.DeductionAmount[i]) {
                                $scope.DeductionDetailScope[i].SalaryDeductionId = salaryDetailScope.DeductionAmount[i].SalaryDeductionId;
                            } else {
                                $scope.DeductionDetailScope[i].SalaryDeductionId = 0;
                            }

                            delete $scope.DeductionDetailScope[i].EntityKey;
                        }
                        // Allowance Details Api
                        SalaryService.SaveAllowanceDetails($scope.AllowancesDetailScope).then(function (res) {
                            // Deducion Details Api
                            SalaryService.SaveDeductionDetails($scope.DeductionDetailScope).then(function (res) {
                                
                                var data = res.data;
                                if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {

                                    $scope.ClearFormData(frmSalary);
                                    toastr.success(data.Message, successTitle);
                                    $scope.tableParams.reload();
                                    // set Timeout and then Redirect To Salary Details Page
                                    setTimeout(function () {
                                        $window.location.href = "/Salary/Salary/SalaryDetails";
                                    }, 2000);
                                }
                                else if (data.MessageType == messageTypes.Error) {
                                    toastr.error(data.Message, errorTitle);
                                }
                                else if (data.MessageType == messageTypes.Warning) {
                                    toastr.warning(data.Message, warningTitle);
                                }
                            });

                        });
                    }


                });
            }
        }






        $scope.EditSalaryDetails = function (salaryId, frmSalary) {


            SalaryService.GetSalaryById(salaryId).then(function (res) {
                
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        $scope.FullnameURL = SalaryService.GetFullName(true);
                        $scope.salaryDetailScope = data.Result;
                        // Allowance Amount
                        
                        for (var i = 0; i < $scope.salaryDetailScope.AllowanceAmount.length; i++) {

                            $scope.AllowancesDetailScope[i].AllowanceAmount = $scope.salaryDetailScope.AllowanceAmount[i].AllowanceAmount;
                        }
                        // Deduction Amount
                        for (var i = 0; i < $scope.salaryDetailScope.DeductionAmount.length; i++) {
                            $scope.DeductionDetailScope[i].DeductionAmount = $scope.salaryDetailScope.DeductionAmount[i].DeductionAmount;
                        }


                        $scope.lastStorageAudit = angular.copy(data.Result);
                        $scope.$broadcast('angucomplete-alt:changeInput', 'txtEmp', $scope.salaryDetailScope.Name);
                        CommonFunctions.ScrollUpAndFocus("txtSalary");
                        console.log("Salary Details:", $scope.salaryDetailScope);
                        console.log("Full Name: ", $scope.salaryDetailScope.Name);

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
            sort: { FirstName: 'asc' }
        }, {
            getData: function ($defer, params) {
                if (salaryDetailsParams == null) {
                    salaryDetailsParams = {};
                }

                salaryDetailsParams.Paging = CommonFunctions.GetPagingParams(params);
                salaryDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';

                SalaryService.GetEmployeeSalary(salaryDetailsParams.Paging).then(function (res) {

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

        $scope.employeesScope = function () {
            //debugger
            SalaryService.GetEmployeelist().then(function (res) {
                $scope.Employees = res.data.Result;
                console.log($scope.Employees);
            });
        };


        $scope.SalaryDetails = function () {

            SalaryService.GetSalConfig().then(function (res) {
                var data = res.data.Result;
                $scope.salaryDetailScope = data;

                console.log(data);
            });
        }



        $scope.DeductionDetailScope = {};
        $scope.TotalAllowance = 0;
        // Calculate Total Salary
        $scope.CalculateSalary = function () {
            
            var totalAllowance = 0;
            //Total Allowances
            for (var i = 0; i < $scope.AllowancesDetailScope.length; i++) {
                var a = $scope.AllowancesDetailScope[i];
                var allowance = ($scope.salaryDetailScope.BasicSalary * a.Value) / 100;
                totalAllowance += allowance;
                $scope.AllowancesDetailScope[i].AllowanceAmount = allowance;
            }

            $scope.salaryDetailScope.TotalAllowance = totalAllowance;


            //Total Deduction
            var totalDeductoin = 0;
            for (var i = 0; i < $scope.DeductionDetailScope.length; i++) {
                var d = $scope.DeductionDetailScope[i];
                var deduction = ($scope.salaryDetailScope.BasicSalary * d.Value) / 100;
                totalDeductoin += deduction;
                $scope.DeductionDetailScope[i].DeductionAmount = deduction;
            }

            $scope.salaryDetailScope.TotalDeductoin = totalDeductoin;

        }

        //Update Allowances Calculate 
        $scope.UpdateTotalAllowance = function () {
            var totalAllowance = 0;
            for (var i = 0; i < $scope.AllowancesDetailScope.length; i++) {
                var d = $scope.AllowancesDetailScope[i];
                totalAllowance += parseFloat(d.AllowanceAmount);
            }
            $scope.salaryDetailScope.TotalAllowance = totalAllowance;
        };

        //Update Deduction Calculate
        $scope.UpdateTotalDeduction = function () {
            var totalDeductoin = 0;
            for (var i = 0; i < $scope.DeductionDetailScope.length; i++) {
                var d = $scope.DeductionDetailScope[i];
                totalDeductoin += parseFloat(d.DeductionAmount);
            }
            $scope.salaryDetailScope.TotalDeductoin = totalDeductoin;
        };



        //Gross Salary
        $scope.CalGrossSalary = function () {
            $scope.GrossSalary = parseFloat($scope.salaryDetailScope.BasicSalary) + $scope.salaryDetailScope.TotalAllowance;
        }

        $scope.CalNetSalary = function () {
            //Net Salary
            $scope.salaryDetailScope.netSalary = parseFloat($scope.salaryDetailScope.BasicSalary) + $scope.salaryDetailScope.TotalAllowance - $scope.salaryDetailScope.TotalDeductoin;

        }


        //Salary Allowance 
        $scope.Allowance = function () {
            
            SalaryService.Allowances().then(function (res) {
                $scope.Allowances = res.data.Result;
                $scope.AllowancesDetailScope = $scope.Allowances;
                $scope.EditSalaryDetails($location.search().Id);
            })
        }
        $scope.Allowance();

        //Salary Deduction 
        $scope.Deduction = function () {
            SalaryService.Deductions().then(function (res) {
                $scope.Deductions = res.data.Result;
                $scope.DeductionDetailScope = $scope.Deductions;
            })
        }
        $scope.Deduction();

        $scope.Init = function () {
            
            $scope.SalaryDetails();
            $scope.Allowance();
            $scope.Deduction();
        }





        $scope.ClearFormData = function (frmSalary) {
            $scope.salaryDetailScope = {
                SalaryId: 0,
                EmployeeId: 0,
                BasicSalary: '',
                netSalary: null,
                IsActive: true

            };
            $scope.SalaryDetails();
            $scope.salaryDetailScope.PFamt = "";
            $scope.salaryDetailScope.HRAamt = "";
            $scope.salaryDetailScope.DAamt = "";
            for (let allowance of $scope.AllowancesDetailScope) {
                allowance.AllowanceAmount = 0;
            }

            for (let deduction of $scope.DeductionDetailScope) {
                deduction.DeductionAmount = 0;
            }
            $scope.TotalAllowance = "";
            $scope.TotalDeductoin = "";
            $scope.$broadcast('angucomplete-alt:clearInput');
            frmSalary.$setPristine();
            $("#txtSalary").focus();
            CommonFunctions.ScrollToTop();
        };

        $scope.FullnameURL = SalaryService.GetFullName(true);

        $scope.employee = [];
        $scope.selectedProject = function (selected) {
            if (angular.isDefined(selected)) {
                $scope.employee = selected.originalObject
                $scope.getEmployeebyId($scope.employee.Id)
            }
        }

        $scope.getEmployeebyId = function (Id) {
            SalaryService.getEmployeebyId(Id).then(function (res) {

                var data = res.data;
                if (!angular.isUndefined(data.Result) && data.Result != '') {
                    $scope.salaryDetailScope = res.data.Result;
                    for (var i = 0; i < $scope.salaryDetailScope.AllowanceAmount.length; i++) {
                        $scope.AllowancesDetailScope[i].AllowanceAmount = $scope.salaryDetailScope.AllowanceAmount[i].AllowanceAmount;
                    }
                    for (var i = 0; i < $scope.salaryDetailScope.DeductionAmount.length; i++) {
                        $scope.DeductionDetailScope[i].DeductionAmount = $scope.salaryDetailScope.DeductionAmount[i].DeductionAmount;
                    }
                    console.log(data);


                }
                else {
                    $scope.salaryDetailScope.EmployeeId = $scope.employee.Id;
                    $scope.salaryDetailScope.DesignationName = $scope.employee.DesignationName;
                    $scope.salaryDetailScope.DepartmentName = $scope.employee.DepartmentName;

                    for (var i = 0; i < $scope.salaryDetailScope.AllowanceAmount.length; i++) {
                        $scope.AllowancesDetailScope[i].AllowanceAmount = $scope.salaryDetailScope.AllowanceAmount[i].AllowanceAmount;
                    }
                    for (var i = 0; i < $scope.salaryDetailScope.DeductionAmount.length; i++) {
                        $scope.DeductionDetailScope[i].DeductionAmount = $scope.salaryDetailScope.DeductionAmount[i].DeductionAmount;
                    }
                    $scope.salaryDetailScope.TotalAllowance = $scope.employee.TotalAllowance;
                    $scope.salaryDetailScope.TotalDeductoin = $scope.employee.TotalDeductoin;
                    console.log($scope.employee.Id);
                }
            })
        }

        $scope.Export = function () {

            SalaryService.CreateExcelReport().then(function (res) {
                var data = res.data;

                if (data.MessageType == messageTypes.Success) {

                    var fileName = res.data.Result;
                    var params = { fileName: fileName };
                    var form = document.createElement("form");
                    form.setAttribute("method", "POST");
                    form.setAttribute("action", "/Salary/Salary/DownloadFile");
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

        $scope.PayslipGenerate = function (employeeId) {
            if (employeeId != undefined) {
                SalaryService.GeneratePayslip(employeeId).then(function (res) {
                    if (res != null) {
                        $scope.PayslipGenerate = res.data.Result;
                    }
                });
            }
        };

        $scope.GetData = function (employeeId) {

            var param = $location.search();
            $scope.employeeId = param.EmployeeId;

            SalaryService.GetEmployeeById($scope.employeeId).then(function (res) {
                if (res != null) {

                    $scope.employeeData = res.data.Result;
                }
            });
        }
    }
})();








