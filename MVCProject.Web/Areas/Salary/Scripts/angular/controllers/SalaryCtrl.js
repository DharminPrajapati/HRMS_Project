(function () {
    'use strict';

    angular.module("MVCApp").controller('SalaryCtrl', [
        '$scope', '$rootScope', '$location','ngTableParams', 'CommonFunctions', 'FileService', 'SalaryService', SalaryCtrl
    ]);

    function SalaryCtrl($scope, $rootScope, $location, ngTableParams, CommonFunctions, FileService, SalaryService) {
        //Initial Declaration
        var salaryDetailsParams = {};
        



        $scope.salaryDetailScope = {

            SalaryId: 0,
            EmployeeId: 0,
            BasicSalary: '',
            FullnameURL: '',
            selectedProject: '',
            Id: 0,
            Name: '',
            netSalary: '',
            IsActive: true
        };

        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return salaryDetailScope.SalaryId > 0 ? "Update" : "Save";
        };

        $scope.SaveSalaryDetails = function (salaryDetailScope , frmSalary) {
            if (frmSalary.$valid) {
                
                SalaryService.SaveSalaryDetails(salaryDetailScope).then(function (res) {
                    if (res) {
                       
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                            $scope.ClearFormData(frmSalary);
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

    
  

        $scope.EditSalaryDetails = function (salaryId, frmSalary) {
          
            // $scope.ClearFormData(frmSalary);

            SalaryService.GetSalaryById(salaryId).then(function (res) {
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        

                        $scope.FullnameURL = SalaryService.GetFullName(true);
                        $scope.salaryDetailScope = data.Result;
                        $scope.lastStorageAudit = angular.copy(data.Result);
                        $scope.$broadcast('angucomplete-alt:changeInput', 'txtEmp', $scope.salaryDetailScope.Name);
                        CommonFunctions.ScrollUpAndFocus("txtSalary");
                        //$scope.FullnameURL = SalaryService.GetFullName(true);
                        // $scope.salaryDetailScope.Name = data.Result.Name;
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

      
        $scope.Allowances = [];
        $scope.TotalAllowances = 0;
        // Calculate Total Salary
        $scope.CalculateSalary = function () {

            var totalAllowance = 0;
            //Total Allowances
            for (var i = 0; i < $scope.Allowances.length; i++) {
                var a = $scope.Allowances[i];
                var allowance = ($scope.salaryDetailScope.BasicSalary * a.Value) / 100;
                totalAllowance += allowance;
                $scope.Allowances[i].Amount = allowance;
            }
            $scope.TotalAllowances = totalAllowance;

            //Total Deduction
            var totalDeduciton = 0;
            for (var i = 0; i < $scope.Deductions.length; i++) {
                var d = $scope.Deductions[i];
                var deduction = ($scope.salaryDetailScope.BasicSalary * d.Value) / 100;
                totalDeduciton += deduction;
                $scope.Deductions[i].Amount = deduction;
            }
            $scope.TotalDeducitons = totalDeduciton;

        }

        //Update Allowances Calculate 
        $scope.UpdateTotalAllowances = function () {
            var totalAllowance = 0;
            for (var i = 0; i < $scope.Allowances.length; i++) {
                var d = $scope.Allowances[i];
                totalAllowance += parseFloat(d.Amount);
            }
            $scope.TotalAllowances = totalAllowance;
        };

        //Update Deduction Calculate
        $scope.UpdateTotalDeduction = function () {
            var totalDeduciton = 0;
            for (var i = 0; i < $scope.Deductions.length; i++) {
                var d = $scope.Deductions[i];
                totalDeduciton += parseFloat(d.Amount);
            }
            $scope.TotalDeducitons = totalDeduciton;
        };



        //Gross Salary
        $scope.CalGrossSalary = function () {
            $scope.GrossSalary = parseFloat($scope.salaryDetailScope.BasicSalary) + $scope.TotalAllowances;
        }

        $scope.CalNetSalary = function () {
            //Net Salary
            $scope.salaryDetailScope.netSalary = parseFloat($scope.salaryDetailScope.BasicSalary) + $scope.TotalAllowances - $scope.TotalDeducitons;

        }
        //Salary Allowance 
        $scope.Allowance = function () {

            SalaryService.Allowances().then(function (res) {
                $scope.Allowances = res.data.Result;
            })
        }
        $scope.Allowance();

        //Salary Deduction 
        $scope.Deduction = function () {
            SalaryService.Deductions().then(function (res) {
                $scope.Deductions = res.data.Result;
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
                    console.log(data);


                }
                else {
                    $scope.salaryDetailScope.EmployeeId = $scope.employee.Id;
                    $scope.salaryDetailScope.DesignationName = $scope.employee.DesignationName;
                    $scope.salaryDetailScope.DepartmentName = $scope.employee.DepartmentName;
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
                //else if (res.data.MessageType == messageTypes.Error) {
                //    toastr.error(res.data.Message, errorTitle);
                //}
                //  CommonFunctions.DownloadReport('/Employee/CreateEmployeeListReport', fileName);
                $rootScope.isAjaxLoadingChild = false;
                CommonFunctions.SetFixHeader();

                debugger
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








