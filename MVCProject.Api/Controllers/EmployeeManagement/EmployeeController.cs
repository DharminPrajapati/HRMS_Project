namespace MVCProject.Api.Controllers.EmployeeManagement
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
    public class EmployeeController : BaseController
    {
        private MVCProjectEntities entities;

        public EmployeeController()
        {
            this.entities = new MVCProjectEntities();
        }

        /// <summary>
        /// Get All Designation dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse GetDesignationDropDown(int id)
        {
            var data = this.entities.Designation.Where(x => x.IsActive.Value && x.DepartmentId == id).Select(x => new { Name = x.DesignationName, Id = x.DesignationId }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }

        /// Get All Departments dropdown
        /// Get All Departments dropdown
        [HttpGet]
        public ApiResponse GetDepartmentDropDown(int id)
        {
            var data = this.entities.TblDepartments.Where(x => x.IsActive.Value && x.CompanyMasterId == id).Select(x => new { DeptName = x.DepartmentName, DeptId = x.DepartmentId }).OrderBy(x => x.DeptName).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }



        [HttpGet]
        public ApiResponse GetcompanyDropDown()
        {
            var data = this.entities.CompanyMaster.Where(x => x.IsActive.Value).Select(x => new { Name = x.CompanyName, Id = x.CompanyMasterId }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }

        ///Get All Employee Details
        [HttpPost]
        public ApiResponse GetAllEmployees(PagingParams employeeDetailsParams)
        {
            if (string.IsNullOrWhiteSpace(employeeDetailsParams.Search))
            {
                employeeDetailsParams.Search = string.Empty;
            }

            var employeelist = (from d in this.entities.TblEmployees.AsEnumerable().Where(x => x.FirstName.Trim().ToLower().Contains(employeeDetailsParams.Search.Trim().ToLower()))
                                let TotalRecords = this.entities.TblEmployees.AsEnumerable().Where(x => x.FirstName.Trim().ToLower().Contains(employeeDetailsParams.Search.Trim().ToLower())).Count()
                                select new
                                {
                                    //var employeelist = this.entities.TblEmployee.ToList();

                                    EmployeeId = d.EmployeeId,
                                    CompanyMasterId = d.CompanyMasterId,
                                    SrNo = d.SrNo,
                                    BatchNo = d.BatchNo,
                                    FirstName = d.FirstName,
                                    LastName = d.LastName,
                                    Email = d.Email,
                                    Password = d.Password,
                                    JoiningDate = d.JoiningDate,
                                    PhoneNumber = d.PhoneNumber,
                                    AlternatePhoneNumber = d.AlternatePhoneNumber,
                                    Designation = d.DesignationId,
                                    Department = d.DepartmentId,
                                    DepartmentName = d.TblDepartmentReference.Value.DepartmentName,
                                    DesignationName = d.DesignationReference.Value.DesignationName,
                                    
                                    BirthDate = d.BirthDate,
                                    Gender = d.Gender,
                                    PermanentAddress = d.PermanentAddress,
                                    TemporaryAddress = d.TemporaryAddress,
                                    Pincode = d.Pincode,
                                    InstitutionName = d.InstitutionName,
                                    CourseName = d.CourseName,
                                    CourseStartDate = d.CourseStartDate,
                                    CourseEndDate = d.CourseEndDate,
                                    Grade = d.Grade,
                                    Degree = d.Degree,
                                    CompanyName = d.CompanyName,
                                    LastJobLocation = d.LastJobLocation,
                                    JobPosition = d.JobPosition,
                                    FromPeriod = d.FromPeriod,
                                    ToPeriod = d.ToPeriod,
                                    IsActive = d.IsActive,
                                    TotalRecords
                                }).AsQueryable().Skip((employeeDetailsParams.CurrentPageNumber - 1) * employeeDetailsParams.PageSize).Take(employeeDetailsParams.PageSize);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, employeelist);

        }


        /// <summary>
        /// for all record or active record
        /// </summary>
        /// <param name="isGetAll"></param>
        /// <returns></returns>
        [HttpGet]

        public ApiResponse GetEmployeeList(bool isGetAll = false)
        {
            var result = this.entities.TblEmployees.Where(x => (isGetAll || x.IsActive.Value)).Select(x => new { Id = x.EmployeeId, Name = x.FirstName, srno = x.SrNo }).OrderBy(e => e.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, result);
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
        /// Get Employees By Id
        /// </summary>
        //[HttpGet]

        //public ApiResponse GetEmployeeById(int employeeId)
        //{
        //    var employeeDetail = this.entities.TblEmployees.Where(x => x.EmployeeId == employeeId)
        //           .Select(d => new
        //           {
        //               EmployeeId = d.EmployeeId,
        //               SrNo = d.SrNo,
        //               BatchNo = d.BatchNo,
        //               FirstName = d.FirstName,
        //               LastName = d.LastName,
        //               Email = d.Email,
        //               Password = d.Password,
        //               JoiningDate = d.JoiningDate,
        //               PhoneNumber = d.PhoneNumber,
        //               AlternatePhoneNumber = d.AlternatePhoneNumber,
        //               DesignationId = d.DesignationId,
        //               DepartmentId = d.DepartmentId,
        //               BirthDate = d.BirthDate,
        //               Gender = d.Gender,
        //               PermanentAddress = d.PermanentAddress,
        //               TemporaryAddress = d.TemporaryAddress,
        //               Pincode = d.Pincode,
        //               InstitutionName = d.InstitutionName,
        //               CourseName = d.CourseName,
        //               CourseStartDate = d.CourseStartDate,
        //               CourseEndDate = d.CourseEndDate,
        //               Grade = d.Grade,
        //               Degree = d.Degree,
        //               CompanyName = d.CompanyName,
        //               LastJobLocation = d.LastJobLocation,
        //               JobPosition = d.JobPosition,
        //               FromPeriod = d.FromPeriod,
        //               ToPeriod = d.ToPeriod,
        //               IsActive = d.IsActive
        //           }).SingleOrDefault();
        //    if (employeeDetail != null)
        //    {
        //        return this.Response(Utilities.MessageTypes.Success, string.Empty, employeeDetail);
        //    }
        //    else
        //    {
        //        return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
        //    }
        //}

        /// Get Employee using Store Procedure
        /// </summary>
        [HttpGet]

        public ApiResponse GetEmployeeById(int employeeId)
        {
            string root = string.Empty;
            string directory = string.Format(AppUtility.GetEnumDescription(MVCProject.Api.Utilities.DirectoryPath.Attachment_Temp), string.Empty);
            directory = directory.Replace(@"\", "/");
            Uri uri = HttpContext.Current.Request.Url;
            root = uri.OriginalString.Replace(uri.PathAndQuery, "/");
            var employeeDetail = this.entities.sp_Emp_GetAllEmployees().Where(x => x.EmployeeId == employeeId)
                   .Select(d => new
                   {
                       EmployeeId = d.EmployeeId,
                       BatchNo = d.BatchNo,
                       CompanyMasterId = d.CompanyMasterId,
                       FirstName = d.FirstName,
                       LastName = d.LastName,
                       Email = d.Email,
                       JoiningDate = d.JoiningDate,
                       PhoneNumber = d.PhoneNumber,
                       AlternatePhoneNumber = d.AlternatePhoneNumber,
                       DesignationId = d.DesignationId,
                       DepartmentId = d.DepartmentId,
                       DesignationName = d.DesignationName,
                       DepartmentName = d.DepartmentName,
                       companyname = d.CompanyName,
                       BirthDate = d.BirthDate,
                       Gender = d.Gender,
                       GenderName = d.Gender == 1 ? "Male" : "Female",
                       PermanentAddress = d.PermanentAddress,
                       TemporaryAddress = d.TemporaryAddress,
                       Pincode = d.Pincode,
                       InstitutionName = d.InstitutionName,
                       CourseName = d.CourseName,
                       CourseStartDate = d.CourseStartDate,
                       CourseEndDate = d.CourseEndDate,
                       Grade = d.Grade,
                       Degree = d.Degree,
                       CompanyName = d.CompanyName,
                       LastJobLocation = d.LastJobLocation,
                       JobPosition = d.JobPosition,
                       FromPeriod = d.FromPeriod,
                       ToPeriod = d.ToPeriod,
                       IsActive = d.IsActive,
                       Attachment = this.entities.AttachmentMaster.Where(a => a.RefrencedId == d.EmployeeId && a.IsActive == true).ToList().Select(b => new
                       {
                           AttachmentId = b.AttachmentId,
                           FileName = b.FileName,
                           Filepath = b.Filepath,
                           FileRelativePath = b.FileRelativePath,
                           OriginalFileName = b.OriginalFileName,
                           RelativePath = string.Format("{0}{1}{2}", root, directory, b.FileName),
                       }).FirstOrDefault()

                   }).SingleOrDefault();
            if (employeeDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, employeeDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        /// Add/Update Employee Details
        /// 
        [HttpPost]
        public ApiResponse SaveEmployeeDetails(TblEmployee employeeDetail)
        {
            //if (this.entities.TblEmployees.Any(x => x.EmployeeId != employeeDetail.EmployeeId && x.FirstName.Trim() == employeeDetail.FirstName.Trim()))
            //{
            //    return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Department));
            //}


            TblEmployee existingEmployeeDetail = this.entities.TblEmployees.Where(x => x.EmployeeId == employeeDetail.EmployeeId).FirstOrDefault();
            if (existingEmployeeDetail == null)
            {
                var Srno = this.entities.TblEmployees.Max(x => x.SrNo) + 1;
                var Batch = string.Format("{0}{1}", "ASK_", (Srno));

                employeeDetail.SrNo = Srno;
                employeeDetail.BatchNo = Batch;
                this.entities.TblEmployees.AddObject(employeeDetail);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Employee));
                }

                // return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Employee));
            }




            // For Update

            else
            {
                existingEmployeeDetail.FirstName = employeeDetail.FirstName;
                existingEmployeeDetail.CompanyMasterId = employeeDetail.CompanyMasterId;
                existingEmployeeDetail.LastName = employeeDetail.LastName;
                existingEmployeeDetail.Email = employeeDetail.Email;
                existingEmployeeDetail.JoiningDate = employeeDetail.JoiningDate;
                existingEmployeeDetail.PhoneNumber = employeeDetail.PhoneNumber;
                existingEmployeeDetail.AlternatePhoneNumber = employeeDetail.AlternatePhoneNumber;
                existingEmployeeDetail.DesignationId = employeeDetail.DesignationId;
                existingEmployeeDetail.DepartmentId = employeeDetail.DepartmentId;
                existingEmployeeDetail.BirthDate = employeeDetail.BirthDate;
                existingEmployeeDetail.Gender = employeeDetail.Gender;
                existingEmployeeDetail.PermanentAddress = employeeDetail.PermanentAddress;
                existingEmployeeDetail.TemporaryAddress = employeeDetail.TemporaryAddress;
                existingEmployeeDetail.Pincode = employeeDetail.Pincode;
                existingEmployeeDetail.InstitutionName = employeeDetail.InstitutionName;
                existingEmployeeDetail.CourseName = employeeDetail.CourseName;
                existingEmployeeDetail.CourseStartDate = employeeDetail.CourseStartDate;
                existingEmployeeDetail.CourseEndDate = employeeDetail.CourseEndDate;
                existingEmployeeDetail.Grade = employeeDetail.Grade;
                existingEmployeeDetail.Degree = employeeDetail.Degree;
                existingEmployeeDetail.CompanyName = employeeDetail.CompanyName;
                existingEmployeeDetail.LastJobLocation = employeeDetail.LastJobLocation;
                existingEmployeeDetail.JobPosition = employeeDetail.JobPosition;
                existingEmployeeDetail.FromPeriod = employeeDetail.FromPeriod;
                existingEmployeeDetail.ToPeriod = employeeDetail.ToPeriod;
                existingEmployeeDetail.IsActive = employeeDetail.IsActive;

                this.entities.TblEmployees.ApplyCurrentValues(existingEmployeeDetail);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Employee));
                }

                //return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Employee));
            }

            var existingattachfile = this.entities.AttachmentMaster.Where(a => a.RefrencedId == employeeDetail.EmployeeId).ToList();

            if (existingattachfile != null)
            {
                foreach (var item in existingattachfile)
                {
                    this.entities.AttachmentMaster.DeleteObject(item);
                    this.entities.SaveChanges();
                }
            }
            var attachfile = employeeDetail.Attachment;
            attachfile.RefrencedId = employeeDetail.EmployeeId;
            attachfile.IsActive = employeeDetail.IsActive;
            attachfile.FileAttachmentType = "1";
            this.entities.AttachmentMaster.AddObject(attachfile);
            this.entities.SaveChanges();


            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Employee));
        }

        //image path upload to Attachment Table
        //[HttpPost]
        //public ApiResponse FileUploadTODB([FromBody] AttachmentMaster filedata)
        //{
        //    //filedata.FileAttachmentType = (int?)FileAttachmentType.Slider;
        //    this.entities.AttachmentMaster.AddObject(new AttachmentMaster()
        //    {

        //        FileName = filedata.FileName,
        //        Filepath = filedata.Filepath,
        //        // FileAttachmentType="profile_Image",
        //        IsActive = true,
        //        IsDeleted = filedata.IsDeleted,
        //        FileRelativePath = filedata.FileRelativePath,
        //        OriginalFileName = filedata.OriginalFileName
        //    });
        //    if (!(this.entities.SaveChanges() > 0))
        //    {
        //        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Employee));
        //    }
        //    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Employee));
        //}


        /// <summary>
        /// sp for employee list
        /// </summary>
        /// <param name="employeeDetailsParams"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse GetEmployeeDetails(PagingParams employeeDetailsParams)
        {
            if (string.IsNullOrWhiteSpace(employeeDetailsParams.Search))
            {
                employeeDetailsParams.Search = string.Empty;
            }
            var results = this.entities.sp_Emp_GetAllEmployees().Where(x => x.FirstName.Trim().ToLower().Contains(employeeDetailsParams.Search.Trim().ToLower())).ToList().AsQueryable().Skip((employeeDetailsParams.CurrentPageNumber - 1) * employeeDetailsParams.PageSize).Take(employeeDetailsParams.PageSize);
            var TotalRecords = this.entities.sp_Emp_GetAllEmployees().Where(x => x.FirstName.Trim().ToLower().Contains(employeeDetailsParams.Search.Trim().ToLower())).Count();


            return this.Response(Utilities.MessageTypes.Success, string.Empty, new { list = results, Total = TotalRecords });

        }


        /// <summary>
        /// using js pdf
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse GeneratePdf()
        {
            var records = this.entities.sp_Emp_GetAllEmployees().ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, records);
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


