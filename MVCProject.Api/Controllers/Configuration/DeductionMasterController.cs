
namespace MVCProject.Api.Controllers.Configuration
{
    #region Namespaces

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using Newtonsoft.Json;
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    #endregion
    public class DeductionMasterController : BaseController
    {
        private MVCProjectEntities entities;

        public DeductionMasterController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpPost]
        public ApiResponse GetDeduction(PagingParams DeductionDetailsParams)
        {
            if (string.IsNullOrWhiteSpace(DeductionDetailsParams.Search))
            {
                DeductionDetailsParams.Search = string.Empty;
            }
            var deductionlist = (from d in this.entities.DeductionMaster.AsEnumerable().Where(x => x.Description.Trim().ToLower().Contains(DeductionDetailsParams.Search.Trim().ToLower()))
                                 let TotalRecords = this.entities.DeductionMaster.AsEnumerable().Where(x => x.Description.Trim().ToLower().Contains(DeductionDetailsParams.Search.Trim().ToLower())).Count()

                                 select new
                                 {
                                     DeductionId = d.DeductionId,
                                     Description = d.Description,
                                     ShortCode = d.ShortCode,
                                     Value = d.Value,
                                     IsActive = d.IsActive
                                 }).AsQueryable()/*.OrderByField(DeductionDetailsParams.OrderByColumn, DeductionDetailsParams.IsAscending)*/.Skip((DeductionDetailsParams.CurrentPageNumber = -1) * DeductionDetailsParams.PageSize).Take(DeductionDetailsParams.PageSize);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, deductionlist);
        }

        //Get Department Master list By Id
        /// Passing Department id as Parameter.
        [HttpGet]
        public ApiResponse GetDeductionById(int deductionId)
        {
            var deductionDetail = this.entities.DeductionMaster.Where(x => x.DeductionId == deductionId)
                .Select(x => new
                {
                    DeductionId = x.DeductionId,
                    Description = x.Description,
                    ShortCode = x.ShortCode,
                    Value = x.Value,
                    EntryBy = x.EntryBy,
                    EntryDate = x.EntryDate,
                    UpdateBy = x.UpdateBy,
                    UpdatedDate = x.UpdatedDate,
                    IsActive = x.IsActive
                }).SingleOrDefault();
            if (deductionDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, deductionDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }
        /// Add/Update Department Details
        /// Using "deductionDetail" as Paramter for Department Details.

        [HttpPost]

        public ApiResponse SaveDeductionDetails(DeductionMaster deductionDetail)
        {
            if (this.entities.DeductionMaster.Any(x => x.DeductionId != deductionDetail.DeductionId && x.Description.Trim() == deductionDetail.Description.Trim()))
            {
                return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Deduction));
            }
            else
            {
                DeductionMaster existingDeductionDetail = this.entities.DeductionMaster.Where(x => x.DeductionId == deductionDetail.DeductionId).FirstOrDefault();
                if (existingDeductionDetail == null)
                {
                    this.entities.DeductionMaster.AddObject(deductionDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Deduction));
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Deduction));
                }

                // For Update

                else
                {
                    existingDeductionDetail.Description = deductionDetail.Description;
                    existingDeductionDetail.ShortCode = deductionDetail.ShortCode;
                    existingDeductionDetail.Value = deductionDetail.Value;
                    //existingDepartmentDetail.EntryById = departmentDetail.EntryById;
                    //existingDepartmentDetail.EntryDate = departmentDetail.EntryDate;
                    //existingDepartmentDetail.UpdateBy = departmentDetail.UpdateBy;
                    //existingDepartmentDetail.UpdatedDate = departmentDetail.UpdatedDate;
                    existingDeductionDetail.IsActive = deductionDetail.IsActive;

                    this.entities.DeductionMaster.ApplyCurrentValues(existingDeductionDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Deduction));
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Deduction));
                }


            }
        }

        /// <summary>
        /// Get All Deductions For Salary
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public ApiResponse GetAllDeductions()
        {
            var Deduction = this.entities.DeductionMaster.ToList();

            return this.Response(Utilities.MessageTypes.Success, string.Empty, Deduction);
        }

        /// Disposes expensive resources.
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
