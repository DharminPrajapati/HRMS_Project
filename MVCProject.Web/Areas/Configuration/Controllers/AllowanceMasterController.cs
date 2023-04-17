using MVCProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.Configuration.Controllers
{
    public class AllowanceMasterController : BaseController
    {
        //
        // GET: /Configuration/AllowanceMaster/

        public ActionResult Index()
        {
            return View();
        }

    }
}
