
namespace MVCProject.Areas.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.IO;
    using System.Configuration;
    using System.Web.Mvc;
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/Reports/

        public ActionResult Index()
        {
            return View();
        }
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

        public ActionResult DownloadPDF(string filename)
        {
            byte[] filedata = System.IO.File.ReadAllBytes(filename);
            string ext = Path.GetExtension(filename);
            string contentType = "application/" + ext;

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);
        }
    }
}

