using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using MVCProject.Controllers;
using MVCProject.Filters;
using MVCProject.ViewModel;

namespace MVCProject.Areas.Configuration.Controllers
{
    public class UserMasterController : Controller
    {
        public string ApiUrl = ConfigurationManager.AppSettings["apiurl"];
        // GET: /Configuration/UserMaster/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DownloadFile(string filename)
        {
            //string filename = "File.pdf";
            //string filepath = ApiUrl+ "/Reports/" + filename;
            byte[] filedata = System.IO.File.ReadAllBytes(filename);
            string ext = Path.GetExtension(filename);
            string contentType = "application/" + ext;
            //string contentType = System.Web.Mim.GetMimeMapping(filepath);

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
