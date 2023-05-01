
namespace MVCProject.Api.Controllers.Reports
{
    #region NameSapces
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Web;
    using System.Web.Http;
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    using Newtonsoft.Json;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    #endregion

    public class ReportsController : BaseController
    {
        private MVCProjectEntities entities;

        public ReportsController()
        {
            this.entities = new MVCProjectEntities();
        }
        /// <summary>
        /// excel sheet
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse CreateEmployeeListReport()
        {
            var employeeDetail = this.entities.TblEmployees.Select(d => new
            {
                d.EmployeeId,
                d.FirstName,
                d.LastName,
                d.Email,
                d.JoiningDate,
                d.PhoneNumber,
                d.AlternatePhoneNumber,
                d.DesignationId,
                d.DepartmentId,
                DesignationName = this.entities.Designation.FirstOrDefault(x => x.DesignationId == d.DesignationId).DesignationName,
                DepartmentName = this.entities.TblDepartments.FirstOrDefault(x => x.DepartmentId == d.DepartmentId).DepartmentName,
                d.BirthDate,
                Gender = d.Gender == 1 ? "Male" : "Female",
                d.PermanentAddress,
                d.TemporaryAddress,
                d.Pincode,
                d.InstitutionName,
                d.CourseName,
                d.CourseStartDate,
                d.CourseEndDate,
                d.Grade,
                d.Degree,
                IsActive = d.IsActive != null ? d.IsActive == true ? "Active" : "InActive" : string.Empty
            }).ToList();

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");


            // Create a cell style with a background color and border
            ICellStyle headerCellStyle = workbook.CreateCellStyle();
            headerCellStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            headerCellStyle.FillPattern = FillPattern.SolidForeground;
            headerCellStyle.BorderBottom = BorderStyle.Thin;
            headerCellStyle.BorderTop = BorderStyle.Thin;
            headerCellStyle.BorderLeft = BorderStyle.Thin;
            headerCellStyle.BorderRight = BorderStyle.Thin;

            // Add Some Data to Sheet
            IRow headerRow = sheet.CreateRow(0);

            headerRow.HeightInPoints = 20; // Set the height of the header row
            headerRow.CreateCell(0).SetCellValue("Employee Id");
            headerRow.CreateCell(1).SetCellValue("First Name");
            headerRow.CreateCell(2).SetCellValue("Last Name");
            headerRow.CreateCell(3).SetCellValue("Email");
            headerRow.CreateCell(4).SetCellValue("Joining Date");
            headerRow.CreateCell(5).SetCellValue("Phone Number");
            headerRow.CreateCell(6).SetCellValue("Emergency Phone Number");
            headerRow.CreateCell(7).SetCellValue("Designation Name");
            headerRow.CreateCell(8).SetCellValue("Department Name");
            headerRow.CreateCell(9).SetCellValue("Birth Date");
            headerRow.CreateCell(10).SetCellValue("Gender");
            headerRow.CreateCell(11).SetCellValue("Permanent Address");
            headerRow.CreateCell(12).SetCellValue("Temporary Address");
            //headerRow.CreateCell(13).SetCellValue("Pincode");
            headerRow.CreateCell(13).SetCellValue("Institution Name");
            headerRow.CreateCell(14).SetCellValue("Course Name");
            //headerRow.CreateCell(16).SetCellValue("Course Start Date");
            //headerRow.CreateCell(17).SetCellValue("Course End Date");
            headerRow.CreateCell(15).SetCellValue("Grade");
            headerRow.CreateCell(16).SetCellValue("Degree");
            headerRow.CreateCell(17).SetCellValue("IsActive");
            // Set the cell style for the header row
            foreach (var cell in headerRow.Cells)
            {
                cell.CellStyle = headerCellStyle;
            }

            int rowNumber = 1;
            foreach (var emp in employeeDetail)
            {
                IRow row = sheet.CreateRow(rowNumber++);
                row.CreateCell(0).SetCellValue(emp.EmployeeId);
                row.CreateCell(1).SetCellValue(emp.FirstName);
                row.CreateCell(2).SetCellValue(emp.LastName);
                row.CreateCell(3).SetCellValue(emp.Email);
                row.CreateCell(4).SetCellValue(((DateTime)emp.JoiningDate).ToString("dd/MM/yyyy"));
                row.CreateCell(5).SetCellValue(emp.PhoneNumber);
                row.CreateCell(6).SetCellValue(emp.AlternatePhoneNumber);
                row.CreateCell(7).SetCellValue(emp.DesignationName);
                row.CreateCell(8).SetCellValue(emp.DepartmentName);
                //row.CreateCell(9).SetCellValue((DateTime)emp.BirthDate);
                row.CreateCell(9).SetCellValue(((DateTime)emp.BirthDate).ToString("dd/MM/yyyy"));
                row.CreateCell(10).SetCellValue(emp.Gender);
                row.CreateCell(11).SetCellValue(emp.PermanentAddress);
                row.CreateCell(12).SetCellValue(emp.TemporaryAddress);
                // row.CreateCell(13).SetCellValue((double)emp.Pincode);
                row.CreateCell(13).SetCellValue(emp.InstitutionName);
                row.CreateCell(14).SetCellValue(emp.CourseName);
                //row.CreateCell(16).SetCellValue(((DateTime)emp.CourseStartDate).ToString("dd/MM/yyyy"));
                //row.CreateCell(17).SetCellValue(((DateTime)emp.CourseEndDate).ToString("dd/MM/yyyy"));
                row.CreateCell(15).SetCellValue(emp.Grade);
                row.CreateCell(16).SetCellValue(emp.Degree);
                row.CreateCell(17).SetCellValue(emp.IsActive);
            }
            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            string filePath = HttpContext.Current.Server.MapPath("~/Reports/Employee.xlsx");
            string fileName = Path.GetFileName(filePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            workbook.Write(fileStream);
            var memorystream = new MemoryStream();
            var byteArray = memorystream.ToArray();
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = new ByteArrayContent(byteArray);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "MyExcelFile.xlsx";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentLength = byteArray.Length;
            Console.WriteLine(response);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, filePath);
        }

