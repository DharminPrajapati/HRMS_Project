
namespace MVCProject.Api.Controllers.Configuration
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
    using System.IO;
    using System.Net.Http.Headers;
    using System.Web;
    using Newtonsoft.Json;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.Util;
    using System.Web.Hosting;
    using NPOI.HPSF;
    #endregion
    public class UserMastersController : BaseController
    {

        private MVCProjectEntities entities;

        public UserMastersController()
        {
            this.entities = new MVCProjectEntities();
        }
        /// <summary>
        /// Gets all UserMaster details. 
        /// </summary>
        /// <param name="userMasterDetailParams">Pass parameters of UserMaster details</param>
        /// <returns>Returns response of type <see cref="ApiResonse"/> class.</returns>
        [HttpPost]
        public ApiResponse GetAllUserMasters(PagingParams userMasterDetailParams)
        {
            if (string.IsNullOrWhiteSpace(userMasterDetailParams.Search))
            {
                userMasterDetailParams.Search = string.Empty;
            }

            var UserMasterList = (from s in this.entities.UserMaster.AsEnumerable().Where(x => x.UserName.Trim().ToLower().Contains(userMasterDetailParams.Search.Trim().ToLower()))
                                  let TotalRecords = this.entities.UserMaster.AsEnumerable().Where(x => x.UserName.Trim().ToLower().Contains(userMasterDetailParams.Search.Trim().ToLower())).Count()
                                  select new
                                  {
                                      UserId = s.UserId,
                                      EmpId = s.EmployeeId,
                                      FirstName = s.EmployeeId > 0 ? this.entities.TblEmployees.FirstOrDefault(x => x.EmployeeId == s.EmployeeId).FirstName : string.Empty,
                                      LastName = s.EmployeeId > 0 ? this.entities.TblEmployees.FirstOrDefault(x => x.EmployeeId == s.EmployeeId).LastName : string.Empty,
                                      UserName = s.UserName,
                                      Password = s.UserPassword,
                                      IsActive = s.IsActive,
                                      IsLock = s.IsLock,
                                      UserRoleName = String.Join(",", (from r in this.entities.UserRole.Where(x => x.UserId == s.UserId).ToList()
                                                                       join rm in this.entities.UserRoleMaster on r.RoleId equals rm.RoleId
                                                                       select rm.UserRoleName).ToArray()),
                                      TotalRecords
                                      //}).AsQueryable().Skip((userMasterDetailParams.CurrentPageNumber - 1) * userMasterDetailParams.PageSize).Take(userMasterDetailParams.PageSize);
                                  }).AsQueryable().OrderByField(userMasterDetailParams.OrderByColumn, userMasterDetailParams.IsAscending).Skip((userMasterDetailParams.CurrentPageNumber - 1) * userMasterDetailParams.PageSize).Take(userMasterDetailParams.PageSize);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, UserMasterList);
        }


        //auto complete
        [HttpGet]
        public ApiResponse GetFullname(bool isActive, string searchText)
        {
            var data = this.entities.TblEmployees.Where(x => x.IsActive.Value == isActive && x.FirstName.Contains(searchText)).Select(x => new { Name = x.FirstName +" "+ x.LastName, Id = x.EmployeeId }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }

        /// <summary>
        /// Get UserMaster 
        /// </summary>
        /// <param name="isGetAll">To get active records</param>
        /// <returns>Returns response of type</returns>class.
        [HttpGet]
        public ApiResponse GetUserMasterList(bool isGetAll = false)
        {
            var result = this.entities.UserMaster.Where(x => (isGetAll || x.IsActive.Value)).Select(x => new { Id = x.UserId, Name = x.UserName }).OrderBy(y => y.Name).ToList();
            return this.Response(MessageTypes.Success, string.Empty, result);
        }


        /// <summary>
        /// Get UserMaster Master List by Id
        /// </summary>
        /// <param name="UserId">UserMaster id.</param>
        /// <returns>Returns response type of <see cref="ApiResponse"/> class.></returns>
        [HttpGet]
        public ApiResponse GetUserMasterById(int UserId)
        {
            var userMasterDetail = this.entities.UserMaster

                 .Where(a => a.UserId == UserId).ToList()
                        .Select(g => new
                        {
                            UserId = g.UserId,
                            UserName = g.UserName,
                            EmployeeId = g.EmployeeId,
                            FirstName = g.EmployeeId > 0 ? this.entities.TblEmployees.FirstOrDefault(x => x.EmployeeId == g.EmployeeId).FirstName : string.Empty,
                            UserPassword = g.UserPassword,
                            IsActive = g.IsActive,
                            IsLock = g.IsLock,
                            UserRole = this.entities.UserRole.Where(x => x.UserId == g.UserId).Select(r => new
                            {
                                r.RoleId,
                                r.UserRoleId,
                                r.UserId
                            }).ToList(),
                            //Remarks = g.Remarks,
                            // EntryBy = g.EntryBy,
                            //EntryDate = g.EntryDate,
                        }).FirstOrDefault();
            if (userMasterDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, userMasterDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }
        //check foreignkey and primaray key 
        [HttpGet]
        public ApiResponse GetUserbyEmployeeId(int Id)
        {
            object user = null;
            if (this.entities.UserMaster.Any(x => x.EmployeeId == Id))

            {
                user = GetUserMasterById(this.entities.UserMaster.FirstOrDefault(x => x.EmployeeId == Id).UserId).Result;
            }

            if (user != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, user);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }


        /// <summary>
        /// Add/update UserMaster details
        /// </summary>
        /// <param name="userMasterDetail">UserMaster Details</param>
        /// <returns>Returns response type of <see cref="ApiResponse"/> class.></returns>
        [HttpPost]
        public ApiResponse SaveuserMasterDetails(UserMaster userMasterDetail)
        {
            if (this.entities.UserMaster.Any(x => x.UserId != userMasterDetail.UserId && x.UserName.Trim() == userMasterDetail.UserName.Trim()))
            {
                return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.User_Master));
            }
            else
            {
                UserMaster existinguserMasterDetail = this.entities.UserMaster.Where(a => a.UserId == userMasterDetail.UserId).FirstOrDefault();
                if (existinguserMasterDetail == null)
                {
                    //userMasterDetail.EntryDate = DateTime.UtcNow;
                    //userMasterDetail.EntryBy = UserContext.EmployeeId;

                    this.entities.UserMaster.AddObject(userMasterDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.User));
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.User_Master));
                }
                else
                {
                    // For update record
                    existinguserMasterDetail.UserName = userMasterDetail.UserName;
                    existinguserMasterDetail.UserPassword = userMasterDetail.UserPassword;
                    existinguserMasterDetail.EmployeeId = userMasterDetail.EmployeeId;
                    existinguserMasterDetail.UserId = userMasterDetail.UserId;
                    existinguserMasterDetail.UserRole = userMasterDetail.UserRole;
                    existinguserMasterDetail.IsActive = userMasterDetail.IsActive;
                    existinguserMasterDetail.IsLock = userMasterDetail.IsLock;
                    // existinguserMasterDetail.Remarks = userMasterDetail.Remarks;
                    //existinguserMasterDetail.UpdateBy = UserContext.EmployeeId;
                    //existinguserMasterDetail.UpdateDate = DateTime.UtcNow;
                    this.entities.UserMaster.ApplyCurrentValues(existinguserMasterDetail);

                }

                var existingUserRole = this.entities.UserRole.Where(ur => ur.UserId == userMasterDetail.UserId).ToList();
                if (existingUserRole != null)
                {
                    foreach (var item in existingUserRole)
                    {
                        this.entities.UserRole.DeleteObject(item);
                        this.entities.SaveChanges();
                    }
                }

                foreach (UserRole userRole in userMasterDetail.UserRole.ToList())
                {
                    userRole.UserId = userMasterDetail.UserId;
                    userRole.UserMaster = null;
                    this.entities.UserRole.AddObject(userRole);
                }
                this.entities.SaveChanges();

                //foreach (UserRole userRole in userMasterDetail.UserRole)
                //{
                //    UserRole existingUserRole = this.entities.UserRole.FirstOrDefault(ur => ur.UserId == userMasterDetail.UserId);
                //    if (existingUserRole != null)
                //    {
                //        this.entities.DeleteObject(existingUserRole);
                //        this.entities.SaveChanges();
                //    }

                //    userRole.UserId = userMasterDetail.UserId;
                //    this.entities.UserRole.AddObject(userRole);
                //    this.entities.SaveChanges();
                //}
                //foreach (UserRole userRole in userMasterDetail.UserRole)
                //{
                //    userRole.UserId = userMasterDetail.UserId;
                //    this.entities.UserRole.AddObject(userRole);
                //    if (!(this.entities.SaveChanges() > 0))
                //    {
                //        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.UserMaster);
                //    }
                //}
                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.User_Master));
            }

        }


        [HttpGet]
        public ApiResponse Getuserroledropdown()
        {
            var data = this.entities.UserRoleMaster.Where(x => x.IsActive.Value).Select(x => new { Name = x.UserRoleName, RoleId = x.RoleId }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }
        /// <summary>
        /// Disposes expensive resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether to dispose or not.</param>
        ///
        [HttpGet]
        public ApiResponse Export()
        {
            //var data = entities.CompanyMaster.ToList();
            var data = this.entities.UserMaster.ToList().Select(s => new
            {
                UserId = s.UserId,
                EmployeeId = s.EmployeeId,
                EmpName = s.EmployeeId > 0 ? this.entities.TblEmployees.FirstOrDefault(x => x.EmployeeId == s.EmployeeId).FirstName : string.Empty,
                UserName = s.UserName,
                UserPassword = s.UserPassword,
                IsActive = s.IsActive != null ? s.IsActive == true ? "Active" : "InActive" : string.Empty,
                IsLock = s.IsLock,
                UserRoleName = (from r in this.entities.UserRole.Where(x => x.UserId == s.UserId).ToList()
                                join rm in this.entities.UserRoleMaster on r.RoleId equals rm.RoleId
                                select rm.UserRoleName).ToList(),

            });


            //Create a new workbook
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

            //Add some data to the sheet
            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("UserId");
            headerRow.CreateCell(1).SetCellValue("FullName");
            headerRow.CreateCell(2).SetCellValue("UserName");
            headerRow.CreateCell(3).SetCellValue("UserRoleName");
            headerRow.CreateCell(4).SetCellValue("IsActive");

            // Set the cell style for the header row
            foreach (var cell in headerRow.Cells)
            {
                cell.CellStyle = headerCellStyle;
            }

            int rowNumber = 1;
            foreach (var UserMaster in data)
            {
                IRow row = sheet.CreateRow(rowNumber++);
                row.CreateCell(0).SetCellValue(UserMaster.UserId);
                row.CreateCell(1).SetCellValue(UserMaster.EmpName);
                row.CreateCell(2).SetCellValue(UserMaster.UserName);
                row.CreateCell(3).SetCellValue(string.Join(",", UserMaster.UserRoleName.Select(x => x.ToString()).ToList()));
                row.CreateCell(4).SetCellValue(UserMaster.IsActive);

            }
            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            //Set the File Path.
            string filePath = HttpContext.Current.Server.MapPath("~/Reports/UserMaster.xlsx");
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
            response.Content.Headers.ContentDisposition.FileName = "MyExcelFile.xlsx";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentLength = byteArray.Length;
            Console.WriteLine(response);
            //return response;

            return this.Response(Utilities.MessageTypes.Success, string.Empty, filePath);
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
