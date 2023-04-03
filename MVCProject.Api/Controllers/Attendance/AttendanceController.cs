
namespace MVCProject.Api.Controllers.Attendance
{
    #region NameSapces
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.EntityClient;
    using System.Data.SqlClient;
    using System.Globalization;
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
                AttendanceDetail.Date = DateTime.Now;
                TimeSpan intime = TimeSpan.ParseExact(DateTime.Now.ToString("hh:mm:ss"), "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
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
            Attendance existingAttendanceDetail = this.entities.Attendance.Where(x => x.EmployeeId == AttendanceDetail.EmployeeId).FirstOrDefault();

            TimeSpan outtime = TimeSpan.ParseExact(DateTime.Now.ToString("hh:mm:ss"), "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
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

        [HttpGet]
        public ApiResponse GetAllAttendance()
        {
            //var employeelist = this.entities.TblEmployee.ToList();
            var Attendanelist = this.entities.Attendance.Select(d => new {
                AttendanceId = d.AttendanceId,
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


        [HttpGet]
        public ApiResponse AttendanceStatus(int employeeId)
        {
            var data = this.entities.Attendance.Where(x => x.EmployeeId == employeeId && x.Date == DateTime.Today).Any();
            if (data == true)
            {
                return this.Response(Utilities.MessageTypes.Success, responseToReturn: true);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.Success, responseToReturn: false);
            }
        }

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
