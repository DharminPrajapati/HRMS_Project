
namespace MVCProject.Api.Controllers.Attendance
{
    #region NameSapces
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.EntityClient;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    #endregion
    public class AttendanceController : BaseController
    {
        private MVCProjectEntities entities;

        public AttendanceController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpGet]
        public ApiResponse GetInTimeByEmployeeId(int employeeId)
        {
            var AttendanceIntime = this.entities.Attendance.Where(x => x.EmployeeId == employeeId && x.Date == DateTime.Today)
                .Select(d => new
                {
                    EmployeeId = d.EmployeeId,
                    InTime = d.InTime
                }).ToList();
            if (AttendanceIntime != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, AttendanceIntime);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);

            }
        }

        [HttpGet]
        public ApiResponse GetAttendance(int employeeId)
        {
            var attendanceDetail = this.entities.Sp_HRMS_Attendance().Where(x => x.EmployeeId == employeeId)
                .Select(d => new
                {
                    EmployeeId = d.EmployeeId,
                    InTime = d.InTime,
                    OutTime = d.OutTime,
                    Date = d.Date,
                    IsActive = d.IsActive
                }).SingleOrDefault();
            if(attendanceDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, attendanceDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);

            }
        }

        [HttpPost]
        public ApiResponse SaveAttendanceDetails([FromBody] Attendance AttendanceDetail)
        {


            if (AttendanceDetail.EmployeeId > 0)
            {
                AttendanceDetail.Date = DateTime.Today;
                //TimeSpan intime = DateTime.UtcNow.TimeOfDay;
                TimeSpan intime = TimeSpan.ParseExact(DateTime.UtcNow.ToString("hh:mm:ss"), "hh\\:mm\\:ss", CultureInfo.InvariantCulture);

                AttendanceDetail.InTime = intime;
                entities.Attendance.AddObject(AttendanceDetail);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Attendance));
                }
            }
            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Attendance), AttendanceDetail.EmployeeId);


        }

        [HttpPost]
        public ApiResponse UpdateAttendance([FromBody] Attendance AttendanceDetail)
        {
            Attendance existingAttendanceDetail = this.entities.Attendance.Where(x => x.EmployeeId == AttendanceDetail.EmployeeId && x.Date == DateTime.Today).FirstOrDefault();
            //TimeSpan outtime = DateTime.UtcNow.TimeOfDay;
            TimeSpan outtime = TimeSpan.ParseExact(DateTime.UtcNow.ToString("hh:mm:ss"), "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
            existingAttendanceDetail.OutTime = outtime;
            existingAttendanceDetail.OutLatitude = AttendanceDetail.OutLatitude;
            existingAttendanceDetail.OutLongitude = AttendanceDetail.OutLongitude;
            existingAttendanceDetail.OutDiscription = AttendanceDetail.OutDiscription;



            this.entities.Attendance.ApplyCurrentValues(existingAttendanceDetail);
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Attendance));
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Attendance), AttendanceDetail.EmployeeId);
        }


        [HttpPost]
        public ApiResponse UploadImage([FromBody] AttachmentMaster data, int EmployeeId, string databaseName = "HRMS", string directoryPathEnumName = "Attachment_Attendance")
        {
            string FileURL = string.Empty;
            string directoryPath = string.Empty;
            DirectoryPath enumDirectoryPath = new DirectoryPath();
            if (Enum.IsDefined(typeof(DirectoryPath), directoryPathEnumName))
            {
                Enum.TryParse(directoryPathEnumName, out enumDirectoryPath);
                directoryPath = AppUtility.GetDirectoryPath(enumDirectoryPath, databaseName, false, FileURL);
            }
            File.Copy(Path.Combine(directoryPath, data.FileName), Path.Combine(AppUtility.GetDirectoryPath(DirectoryPath.Attachment, databaseName, false, FileURL), data.FileName), true);


            string filePath = Path.Combine(AppUtility.GetDirectoryPath(DirectoryPath.Attachment, databaseName, false, FileURL), data.FileName);
            string fileRelativePath = string.Format("{0}{1}", AppUtility.GetDirectoryPath(DirectoryPath.Attachment, databaseName, true, FileURL), data.FileName);

            entities.AttachmentMaster.AddObject(new AttachmentMaster()
            {
                FileName = data.FileName,
                Filepath = filePath,
                FileRelativePath = fileRelativePath,
                OriginalFileName = data.OriginalFileName,
                IsDeleted = false,
                RefrencedId = EmployeeId,
                FileAttachmentType = "4"
            });
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.File));
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.File), data);
        }



        //[HttpPost]
        //public ApiResponse SaveAttendanceDetails([FromBody] Attendance AttendanceDetail)
        //{


        //    if (AttendanceDetail.EmployeeId > 0)
        //    {
        //        AttendanceDetail.Date = DateTime.UtcNow;
        //        //TimeSpan intime = TimeSpan.ParseExact(DateTime.UtcNow.ToString("HH:mm:ss"), "HH.mm.ss", CultureInfo.InvariantCulture);
        //        TimeSpan intime = TimeSpan.ParseExact(DateTime.UtcNow.ToString("HH:mm:ss"), "HH\\:mm\\:ss", CultureInfo.InvariantCulture);

        //        AttendanceDetail.InTime = intime;
        //        entities.Attendance.AddObject(AttendanceDetail);
        //        if (!(this.entities.SaveChanges() > 0))
        //        {
        //            return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Attendance));
        //        }
        //    }
        //    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Attendance), AttendanceDetail.EmployeeId);


        //}

        //[HttpPost]
        //public ApiResponse UpdateAttendance([FromBody] Attendance AttendanceDetail)
        //{
        //    Attendance existingAttendanceDetail = this.entities.Attendance.Where(x => x.EmployeeId == AttendanceDetail.EmployeeId && x.Date == DateTime.Today).FirstOrDefault();

        //    TimeSpan outtime = TimeSpan.ParseExact(DateTime.UtcNow.ToString("HH:mm:ss"), "HH\\:mm\\:ss", CultureInfo.InvariantCulture);
        //    existingAttendanceDetail.OutTime = outtime;
        //    existingAttendanceDetail.OutLatitude = AttendanceDetail.OutLatitude;
        //    existingAttendanceDetail.OutLongitude = AttendanceDetail.OutLongitude;
        //    existingAttendanceDetail.OutDiscription = AttendanceDetail.OutDiscription;



        //    this.entities.Attendance.ApplyCurrentValues(existingAttendanceDetail);
        //    if (!(this.entities.SaveChanges() > 0))
        //    {
        //        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Attendance));
        //    }

        //    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Attendance), AttendanceDetail.EmployeeId);
        //}

        [HttpGet]
        public ApiResponse GetAllAttendance(int employeeId)
        {
            //var employeelist = this.entities.TblEmployee.ToList();
            var Attendanelist = this.entities.Attendance.Where(x=>x.EmployeeId == employeeId)
                .Select(d => new {
                AttendanceId = d.AttendanceId,
                EmployeeId = d.EmployeeId,
                Date = d.Date,
                InTime = d.InTime,
                OutTime = d.OutTime,
                InLatitude = d.InLatitude,
                OutLatitude = d.OutLatitude,
                InLongitude = d.InLongitude,
                InDiscription = d.InDiscription,
                OutDiscription = d.OutDiscription
            });

            return this.Response(Utilities.MessageTypes.Success, string.Empty, Attendanelist);

        }
        [HttpGet]

        public ApiResponse GetAttendanceById(int employeeId,DateTime date)
        {
            var attendanceDetail = this.entities.Attendance.Where(x => x.EmployeeId == employeeId && x.Date==date)
                   .Select(d => new
                   {
                       AttendanceId = d.AttendanceId,
                       EmployeeId = d.EmployeeId,
                       Date = d.Date,
                       InTime = d.InTime,
                       OutTime = d.OutTime,
                       InLatitude = d.InLatitude,
                       OutLatitude = d.OutLatitude,
                       InLongitude = d.InLongitude,
                       InDiscription = d.InDiscription,
                       OutDiscription = d.OutDiscription

                   }).SingleOrDefault();
            if (attendanceDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, attendanceDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        [HttpGet]

        public ApiResponse GetAllAttendanceById(int employeeId)
        {
            var attendanceDetail = this.entities.Attendance.Where(x => x.EmployeeId == employeeId)
                   .Select(d => new
                   {
                       AttendanceId = d.AttendanceId,
                       EmployeeId = d.EmployeeId,
                       Date = d.Date,
                       InTime = d.InTime,
                       OutTime = d.OutTime,
                       InLatitude = d.InLatitude,
                       OutLatitude = d.OutLatitude,
                       InLongitude = d.InLongitude,
                       InDiscription = d.InDiscription,
                       OutDiscription = d.OutDiscription

                   }).ToList();
            if (attendanceDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, attendanceDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        [HttpGet]
        public ApiResponse AttendanceStatus(int employeeId)
        {
            var data = this.entities.Attendance.Where(x => x.EmployeeId == employeeId && x.Date == DateTime.Today && x.OutLatitude != null && x.OutLongitude != null && x.OutDiscription != null).Any();
            if (data == true)
            {
                return this.Response(Utilities.MessageTypes.Success, responseToReturn: true);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.Success, responseToReturn: false);
            }
        }

        ///// <summary>
        ///// HR Attendance By Date Using Store Procedure
        ///// </summary>
        ///// <param name="month"></param>
        ///// <param name="year"></param>
        ///// <returns></returns>
        [HttpGet]
        public ApiResponse GetHRAttendanceByMonthYear(int month, int year, int? pagesize = null, int? pageNumber = null, string search = null)
        {
            object result = null;
            int totalRecords = 0;
            DataSet ds = new DataSet("CalendarData");
            string providerString = ((EntityConnection)this.entities.Connection).StoreConnection.ConnectionString;
            using (var conn = new System.Data.SqlClient.SqlConnection(providerString))
            {
                SqlCommand cmd = new SqlCommand("usp_EmployeeDaysInMonth", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                if (pagesize != null)
                {
                    cmd.Parameters.AddWithValue("@PageSize", pagesize);
                }
                if (pageNumber != null)
                {
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    // Escape single quote characters in the search parameter
                    search = search.Replace("'", "''");
                    cmd.Parameters.AddWithValue("@Search", search);
                }
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                conn.Close();
                if (ds.Tables.Count > 0)
                {
                    var table = ds.Tables[0];
                    if (pagesize != null && pageNumber != null)
                    {
                        int count = pagesize.Value * (pageNumber.Value - 1);
                        var rows = table.AsEnumerable().Take(pagesize.Value).ToList();
                        if (rows.Any())
                        {
                            result = rows.CopyToDataTable();
                            totalRecords = table.Rows.Count;
                        }
                        else
                        {
                            result = table.Clone();
                        }
                    }
                    else
                    {
                        result = table;
                        totalRecords = table.Rows.Count;
                    }
                }
                else
                {
                    result = new DataTable();
                }
            }
            return this.Response(Utilities.MessageTypes.Success, string.Empty, new { result = result, TotalRecords = totalRecords });
        }
        //[HttpGet]

        //public ApiResponse GetAttendanceById(int employeeId)
        //{
        //    var attendanceDetail = this.entities.Attendance.Where(x => x.EmployeeId == employeeId)
        //           .Select(d => new
        //           {
        //               AttendanceId = d.AttendanceId,
        //               EmployeeId = d.EmployeeId,
        //               Date = d.Date,
        //               InTime = d.InTime,
        //               OutTime = d.OutTime,
        //               InLatitude = d.InLatitude,
        //               OutLatitude = d.OutLatitude,
        //               InLongitude = d.InLongitude,
        //               InDiscription = d.InDiscription,
        //               OutDiscription = d.OutDiscription

        //           }).SingleOrDefault();
        //    if (attendanceDetail != null)
        //    {
        //        return this.Response(Utilities.MessageTypes.Success, string.Empty, attendanceDetail);
        //    }
        //    else
        //    {
        //        return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
        //    }
        //}


        //[HttpGet]
        //public ApiResponse AttendanceStatus(int employeeId)
        //{
        //    var data = this.entities.Attendance.Where(x => x.EmployeeId == employeeId && x.Date == DateTime.Today).Any();
        //    if (data == true)
        //    {
        //        return this.Response(Utilities.MessageTypes.Success, responseToReturn: true);
        //    }
        //    else
        //    {
        //        return this.Response(Utilities.MessageTypes.Success, responseToReturn: false);
        //    }
        //}

        //[HttpGet]
        //public ApiResponse GetHRAttendanceByMonthYear(int month, int year,PagingParams pagingParams)
        //{

        //    object result = null;
        //    DataSet ds = new DataSet("CalendarData");
        //    string providerString = ((EntityConnection)this.entities.Connection).StoreConnection.ConnectionString;
        //    using (var conn = new System.Data.SqlClient.SqlConnection(providerString))
        //    {
        //        SqlCommand cmd = new SqlCommand("usp_EmployeeDaysInMonth", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandTimeout = 0;
        //        cmd.Parameters.AddWithValue("@Month", month);
        //        cmd.Parameters.AddWithValue("@Year", year);
        //        conn.Open();
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = cmd;
        //        da.Fill(ds);
        //        conn.Close();
        //        if (ds.Tables.Count > 0)
        //        {
        //            var results = ds.Tables[0].AsEnumerable().Skip((pagingParams.CurrentPageNumber - 1) * pagingParams.PageSize).Take(pagingParams.PageSize);
        //            int TotalCount = results.Count();

        //            result = new { List=results,Total= TotalCount };
        //        }

        //    }
        //    return this.Response(Utilities.MessageTypes.Success, string.Empty, result);


        //}
        //[HttpGet]
        //public ApiResponse GetHRAttendanceByMonthYear(int month, int year)
        //{
        //    object result = null;
        //    DataSet ds = new DataSet("CalendarData");
        //    string providerString = ((EntityConnection)this.entities.Connection).StoreConnection.ConnectionString;
        //    using (var conn = new System.Data.SqlClient.SqlConnection(providerString))
        //    {
        //        SqlCommand cmd = new SqlCommand("usp_EmployeeDaysInMonth", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandTimeout = 0;
        //        cmd.Parameters.AddWithValue("@Month", month);
        //        cmd.Parameters.AddWithValue("@Year", year);
        //        conn.Open();
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = cmd;
        //        da.Fill(ds);
        //        conn.Close();
        //        if (ds.Tables.Count > 0)
        //        { result = ds.Tables[0]; }

        //    }
        //    return this.Response(Utilities.MessageTypes.Success, string.Empty, result);


        //}


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