        /// <summary>
        /// Generate PDf 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse ExportPDF()
        {
            var data = this.entities.TblEmployees.Select(d => new {
                d.EmployeeId,
                d.FirstName,
                d.LastName,
                d.Email,
                d.DesignationId,
                d.DepartmentId,
                DesignationName = this.entities.Designation.FirstOrDefault(x => x.DesignationId == d.DesignationId).DesignationName,
                DepartmentName = this.entities.TblDepartments.FirstOrDefault(x => x.DepartmentId == d.DepartmentId).DepartmentName,
                IsActive = d.IsActive != null ? d.IsActive == true ? "Active" : "InActive" : string.Empty
            }).ToList();

            string filePath = HttpContext.Current.Server.MapPath("~/Reports/Employee.pdf");
            string fileName = Path.GetFileName(filePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (iTextSharp.text.Document document = new iTextSharp.text.Document())
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                using (iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fs))
                {
                    document.Open();

                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(6);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 1.5f, 2f, 1.5f, 2f, 1.5f, 1.5f });

                    iTextSharp.text.pdf.PdfPCell headerCell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Employee Report"));
                    headerCell.Colspan = 6;
                    headerCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER; //Center
                    headerCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    headerCell.BackgroundColor = new iTextSharp.text.BaseColor(0, 119, 187); // Blue color
                    headerCell.BorderColor = iTextSharp.text.BaseColor.WHITE;

                    headerCell.PaddingTop = 10f;
                    headerCell.PaddingBottom = 10f;
                    table.AddCell(headerCell);
                    // cell.BackgroundColor = new BaseColor(255, 0, 0); // Set background color to red
                    table.AddCell("First Name");
                    table.AddCell("Last Name");
                    table.AddCell("Email");
                    table.AddCell("Designation Name");
                    table.AddCell("Department Name");
                    table.AddCell("IsActive");


                    foreach (var dt in data)
                    {
                        table.AddCell(dt.FirstName);
                        table.AddCell(dt.LastName);
                        table.AddCell(dt.Email);
                        table.AddCell(dt.DepartmentName);
                        table.AddCell(dt.DesignationName);
                        table.AddCell(dt.IsActive);
                    }

                    document.Add(table);

                    document.Close();
                }
            }

            //Return a response containing the file path
            return this.Response(MVCProject.Api.Utilities.MessageTypes.Success, string.Empty, filePath);
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
