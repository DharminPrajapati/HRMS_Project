using MVCProject.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.Attendance.Controllers
{
    public class AttendanceController : Controller
    {
        //
        // GET: /Attendance/Attendance/

        public ActionResult HRAttendanceView()
        {
            return View();
        }

        public ActionResult EmployeeAttendanceView()
        {
            return View();
        }

       
    }
}
