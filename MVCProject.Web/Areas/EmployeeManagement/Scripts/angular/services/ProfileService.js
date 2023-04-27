angular.module("MVCApp")
    .service('ProfileService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        // Get Application Configuration
        list.GetUserDetail = function (employeeId) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Employee/GetEmployeeById?employeeId=' + employeeId
            });
        };


        //list.checkpass = function (pass) {
        //    return $http({
        //        method: 'POST',
        //        url: $rootScope.apiURL + '/Account/VerifyPassword?pass=' + pass,
        //        data: JSON.stringify(pass)
        //    });
        //};

        //list.updatepassword = function (pass) {
        //    return $http({
        //        method: 'POST',
        //        url: $rootScope.apiURL + '/LoginMaster/UpdatePassword?pass=' + pass,
        //        data: JSON.stringify(pass)
        //    });
        //};

        //list.DoLogout = function () {
        //    return $http({
        //        method: 'GET',
        //        url: $rootScope.apiURL + '/LoginMaster/Logout'
        //    });
        //};

        ////Upload File
        //list.UploadFile = function (directoryPathEnumName) {
        //    return $http({
        //        method: 'POST',
        //        url: $rootScope.apiURL + '/Upload/UploadImage?directoryPathEnumName=' + directoryPathEnumName
        //    });
        //};
        //list.AddFileToDB = function (filedata) {
        //    return $http({
        //        method: 'POST',
        //        url: $rootScope.apiURL + '/Profile/FileUploadToDB',
        //        data: JSON.stringify(filedata)
        //    });
        //};

        ////Add And Update Records
        //list.SaveProfileDetails = function (ProfilePic) {

        //    return $http({
        //        method: 'POST',
        //        url: $rootScope.apiURL + '/Account/SaveProfileDetail',
        //        data: JSON.stringify(ProfilePic)
        //    });
        // };

        return list;
    }]);