using System.Web.Mvc;
using System.Web.Optimization;

namespace MVCProject.Areas.Doument
{
    public class DoumentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Doument";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            this.RegisterRoutes(context);
            this.RegisterBundles(BundleTable.Bundles);
        }
        public  void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Doument_default",
                "Doument/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

        private void RegisterBundles(BundleCollection bundles)
        {
            // Employee 
            bundles.Add(new ScriptBundle("~/bundles/Document/Document")
                .Include("~/Areas/Document/Scripts/angular/services/DocumentCtrl.js")
                .Include("~/Areas/Document/Scripts/angular/controllers/DocumentService.js"));
        }
    }
}