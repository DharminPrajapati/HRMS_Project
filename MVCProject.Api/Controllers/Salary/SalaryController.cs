

namespace MVCProject.Api.Controllers.Salary
{
    #region NameSapces
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    #endregion
    public class SalaryController : BaseController
    {
        private MVCProjectEntities entities;

        public SalaryController()
        {
            this.entities = new MVCProjectEntities();
        }


        /// Get All Departments 
        [HttpGet]
        public ApiResponse GetEmployeeDropDown()
        {
            var data = this.entities.TblEmployees.Where(x => x.IsActive.Value).Select(x => new { EmpName = x.FirstName, EmpId = x.EmployeeId }).OrderBy(x => x.EmpName).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }


        [HttpGet]

        public ApiResponse GetSalaryList(bool isGetAll = false)
        {
            var result = this.entities.AddSalary.Where(x => (isGetAll || x.IsActive.Value)).Select(x => new { Id = x.SalaryId }).OrderBy(e => e.Id).ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, result);
        }

        

        /// <summary>
        /// Get Employees By Id
        /// </summary>
        [HttpGet]

        public ApiResponse GetSalaryById(int salaryId)
        {
            var salaryDetail = this.entities.AddSalary.Where(x => x.SalaryId == salaryId)
                   .Select(d => new
                   {
                       SalaryId = d.SalaryId,
                       EmployeeId = d.EmployeeId,
                       BasicSalary = d.BasicSalary,
                       DA = d.DA,
                       HRA = d.HRA,
                       PF = d.PF,
                       IsActive = d.IsActive
                   }).SingleOrDefault();
            if (salaryDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, salaryDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }


        ///Get All Employee Details
        [HttpPost]
        public ApiResponse GetAllSalary(PagingParams salaryDetailsParams)
        {
            if (string.IsNullOrWhiteSpace(salaryDetailsParams.Search))
            {
                salaryDetailsParams.Search = string.Empty;
            }

            var salarylist = (from d in this.entities.AddSalary.AsEnumerable()
                                let TotalRecords = this.entities.AddSalary.AsEnumerable().Count()
                                select new
                                {
                                    //var employeelist = this.entities.TblEmployee.ToList();
                                    SalaryId = d.SalaryId,
                                    EmployeeId = d.EmployeeId,
                                    
                                    BasicSalary = d.BasicSalary,
                                    DA = d.DA,
                                    HRA = d.HRA,
                                    PF = d.PF,
                                    IsActive = d.IsActive,
                                    TotalRecords
                                }).AsQueryable().Skip((salaryDetailsParams.CurrentPageNumber - 1) * salaryDetailsParams.PageSize).Take(salaryDetailsParams.PageSize);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, salarylist);

        }

        [HttpPost]
        public ApiResponse SaveSalaryDetails(AddSalary SalaryDetail)
        {
            //if (this.entities.Attendance.Any(x => x.AttendanceId == SalaryDetail.SalaryId))
            //{
            //    return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Salary));
            //}
            //else
            //{
            AddSalary existingSalaryDetail = this.entities.AddSalary.Where(x => x.SalaryId == SalaryDetail.SalaryId).FirstOrDefault();
            if (existingSalaryDetail == null)
            {
                this.entities.AddSalary.AddObject(SalaryDetail);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Salary));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Salary));
            }

            // For Update

            else
            {

                existingSalaryDetail.EmployeeId = SalaryDetail.EmployeeId;
                existingSalaryDetail.BasicSalary = SalaryDetail.BasicSalary;
                existingSalaryDetail.DA = SalaryDetail.DA;
                existingSalaryDetail.HRA = SalaryDetail.HRA;
                existingSalaryDetail.PF = SalaryDetail.PF;
                existingSalaryDetail.IsActive = SalaryDetail.IsActive;



                this.entities.AddSalary.ApplyCurrentValues(existingSalaryDetail);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Salary));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Salary));
            }

        //}
        }
        //[HttpGet]
        //public ApiResponse GetAllSalary()
        //{
        //    //var employeelist = this.entities.TblEmployee.ToList();
        //    var Salarylist = this.entities.AddSalary.Select(d => new
        //    {
        //        SalaryId = d.SalaryId,
        //        BasicSalary = d.BasicSalary,
        //        DA = d.DA,
        //        HRA = d.HRA,
        //        PF = d.PF,
        //        IsActive = d.IsActive,
        //    });

        //    return this.Response(Utilities.MessageTypes.Success, string.Empty, Salarylist);

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
