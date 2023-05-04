

namespace MVCProject.Api.Controllers.Salary
{
    #region NameSapces
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web;
    using System.Web.Http;
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using NReco.PdfGenerator;
    #endregion
    public class SalaryController : BaseController
    {
        private MVCProjectEntities entities;

        public SalaryController()
        {
            this.entities = new MVCProjectEntities();
        }


        /// Get All Departments 
        [HttpGet]
        public ApiResponse GetEmployeeDropDown()
        {
            var data = this.entities.TblEmployees.Where(x => x.IsActive.Value).Select(x => new { EmpName = x.FirstName, EmpId = x.EmployeeId }).OrderBy(x => x.EmpName).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }

        [HttpGet]
        public ApiResponse GetDesignationDropDown()
        {
            var data = this.entities.Designation.Where(x => x.IsActive.Value).Select(x => new { Name = x.DesignationName, Id = x.DesignationId }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }

        /// Get All Departments 
        [HttpGet]
        public ApiResponse GetDepartmentDropDown()
        {
            var data = this.entities.TblDepartments.Where(x => x.IsActive.Value).Select(x => new { DeptName = x.DepartmentName, DeptId = x.DepartmentId }).OrderBy(x => x.DeptName).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }

        [HttpGet]

        public ApiResponse GetSalaryList(bool isGetAll = false)
        {
            var result = this.entities.AddSalary.Where(x => (isGetAll || x.IsActive.Value)).Select(x => new { Id = x.SalaryId }).OrderBy(e => e.Id).ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, result);
        }



        /// <summary>
        /// Get Employees By Id
        /// </summary>
        [HttpGet]

