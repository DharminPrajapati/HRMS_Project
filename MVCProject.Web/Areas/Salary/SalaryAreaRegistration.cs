using System.Web.Mvc;
using System.Web.Optimization;

namespace MVCProject.Areas.Salary
{
    public class SalaryAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Salary";
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
                "Salary_default",
                "Salary/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
        private void RegisterBundles(BundleCollection bundles)
        {
            // Employee 
            bundles.Add(new ScriptBundle("~/bundles/Salary/Salary")
                .Include("~/Areas/Salary/Scripts/angular/services/SalaryService.js")
                .Include("~/Areas/Salary/Scripts/angular/controllers/SalaryCtrl.js"));
        }
    }
}
