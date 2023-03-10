namespace MVCProject.Api.Controllers.EmployeeManagement
{

    #region NameSapces
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http;
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    using Newtonsoft.Json;
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

            var response = new HttpResponseMessage(HttpStatusCode.OK);


            response.Content = new StringContent(JsonConvert.SerializeObject(employeeDetail), Encoding.UTF8, "application/json");

            return this.Response(Utilities.MessageTypes.Success, string.Empty, response);
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
    

