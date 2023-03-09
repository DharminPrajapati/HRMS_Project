
namespace MVCProject.Api.Controllers.Attendance
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
    public class AttendanceController : BaseController
    {
        private MVCProjectEntities entities;

        public AttendanceController()
        {
            this.entities = new MVCProjectEntities();
        }
        [HttpPost]
        public ApiResponse SaveAttendanceDetails(Attendance AttendanceDetail)
        {
            if (this.entities.Attendance.Any(x => x.AttendanceId == AttendanceDetail.AttendanceId))
            {
                return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.Attendance));
            }
            else
            {
                Attendance existingAttendanceDetail = this.entities.Attendance.Where(x => x.AttendanceId == AttendanceDetail.AttendanceId).FirstOrDefault();
                if (existingAttendanceDetail == null)
                {
                    this.entities.Attendance.AddObject(AttendanceDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Attendance));
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Attendance));
                }

                // For Update

                else
                {
                    existingAttendanceDetail.Date = AttendanceDetail.Date;
                    existingAttendanceDetail.StartingTime = AttendanceDetail.StartingTime;
                    existingAttendanceDetail.EndingTime = AttendanceDetail.EndingTime;
                    existingAttendanceDetail.StartingLatitude = AttendanceDetail.StartingLatitude;
                    existingAttendanceDetail.EndingLatitude = AttendanceDetail.EndingLatitude;
                    existingAttendanceDetail.StartingLongitude = AttendanceDetail.StartingLongitude;
                    existingAttendanceDetail.EndingLongitude = AttendanceDetail.EndingLongitude;
                    existingAttendanceDetail.IsActive = AttendanceDetail.IsActive;
                    existingAttendanceDetail.Discription = AttendanceDetail.Discription;


                    this.entities.Attendance.ApplyCurrentValues(existingAttendanceDetail);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Attendance));
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Attendance));
                }

            }
        }



        [HttpGet]
        public ApiResponse GetAllAttendance()
        {
            //var employeelist = this.entities.TblEmployee.ToList();
            var Attendanelist = this.entities.Attendance.Select(d => new {
                Attendance = d.AttendanceId,
                Date = d.Date,
                StartingTime = d.StartingTime,
                EndingTime = d.EndingTime,
                StartingLatitude = d.StartingLatitude,
                EndingLatitude = d.EndingLatitude,
                StartingLongitude = d.StartingLongitude,
                IsActive = d.IsActive,
                Discription = d.Discription
            });

            return this.Response(Utilities.MessageTypes.Success, string.Empty, Attendanelist);

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
