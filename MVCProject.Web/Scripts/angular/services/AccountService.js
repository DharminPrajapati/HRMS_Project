angular.module("MVCApp").service('AccountService', ["$http", "$rootScope", function ($http, $rootScope) {
    var list = {};

    // Login
    list.DoLogin = function (Model) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Account/Login',
            data: JSON.stringify(Model)
        });
    };

    // Login
    list.DoLogOut = function () {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Account/LogOut'
        });
    };

    list.SendResetPassword = function (user) {
        debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Account/SendResetPassword',
            data: JSON.stringify(user)
        });
    }
    //for create session
    list.CreateSession = function (response) {
        return $http({
            method: 'POST',
            url: '/Account/CreateSession',
            data: JSON.stringify(response)
        });
    }
    //for create session
    list.Relativepath = function (employeeId) {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Employee/GetEmployeeById?employeeId=' + employeeId
        });
    }

    //for display role
    list.GetRoles = function (UserId) {

        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Designations/GetRoles?UserId=' + UserId
        });
    }


    // Change current role of user
    list.ChangeRole = function (roleId) {
        return $http({
            method: 'PUT',
            url: $rootScope.apiURL + '/Account/ChangeRole?roleId=' + roleId
        });
    }

    // VerifyCode
    list.verifycodes = function (code) {
        debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Account/VerifyCode?code=' + code,
            data: JSON.stringify(code)
        });
    };

    // upadate pass
    list.updatepass = function (pass) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Account/UpdatePassword?pass=' + pass,
            data: JSON.stringify(pass)
        });
    };
    // Generate Security Code of User
    list.generatesecurecode = function (id) {
        debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Account/GetRandomString?id=' + id,
            data: JSON.stringify(id),
        });
    };

    // emaildid of user
    list.getuserlist = function (user) {
        debugger
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Account/getusersdetails?user=' + user
        });
    };

    return list;
}]);