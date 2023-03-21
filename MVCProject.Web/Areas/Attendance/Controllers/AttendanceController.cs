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

        public JsonResult GetEvents()  
        {  
            using (MVCProjectEntities dc = new MVCProjectEntities())  
            {  
                var v = dc.Attendance.OrderBy(a => a.InTime).ToList();  
                return new JsonResult { Data = v, JsonRequestBehavior = JsonRequestBehavior.AllowGet };  
            }  
        }   
    }
}
