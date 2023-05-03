using MVCProject.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.Dashboard.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/Dashboard/

        public ActionResult Index()
        {
            return View();
        }

    }
}
