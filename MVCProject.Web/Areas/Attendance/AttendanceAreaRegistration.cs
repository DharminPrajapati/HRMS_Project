using System.Web.Mvc;
using System.Web.Optimization;

namespace MVCProject.Areas.Attendance
{
    public class AttendanceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Attendance";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            this.RegisterRoutes(context);
            this.RegisterBundles(BundleTable.Bundles);
            
        }
        private void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                 "Attendance_default",
                 "Attendance/{controller}/{action}/{id}",
                 new { action = "Index", id = UrlParameter.Optional }
             );
        }

        private void RegisterBundles(BundleCollection bundles)
        {
            // Attendance
            bundles.Add(new ScriptBundle("~/bundles/Attendance/Attendance/EmployeeAttendanceView")
                .Include("~/Areas/Attendance/Scripts/angular/services/AttendanceService.js")
                .Include("~/Areas/Attendance/Scripts/angular/controllers/AttendanceCtrl.js")
                .Include("~/Scripts/js/calendar.js")
                .Include("~/Scripts/js/fullcalendar.js")
                .Include("~/Scripts/js/gcal.js")
                .Include("~/Areas/Attendance/Scripts/angular/controllers/calendarDemo.js"));

            // HR Attendance View
            bundles.Add(new ScriptBundle("~/bundles/Attendance/Attendance/HRAttendanceView")
                .Include("~/Areas/Attendance/Scripts/angular/services/AttendanceService.js")
                .Include("~/Areas/Attendance/Scripts/angular/controllers/AttendanceCtrl.js"));
        }
    }
}
