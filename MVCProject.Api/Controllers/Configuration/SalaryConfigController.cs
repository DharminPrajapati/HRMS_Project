
namespace MVCProject.Api.Controllers.Configuration
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
    public class SalaryConfigController : BaseController
    {
        private MVCProjectEntities entities;

        public SalaryConfigController()
        {
            this.entities = new MVCProjectEntities();
        }


        ///// Get All Departments 
        //[HttpGet]
        //public ApiResponse GetEmployeeDropDown()
        //{
        //    var data = this.entities.TblEmployees.Where(x => x.IsActive.Value).Select(x => new { EmpName = x.FirstName, EmpId = x.EmployeeId }).OrderBy(x => x.EmpName).ToList();
        //    return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        //}


        [HttpGet]

        public ApiResponse GetConfigSalaryList(bool isGetAll = false)
        {
            var result = this.entities.SalaryConfiguration.Where(x => (isGetAll || x.IsActive.Value)).Select(x => new { Id = x.SalaryConfigurationId }).OrderBy(e => e.Id).ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, result);
        }



        /// <summary>
        /// Get Employees By Id
        /// </summary>
        [HttpGet]

        public ApiResponse GetConfigSalaryById(int configsalaryId)
        {
            var configsalaryDetail = this.entities.SalaryConfiguration.Where(x => x.SalaryConfigurationId == configsalaryId)
                   .Select(d => new
                   {
                       SalaryConfigurationId = d.SalaryConfigurationId,
                       DA = d.DA,
                       HRA = d.HRA,
                       PF = d.PF,
                       IsActive = d.IsActive
                   }).SingleOrDefault();
            if (configsalaryDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, configsalaryDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }


        ///Get All Employee Details
        [HttpPost]
        public ApiResponse GetAllConfigSalary(PagingParams configsalaryDetailsParams)
        {
            if (string.IsNullOrWhiteSpace(configsalaryDetailsParams.Search))
            {
                configsalaryDetailsParams.Search = string.Empty;
            }

            var salarylist = (from d in this.entities.SalaryConfiguration.AsEnumerable()
                              let TotalRecords = this.entities.SalaryConfiguration.AsEnumerable().Count()
                              select new
                              {
                                  //var employeelist = this.entities.TblEmployee.ToList();

                                  SalaryConfigurationId = d.SalaryConfigurationId,
                                  DA = d.DA,
                                  HRA = d.HRA,
                                  PF = d.PF,
                                  IsActive = d.IsActive,
                                  TotalRecords
                              }).AsQueryable().Skip((configsalaryDetailsParams.CurrentPageNumber - 1) * configsalaryDetailsParams.PageSize).Take(configsalaryDetailsParams.PageSize);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, salarylist);

        }

        [HttpPost]
        public ApiResponse SaveConfigSalaryDetails(SalaryConfiguration configsalaryDetail)
        {
            //if (this.entities.Attendance.Any(x => x.AttendanceId == SalaryDetail.SalaryId))
            //{
            //    return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Salary));
            //}
            //else
            //{
            SalaryConfiguration existingSalaryDetail = this.entities.SalaryConfiguration.Where(x => x.SalaryConfigurationId == configsalaryDetail.SalaryConfigurationId).FirstOrDefault();
            if (existingSalaryDetail == null)
            {
                this.entities.SalaryConfiguration.AddObject(configsalaryDetail);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Salary));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Salary));
            }

            // For Update

            else
            {

                existingSalaryDetail.SalaryConfigurationId = configsalaryDetail.SalaryConfigurationId;
                existingSalaryDetail.DA = configsalaryDetail.DA;
                existingSalaryDetail.HRA = configsalaryDetail.HRA;
                existingSalaryDetail.PF = configsalaryDetail.PF;
                existingSalaryDetail.IsActive = configsalaryDetail.IsActive;



                this.entities.SalaryConfiguration.ApplyCurrentValues(existingSalaryDetail);
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
