using MVCProject.Web.Models;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.Salary.Controllers
{
    public class SalaryController : Controller
    {
        //
        // GET: /Salary/Salary/
        public string ApiUrl = ConfigurationManager.AppSettings["apiurl"];

        /// <summary>
        /// Add Salary Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string Id = Request.QueryString["Id"];
            if (Id != null)
            {
                ViewBag.Id = Id;
            }
            return View();
        }
        /// <summary>
        /// Payroll View
        /// </summary>
        /// <returns></returns>
        public ActionResult Payroll()
        {
            return View();
        }


        /// <summary>
        /// Display Salary Details
        /// </summary>
        /// <returns></returns>
        public ActionResult SalaryDetails()
        {
            return View();
        }

        /// <summary>
        /// Payslip  View Page
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ActionResult> Payslip(int employeeId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ApiUrl}Salary/GetEmployeeById?employeeId={employeeId}");
                if (response.IsSuccessStatusCode)
                {
                    var employeeData = await response.Content.ReadAsAsync<EmployeeData>();
                    return View(employeeData);
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
        }
        /// <summary>
        /// Open PaySlip
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ActionResult> _Payslip(int employeeId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ApiUrl}Salary/GetEmployeeById?employeeId={employeeId}");
                if (response.IsSuccessStatusCode)
                {
                    var employeeData = await response.Content.ReadAsAsync<EmployeeData>();
                    return View(employeeData);
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
        }

        /// <summary>
        /// Print Page
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public async Task<ActionResult> PrintAboutPage(int employeeId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ApiUrl}Salary/GetEmployeeById?employeeId={employeeId}");
                if (response.IsSuccessStatusCode)
                {
                    var employeeData = await response.Content.ReadAsAsync<EmployeeData>();
                    var report = new Rotativa.ViewAsPdf("_Payslip", employeeData);
                    return report;
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
        }

        /// <summary>
        /// Save Page
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveAboutPage(int employeeId)
        {
            string fileName = "Payslip.pdf";
            string virtualpath = "~/Reports/" + fileName;
            string phsicalpath = Server.MapPath(virtualpath);
            //string fullPath = "E:\\New folder\\HRMS_Project\\MVCProject.Api\\Reports\\" + fileName;
            var report = new Rotativa.ActionAsPdf("_Payslip", new { employeeId })
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A3,
                PageMargins = new Margins(0, 0, 0, 0),
            };
            if (!System.IO.File.Exists(phsicalpath))
            {
                var byteArray = report.BuildPdf(ControllerContext);
                var fileStream = new FileStream(phsicalpath, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();
            }
            return report;
            //return File(virtualpath, "application/pdf", fileName);
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
