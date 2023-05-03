using System.Web.Mvc;
using System.Web.Optimization;

namespace MVCProject.Areas.Dashboard
{
    public class DashboardAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Dashboard";
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
                "Dashboard_default",
                "Dashboard/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

        private void RegisterBundles(BundleCollection bundles)
        {
            // Dashboard 
            bundles.Add(new ScriptBundle("~/bundles/Dashboard/Dashboard/Index")
                .Include("~/Areas/Dashboard/Scripts/angular/services/DashboardService.js")
                .Include("~/Areas/Dashboard/Scripts/angular/controllers/DashboardCtrl.js"));


        }
    }
}
