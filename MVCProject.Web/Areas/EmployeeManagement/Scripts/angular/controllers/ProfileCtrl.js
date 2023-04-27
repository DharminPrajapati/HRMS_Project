(function () {
    'use strict';

    angular.module("MVCApp").controller('ProfileCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'ProfileService', ProfileCtrl
    ]);

    //BEGIN DesignationCtrl
    function ProfileCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, ProfileService) {
        /* Initial Declaration */
        //$scope.ProfileDetailScope = {
        //    AttachFileId: 0,
        //    FileName: '',
        //    RefId: 0,
        //    IsActive: true
        //};
        $scope.EmployeeName = $rootScope.userContext.EmployeeName;
        $scope.Designation = $rootScope.userContext.Designation;
        $scope.Department = $rootScope.userContext.Department;


        $scope.GetUserDetails = function (employeeId) {
            ProfileService.GetUserDetail($rootScope.userContext.EmployeeId)
                .then(function (res) {
                    
                    $scope.eventmasterlist = res.data.Result;
                    //var attchementData = [];
                    //var attchementData = res.data.Result[0].FileReleativePath;
                    // var output = document.getElementById('output');
                    //output.src = attchementData.ReleativePath;
                    //output.src = attchementData;
                });
        }
        $scope.GetUserDetails();
        //    $scope.showProfile = function () {
        //        $scope.ProfilePage = true;
        //        $scope.changepasswords = false;
        //    };

        //    $scope.ProfilePage = true;
        //    $scope.changepasswords = false;

        //    $scope.showChangePassword = function () {

        //        $scope.changepasswords = true;
        //        $scope.ProfilePage = false;
        //    };

        //    $scope.changePassword = function (forgetnewpassword) {

        //        if (forgetnewpassword.newPassword != forgetnewpassword.confirmPassword) {
        //            toastr.error("Please Confirm Password and Confirm New Password Must be Same", errorTitle);
        //        }
        //        else {
        //            forgetnewpassword.PassWord = CommonFunctions.EncryptData(forgetnewpassword.PassWord)
        //            forgetnewpassword.EmpId = (userContext.EmployeeId)
        //            ProfileService.checkpass(forgetnewpassword)
        //                .then(function (res) {
        //                    if (res.data.MessageType == messageTypes.Success && res.data.IsAuthenticated) {
        //                        forgetnewpassword.PassWord = CommonFunctions.EncryptData(forgetnewpassword.newPassword)
        //                        ProfileService.updatepassword(forgetnewpassword)
        //                            .then(function (res) {
        //                                $scope.UpdatedPass = res.data.Result;
        //                                toastr.success("Password Updated", successTitle);
        //                            });
        //                    }
        //                    else {
        //                        toastr.error("Wrong Paswword", errorTitle);
        //                    }
        //                });
        //        }
        //    }
        //    $scope.showinputcode = true;



        //    //Save Profile Pic
        //    $scope.SaveProfileDetails = function (profilepic) {
        //        $scope.Attchment = $scope.profilepic;
        //        ProfileService.SaveProfileDetails(profilepic).then(function (res) {
        //            if (res) {
        //                var data = res.data;
        //                if (data.MessageType == messageTypes.Success && data.IsAuthenticated) {
        //                    toastr.success(data.Message, successTitle);
        //                    //$scope.EmployeeMasterDetailScope.output = document.getElementById('output');
        //                    //$scope.EmployeeMasterDetailScope.file = document.getElementById('file');
        //                    //output.src = '';
        //                    //file.value = "";
        //                }
        //            }
        //        });
        //    }

        //    //Display Profile Picture Onload and Validation
        //    $scope.loadFile = function (event) {
        //        var input = event.target;
        //        if (input.files && input.files[0]) {
        //            var fileName = input.files[0].name;
        //            var extension = fileName.split('.').pop().toLowerCase();
        //            if (extension !== 'jpg' && extension !== 'jpeg' && extension !== 'png' && extension !== 'gif') {
        //                toastr.warning("Please select a valid image file.", warningTitle);
        //                $("#file").focus();
        //                input.value = '';
        //            } else {
        //                var output = document.getElementById('output');
        //                output.src = URL.createObjectURL(event.target.files[0]);
        //                output.onload = function () {
        //                    URL.revokeObjectURL(output.src) // free memory
        //                    //$scope.IsImg = true;
        //                    $scope.Attachment = null
        //                }

        //                //if (output.src) {
        //                //    $scope.IsImg = true; // set the variable
        //                //}

        //            }
        //        }
        //    };

        //    //UploadFile
        //    $scope.uploadProfilePic = function (fileobj) {

        //        var fileInput = document.getElementById('file');
        //        if (fileInput.files.length > 0) {
        //            $scope.IsImg = true;
        //            var fileInput = document.getElementById('file');

        //            if (fileInput.files.length === 0) return;
        //            var file = fileInput.files[0];
        //            var payload = new FormData();
        //            payload.append("file", file);

        //            var url = $rootScope.apiURL + '/Upload/UploadImage/'

        //            FileService.uploadFiles(url, payload).then(function successCallback(response) {

        //                toastr.success("Image Uploaded", successTitle);
        //                //console.log(response);
        //                $scope.FileData = response.data.Result;
        //                $scope.profilepic = response.data.Result;
        //                $scope.SaveProfileDetails($scope.profilepic)
        //            }).catch(function (response) {
        //                response
        //            });
        //        }
        //        else {
        //            toastr.warning("Please select an image to upload", warningTitle);
        //            $("#file").focus();
        //        }
        //    }
    }

})();