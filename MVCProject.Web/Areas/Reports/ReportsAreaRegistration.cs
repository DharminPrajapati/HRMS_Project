using System.Web.Mvc;
using System.Web.Optimization;

namespace MVCProject.Areas.Reports
{
    public class ReportsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Reports";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            this.RegisterRoutes(context);
            this.RegisterBundles(BundleTable.Bundles);
        }

        public void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Reports_default",
                "Reports/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
        private void RegisterBundles(BundleCollection bundles)
        {
            // Employee 
            bundles.Add(new ScriptBundle("~/bundles/Reports/Reports")
                .Include("~/Areas/Reports/Scripts/angular/services/ReportsService.js")
              .Include("~/Areas/Reports/Scripts/angular/controllers/ReportsCtrl.js"));

            // Employee 
            bundles.Add(new ScriptBundle("~/bundles/EmployeeManagement/Employee")
                .Include("~/Areas/EmployeeManagement/Scripts/angular/services/EmployeeService.js")
                .Include("~/Areas/EmployeeManagement/Scripts/angular/controllers/EmployeeCtrl.js"));
        }

    }
}
