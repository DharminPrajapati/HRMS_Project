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
        /// Get All Designation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse GetDesignationDropDown()
        {
            var data = this.entities.Designations.Where(x => x.IsActive.Value).Select(x => new { Name = x.DesignationName, Id = x.DesignationId }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }

        /// Get All Departments 
        [HttpGet]
        public ApiResponse GetDepartmentDropDown()
        {
            var data = this.entities.TblDepartments.Where(x => x.IsActive.Value).Select(x => new { DeptName = x.DepartmentName, DeptId = x.DepartmentId }).OrderBy(x => x.DeptName).ToList();
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

        //[HttpPost]
        //public ApiResponse GetAllEmployees(PagingParams employeeDetailsParams)
        //{
        //    //if (string.IsNullOrWhiteSpace(employeeDetailParams.Search))
        //    //{
        //    //    employeeDetailParams.Search = string.Empty;
        //    //}

        //    var employeelist = (from d in this.entities.TblEmployees.AsEnumerable()
        //                        let TotalRecords = this.entities.TblEmployees.AsEnumerable().Count()
        //                        select new
        //                        {
        //                            //var employeelist = this.entities.TblEmployee.ToList();

        //                            EmployeeId = d.EmployeeId,
        //                            FirstName = d.FirstName,
        //                            LastName = d.LastName,
        //                            Email = d.Email,
        //                            Password = d.Password,
        //                            JoiningDate = d.JoiningDate,
        //                            PhoneNumber = d.PhoneNumber,
        //                            AlternatePhoneNumber = d.AlternatePhoneNumber,
        //                            Designation = d.DesignationId,
        //                            Department = d.DepartmentId,
        //                            BirthDate = d.BirthDate,
        //                            Gender = d.Gender,
        //                            PermanentAddress = d.PermanentAddress,
        //                            TemporaryAddress = d.TemporaryAddress,
        //                            Pincode = d.Pincode,
        //                            InstitutionName = d.InstitutionName,
        //                            CourseName = d.CourseName,
        //                            CourseStartDate = d.CourseStartDate,
        //                            CourseEndDate = d.CourseEndDate,
        //                            Grade = d.Grade,
        //                            Degree = d.Degree,
        //                            CompanyName = d.CompanyName,
        //                            LastJobLocation = d.LastJobLocation,
        //                            JobPosition = d.JobPosition,
        //                            FromPeriod = d.FromPeriod,
        //                            ToPeriod = d.ToPeriod,
        //                            IsActive = d.IsActive
        //                        }).AsQueryable().Skip((employeeDetailsParams.CurrentPageNumber - 1) * employeeDetailsParams.PageSize).Take(employeeDetailsParams.PageSize);

        //    return this.Response(Utilities.MessageTypes.Success, string.Empty, employeelist);

        //}
        // [HttpGet]
        //public ApiResponse GetAllEmployees()
        //{
        //var employeelist = this.entities.TblEmployees.Select(d => new {
        //    EmployeeId = d.EmployeeId,
        //    FirstName = d.FirstName,
        //    LastName = d.LastName,
        //    Email = d.Email,
        //    Password = d.Password,
        //    JoiningDate = d.JoiningDate,
        //    PhoneNumber = d.PhoneNumber,
        //    AlternatePhoneNumber = d.AlternatePhoneNumber,
        //    Designation = d.DesignationId,
        //    Department = d.DepartmentId,
        //    BirthDate = d.BirthDate,
        //    Gender = d.Gender,
        //    PermanentAddress = d.PermanentAddress,
        //    TemporaryAddress = d.TemporaryAddress,
        //    Pincode = d.Pincode,
        //    InstitutionName = d.InstitutionName,
        //    CourseName = d.CourseName,
        //    CourseStartDate = d.CourseStartDate,
        //    CourseEndDate = d.CourseEndDate,
        //    Grade = d.Grade,
        //    Degree = d.Degree,
        //    CompanyName = d.CompanyName,
        //    LastJobLocation = d.LastJobLocation,
        //    JobPosition = d.JobPosition,
        //    FromPeriod = d.FromPeriod,
        //    ToPeriod = d.ToPeriod,
        //    IsActive = d.IsActive
        //});
        //         return this.Response(Utilities.MessageTypes.Success, string.Empty, employeelist);

        //}

        [HttpGet]

        public ApiResponse GetEmployeeList(bool isGetAll = false)
        {
            var result = this.entities.TblEmployees.Where(x => (isGetAll || x.IsActive.Value)).Select(x => new { Id = x.EmployeeId, Name = x.FirstName }).OrderBy(e => e.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, result);
        }



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
                //DesignationName = d.DesignationReference.Value.DesignationName,
                //DepartmentName = d.TblDepartmentReference.Value.DepartmentName,
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

            // Add Some Data to Sheet
            // 
            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Employee Id");
            headerRow.CreateCell(1).SetCellValue("First Name");
            headerRow.CreateCell(2).SetCellValue("Last Name");
            headerRow.CreateCell(3).SetCellValue("Email");
            headerRow.CreateCell(4).SetCellValue("Joining Date");
            headerRow.CreateCell(5).SetCellValue("Phone Number");
            headerRow.CreateCell(6).SetCellValue("Alternate Phone Number");
            headerRow.CreateCell(7).SetCellValue("Designation Name");
            headerRow.CreateCell(8).SetCellValue("Department Name");
            headerRow.CreateCell(9).SetCellValue("Birth Date");
            headerRow.CreateCell(10).SetCellValue("Gender");
            headerRow.CreateCell(11).SetCellValue("Permanent Address");
            headerRow.CreateCell(12).SetCellValue("Temporary Address");
            headerRow.CreateCell(13).SetCellValue("Pincode");
            headerRow.CreateCell(14).SetCellValue("Institution Name");
            headerRow.CreateCell(15).SetCellValue("Course Name");
            headerRow.CreateCell(16).SetCellValue("Course Start Date");
            headerRow.CreateCell(17).SetCellValue("Course End Date");
            headerRow.CreateCell(18).SetCellValue("Grade");
            headerRow.CreateCell(19).SetCellValue("Degree");
            headerRow.CreateCell(20).SetCellValue("IsActive");

            int rowNumber = 1;
            foreach (var emp in employeeDetail)
            {
                IRow row = sheet.CreateRow(rowNumber++);
                row.CreateCell(0).SetCellValue(emp.EmployeeId);
                row.CreateCell(1).SetCellValue(emp.FirstName);
                row.CreateCell(2).SetCellValue(emp.LastName);
                row.CreateCell(3).SetCellValue(emp.Email);
                row.CreateCell(4).SetCellValue((DateTime)emp.JoiningDate);
                row.CreateCell(5).SetCellValue(emp.PhoneNumber);
                row.CreateCell(6).SetCellValue(emp.AlternatePhoneNumber);
                row.CreateCell(7).SetCellValue((double)emp.DesignationId);
                row.CreateCell(8).SetCellValue((double)emp.DepartmentId);
                row.CreateCell(9).SetCellValue((DateTime)emp.BirthDate);
                row.CreateCell(10).SetCellValue(emp.Gender);
                row.CreateCell(11).SetCellValue(emp.PermanentAddress);
                row.CreateCell(12).SetCellValue(emp.TemporaryAddress);
                row.CreateCell(13).SetCellValue((double)emp.Pincode);
                row.CreateCell(14).SetCellValue(emp.InstitutionName);
                row.CreateCell(15).SetCellValue(emp.CourseName);
                row.CreateCell(16).SetCellValue((DateTime)emp.CourseStartDate);
                row.CreateCell(17).SetCellValue((DateTime)emp.CourseEndDate);
                row.CreateCell(18).SetCellValue(emp.Grade);
                row.CreateCell(19).SetCellValue(emp.Degree);
                row.CreateCell(20).SetCellValue(emp.IsActive);
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
        [HttpGet]

        public ApiResponse GetEmployeeById(int employeeId)
        {
            var employeeDetail = this.entities.TblEmployees.Where(x => x.EmployeeId == employeeId)
                   .Select(d => new
                   {
                       EmployeeId = d.EmployeeId,
                       FirstName = d.FirstName,
                       LastName = d.LastName,
                       Email = d.Email,
                       Password = d.Password,
                       JoiningDate = d.JoiningDate,
                       PhoneNumber = d.PhoneNumber,
                       AlternatePhoneNumber = d.AlternatePhoneNumber,
                       DesignationId = d.DesignationId,
                       DepartmentId = d.DepartmentId,
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
                       IsActive = d.IsActive
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
                this.entities.TblEmployees.AddObject(employeeDetail);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Employee));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Employee));
            }

            // For Update

            else
            {
                existingEmployeeDetail.FirstName = employeeDetail.FirstName;
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

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Employee));
            }
        }

        //image path upload to Attachment Table
        [HttpPost]
        public ApiResponse FileUploadTODB([FromBody] AttachmentMaster filedata)
        {
            //filedata.FileAttachmentType = (int?)FileAttachmentType.Slider;
            this.entities.AttachmentMaster.AddObject(new AttachmentMaster()
            {

                FileName = filedata.FileName,
                Filepath = filedata.Filepath,
                // FileAttachmentType="profile_Image",
                IsActive = true,
                IsDeleted = filedata.IsDeleted,
                FileRelativePath = filedata.FileRelativePath,
                OriginalFileName = filedata.OriginalFileName
            });
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Employee));
            }
            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Employee));
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
    