        public ApiResponse GetSalaryById(int salaryId)
        {
            var AllowanceAmount = this.entities.SalaryAllowance
            .Where(x => x.SalaryId == salaryId)
            .Select(x => new { AllowanceAmount = x.AllowanceAmount == null ? 0 : x.AllowanceAmount, SalaryAllowanceId = x.AllowanceAmount == null ? 0 : x.SalaryAllowanceId })
            .ToList();
            var DeductionAmount = this.entities.SalaryDeduction
           .Where(x => x.SalaryId == salaryId)
           .Select(x => new { DeductionAmount = x.DeductionAmount == null ? 0 : x.DeductionAmount, SalaryDeductionId = x.DeductionAmount == null ? 0 : x.SalaryDeductionId })
           .ToList();
            var salaryDetail = this.entities.Sp_Salary_DisplayAllEmployees().Where(x => x.SalaryId == salaryId)
                .Select(d => new
                {
                    SalaryId = d.SalaryId,
                    EmployeeId = d.EmployeeId,
                    Name = d.FirstName + ' ' + d.LastName,
                    DepartmentId = d.DepartmentId,
                    DesignationId = d.DepartmentId,
                    DesignationName = d.DesignationName,
                    DepartmentName = d.DepartmentName,
                    BasicSalary = d.BasicSalary,
                    DA = d.DA,
                    HRA = d.HRA,
                    PF = d.PF,
                    DAamt = d.DAamt,
                    HRAamt = d.HRAamt,
                    PFamt = d.PFamt,
                    netSalary = d.netSalary,
                    IsActive = d.IsActive,
                    TotalAllowance = d.TotalAllowance,
                    TotalDeductoin = d.TotalDeductoin,
                    AllowanceAmount,
                    DeductionAmount
                }).SingleOrDefault();

            if (salaryDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, salaryDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        /// <summary>
        /// Get Employees By Id
        /// </summary>
        [HttpGet]

        public ApiResponse GetEmployeeById(int employeeId)
        {
            var AllowanceAmounts = this.entities.SalaryAllowance
           .Where(x => x.EmployeeId == employeeId)
           .Select(x => new { EmployeeId = x.EmployeeId, AllowanceAmount = x.AllowanceAmount == null ? 0 : x.AllowanceAmount, SalaryAllowanceId = x.AllowanceAmount == null ? 0 : x.SalaryAllowanceId })
           .ToList();
            var DeductionAmounts = this.entities.SalaryDeduction
           .Where(x => x.EmployeeId == employeeId)
           .Select(x => new { EmployeeId = x.EmployeeId, DeductionAmount = x.DeductionAmount == null ? 0 : x.DeductionAmount, SalaryDeductionId = x.DeductionAmount == null ? 0 : x.SalaryDeductionId })
           .ToList();
            var Allowances = this.entities.AllowanceMaster.Select(x => new { Description = x.Description, Value = x.Value }).ToList();
            var Deductions = this.entities.DeductionMaster.Select(x => new { Description = x.Description, Value = x.Value }).ToList();
            var salaryDetail = this.entities.Sp_Salary_DisplayAllEmployees().Where(x => x.EmployeeId == employeeId)
                .Select(d => new
                {
                    SalaryId = d.SalaryId,
                    EmployeeId = d.EmployeeId,
                    BatchNo = d.BatchNo,
                    Name = d.FirstName + ' ' + d.LastName,
                    DepartmentId = d.DepartmentId,
                    DesignationId = d.DepartmentId,
                    DesignationName = d.DesignationName,
                    DepartmentName = d.DepartmentName,
                    BasicSalary = d.BasicSalary,
                    //DA = d.DA,
                    //HRA = d.HRA,
                    //PF = d.PF,
                    //DAamt = d.DAamt,
                    //HRAamt = d.HRAamt,
                    //PFamt = d.PFamt,
                    netSalary = d.netSalary,
                    IsActive = d.IsActive,
                    TotalAllowance = d.TotalAllowance,
                    TotalDeductoin = d.TotalDeductoin,
                    Allowances,
                    Deductions,
                    AllowanceAmounts,
                    DeductionAmounts
                }).SingleOrDefault();

            if (salaryDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, salaryDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }


        ///Get All Salary Details
        [HttpPost]
        public ApiResponse GetAllSalary(PagingParams salaryDetailsParams)
        {
            if (string.IsNullOrWhiteSpace(salaryDetailsParams.Search))
            {
                salaryDetailsParams.Search = string.Empty;
            }

            var salarylist = (from d in this.entities.AddSalary.AsEnumerable()
                              let TotalRecords = this.entities.AddSalary.AsEnumerable().Count()
                              select new
                              {
                                  //var employeelist = this.entities.TblEmployee.ToList();
                                  SalaryId = d.SalaryId,
                                  EmployeeId = d.EmployeeId,
                                  BasicSalary = d.BasicSalary,
                                  DA = d.DA,
                                  HRA = d.HRA,
                                  PF = d.PF,
                                  IsActive = d.IsActive,
                                  TotalRecords
                              }).AsQueryable().Skip((salaryDetailsParams.CurrentPageNumber - 1) * salaryDetailsParams.PageSize).Take(salaryDetailsParams.PageSize);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, salarylist);

        }

        [HttpPost]
        public ApiResponse SaveSalaryDetails(AddSalary SalaryDetail)
        {

            AddSalary existingSalaryDetail = this.entities.AddSalary.Where(x => x.SalaryId == SalaryDetail.SalaryId).FirstOrDefault();
            if (existingSalaryDetail == null)
            {
                this.entities.AddSalary.AddObject(SalaryDetail);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Salary));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Salary), SalaryDetail.SalaryId);
            }

            // For Update

