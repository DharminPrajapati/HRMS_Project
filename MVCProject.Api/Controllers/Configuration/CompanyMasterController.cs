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
    public class CompanyMasterController : BaseController
    {
        private MVCProjectEntities entities;
        public CompanyMasterController()
        {
            this.entities = new MVCProjectEntities();
        }


        /// Add/Update company Details
        /// Using "departmentDetail" as Paramter for Department Details.

        [HttpPost]

        public ApiResponse SaveCompanyMasterDetails(CompanyMaster CompanyDetail)
        {
            if (this.entities.CompanyMaster.Any(x => x.CompanyMasterId != CompanyDetail.CompanyMasterId && x.CompanyName.Trim() == CompanyDetail.CompanyName.Trim()))
            {
                return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Company_Name));
            }
            else
            {
                CompanyMaster existingCompanyDetail = this.entities.CompanyMaster.Where(x => x.CompanyMasterId == CompanyDetail.CompanyMasterId).FirstOrDefault();
                if (existingCompanyDetail == null)
                {
                    this.entities.CompanyMaster.AddObject(CompanyDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Company_Name));
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Company_Name));
                }

                // For Update

                else
                {
                    existingCompanyDetail.CompanyName = CompanyDetail.CompanyName;
                    existingCompanyDetail.ShortCode = CompanyDetail.ShortCode;
                    //existingCompanyDetail.EntryById = CompanyDetail.EntryById;
                    //existingCompanyDetail.EntryDate = CompanyDetail.EntryDate;
                    //existingCompanyDetail.UpdateBy = CompanyDetail.UpdateBy;
                    //existingCompanyDetail.UpdateDate = CompanyDetail.UpdateDate;
                    existingCompanyDetail.IsActive = CompanyDetail.IsActive;

                    this.entities.CompanyMaster.ApplyCurrentValues(existingCompanyDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Company_Name));
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Company_Name));
                }


            }
        }
        ///Get companyMasterlist By Id
        /// Passing companyMasterid as Parameter.
        [HttpGet]
        public ApiResponse GetCompanyMasterById(int CompanyMasterId)
        {
            var CompanyDetail = this.entities.CompanyMaster.Where(x => x.CompanyMasterId == CompanyMasterId)
                .Select(x => new
                {
                    CompanyMasterId = x.CompanyMasterId,
                    CompanyName=x.CompanyName,
                    ShortCode = x.ShortCode,
                    EntryBy = x.EntryBy,
                    EntryDate = x.EntryDate,
                    UpdateBy = x.UpdateBy,
                    UpdatedDate = x.UpdateDate,
                    IsActive = x.IsActive
                }).SingleOrDefault();
            if (CompanyDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, CompanyDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }
        /// Gets all Department details.

        [HttpPost]
        public ApiResponse GetAllCompanyMaster(PagingParams CompanyMasterDetailsParams)
        {
            if (string.IsNullOrWhiteSpace(CompanyMasterDetailsParams.Search))
            {
                CompanyMasterDetailsParams.Search = string.Empty;
            }
            var companyMasterlist = (from d in this.entities.CompanyMaster.AsEnumerable().Where(x => x.CompanyName.Trim().ToLower().Contains(CompanyMasterDetailsParams.Search.Trim().ToLower()))
                                       let TotalRecords = this.entities.CompanyMaster.AsEnumerable().Where(x => x.CompanyName.Trim().ToLower().Contains(CompanyMasterDetailsParams.Search.Trim().ToLower())).Count()

                                       select new
                                       {
                                           CompanyMasterId = d.CompanyMasterId,
                                           CompanyName = d.CompanyName,
                                           ShortCode = d.ShortCode,
                                           EntryBy = d.EntryBy,
                                           EntryDate = d.EntryDate,
                                           UpdateBy = d.UpdateBy,
                                           UpdatedDate = d.UpdateDate,
                                           IsActive = d.IsActive
                                       }).AsQueryable().OrderByField(CompanyMasterDetailsParams.OrderByColumn, CompanyMasterDetailsParams.IsAscending).Skip((CompanyMasterDetailsParams.CurrentPageNumber = -1) * CompanyMasterDetailsParams.PageSize).Take(CompanyMasterDetailsParams.PageSize);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, companyMasterlist);

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
