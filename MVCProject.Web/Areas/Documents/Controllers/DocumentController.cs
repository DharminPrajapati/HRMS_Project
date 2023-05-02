using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.Documents.Controllers
{
    public class DocumentController : Controller
    {
        //
        // GET: /Documents/Document/

        public ActionResult Index()
        {
            return View();
        }

    }
}
