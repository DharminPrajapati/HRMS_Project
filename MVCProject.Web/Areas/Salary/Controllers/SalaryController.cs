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
        public ActionResult Index()
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
                var response = await client.GetAsync($"http://localhost:56562/api/Salary/GetEmployeeById?employeeId={employeeId}");
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
                var response = await client.GetAsync($"http://localhost:56562/api/Salary/GetEmployeeById?employeeId={employeeId}");
                if (response.IsSuccessStatusCode)
                {
                    var employeeData = await response.Content.ReadAsAsync<EmployeeData>();
                    var report = new Rotativa.ViewAsPdf("Payslip", employeeData);
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
            string fullPath = "E:\\New folder\\HRMS_Project\\MVCProject.Api\\Reports\\" + fileName;
            var report = new Rotativa.ActionAsPdf("Payslip", new { employeeId })
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(0, 0, 0, 0),
            };
            if (!System.IO.File.Exists(fullPath))
            {
                var byteArray = report.BuildPdf(ControllerContext);
                var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();
            }
            return report;

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
