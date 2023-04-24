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

    return list;
}]);