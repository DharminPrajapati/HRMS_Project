namespace MVCProject.Api.Controllers.Configuration
{
    #region Namespaces
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
    public class AllowanceMasterController : BaseController
    {
        private MVCProjectEntities entities;
        public AllowanceMasterController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpPost]

        public ApiResponse SaveAllowanceMasterDetails(AllowanceMaster AllowanceDetail)
        {
            if (this.entities.AllowanceMaster.Any(x => x.AllowanceId != AllowanceDetail.AllowanceId && x.Description.Trim() == AllowanceDetail.Description.Trim()))
            {
                return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Department));
            }
            else
            {
                AllowanceMaster existingAllowanceDetail = this.entities.AllowanceMaster.Where(x => x.AllowanceId == AllowanceDetail.AllowanceId).FirstOrDefault();
                if (existingAllowanceDetail == null)
                {
                    this.entities.AllowanceMaster.AddObject(AllowanceDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Department));
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Department));
                }

                // For Update

                else
                {
                    existingAllowanceDetail.Description = AllowanceDetail.Description;
                    existingAllowanceDetail.ShortCode = AllowanceDetail.ShortCode;
                    existingAllowanceDetail.Value = AllowanceDetail.Value;
                    //existingDepartmentDetail.EntryById = departmentDetail.EntryById;
                    //existingDepartmentDetail.EntryDate = departmentDetail.EntryDate;
                    //existingDepartmentDetail.UpdateBy = departmentDetail.UpdateBy;
                    //existingDepartmentDetail.UpdatedDate = departmentDetail.UpdatedDate;
                    existingAllowanceDetail.IsActive = AllowanceDetail.IsActive;

                    this.entities.AllowanceMaster.ApplyCurrentValues(existingAllowanceDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Department));
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Department));
                }
            }
        }
        ///Get AllowanceMasterlist By Id
        /// Passing Allowanceid as Parameter.
        [HttpGet]
        public ApiResponse GetAllowanceMasterById(int AllowanceId)
        {
            var AllowanceDetail = this.entities.AllowanceMaster.Where(x => x.AllowanceId == AllowanceId)
                .Select(x => new
                {
                    AllowanceId = x.AllowanceId,
                    Description = x.Description,
                    ShortCode = x.ShortCode,
                    Value = x.Value,
                    EntryBy = x.EntryBy,
                    EntryDate = x.EntryDate,
                    UpdateBy = x.UpdateBy,
                    UpdatedDate = x.UpdatedDate,
                    IsActive = x.IsActive
                }).SingleOrDefault();
            if (AllowanceDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, AllowanceDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }
        /// Gets all Department details.

        [HttpPost]
        public ApiResponse GetAllowanceMaster(PagingParams AllowanceMasterDetailsParams)
        {
            if (string.IsNullOrWhiteSpace(AllowanceMasterDetailsParams.Search))
            {
                AllowanceMasterDetailsParams.Search = string.Empty;
            }
            var AllowanceMasterlist = (from d in this.entities.AllowanceMaster.AsEnumerable().Where(x => x.Description.Trim().ToLower().Contains(AllowanceMasterDetailsParams.Search.Trim().ToLower()))
                                       let TotalRecords = this.entities.AllowanceMaster.AsEnumerable().Where(x => x.Description.Trim().ToLower().Contains(AllowanceMasterDetailsParams.Search.Trim().ToLower())).Count()

                                       select new
                                       {
                                           AllowanceId = d.AllowanceId,
                                           Description = d.Description,
                                           ShortCode = d.ShortCode,
                                           Value = d.Value,
                                           EntryBy = d.EntryBy,
                                           EntryDate = d.EntryDate,
                                           UpdateBy = d.UpdateBy,
                                           UpdatedDate = d.UpdatedDate,
                                           IsActive = d.IsActive
                                       }).AsQueryable().OrderByField(AllowanceMasterDetailsParams.OrderByColumn, AllowanceMasterDetailsParams.IsAscending).Skip((AllowanceMasterDetailsParams.CurrentPageNumber = -1) * AllowanceMasterDetailsParams.PageSize).Take(AllowanceMasterDetailsParams.PageSize);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, AllowanceMasterlist);

        }

        /// <summary>
        ///  Get All Allowance For Salary 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse GetAllowance()
        {
            var Allowances = this.entities.AllowanceMaster.ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, Allowances);
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
