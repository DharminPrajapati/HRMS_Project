(function () {
    'use strict';

    angular.module("MVCApp").controller('AccountCtrl', [
        '$scope', '$rootScope', '$uibModal', '$cookies', 'AccountService', 'CommonFunctions', 'CommonService', AccountCtrl
    ]);

    //BEGIN AccountCtrl
    function AccountCtrl($scope, $rootScope, $uibModal, $cookies, AccountService, CommonFunctions, CommonService) {

        $scope.applicationLogo = "/Content/images/company-logo.png";
        $scope.isLogoLoaded = true;

        //BEGIN Check login
        $scope.CheckLogin = function (isSessionExpired) {
            if (isSessionExpired) {
                toastr.warning(sessionHasBeenExpiredMsg ? sessionHasBeenExpiredMsg : '', warningTitle);
            }
            localStorage.setItem("logout", true);
            localStorage.removeItem("logout");

            var userdata = CommonFunctions.GetCookie("REM", userdata);
            if (userdata) {
                userdata = CommonFunctions.DecryptData(userdata);
                userdata = userdata.split('░');
                if (userdata.length > 1) {
                    $scope.loginFormData = {
                        UserName: userdata[0],
                        UserPassword: userdata[1],
                        Remember: true
                    }
                }
            } else {
                $scope.loginFormData = { Remember: false };
            }
        };
        //End Check login

        //BEGIN Do Login
        $scope.DoLogin = function (Login, frmLogin) {
            var username = $scope.loginFormData.UserName;
            var password = $scope.loginFormData.UserPassword;
            if ($scope.loginFormData.Remember == true) {
                // Set a cookie with the login credentials that expires in 30 days
                var expiryDate = new Date();
                expiryDate.setDate(expiryDate.getDate() + 30);
                $cookies.put('username', username, { expires: expiryDate });
                $cookies.put('password', password, { expires: expiryDate });
            } else {
                // Delete the cookie if the checkbox is unchecked
                $cookies.remove('username');
                $cookies.remove('password');
            }
            if (frmLogin.$invalid) {
                if (frmLogin.txtUserName.$error.required) {
                    frmLogin.txtUserName.$dirty = true;
                    $("#txtUserName").focus();
                } else if (frmLogin.txtPassword.$error.required) {
                    frmLogin.txtPassword.$dirty = true;
                    $("#txtPassword").focus();
                }
                //return;

            }
            Login.UserPassword = CommonFunctions.EncryptData(Login.UserPassword);
            Login.TimeZoneMinutes = CommonFunctions.GetTimeZoneMinutes();
            AccountService.DoLogin(Login).then(function (res) {

                if (res) {
                    debugger
                    var data = res.data;
                    if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
                        AccountService.CreateSession(data.Result).then(function (response) {
                            $rootScope.isAjaxLoadingChild = true;
                            if (Login.Remember) {
                                var userdata = Login.UserName + "░" + Login.UserPassword;
                                userdata = CommonFunctions.EncryptData(userdata);
                                CommonFunctions.SetCookie("REM", userdata);
                            } else {
                                CommonFunctions.SetCookie("REM", "");
                            }
                            response.userdata;
                            CommonFunctions.RedirectToDefaultUrl();
                        });
                        //window.open("http://localhost:56563/EmployeeManagement/Employee", "_self");
                    } else {
                        $("#txtUserName").focus();
                        toastr.error(data.Message, errorTitle);
                    }
                }
            });
        };
        //END Do Login

        $scope.OpenResetPasswordModel = function () {
            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'template/ResetPasswordModel.html',
                controller: "ResetPasswordModel",
                size: 'sm',
                keyboard: true,
                backdrop: 'static',
                resolve: {}
            });
        };

        //forgetpassword
        $scope.forget = function (user) {
            debugger
            if (user == undefined) {
                return;
            }
            AccountService.getuserlist(user)
                .then(function (res) {

                    $scope.Company = res.data.Result;
                    $scope.EmployeeId = $scope.Company[0].Id;
                });
            $scope.useremail = true;
        }

        $scope.useremail = false;
        $scope.newpassword = false;
        $scope.showinputcode = false;
        $scope.showallpage = true;

        //generatesecuritycode
        $scope.generatecode = function (id) {
            debugger
            AccountService.generatesecurecode(id)
                .then(function (res) {
                    $scope.SecurityCode = res.data.Result;
                    toastr.success(res.data.Message, successTitle);
                });
            $scope.showinputcode = true;
        }

        //verify code

        $scope.verifycode = function (code) {
            debugger
            code.EmployeeId = $scope.Company[0].Id;
            AccountService.verifycodes(code)
                .then(function (res) {

                    $scope.veryfied = res.data.Result;
                    if (res.data.MessageType == messageTypes.Success && res.data.IsAuthenticated) {
                        $scope.newpassword = true;
                        $scope.showallpage = false;
                        toastr.success(res.data.Result, successTitle);
                    }
                    else {
                        toastr.error(res.data.Message, errorTitle);
                    }
                });
            $scope.showinputcode = true;
        }
        //update password
        $scope.updatepassword = function (forgetnewpassword) {
            debugger
            if (forgetnewpassword.UserPassword != forgetnewpassword.confirmPassword) {
                toastr.error("Please Put Same Password", errorTitle)
            }
            else {


                forgetnewpassword.UserPassword = CommonFunctions.EncryptData(forgetnewpassword.UserPassword)
                forgetnewpassword.EmployeeId = $scope.Company[0].Id;
                AccountService.updatepass(forgetnewpassword)
                    .then(function (res) {
                        $scope.UpdatedPass = res.data.Result;
                        toastr.success(res.data.Result, successTitle);
                        CommonFunctions.RedirectToLoginPage();
                    });
                $scope.showinputcode = true;
            }
        }

        // Init
        $scope.Init = function () {

            if (sessionStorage.getItem("resetPasswordSuccessMessage")) {
                toastr.success(sessionStorage.getItem("resetPasswordSuccessMessage"));
                sessionStorage.removeItem("resetPasswordSuccessMessage");
            }
        }();
    }

    //END AccountCtrl

    angular.module("MVCApp").controller('ResetPasswordModel', [
        "$scope", "$rootScope", "$filter", "$uibModalInstance", 'AccountService', ResetPasswordModel
    ]);

    //BEGIN AccountCtrl
    function ResetPasswordModel($scope, $rootScope, $filter, $uibModalInstance, AccountService) {
        debugger
        $scope.user = { UserName: '' };

        $scope.SendResetPasswordMail = function (frmResetPassword) {
            if (frmResetPassword.$valid) {
                AccountService.SendResetPassword($scope.user).then(function (res) {
                    if (res) {
                        var data = res.data;
                        if (data.MessageType == messageTypes.Success) {
                            toastr.success(data.Message);
                            $uibModalInstance.dismiss('cancel');
                        } else {
                            toastr.error(data.Message, errorTitle);
                        }
                    }
                });
            }
        };

        $scope.cancelActionToPerform = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }
})();