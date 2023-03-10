﻿

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

        [HttpPost]
        public ApiResponse SaveSalaryDetails(AddSalary SalaryDetail)
        {
            if (this.entities.Attendance.Any(x => x.AttendanceId == SalaryDetail.SalaryId))
            {
                return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Salary));
            }
            else
            {
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

            }
        }
        [HttpGet]
        public ApiResponse GetAllSalary()
        {
            //var employeelist = this.entities.TblEmployee.ToList();
            var Salarylist = this.entities.AddSalary.Select(d => new
            {
                SalaryId = d.SalaryId,
                BasicSalary = d.BasicSalary,
                DA = d.DA,
                HRA = d.HRA,
                PF = d.PF,
                IsActive = d.IsActive,
            });

            return this.Response(Utilities.MessageTypes.Success, string.Empty, Salarylist);

        }
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
