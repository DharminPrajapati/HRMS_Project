// -----------------------------------------------------------------------
// <copyright file="DesignationController.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Areas.Configuration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using MVCProject.Controllers;
    using MVCProject.Filters;
    using MVCProject.ViewModel;

    public class DesignationController : Controller
    {
        //
        // GET: /Configuration/Designation/
        public string ApiUrl = ConfigurationManager.AppSettings["apiurl"];
        // [WebAuthorizeAttribute(Page = Pages.General.Designation)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Download xls File 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ActionResult DownloadFile(string filename)
        {
            byte[] filedata = System.IO.File.ReadAllBytes(filename);
            string ext = Path.GetExtension(filename);
            string contentType = "application/" + ext;

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);
        }

    }
}
