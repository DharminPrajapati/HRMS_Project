

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
    using System.Web;
    using System.Web.Http;
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

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
            var data = this.entities.TblDepartments.Where(x => x.IsActive.Value).Select(x => new { Name = x.DepartmentName, Id = x.DepartmentId }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }

        [HttpGet]
        public ApiResponse GetcompanyDropDown()
        {
            var data = this.entities.CompanyMaster.Where(x => x.IsActive.Value).Select(x => new { Name = x.CompanyName, Id = x.CompanyMasterId }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
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
                                      DepartmentId = d.DepartmentId,
                                      DepartmentName = d.DepartmentName,
                                      CompanyMasterId = d.CompanyMasterId,
                                      EntryById = d.EntryById,
                                      EntryDate = d.EntryDate,
                                      UpdateBy = d.UpdateBy,
                                      UpdatedDate = d.UpdatedDate,
                                      IsActive = d.IsActive
                                  }).AsQueryable().OrderByField(departmentDetailsParams.OrderByColumn, departmentDetailsParams.IsAscending).Skip((departmentDetailsParams.CurrentPageNumber = -1) * departmentDetailsParams.PageSize).Take(departmentDetailsParams.PageSize);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, departmentlist);

        }

        ///Get Active Departments
        ///
        /// <param name="isGetAll">To get active records</param>
        /// <returns>Returns response of type</returns>class.
        [HttpGet]

        public ApiResponse GetDepartmentList(bool isGetAll = false)
        {
            var result = this.entities.TblDepartments.Where(x => (isGetAll || x.IsActive.Value)).Select(x => new { Id = x.DepartmentId, Name = x.DepartmentName }).OrderBy(e => e.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, result);
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
                    CompanyMasterId = x.CompanyMasterId,
                    EntryById = x.EntryById,
                    EntryDate = x.EntryDate,
                    UpdateBy = x.UpdateBy,
                    UpdatedDate = x.UpdatedDate,
                    IsActive = x.IsActive
                }).SingleOrDefault();
            if (departmentDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, departmentDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }


        /// Add/Update Department Details
        /// Using "departmentDetail" as Paramter for Department Details.

        [HttpPost]

        public ApiResponse SaveDepartmentDetails(TblDepartment departmentDetail)
        {
            //if (this.entities.TblDepartments.Any(x => x.DepartmentId != departmentDetail.DepartmentId && x.DepartmentName.Trim() == departmentDetail.DepartmentName.Trim()))
            //{
            //    return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Department));
            //}
            //else
            //{
                TblDepartment existingDepartmentDetail = this.entities.TblDepartments.Where(x => x.DepartmentId == departmentDetail.DepartmentId).FirstOrDefault();
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
                    existingDepartmentDetail.CompanyMasterId = departmentDetail.CompanyMasterId;
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


            //}
        }
        /// <summary>
        ///  Create Excel Report Of Designation
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public ApiResponse CreateEmployeeListReport()
        {
            var departmentDetails = this.entities.TblDepartments.Select(d => new
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
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
            headerRow.CreateCell(0).SetCellValue("Department Id");
            headerRow.CreateCell(1).SetCellValue("Department Name");
            headerRow.CreateCell(2).SetCellValue("IsActive");

            // Set the cell style for the header row
            foreach (var cell in headerRow.Cells)
            {
                cell.CellStyle = headerCellStyle;
            }

            int rowNumber = 1;
            foreach (var dp in departmentDetails)
            {
                IRow row = sheet.CreateRow(rowNumber++);
                row.CreateCell(0).SetCellValue(dp.DepartmentId);
                row.CreateCell(1).SetCellValue(dp.DepartmentName);
                row.CreateCell(2).SetCellValue(dp.IsActive);
            }

            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            string filePath = HttpContext.Current.Server.MapPath("~/Reports/Department.xlsx");
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
            response.Content.Headers.ContentDisposition.FileName = "MyExcelFile_Department.xlsx";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentLength = byteArray.Length;
            Console.WriteLine(response);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, filePath);
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
