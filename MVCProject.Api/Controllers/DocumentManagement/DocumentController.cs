

namespace MVCProject.Api.Controllers.DocumentManagement
{
    #region NameSapces
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web;
    using System.Web.Http;
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    #endregion
    public class DocumentController : BaseController
    {
        private MVCProjectEntities entities;

        public DocumentController()
        {
            this.entities = new MVCProjectEntities();
        }

      /// <summary>
      /// Get Document Type Drop Down
      /// </summary>
      /// <returns></returns>
        [HttpGet]
        public ApiResponse DocumentTypeDropDown()
        {
            var data = this.entities.DocumentTypeMaster.Where(x => x.IsActive.Value).Select(x => new { Name = x.DocumentName, Id = x.DocumentId }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }


        /// <summary>
        /// Get  Full  Employee Name 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse GetFullName(bool isActive, string searchText)
        {
            var data = this.entities.TblEmployees.Where(x => x.IsActive.Value == isActive && x.FirstName.Contains(searchText)).Select(x => new { Name = x.FirstName + " " + x.LastName, Id = x.EmployeeId, DepartmentId = x.DepartmentId, DepartmentName = this.entities.TblDepartments.FirstOrDefault(d => d.DepartmentId == x.DepartmentId).DepartmentName, DesignationName = this.entities.Designation.FirstOrDefault(d => d.DesignationId == x.DesignationId).DesignationName }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);

        }

        /// <summary>
        ///  Get Employee Document By Id
        /// </summary>
        /// <param name="EmpDocId"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse GetEmpDocumentById(int EmpDocId)
        {
            var EmpDocumentDetail = this.entities.EmployeeDocumentMaster
                .Where(x => x.EmpDocumentId == EmpDocId)
                .Join(this.entities.TblEmployees,
                      edm => edm.EmployeeId,
                      emp => emp.EmployeeId,
                      (edm, emp) => new { edm, emp })
                .Join(this.entities.TblDepartments,
                      e => e.emp.DepartmentId,
                      d => d.DepartmentId,
                      (e, d) => new
                      {
                          EmpDocumentId = e.edm.EmpDocumentId,
                          EmployeeId = e.emp.EmployeeId,
                          DocumentType = e.edm.DocumentType,
                          DesignationName = this.entities.Designation
                                    .Where(ds => ds.DesignationId == e.emp.DesignationId)
                                    .Select(des => des.DesignationName)
                                    .FirstOrDefault(),
                          DepartmentName = d.DepartmentName,
                          Attachments = this.entities.EmployeeDocumentMaster
                              .Where(a => a.EmployeeId == e.emp.EmployeeId)
                              .Select(b => new
                              {
                                  EmpDocumentId = b.EmpDocumentId,
                                  FileName = b.FileName,
                                  Filepath = b.FilePath,
                                  OriginalName = b.OriginalName,
                                  DocumentType = b.DocumentType,
                              })
                      })
                .SingleOrDefault();
            if (EmpDocumentDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, EmpDocumentDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

       /// <summary>
       /// Get Data From GetEmpDocumentById
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
        [HttpGet]
        public ApiResponse GetDeptDesiByEmployeeId(int Id)
        {
            object Employee = null;
            if (this.entities.EmployeeDocumentMaster.Any(x => x.EmployeeId == Id))
            {
                Employee = GetEmpDocumentById(this.entities.EmployeeDocumentMaster.FirstOrDefault(x => x.EmployeeId == Id).EmpDocumentId).Result;
            }
            if (Employee != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, Employee);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        /// <summary>
        /// Add And Update Api
        /// </summary>
        /// <param name="employeeDocumentMaster"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse SaveDocumentDetails(EmployeeDocumentMaster employeeDocumentMaster)
        {
            EmployeeDocumentMaster existingDocmentDetail = this.entities.EmployeeDocumentMaster.Where(x => x.EmpDocumentId == employeeDocumentMaster.EmpDocumentId).FirstOrDefault();
            var Exist = employeeDocumentMaster.Attachments.ToList();

            foreach (EmployeeDocumentMaster item in Exist)
            {
                if (item.EmpDocumentId != 0)
                {
                    item.EmpDocumentId = employeeDocumentMaster.EmpDocumentId;
                    item.DocumentType = employeeDocumentMaster.DocumentType;
                    item.EmployeeId = employeeDocumentMaster.EmployeeId;
                  
                    item.IsActive = true;
                    this.entities.EmployeeDocumentMaster.ApplyCurrentValues(item);
                    this.entities.SaveChanges();
                   
                }
                else
                {
                    item.EmployeeId = employeeDocumentMaster.EmployeeId;
                    item.IsActive = true;

                    this.entities.EmployeeDocumentMaster.AddObject(item);
                    this.entities.SaveChanges();
                   
                }
            }
            var DeleteAttch = employeeDocumentMaster.DeleteAttachments.ToList();
            foreach (EmployeeDocumentMaster item in DeleteAttch)
            {
                if (item.EmpDocumentId != 0)
                {
                    var existing = this.entities.EmployeeDocumentMaster.Where(x => x.EmpDocumentId == item.EmpDocumentId).FirstOrDefault();
                   
                    if (existing.EmpDocumentId != 0)
                    {
                        this.entities.EmployeeDocumentMaster.DeleteObject(existing);
                        this.entities.SaveChanges();
                    }

                }
            }
            if (existingDocmentDetail == null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Document), employeeDocumentMaster.EmpDocumentId);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Document), employeeDocumentMaster.EmpDocumentId);
            }
        }

        
        /// <summary>
        /// Get Document By Employee Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse GetDocumentById(int employeeId)
        {
            string root = string.Empty;
            string directory = string.Format(AppUtility.GetEnumDescription(MVCProject.Api.Utilities.DirectoryPath.Attachment_Temp), string.Empty);
            directory = directory.Replace(@"\", "/");
            Uri uri = HttpContext.Current.Request.Url;
            root = uri.OriginalString.Replace(uri.PathAndQuery, "/");
            var employeeDetail = this.entities.EmployeeDocumentMaster
           .Where(x => x.EmployeeId == employeeId && x.IsActive == true)
           .Select(d => new
             {
                EmployeeId = d.EmployeeId,
                EmpDocumentId = d.EmpDocumentId,
                IsActive = d.IsActive,
                Attachments = this.entities.EmployeeDocumentMaster
                .Where(a => a.EmployeeId == employeeId/* && a.IsActive == true*/)
                .Select(b => new
                {
                    FileName = b.FileName,
                    Filepath = b.FilePath,
                    OriginalName = b.OriginalName,
                    DocumentType = b.DocumentType,
                })
           }).FirstOrDefault();

            if (employeeDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, employeeDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
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
