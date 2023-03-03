(function () {
    'use strict';

    angular.module("MVCApp").controller('EmployeeCtrl', [
        '$scope', '$rootScope', 'ngTableParams', 'CommonFunctions', 'FileService', 'EmployeeService', EmployeeCtrl
    ]);

    function EmployeeCtrl($scope, $rootScope, ngTableParams, CommonFunctions, FileService, EmployeeService) {

       //Initial Declaration
        var employeeDetailsParams = {};
        $scope.tableParams = new ngTableParams({
            page: 1,
            count: $rootScope.pageSize,
            sort: { FirstName: 'asc' }
        }, {
            getData: function ($defer, params) {
                if (employeeDetailsParams == null) {
                    employeeDetailsParams = {};
                }

                employeeDetailsParams.Paging = CommonFunctions.GetPagingParams(params);
                employeeDetailsParams.Paging.Search = $scope.isSearchClicked ? $scope.search : '';

                EmployeeService.GetAllEmployees(employeeDetailsParams.Paging).then(function (res) {

                    if (res) {
                        var data = res.data;
                        if (res.data.MessageType == messageTypes.Success) {
                            $defer.resolve(res.data.Result);
                            //if (res.data.Result.length == 0) { }
                            //else { params.total(50); }
                            /*else { params.total(res.data.Result[0].TotalRecords); }*/

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
    }
})();