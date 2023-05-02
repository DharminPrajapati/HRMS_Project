using System.Web.Mvc;
using System.Web.Optimization;

namespace MVCProject.Areas.Documents
{
    public class DocumentsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Documents";
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
                 "Documents_default",
                 "Documents/{controller}/{action}/{id}",
                 new { action = "Index", id = UrlParameter.Optional }
             );
        }
        private void RegisterBundles(BundleCollection bundles)
        {
            // Employee 
            bundles.Add(new ScriptBundle("~/bundles/Documents/Document")
                .Include("~/Areas/Documents/Scripts/angular/services/DocumentService.js")
                .Include("~/Areas/Documents/Scripts/angular/controllers/DocumentCtrl.js"));
        }
    }
}
