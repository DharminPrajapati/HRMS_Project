using System.Web.Mvc;
using System.Web.Optimization;

namespace MVCProject.Areas.EmployeeManagement
{
    public class EmployeeManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "EmployeeManagement";
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
                    "EmployeeManagement_default",
                    "EmployeeManagement/{controller}/{action}/{id}",
                    new { action = "Index", id = UrlParameter.Optional }
                );
        }

        private void RegisterBundles(BundleCollection bundles)
        {
            // Employee 
            bundles.Add(new ScriptBundle("~/bundles/EmployeeManagement/Employee")
                .Include("~/Areas/EmployeeManagement/Scripts/angular/services/EmployeeService.js")
                .Include("~/Areas/EmployeeManagement/Scripts/angular/controllers/EmployeeCtrl.js"));
            //SearchEmployee
            bundles.Add(new ScriptBundle("~/bundles/EmployeeManagement/SearchEmployee")
                .Include("~/Areas/EmployeeManagement/Scripts/angular/services/SearchEmployeeService.js")
                .Include("~/Areas/EmployeeManagement/Scripts/angular/controllers/SearchEmployeeCtrl.js"));
        }
    }
}
