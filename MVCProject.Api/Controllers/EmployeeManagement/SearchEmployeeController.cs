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
    using MVCProject.Api.Models.Extended.FilterCriterias;
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

        [HttpPost]
        public ApiResponse AdvancedSearchEmployee([FromBody] PagingParams SearchEmployeeDetailsParams, [FromUri] SearchParam searchParam)
        {
            var result = this.entities.sp_hrms_searchemp(searchParam.FirstName, searchParam.DepartmentId, searchParam.DesignationId).ToList();
            var TotalRecords = result.Count();
            var searchemployee = result.Select(g => new
            {
                EmployeeId = g.EmployeeId,
                FirstName = g.FirstName,
                DepartmentId = g.DepartmentId,
                DesignationId = g.DesignationId,
                DepartmentName = g.DepartmentName,
                DesignationName = g.DesignationName,
                BatchNo = g.BatchNo,
                LastName = g.LastName,
                Email = g.Email,
                Password = g.Password,
                JoiningDate = g.JoiningDate,
                PhoneNumber = g.PhoneNumber,
                AlternatePhoneNumber = g.AlternatePhoneNumber,
                BirthDate = g.BirthDate,
                Gender = g.Gender,
                PermanentAddress = g.PermanentAddress,
                TemporaryAddress = g.TemporaryAddress,
                Pincode = g.Pincode,
                InstitutionName = g.InstitutionName,
                CourseName = g.CourseName,
                CourseStartDate = g.CourseStartDate,
                CourseEndDate = g.CourseEndDate,
                Grade = g.Grade,
                Degree = g.Degree,
                CompanyName = g.CompanyName,
                LastJobLocation = g.LastJobLocation,
                JobPosition = g.JobPosition,
                FromPeriod = g.FromPeriod,
                ToPeriod = g.ToPeriod,
                TotalRecords
            }).AsEnumerable()
            .Skip((SearchEmployeeDetailsParams.CurrentPageNumber - 1) * SearchEmployeeDetailsParams.PageSize).Take(SearchEmployeeDetailsParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, searchemployee);

        }

        //[HttpPost]
        //public ApiResponse AdvancedSearchEmployee([FromBody] PagingParams SearchEmployeeDetailsParams, [FromUri] SearchParam searchParam)
        //{
        //    var result = this.entities.sp_hrms_searchemp(searchParam.FirstName, searchParam.DepartmentId, searchParam.DesignationId).ToList();
        //    var TotalRecords = result.Count();
        //    var searchemployee = result.Select(g => new
        //    {
        //        EmployeeId = g.EmployeeId,
        //        FirstName = g.FirstName,
        //        DepartmentId = g.DepartmentId,
        //        DesignationId = g.DesignationId,
        //        DepartmentName = g.DepartmentName,
        //        DesignationName = g.DesignationName,
        //        TotalRecords
        //    }).AsEnumerable()
        //    .Skip((SearchEmployeeDetailsParams.CurrentPageNumber - 1) * SearchEmployeeDetailsParams.PageSize).Take(SearchEmployeeDetailsParams.PageSize);
        //    return this.Response(MessageTypes.Success, string.Empty, searchemployee);

        //}

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
