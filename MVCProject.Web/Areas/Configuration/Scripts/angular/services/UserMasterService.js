angular.module('MVCApp')
    .service('UserMasterService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        ////dropdown for userrole
        //list.getrolelist = function () {
        //    return $http({
        //        method: 'GET',
        //        url: $rootScope.apiURL + '/Dropdown/Getuserroledropdown'
        //    });
        //};

        //autocmplete full name
        list.GetFullname = function (IsActive) {
            return $rootScope.apiURL + '/UserMasters/GetFullname?isActive=' + IsActive + '&searchText=';
        };

        //get the data which is already in database by id
        list.getUserbyEmployeeId = function (Id) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/UserMasters/GetUserbyEmployeeId?Id=' + Id
            });
        };

        // Get All list of UserMaster Details
        list.GetAllUserMasters = function (userMasterDetailParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/UserMasters/GetAllUserMasters/',
                data: JSON.stringify(userMasterDetailParams)
            });
        };

        // Get All list of UserMaster Details
        list.GetUserMasterList = function (isGetAll) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/UserMasters/GetUserMasterList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
            });
        };

        // Add/Update UserMaster Details
        list.SaveuserMasterDetails = function (userMasterDetail) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/UserMasters/SaveUserMasterDetails',
                data: JSON.stringify(userMasterDetail)
            });
        }

        // Get UserMaster Items
        list.GetUserMasterById = function (UserId) {
            debugger
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/UserMasters/GetUserMasterById?UserId=' + UserId
            });
        };


        ////create excel report
        //list.Export = function () {
        //    return $http({
        //        method: 'GET',
        //        url: $rootScope.apiURL + '/UserMasters/Export'
        //    });
        //};

        return list;
    }]);
