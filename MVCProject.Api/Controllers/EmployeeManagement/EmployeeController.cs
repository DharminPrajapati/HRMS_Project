namespace MVCProject.Api.Controllers.EmployeeManagement
{
    
        #region NameSapces
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
        public class EmployeeController : BaseController
        {
            private MVCProjectEntities entities;

            public EmployeeController()
            {
                this.entities = new MVCProjectEntities();
            }


            ///Get All Employee Details

            //[HttpPost]
            //public ApiResponse GetAllEmployees(PagingParams employeeDetailsParams)
            //{
            //    if (string.IsNullOrWhiteSpace(employeeDetailsParams.Search))
            //    {
            //        employeeDetailsParams.Search = string.Empty;
            //    }
            //    var employeelist = (from d in this.entities.TblEmployee.AsEnumerable().Where(x => x.FirstName.Trim().ToLower().Contains(employeeDetailsParams.Search.Trim().ToLower()))
            //                        let TotalRecords = this.entities.TblEmployee.AsEnumerable().Where(x => x.FirstName.Trim().ToLower().Contains(employeeDetailsParams.Search.Trim().ToLower())).Count()
            //                        select new
            //                        {
            //                            EmployeeId = d.EmployeeId,
            //                            FirstName = d.FirstName,
            //                            LastName = d.LastName,
            //                            Email = d.Email,
            //                            Password = d.Password,
            //                            JoiningDate = d.JoiningDate,
            //                            PhoneNumber = d.PhoneNumber,
            //                            AlternatePhoneNumber = d.AlternatePhoneNumber,
            //                            IsActive = d.IsActive
            //                        }).AsQueryable().OrderByField(employeeDetailsParams.OrderByColumn, employeeDetailsParams.IsAscending).Skip((employeeDetailsParams.CurrentPageNumber = -1) * employeeDetailsParams.PageSize).Take(employeeDetailsParams.PageSize);

            //    return this.Response(Utilities.MessageTypes.Success, string.Empty, employeelist);

            //}

        [HttpGet]
        public ApiResponse GetAllEmployees()
        {
            //var employeelist = this.entities.TblEmployee.ToList();
            var employeelist = this.entities.TblEmployee.Select(d=> new {
                EmployeeId = d.EmployeeId,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
                Password = d.Password,
                JoiningDate = d.JoiningDate,
                PhoneNumber = d.PhoneNumber,
                AlternatePhoneNumber = d.AlternatePhoneNumber,
                IsActive = d.IsActive
            });
           
            return this.Response(Utilities.MessageTypes.Success, string.Empty, employeelist);

        }

        [HttpGet]

            public ApiResponse GetEmployeeList(bool isGetAll = false)
            {
                var result = this.entities.TblEmployee.Where(x => (isGetAll || x.IsActive.Value)).Select(x => new { Id = x.EmployeeId, Name = x.FirstName }).OrderBy(e => e.Name).ToList();
                return this.Response(Utilities.MessageTypes.Success, string.Empty, result);
            }

            /// <summary>
            /// Get Employees By Id
            /// </summary>
            [HttpGet]

            public ApiResponse GetEmployeeById(int employeeId)
            {
                var employeeDetail = this.entities.TblEmployee.Where(x => x.EmployeeId == employeeId)
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
                if (this.entities.TblEmployee.Any(x => x.EmployeeId != employeeDetail.EmployeeId && x.FirstName.Trim() == employeeDetail.FirstName.Trim()))
                {
                    return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Department));
                }
                else
                {
                    TblEmployee existingEmployeeDetail = this.entities.TblEmployee.Where(x => x.EmployeeId == employeeDetail.EmployeeId).FirstOrDefault();
                    if (existingEmployeeDetail == null)
                    {
                        this.entities.TblEmployee.AddObject(employeeDetail);
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
                        existingEmployeeDetail.Password = employeeDetail.Password;
                        existingEmployeeDetail.JoiningDate = employeeDetail.JoiningDate;
                        existingEmployeeDetail.PhoneNumber = employeeDetail.PhoneNumber;
                        existingEmployeeDetail.AlternatePhoneNumber = employeeDetail.AlternatePhoneNumber;
                        existingEmployeeDetail.IsActive = employeeDetail.IsActive;

                        this.entities.TblEmployee.ApplyCurrentValues(existingEmployeeDetail);
                        if (!(this.entities.SaveChanges() > 0))
                        {
                            return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Employee));
                        }

                        return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Employee));
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
