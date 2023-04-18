// -----------------------------------------------------------------------
// <copyright file="DesignationsController.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

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
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using System.Web;

    #endregion

    /// <summary>
    /// Holds Designations Master related methods
    /// </summary>
    public class DesignationsController : BaseController
    {
        /// <summary>
        /// Holds context object. 
        /// </summary>
        private MVCProjectEntities entities;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignationsController"/> class.
        /// </summary>
        public DesignationsController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpGet]
        public ApiResponse GetForDropDown()
        {
            var data = this.entities.Designations.Where(x => x.IsActive.Value).Select(x => new { Name = x.DesignationName, Id = x.DesignationId }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }

        [HttpGet]
        public ApiResponse GetcompanyDropDown()
        {
            var data = this.entities.CompanyMaster.Where(x => x.IsActive.Value).Select(x => new { Name = x.CompanyName, Id = x.CompanyMasterId }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }

        /// Get All Departments dropdown
        [HttpGet]
        public ApiResponse GetDepartmentDropDown(int id)
        {
            var data = this.entities.TblDepartments.Where(x => x.IsActive.Value && x.CompanyMasterId == id).Select(x => new { DeptName = x.DepartmentName, DeptId = x.DepartmentId }).OrderBy(x => x.DeptName).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }


        /// <summary>
        /// Get Designations for dropdown
        /// </summary>
        /// <returns>Returns response of type <see cref="ApiResonse"/> class.</returns>
        //[HttpGet]
        //public ApiResponse GetDesignationDropDown()
        //{
        //    var data = this.entities.Designations.Where(x => x.IsActive.Value).Select(x => new { Name = x.DesignationName, Id = x.DesignationId }).OrderBy(x => x.Name).ToList();
        //    return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        //}

        /// <summary>
        /// Gets all Designation details. 
        /// </summary>
        /// <param name="designationDetailParams">Pass parameters of designation details</param>
        /// <returns>Returns response of type <see cref="ApiResonse"/> class.</returns>
        [HttpPost]
        public ApiResponse GetAllDesignations(PagingParams designationDetailParams)
        {
            if (string.IsNullOrWhiteSpace(designationDetailParams.Search))
            {
                designationDetailParams.Search = string.Empty;
            }

            var designationList = (from s in this.entities.Designations.AsEnumerable().Where(x => x.DesignationName.Trim().ToLower().Contains(designationDetailParams.Search.Trim().ToLower()))
                                   let TotalRecords = this.entities.Designations.AsEnumerable().Where(x => x.DesignationName.Trim().ToLower().Contains(designationDetailParams.Search.Trim().ToLower())).Count()
                                   select new
                                   {
                                       DesignationId = s.DesignationId,
                                       DesignationName = s.DesignationName,
                                       DepartmentId = s.DepartmentId,
                                       CompanyMasterId = s.CompanyMasterId,
                                       IsActive = s.IsActive,
                                       Remarks = s.Remarks,
                                       TotalRecords
                                   }).AsQueryable().OrderByField(designationDetailParams.OrderByColumn, designationDetailParams.IsAscending).Skip((designationDetailParams.CurrentPageNumber - 1) * designationDetailParams.PageSize).Take(designationDetailParams.PageSize);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, designationList);
        }

        /// <summary>
        /// Get Designation 
        /// </summary>
        /// <param name="isGetAll">To get active records</param>
        /// <returns>Returns response of type</returns>class.
        [HttpGet]
        public ApiResponse GetDesignationList(bool isGetAll = false)
        {
            var result = this.entities.Designations.Where(x => (isGetAll || x.IsActive.Value)).Select(x => new { Id = x.DesignationId, Name = x.DesignationName }).OrderBy(y => y.Name).ToList();
            return this.Response(MessageTypes.Success, string.Empty, result);
        }

        /// <summary>
        /// Get Designation Master List by Id
        /// </summary>
        /// <param name="designationId">Designation id.</param>
        /// <returns>Returns response type of <see cref="ApiResponse"/> class.></returns>
        [HttpGet]
        public ApiResponse GetDesignationById(int designationId)
        {
            var designationDetail = this.entities.Designations.Where(a => a.DesignationId == designationId)
                        .Select(g => new
                        {
                            DesignationId = g.DesignationId,
                            DesignationName = g.DesignationName,
                            DepartmentId = g.DepartmentId,
                            CompanyMasterId = g.CompanyMasterId,
                            IsActive = g.IsActive,
                            Remarks = g.Remarks,
                            // EntryBy = g.EntryBy,
                            //EntryDate = g.EntryDate,
                        }).SingleOrDefault();
            if (designationDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, designationDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        /// <summary>
        /// Add/update Designation details
        /// </summary>
        /// <param name="designationDetail">Designation Details</param>
        /// <returns>Returns response type of <see cref="ApiResponse"/> class.></returns>
        [HttpPost]
        public ApiResponse SaveDesignationDetails(Designation designationDetail)
        {
            if (this.entities.Designations.Any(x => x.DesignationId != designationDetail.DesignationId && x.DesignationName.Trim() == designationDetail.DesignationName.Trim()))
            {
                return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Designation));
            }
            else
            {
                Designation existingDesignationDetail = this.entities.Designations.Where(a => a.DesignationId == designationDetail.DesignationId).FirstOrDefault();
                if (existingDesignationDetail == null)
                {
                    //designationDetail.EntryDate = DateTime.UtcNow;
                    //designationDetail.EntryBy = UserContext.EmployeeId;
                    this.entities.Designations.AddObject(designationDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Designation));
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Designation));
                }
                else
                {
                    // For update record
                    existingDesignationDetail.DesignationName = designationDetail.DesignationName;
                    existingDesignationDetail.CompanyMasterId = designationDetail.CompanyMasterId;
                    existingDesignationDetail.DepartmentId = designationDetail.DepartmentId;

                    existingDesignationDetail.IsActive = designationDetail.IsActive;
                    existingDesignationDetail.Remarks = designationDetail.Remarks;
                    //existingDesignationDetail.UpdateBy = UserContext.EmployeeId;
                    //existingDesignationDetail.UpdateDate = DateTime.UtcNow;
                    this.entities.Designations.ApplyCurrentValues(existingDesignationDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.Designation);
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Designation));
                }
            }
        }


        [HttpGet]
        public ApiResponse GetRoles(int UserId)
        {
            //UserContext userContext1 = new UserContext();
            var userContext = (from role in this.entities.UserRole
                               join user in this.entities.UserMaster on role.UserId equals user.UserId
                               join userRole in this.entities.UserRoleMaster on role.RoleId equals userRole.RoleId
                               where user.UserId == UserId
                               select new
                               {
                                   //user.UserId,
                                   //user.EmpId,
                                   //user.UserName,
                                   //user.Password,
                                   //userRole.UserRoleName,
                                   //IsActive = user.IsActive != null ? user.IsActive == true ? "Active" : "InActive" : string.Empty,
                                   //role.RoleId,
                                   //user.UserRole
                                   RoleId = userRole.RoleId,
                                   UserRoleName = userRole.UserRoleName
                               }).ToList();
            var userCurrentRoles = userContext;
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: userContext);
        }

        /// <summary>
        ///  Create Excel Report Of Designation
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public ApiResponse CreateEmployeeListReport()
        {
            var designationDetails = this.entities.Designations.Select(d => new
            {
                DesignationId = d.DesignationId,
                DesignationName = d.DesignationName,
                Remarks = d.Remarks,
                IsActive = d.IsActive != null ? d.IsActive == true ? "Active" : "InActive" : string.Empty
            }).ToList();

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");



            // Create a cell style with a background color
            ICellStyle headerCellStyle = workbook.CreateCellStyle();
            headerCellStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            headerCellStyle.FillPattern = FillPattern.SolidForeground;


            // Add Some Data to Sheet
            // 
            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Designation Id");
            headerRow.CreateCell(1).SetCellValue("Designation Name");
            headerRow.CreateCell(2).SetCellValue("Remarks");
            headerRow.CreateCell(3).SetCellValue("IsActive");

            // Set the cell style for the header row
            foreach (var cell in headerRow.Cells)
            {
                cell.CellStyle = headerCellStyle;
            }

            int rowNumber = 1;
            foreach (var de in designationDetails)
            {
                IRow row = sheet.CreateRow(rowNumber++);
                row.CreateCell(0).SetCellValue(de.DesignationId);
                row.CreateCell(1).SetCellValue(de.DesignationName);
                row.CreateCell(2).SetCellValue(de.Remarks);
                row.CreateCell(3).SetCellValue(de.IsActive);
            }

            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            string filePath = HttpContext.Current.Server.MapPath("~/Reports/Designation.xlsx");
            string fileName = Path.GetFileName(filePath);
            //if (File.Exists(filePath))
            //{
            //    File.Delete(filePath);
            //}
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            workbook.Write(fileStream);
            var memorystream = new MemoryStream();
            var byteArray = memorystream.ToArray();
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = new ByteArrayContent(byteArray);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "MyExcelFile_Designation.xlsx";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentLength = byteArray.Length;
            Console.WriteLine(response);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, filePath);
        }


        /// <summary>
        /// Disposes expensive resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether to dispose or not.</param>
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
