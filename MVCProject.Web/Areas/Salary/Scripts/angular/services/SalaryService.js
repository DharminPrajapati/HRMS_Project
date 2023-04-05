angular.module('MVCApp')
    .service('SalaryService', ['$rootScope', '$http', function ($rootScope, $http) {
        var list = [];


        //Get Salary With Employees
        list.GetEmployeeSalary = function (salaryDetailsParams) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Salary/GetEmployeeSalary/',
                data: JSON.stringify(salaryDetailsParams)
            });
        }



        // Add/Update Employee Details
        list.SaveSalaryDetails = function (SalaryDetail) {
            return $http({
                method: 'POST',
                url: $rootScope.apiURL + '/Salary/SaveSalaryDetails',
                data: JSON.stringify(SalaryDetail)
            });
        }

        // Get Employees By Id
        list.GetSalaryById = function (salaryId) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Salary/GetSalaryById?salaryId=' + salaryId
            });
        };

        //Get employee DropDown
        list.GetEmployeelist = function () {
            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/Salary/GetSalaryEmployeeList/'
            });
        };


        list.GetFullName = function (IsActive) {
            return $rootScope.apiURL + '/Salary/GetFullName?isActive=' + IsActive + '&searchText=';
        };

        list.getEmployeebyId = function (Id) {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Salary/GetDeptDesiByEmployeeId?Id=' + Id
            });
        }



        list.GetSalConfig = function () {

            return $http({
                methd: 'GET',
                url: $rootScope.apiURL + '/SalaryConfig/GetSalConfig'
            });

        };
        //Create Excel Salray Report For Employees
        list.CreateExcelReport = function () {
            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Salary/CreateEmployeeListReport'
            });
        };

        //Generate Pdf Payslip
        list.GeneratePayslip = function (employeeId) {

            return $http({
                method: 'GET',
                url: $rootScope.apiURL + '/Salary/GeneratePaySlipByEmployeeId?employeeId=' + employeeId
            });
        }

        return list;
    }]);