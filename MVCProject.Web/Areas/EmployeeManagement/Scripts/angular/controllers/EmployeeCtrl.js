(function () {
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
            Password : '',
            JoiningDate : '',
            PhoneNumber : null,
            AlternatePhoneNumber : null,
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
            if (frmEmployees.$valid)
            {
                EmployeeService.SaveEmployeeDetails(emplyeeDetailScope).then(function (res)
                {
                    if (res)
                    {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success && data.IsAuthenticated)
                        {
                            $scope.ClearFormData(frmEmployees);
                            toastr.success(data.Message, successTitle);
                            $scope.tableParams.reload();
                        }
                        else if (data.MessageType == messageTypes.Error)
                        {
                            toastr.error(data.Message, errorTitle);
                        }
                        else if (data.MessageType == messageTypes.Warning)
                        {
                            toastr.warning(data.Message, warningTitle);
                        }
                    }
                });
            }
        }

        $scope.EditEmployeeDetails = function (employeeId)
        {
            EmployeeService.GetEmployeeById(employeeId).then(function (res) {
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {
                        $scope.emplyeeDetailScope = data.Result;
                        $scope.emplyeeDetailScope.JoiningDate = new Date($scope.emplyeeDetailScope.JoiningDate);
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
                            else { params.total(res.data.Result[0].TotalRecords); }
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
                Password: '',
                JoiningDate: '',
                PhoneNumber: null,
                AlternatePhoneNumber: null,
                IsActive: true
            };

            frmEmployees.$setPristine();
            $("#txtEmployee").focus();
            CommonFunctions.ScrollToTop();
        };
    }
    
})();