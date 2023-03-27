(function () {
    'use strict';

    angular.module("MVCApp").controller('SalaryCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'SalaryService', SalaryCtrl
    ]);

    function SalaryCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, SalaryService) {
        //Initial Declaration
        var salaryDetailsParams = {};
        



        $scope.salaryDetailScope = {

            SalaryId: 0,
            EmployeeId: 0,
            BasicSalary: '',
            IsActive: true
        };

        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return salaryDetailScope.SalaryId > 0 ? "Update" : "Save";
        };

        $scope.SaveSalaryDetails = function (salaryDetailScope , frmSalary) {
            if (frmSalary.$valid) {
                debugger;
                SalaryService.SaveSalaryDetails(salaryDetailScope).then(function (res) {
                    if (res) {
                        debugger
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

    
  

        $scope.EditSalaryDetails = function (salaryId) {
        debugger
        SalaryService.GetSalaryById(salaryId).then(function (res) {
            if (res) {
                var data = res.data;
                if (data.MessageType == messageTypes.Success) {
                    debugger;
                    $scope.salaryDetailScope = data.Result;
                    $scope.lastStorageAudit = angular.copy(data.Result);
                    CommonFunctions.ScrollUpAndFocus("txtSalary");
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

      
        $scope.Init = function () {
        
            $scope.SalaryDetails();
        }
        
   

        $scope.ClearFormData = function (frmSalary) {
            $scope.salaryDetailScope = {
                SalaryId: 0,
                EmployeeId: 0,
                BasicSalary: '',
                IsActive: true
                
            };
            $scope.PF = "";
            $scope.HRA ="";
            $scope.DA = "";
            $scope.netsalary ="" ;
            $scope.$broadcast('angucomplete-alt:clearInput');
            frmSalary.$setPristine();
            $("#txtSalary").focus();
            CommonFunctions.ScrollToTop();
        };

        $scope.calculateSalary = function () {

            var pf = ($scope.salaryDetailScope.BasicSalary * $scope.salaryDetailScope.PF) / 100;
            var da = ($scope.salaryDetailScope.BasicSalary * $scope.salaryDetailScope.DA) / 100;
            var hra = ($scope.salaryDetailScope.BasicSalary * $scope.salaryDetailScope.HRA) / 100;

            var netsalary = parseFloat($scope.salaryDetailScope.BasicSalary) + parseFloat(da) + parseFloat(hra) - parseFloat(pf);
            $scope.PF = pf.toFixed(2);
            $scope.HRA = hra.toFixed(2);
            $scope.DA = da.toFixed(2);
            $scope.netsalary = netsalary.toFixed(2);

        }

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
                debugger
                var data = res.data;
                if (!angular.isUndefined(data.Result) && data.Result != '') {
                    $scope.salaryDetailScope.DesignationName = res.data.Result.DesignationName;
                    $scope.salaryDetailScope.DepartmentName = res.data.Result.DepartmentName;
                    $scope.salaryDetailScope.BasicSalary = res.data.Result.BasicSalary;
                    $scope.salaryDetailScope.DA = res.data.Result.DA;
                    $scope.salaryDetailScope.HRA = res.data.Result.HRA;
                    $scope.salaryDetailScope.PF = res.data.Result.PF;

                    console.log(data);


                }
                else {
                    $scope.salaryDetailScope.EmployeeId = $scope.employee.Id; 
                    console.log($scope.employee.Id);
                }
            })
        }

}
})();








