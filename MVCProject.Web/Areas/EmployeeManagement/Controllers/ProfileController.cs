﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.EmployeeManagement.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /EmployeeManagement/Profile/

        public ActionResult Index()
        {
            return View();
        }

    }
}
