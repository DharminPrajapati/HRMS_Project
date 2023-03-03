using System.Web.Mvc;
using System.Web.Optimization;

namespace MVCProject.Web.Areas.Attendance
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
            bundles.Add(new ScriptBundle("~/bundles/Attendance/Attendance")
                .Include("~/Areas/Attendance/Scripts/angular/services/AttendanceService.js")
                .Include("~/Areas/Attendance/Scripts/angular/controllers/AttendanceCtrl.js"));
        }
    }
}