            else
            {

                existingSalaryDetail.EmployeeId = SalaryDetail.EmployeeId;
                existingSalaryDetail.BasicSalary = SalaryDetail.BasicSalary;
                existingSalaryDetail.DA = SalaryDetail.DA;
                existingSalaryDetail.HRA = SalaryDetail.HRA;
                existingSalaryDetail.PF = SalaryDetail.PF;
                existingSalaryDetail.DAamt = SalaryDetail.DAamt;
                existingSalaryDetail.HRAamt = SalaryDetail.HRAamt;
                existingSalaryDetail.PFamt = SalaryDetail.PFamt;
                existingSalaryDetail.netSalary = SalaryDetail.netSalary;
                existingSalaryDetail.IsActive = SalaryDetail.IsActive;
                existingSalaryDetail.TotalAllowance = SalaryDetail.TotalAllowance;
                existingSalaryDetail.TotalDeductoin = SalaryDetail.TotalDeductoin;


                this.entities.AddSalary.ApplyCurrentValues(existingSalaryDetail);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Salary));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Salary));
            }

        }

        [HttpPost]
        public ApiResponse SaveAllowanceDetails([FromBody] List<SalaryAllowance> SalaryDetail)
        {
            foreach (var allow in SalaryDetail)
            {
                SalaryAllowance existingSalaryDetail = this.entities.SalaryAllowance.Where(x => x.SalaryAllowanceId == allow.SalaryAllowanceId).FirstOrDefault();
                if (existingSalaryDetail == null)
                {
                    this.entities.SalaryAllowance.AddObject(allow);
                }

                // For Update

                else
                {
                    existingSalaryDetail.AllowanceAmount = allow.AllowanceAmount;
                    this.entities.SalaryAllowance.ApplyCurrentValues(existingSalaryDetail);
                }
            }
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Salary));
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Salary));
        }

        [HttpPost]
        public ApiResponse SaveDeductionDetails([FromBody] List<SalaryDeduction> SalaryDetail)
        {
            foreach (var allow in SalaryDetail)
            {
                SalaryDeduction existingSalaryDetail = this.entities.SalaryDeduction.Where(x => x.SalaryDeductionId == allow.SalaryDeductionId).FirstOrDefault();
                if (existingSalaryDetail == null)
                {
                    this.entities.SalaryDeduction.AddObject(allow);
                }

                // For Update

                else
                {
                    existingSalaryDetail.DeductionAmount = allow.DeductionAmount;
                    this.entities.SalaryDeduction.ApplyCurrentValues(existingSalaryDetail);
                }
            }
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Salary));
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Salary));
        }










        [HttpPost]
        public ApiResponse GetEmployeeSalary(PagingParams salaryDetailsParams)
        {
            if (string.IsNullOrWhiteSpace(salaryDetailsParams.Search))
            {
                salaryDetailsParams.Search = string.Empty;
            }
            var result = this.entities.Sp_Salary_DisplayAllEmployees().Where(x => x.FirstName.Trim().ToLower().Contains(salaryDetailsParams.Search.Trim().ToLower())).AsQueryable().Skip((salaryDetailsParams.CurrentPageNumber - 1) * salaryDetailsParams.PageSize).Take(salaryDetailsParams.PageSize);
            var TotalRecords = this.entities.Sp_Salary_DisplayAllEmployees().Where(x => x.FirstName.Trim().ToLower().Contains(salaryDetailsParams.Search.Trim().ToLower())).AsQueryable().Count();

            return this.Response(Utilities.MessageTypes.Success, string.Empty, new { list = result, Total = TotalRecords });

        }


        [HttpGet]
        public ApiResponse GetFullName(bool isActive, string searchText)
        {
            var data = this.entities.TblEmployees.Where(x => x.IsActive.Value == isActive && x.FirstName.Contains(searchText)).Select(x => new { Name = x.FirstName + " " + x.LastName, Id = x.EmployeeId, DepartmentId = x.DepartmentId, DepartmentName = this.entities.TblDepartments.FirstOrDefault(d => d.DepartmentId == x.DepartmentId).DepartmentName, DesignationName = this.entities.Designation.FirstOrDefault(d => d.DesignationId == x.DesignationId).DesignationName }).OrderBy(x => x.Name).ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);

        }

        [HttpGet]
        public ApiResponse GetDeptDesiByEmployeeId(int Id)
        {
            object Employee = null;
            if (this.entities.AddSalary.Any(x => x.EmployeeId == Id))
            {
                Employee = GetSalaryById(this.entities.AddSalary.FirstOrDefault(x => x.EmployeeId == Id).SalaryId).Result;
            }
            if (Employee != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, Employee);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        [HttpGet]

        public ApiResponse CreateEmployeeListReport()
        {
            var employeeDetail = this.entities.Sp_Salary_DisplayAllEmployees().Select(d => new
            {
                SalaryId = d.SalaryId,
                EmployeeId = d.EmployeeId,
                Name = d.FirstName + ' ' + d.LastName,
                DepartmentId = d.DepartmentId,
                DesignationId = d.DepartmentId,
                DesignationName = d.DesignationName,
                DepartmentName = d.DepartmentName,
                BasicSalary = d.BasicSalary,
                DAamt = d.DAamt,
                HRAamt = d.HRAamt,
                PFamt = d.PFamt,
                netSalary = d.netSalary,
                IsActive = d.IsActive != null ? d.IsActive == true ? "Active" : "InActive" : string.Empty
            }).ToList();

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");



            // Create a cell style with a background color
            ICellStyle headerCellStyle = workbook.CreateCellStyle();
            headerCellStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            headerCellStyle.FillPattern = FillPattern.SolidForeground;


            // Add Some Data to Sheet
            // 
            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Employee Id");
            headerRow.CreateCell(1).SetCellValue("Employee Name");
            headerRow.CreateCell(2).SetCellValue("Designation Name");
            headerRow.CreateCell(3).SetCellValue("Department Name");
            headerRow.CreateCell(4).SetCellValue("Basic Salary");
            headerRow.CreateCell(5).SetCellValue("DA");
            headerRow.CreateCell(6).SetCellValue("HRA");
            headerRow.CreateCell(7).SetCellValue("PF");
            headerRow.CreateCell(8).SetCellValue("Net Salary");
            headerRow.CreateCell(9).SetCellValue("IsActive");

            // Set the cell style for the header row
            foreach (var cell in headerRow.Cells)
            {
                cell.CellStyle = headerCellStyle;
            }

            int rowNumber = 1;
            foreach (var emp in employeeDetail)
            {
                IRow row = sheet.CreateRow(rowNumber++);
                row.CreateCell(0).SetCellValue((double)emp.EmployeeId);
                row.CreateCell(1).SetCellValue(emp.Name);
                row.CreateCell(2).SetCellValue(emp.DesignationName);
                row.CreateCell(3).SetCellValue(emp.DepartmentName);
                row.CreateCell(4).SetCellValue((double)emp.BasicSalary);
                row.CreateCell(5).SetCellValue(emp.DAamt);
                row.CreateCell(6).SetCellValue(emp.HRAamt);
                row.CreateCell(7).SetCellValue(emp.PFamt);
                row.CreateCell(8).SetCellValue((double)emp.netSalary);
                row.CreateCell(9).SetCellValue(emp.IsActive);
            }

            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            string filePath = HttpContext.Current.Server.MapPath("~/Reports/SalaryDetails.xlsx");
            string fileName = Path.GetFileName(filePath);
            //if (File.Exists(filePath))
            //{
            //    File.Delete(filePath);
            //}
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            workbook.Write(fileStream);
            var memorystream = new MemoryStream();
            var byteArray = memorystream.ToArray();
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = new ByteArrayContent(byteArray);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "MyExcelFile__Salary_Details.xlsx";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentLength = byteArray.Length;
            Console.WriteLine(response);

            return this.Response(Utilities.MessageTypes.Success, string.Empty, filePath);
        }


        [System.Web.Http.HttpGet]
        public ApiResponse GeneratePaySlipByEmployeeId(int employeeId)
        {
            var employeeDetails = this.entities.Sp_Salary_DisplayAllEmployees().Where(d => d.EmployeeId == employeeId).ToList().Select(d => new
            {
                SalaryId = d.SalaryId,
                EmployeeId = d.EmployeeId,
                Name = d.FirstName + ' ' + d.LastName,
                DepartmentId = d.DepartmentId,
                DesignationId = d.DepartmentId,
                DesignationName = d.DesignationName,
                DepartmentName = d.DepartmentName,
                BasicSalary = d.BasicSalary,
                DAamt = d.DAamt,
                HRAamt = d.HRAamt,
                PFamt = d.PFamt,
                netSalary = d.netSalary,
                IsActive = d.IsActive != null ? d.IsActive == true ? "Active" : "InActive" : string.Empty
            }).FirstOrDefault();

            var pdfGenerator = new HtmlToPdfConverter();
            pdfGenerator.CustomWkHtmlArgs = "--page-size A4";
            pdfGenerator.Orientation = PageOrientation.Portrait;

            var Payslip = this.entities.Sp_Salary_DisplayAllEmployees().FirstOrDefault(x => x.EmployeeId == employeeId);

            // Html Pdf Generator

            var htmlContent = $@"
                    <html>
                    <body>
                    <div class=""page - wrapper"">


                    <div class=""page-header"">
                    <div class=""row align-items-center"">
                    <div class=""col"">
                    <h3 class=""page-title"">Payslip</h3>
                    <ul class=""breadcrumb"">
                
                    </ul>
                    </div>
                    </div>

                    <div class=""row"">
                    <div class=""col-md-12"">
                    <div class=""card"">
                    <div class=""card-body"">
                    <h4 class=""payslip-title"">Payslip for the month of Feb 2019</h4>
                    <div class=""row"">
                    <div class=""col-sm-6 m-b-20"">
                    <img src = ""E:\HRMS_Project\MVCProject.Web\Content\images\company_logo.png"" class="""" alt="""">
                    <ul class=""list-unstyled mb-0"">
                    <li>Dreamguy's Technologies</li>
                    <li>3864 Quiet Valley Lane,</li>
                    <li>Sherman Oaks, CA, 91403</li>
                    </ul>
                    </div>
                    <div class=""col-sm-6 m-b-20"">
                    <div class=""invoice-details"">
                    <h3 class=""text-uppercase"">Payslip #49029</h3>
                    <ul class=""list-unstyled"">
                    <li>Salary Month: <span>March, 2019</span></li>
                    </ul>
                    </div>
                    </div>
                    </div>
                    <div class=""row"">
                    <div class=""col-lg-12 m-b-20"">
                    <ul class=""list-unstyled"">
                    <li><h5 class=""mb-0""><strong>{Payslip.FirstName + ' ' + Payslip.LastName}</strong></h5></li>
                    <li><span>{Payslip.DepartmentName}</span></li>
                    <li>Employee ID: {Payslip.EmployeeId}</li>
                    <li>Joining Date: {Payslip.EntryDate}</li>
                    </ul>
                    </div>
                    </div>
                    <div class=""row"">
                    <div class=""col-sm-6"">
                    <div>
                    <h4 class=""m-b-10""><strong>Earnings</strong></h4>
                    <table class=""table table-bordered"">
                    <tbody>
                    <tr>
                    <td><strong>Basic Salary</strong> <span class=""float-end"">&#8377;{Payslip.BasicSalary}</span></td>
                    </tr>
                    <tr>
                    <td><strong>House Rent Allowance(H.R.A.)</strong> <span class=""float-end"">&#8377;{Payslip.HRAamt}</span></td>
                    </tr>
                    <tr>
                    <td><strong>Conveyance</strong> <span class=""float-end"">$55</span></td>
                    </tr>
                    <tr>
                    <td><strong>Other Allowance</strong> <span class=""float-end"">0</span></td>
                    </tr>
                    <tr>
                    <td><strong>Total Earnings</strong> <span class=""float-end""><strong></strong></span></td>
                    </tr>
                    </tbody>
                    </table>
                    </div>
                    </div>
                    <div class=""col-sm-6"">
                    <div>
                    <h4 class=""m-b-10""><strong>Deductions</strong></h4>
                    <table class=""table table-bordered"">
                    <tbody>
                     <tr>
                    <td><strong>Provident Fund </strong> <span class=""float-end"">&#8377;{Payslip.PFamt}</span></td>
                    </tr>             
                    <tr>
                    <td><strong>Loan</strong> <span class=""float-end"">0</span></td>
                    </tr>
                    <tr>
                    <td><strong>Total Deductions</strong> <span class=""float-end""><strong></strong></span></td>
                    </tr>
                    </tbody>
                    </table>
                    </div>
                    </div>
                    <div class=""col-sm-12"">
                    <p><strong>Net Salary: &#8377;{Payslip.netSalary} </strong></p>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>
                    </body>
                    </html>";
            var pdfBytes = pdfGenerator.GeneratePdf(htmlContent);


            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var pdfFilePath = Path.Combine(documentsFolder, Payslip.EmployeeId + "_" + Payslip.FirstName + "_" + Payslip.LastName + "_Payslip.pdf");
            System.IO.File.WriteAllBytes(pdfFilePath, pdfBytes);
            if (employeeDetails != null)
            {

                return this.Response(Utilities.MessageTypes.Success, string.Empty, employeeDetails);
            }

            return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.entities != null)
                {
                    this.entities.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
