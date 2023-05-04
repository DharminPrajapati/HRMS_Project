using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Web.Models
{
    public class EmployeeData
    {
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; }
        public int MessageType { get; set; }
        public ResultData Result { get; set; }
    }

    public class ResultData
    {
        public int SalaryId { get; set; }
        public int EmployeeId { get; set; }
        public string BatchNo { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public bool IsActive { get; set; }
        public decimal BasicSalary { get; set; }
        //public decimal HRA { get; set; }
        //public decimal DA { get; set; }
        //public decimal PF { get; set; }
        //public decimal HRAamt { get; set; }
        //public decimal DAamt { get; set; }
        //public decimal PFamt { get; set; }
        public decimal netSalary { get; set; }
        public decimal TotalAllowance { get; set; }
        public decimal TotalDeductoin { get; set; }
        public List<Allowance> Allowances { get; set; }
        public List<Deduction> Deductions { get; set; }
        public List<AllowanceAmounts> AllowanceAmounts { get; set; }
        public List<DeductionAmounts> DeductionAmounts { get; set; }
    }


    public class Allowance
    {
        public int AllowanceId { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
    }

    public class Deduction
    {
        public int DeductionId { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
    }

    public class AllowanceAmounts
    {
        public int EmployeeId { get; set; }
        public int AllowanceId { get; set; }
        public int SalaryAllowanceId { get; set; }
        public decimal AllowanceAmount { get; set; }
    }

    public class DeductionAmounts
    {
        public int EmployeeId { get; set; }
        public int SalaryDeductionsId { get; set; }
        public decimal DeductionAmount { get; set; }
    }
}
