

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
    /// Department Master 
    public class DepartmentController : BaseController
    {
        ///Holds The Context Object

        private MVCProjectEntities entities;

        ///Initializes a new instance of DepartmentController

        public DepartmentController()
        {
            this.entities = new MVCProjectEntities();
        }

        /// Get the Department for Dropdown
        [HttpGet]
        public ApiResponse GetForDropDown()
        {
            var data = this.entities.TblDepartments.Where(x=>x.IsActive.Value).Select(x=> new { Name=x.DepartmentName,Id=x.DepartmentId}).OrderBy(x=>x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success,responseToReturn:data);
        }


        /// Gets all Department details.

        [HttpPost]
        public ApiResponse GetAllDepartments(PagingParams departmentDetailsParams)
        {
            if (string.IsNullOrWhiteSpace(departmentDetailsParams.Search))
            {
                departmentDetailsParams.Search = string.Empty;
            }
            var departmentlist = (from d in this.entities.TblDepartments.AsEnumerable().Where(x => x.DepartmentName.Trim().ToLower().Contains(departmentDetailsParams.Search.Trim().ToLower()))
                                  let TotalRecords = this.entities.TblDepartments.AsEnumerable().Where(x => x.DepartmentName.Trim().ToLower().Contains(departmentDetailsParams.Search.Trim().ToLower())).Count()

                                  select new
                                  {
                                      DepartmentId=d.DepartmentId,
                                      DepartmentName = d.DepartmentName,
                                      EntryById=d.EntryById,
                                      EntryDate=d.EntryDate,
                                      UpdateBy=d.UpdateBy,
                                      UpdatedDate=d.UpdatedDate,
                                      IsActive=d.IsActive
                                  }).AsQueryable().OrderByField(departmentDetailsParams.OrderByColumn,departmentDetailsParams.IsAscending).Skip((departmentDetailsParams.CurrentPageNumber=-1)*departmentDetailsParams.PageSize).Take(departmentDetailsParams.PageSize);
            
            return this.Response(Utilities.MessageTypes.Success, string.Empty,departmentlist);

        }

        ///Get Active Departments
        ///
        /// <param name="isGetAll">To get active records</param>
        /// <returns>Returns response of type</returns>class.
        [HttpGet]

        public ApiResponse GetDepartmentList(bool isGetAll= false)
        {
            var result = this.entities.TblDepartments.Where(x => (isGetAll || x.IsActive.Value)).Select(x => new { Id = x.DepartmentId, Name = x.DepartmentName }).OrderBy(e => e.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success,string.Empty,result);
        }


        ///Get Department Master list By Id
        /// Passing Department id as Parameter.
        [HttpGet]
        public ApiResponse GetDepartmentById(int departmentId)
        {
            var departmentDetail = this.entities.TblDepartments.Where(x => x.DepartmentId == departmentId)
                .Select(x => new
                {
                    DepartmentId = x.DepartmentId,
                    DepartmentName = x.DepartmentName,
                    EntryById = x.EntryById,
                    EntryDate = x.EntryDate,
                    UpdateBy = x.UpdateBy,
                    UpdatedDate = x.UpdatedDate,
                    IsActive = x.IsActive
                }).SingleOrDefault();
            if (departmentDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success,string.Empty,departmentDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound,string.Empty);    
            }
        }


        /// Add/Update Department Details
        /// Using "departmentDetail" as Paramter for Department Details.

        [HttpPost]

        public ApiResponse SaveDepartmentDetails(TblDepartment departmentDetail)
        {
            if (this.entities.TblDepartments.Any(x => x.DepartmentId != departmentDetail.DepartmentId && x.DepartmentName.Trim() == departmentDetail.DepartmentName.Trim()))
            {
                return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Department));
            }
            else 
            {
                TblDepartment existingDepartmentDetail = this.entities.TblDepartments.Where(x=>x.DepartmentId==departmentDetail.DepartmentId).FirstOrDefault();
                if (existingDepartmentDetail == null)
                {
                    this.entities.TblDepartments.AddObject(departmentDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Department));
                    }
                  
                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Department));
                }

                 // For Update
                
                else
                {
                    existingDepartmentDetail.DepartmentName = departmentDetail.DepartmentName;
                    //existingDepartmentDetail.EntryById = departmentDetail.EntryById;
                    //existingDepartmentDetail.EntryDate = departmentDetail.EntryDate;
                    //existingDepartmentDetail.UpdateBy = departmentDetail.UpdateBy;
                    //existingDepartmentDetail.UpdatedDate = departmentDetail.UpdatedDate;
                    existingDepartmentDetail.IsActive = departmentDetail.IsActive;

                    this.entities.TblDepartments.ApplyCurrentValues(existingDepartmentDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Department));
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Department));
                }

               
            }
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
