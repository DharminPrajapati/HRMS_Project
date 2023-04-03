(function () {
    'use strict';

    angular.module("MVCApp").controller('UserMasterCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'UserMasterService', UserMasterCtrl
    ]);

    //BEGIN UserMasterCtrl
    function UserMasterCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, UserMasterService) {
        console.log($rootScope.apiURL)


        $scope.FullnameURL = UserMasterService.GetFullname(true);
        $scope.employee = [];
        $scope.selectedProject = function (selected) {
            if (angular.isDefined(selected)) {
                $scope.employee = selected.originalObject
                $scope.getUserbyEmployeeId($scope.employee.Id)
            }
        }

        //get the EmpName from the EmpId
        $scope.getUserbyEmployeeId = function (Id) {
            UserMasterService.getUserbyEmployeeId(Id)
                .then(function (res) {

                    var data = res.data;
                    if (!angular.isUndefined(data.Result) && data.Result != '') {

                        $scope.userMasterDetailScope = res.data.Result;
                    } else {
                        $scope.userMasterDetailScope.EmployeeId = $scope.employee.Id;
                        $scope.userMasterDetailScope.FirstName = $scope.employee.Name;
                    }
                });
        }

        ////Get the userRole Multiselect Dropdown
        //$scope.rolelist = [];
        //$scope.Role = [];
        //$scope.multiselectsettingrole = {
        //    idProp: 'RoleId',
        //    displayProp: 'Name',
        //    externalIdProp: 'RoleId',
        //    scrollabe: true
        //}

        //$scope.roleScope = function () {
        //    UserMasterService.getrolelist()
        //        .then(function (res) {
        //            $scope.Role = res.data.Result;
        //        });
        //}

        /* Initial Declaration */
        $scope.sampleDate = new Date();
        var userMasterDetailParams = {};
        $scope.userMasterDetailScope = {
            UserId: 0,
            UserName: '',
            EmployeeId: 0,
            FirstName: '',
            Password: '',
            UserPassword: '',
            //UserRole: [],
            //UserRoleName: '',
            //RoleId: 0,
            //UserRoleId: 0,
            IsLock: true,
            IsActive: true,

        };
        $scope.isSearchClicked = false;
        $scope.lastStorageAudit = $scope.lastStorageAudit || {};
        $scope.operationMode = function () {
            return $scope.userMasterDetailScope.UserId > 0 ? "Update" : "Save";
        };

        console.log($scope.userMasterDetailScope);
        // BEGIN Add/Update UserMaster details
        $scope.SaveuserMasterDetails = function (userMasterDetailScope, frmUserMaster, employee) {
            //if (!$rootScope.permission.CanWrite) { return; }
            if (frmUserMaster.$valid) {
                UserMasterService.SaveuserMasterDetails(userMasterDetailScope).then(function (res) {
                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                            $scope.ClearFormData(frmUserMaster);
                            toastr.success(data.Message, successTitle);
                            //$scope.GetAllUserMasters();
                            $scope.tableParams.reload();
                        } else if (data.MessageType == messageTypes.Error) {// Error
                            toastr.error(data.Message, errorTitle);
                        } else if (data.MessageType == messageTypes.Warning) {// Warning
                            toastr.warning(data.Message, warningTitle);
                        }
                    }
                });
            }
        };

        // BEGIN Bind form data for edit UserMaster
        $scope.EditUserMasterDetails = function (UserId) {
            debugger;
            UserMasterService.GetUserMasterById(UserId).then(function (res) {
                if (res) {
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success) {// Success
                        $scope.userMasterDetailScope = data.Result;
                        $scope.lastStorageAudit = angular.copy(data.Result);
                        CommonFunctions.ScrollUpAndFocus("txtUserMaster");
                    } else if (data.MessageType == messageTypes.Error) {// Error
                        toastr.error(data.Message, errorTitle);
                    }
                }
                $rootScope.isAjaxLoadingChild = false;
            });
        };

        //  Create Excel Report of UserMaster
        //$scope.createReport = function () {
        //    if (!$rootScope.permission.CanWrite) { return; }
        //    var filename = "UserMaster_" + $rootScope.fileDateName + ".xls";
        //    CommonFunctions.DownloadReport('/UserMasters/CreateUserMasterListReport', filename);
        //};

        // Reset UserMaster form data; edit + reset will remove all changes made in edit mode.
        $scope.resetuserMasterDetails = function (frmUserMaster) {
            if ($scope.operationMode() == "Update") {
                $scope.frmUserMaster = angular.copy($scope.lastStorageGroup);
                frmUserMaster.$setPristine();
            } else {
                $scope.clearData(frmUserMaster);
            }
        };

        // Clear UserMaster form data.
        $scope.ClearFormData = function (frmUserMaster) {
            $scope.userMasterDetailScope = {
                UserId: 0,
                UserName: '',
                FullName: '',
                EmployeeId: 0,
                FirstName: '',
                Password: '',
                //UserRole: [],
                //UserRoleName: '',
                //RoleId: 0,
                //UserRoleId: 0,
                IsLock: true,
                IsActive: true,
                FullnameURL: '',
                selectedProject: '',
            };
            $scope.$broadcast('angucomplete-alt:clearInput');
            frmUserMaster.$setPristine();
            $("#txtUserMaster").focus();
            CommonFunctions.ScrollToTop();
        };

        //// export to excel using npoi
        //$scope.Export = function () {
        //    UserMasterService.Export().then(function (res) {
        //        var data = res.data;
        //        if (res.data.MessageType == messageTypes.Success) {// Success
        //            var fileName = res.data.Result;
        //            var params = { fileName: fileName };
        //            var form = document.createElement("form");
        //            form.setAttribute("method", "POST");
        //            form.setAttribute("action", "/UserMaster/DownloadFile");
        //            form.setAttribute("target", "_blank");

        //            for (var key in params) {
        //                if (params.hasOwnProperty(key)) {
        //                    var hiddenField = document.createElement("input");
        //                    hiddenField.setAttribute("type", "hidden");
        //                    hiddenField.setAttribute("name", key);
        //                    hiddenField.setAttribute("value", params[key]);

        //                    form.appendChild(hiddenField);
        //                }
        //            }

        //            document.body.appendChild(form);
        //            form.submit();

        //            $defer.resolve(res.data.Result);
        //            if (res.data.Result.length == 0) {
        //            } else {
        //                params.total(res.data.Result[0].TotalRecords);
        //            }
        //        } else if (res.data.MessageType == messageTypes.Error) {// Error
        //            toastr.error(res.data.Message, errorTitle);
        //        }
        //        $rootScope.isAjaxLoadingChild = false;
        //        CommonFunctions.SetFixHeader();
        //    });
        //};

        //Load UserMaster List
        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sorting: { UserName: 'asc' }
        }, {
            getData: function ($defer, params) {
                if (userMasterDetailParams == null) {
                    userMasterDetailParams = {};
                }
                userMasterDetailParams.Paging = CommonFunctions.GetPagingParams(params);
                userMasterDetailParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';
                //Load Employee List
                UserMasterService.GetAllUserMasters(userMasterDetailParams.Paging).then(function (res) {
                    var data = res.data;
                    if (res.data.MessageType == messageTypes.Success) {// Success
                        $defer.resolve(res.data.Result);
                        if (res.data.Result.length == 0) {
                        } else {
                            params.total(res.data.Result[0].TotalRecords);
                        }
                    } else if (res.data.MessageType == messageTypes.Error) {// Error
                        toastr.error(res.data.Message, errorTitle);
                    }
                    $rootScope.isAjaxLoadingChild = false;
                    CommonFunctions.SetFixHeader();
                });
            }
        });
    }
})();

