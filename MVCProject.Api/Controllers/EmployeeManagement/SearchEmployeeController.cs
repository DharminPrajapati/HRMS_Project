namespace MVCProject.Api.Controllers.EmployeeManagement
{
    #region NameSapces
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Web;
    using System.Web.Http;
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    using Newtonsoft.Json;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    #endregion

    public class SearchEmployeeController : BaseController
    {
        private MVCProjectEntities entities;
        public SearchEmployeeController()
        {
            this.entities = new MVCProjectEntities();
        }

        ///Get All Employee Details
        //[HttpPost]
        //public ApiResponse GetAllEmployees(PagingParams employeeDetailsParams)
        //{
        //    if (string.IsNullOrWhiteSpace(employeeDetailsParams.Search))
        //    {
        //        employeeDetailsParams.Search = string.Empty;
        //    }

        //    var employeelist = (from d in this.entities.TblEmployees.AsEnumerable().Where(x => x.FirstName.Trim().ToLower().Contains(employeeDetailsParams.Search.Trim().ToLower()))
        //                        let TotalRecords = this.entities.TblEmployees.AsEnumerable().Where(x => x.FirstName.Trim().ToLower().Contains(employeeDetailsParams.Search.Trim().ToLower())).Count()
        //                        select new
        //                        {
        //                            //var employeelist = this.entities.TblEmployee.ToList();

        //                            EmployeeId = d.EmployeeId,
        //                            SrNo = d.SrNo,
        //                            BatchNo = d.BatchNo,
        //                            FirstName = d.FirstName,
        //                            LastName = d.LastName,
        //                            Email = d.Email,
        //                            Password = d.Password,
        //                            JoiningDate = d.JoiningDate,
        //                            PhoneNumber = d.PhoneNumber,
        //                            AlternatePhoneNumber = d.AlternatePhoneNumber,
        //                            Designation = d.DesignationId,
        //                            Department = d.DepartmentId,
        //                            DepartmentName = d.TblDepartmentReference.Value.DepartmentName,
        //                            DesignationName = d.DesignationReference.Value.DesignationName,
        //                            BirthDate = d.BirthDate,
        //                            Gender = d.Gender,
        //                            PermanentAddress = d.PermanentAddress,
        //                            TemporaryAddress = d.TemporaryAddress,
        //                            Pincode = d.Pincode,
        //                            InstitutionName = d.InstitutionName,
        //                            CourseName = d.CourseName,
        //                            CourseStartDate = d.CourseStartDate,
        //                            CourseEndDate = d.CourseEndDate,
        //                            Grade = d.Grade,
        //                            Degree = d.Degree,
        //                            CompanyName = d.CompanyName,
        //                            LastJobLocation = d.LastJobLocation,
        //                            JobPosition = d.JobPosition,
        //                            FromPeriod = d.FromPeriod,
        //                            ToPeriod = d.ToPeriod,
        //                            IsActive = d.IsActive,
        //                            TotalRecords
        //                        }).AsQueryable().Skip((employeeDetailsParams.CurrentPageNumber - 1) * employeeDetailsParams.PageSize).Take(employeeDetailsParams.PageSize);

        //    return this.Response(Utilities.MessageTypes.Success, string.Empty, employeelist);

        //}
        //[HttpPost]
        //public ApiResponse GetEmployeeDetails(PagingParams SearchEmployeeDetailsParams)
        //{
        //    if (string.IsNullOrWhiteSpace(SearchEmployeeDetailsParams.Search))
        //    {
        //        SearchEmployeeDetailsParams.Search = string.Empty;
        //    }
        //    var results = this.entities.sp_Emp_GetAllEmployees().Where(x => x.FirstName.Trim().ToLower().Contains(SearchEmployeeDetailsParams.Search.Trim().ToLower())).ToList().AsQueryable().Skip((SearchEmployeeDetailsParams.CurrentPageNumber - 1) * SearchEmployeeDetailsParams.PageSize).Take(SearchEmployeeDetailsParams.PageSize);
        //    var TotalRecords = this.entities.sp_Emp_GetAllEmployees().Where(x => x.FirstName.Trim().ToLower().Contains(SearchEmployeeDetailsParams.Search.Trim().ToLower())).Count();


        //    return this.Response(Utilities.MessageTypes.Success, string.Empty, new { list = results, Total = TotalRecords });

        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.entities != null)
                {
                    this.entities.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
