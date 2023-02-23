using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.Configuration.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Configuration/Employee/

        public ActionResult Index()
        {
            return View();
        }

    }
}
