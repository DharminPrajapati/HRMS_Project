angular.module('MVCApp')
    .service('DashboardService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];

        list.CountEmployees = function () {

            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Dashboard/CountEmployees'
            });

        }

        list.GenderChart = function () {

            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Dashboard/GenderChart'
            });

        }

        list.RoleChart = function () {
           
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Dashboard/RoleChart'
            });

        }


        return list;
    }]);