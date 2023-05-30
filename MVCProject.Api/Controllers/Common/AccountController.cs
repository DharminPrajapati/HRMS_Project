//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using MVCProject.Api.Models;
using MVCProject.Api.Utilities;
using MVCProject.Api.ViewModel;
using MVCProject.Common.Resources;
using System.Text;
using System.Net.Mail;

namespace MVCProject.Api.Controllers.Common
{
    public class AccountController : BaseController
    {
        private MVCProjectEntities entities;
        public AccountController()
        {
            this.entities = new MVCProjectEntities();
        }
        [HttpPost]
        public ApiResponse Login(UserMaster user)
        {
            var emp = entities.UserMaster.Where(u => u.UserName == user.UserName && u.UserPassword == user.UserPassword).FirstOrDefault();
            if (emp == null)
            {
                //return new ApiResponse { Status = "Error", Message = "Invalid username or password." };
                return this.Response(Utilities.MessageTypes.Warning, string.Format("Invalid username or password"));
            }
            else
            {
                UserContext userContext = new UserContext();
                userContext.EmployeeId = (int)emp.EmployeeId;
                userContext.UserId = emp.UserId;
                userContext.UserName = emp.UserName;
                userContext.EmployeeName = emp.TblEmployee.FirstName + " " + emp.TblEmployee.LastName;
                userContext.Designation = emp.TblEmployee.Designation.DesignationName.ToString();
                userContext.Department = emp.TblEmployee.TblDepartment.DepartmentName.ToString();

                userContext.RoleId = (int)emp.UserRole.FirstOrDefault().RoleId;
                userContext.Ticks = DateTime.Now.Ticks;
                userContext.TimeZoneMinutes = 330;
                userContext.Token = SecurityUtility.GetToken(userContext);

                //return this.Response(Utilities.MessageTypes.Success, string.Empty);

                return this.Response(Utilities.MessageTypes.Success, responseToReturn: userContext);

                //return new ApiResponse { Status = "Success", Message = "login sucessfull." };

            }
        }

        [HttpPost]
        public ApiResponse Logout()
        {
            UserContext.Token = null;
            return this.Response(Utilities.MessageTypes.Success);
        }

        [HttpPost]
        public ApiResponse SendResetPassword(UserMaster user)
        {
            var emp = entities.UserMaster.Where(u => u.UserName == user.UserName && u.UserPassword == user.UserPassword).FirstOrDefault();
            if (emp == null)
            {
                //return new ApiResponse { Status = "Error", Message = "Invalid username or password." };
                return this.Response(Utilities.MessageTypes.Warning, string.Format("Invalid username or password"));
            }
            else
            {
                UserContext userContext = new UserContext();

                userContext.UserId = emp.UserId;
                userContext.UserName = emp.UserName;

                return this.Response(Utilities.MessageTypes.Success, responseToReturn: userContext);
            }
        }

        [HttpPut]
        public ApiResponse ChangeRole(int RoleId)

        {
            UserContext.RoleId = RoleId;
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: "Updated");
         }

        [HttpGet]
        public ApiResponse getusersdetails(string user)
        {
            var data = this.entities.UserMaster
                .Where(x => x.IsActive.Value && x.UserName == user)
                .Select(x => new
                {
                    Id = x.EmployeeId,
                    UserName = x.UserName,
                    EmailId = x.EmployeeId > 0 ? this.entities.TblEmployees.FirstOrDefault(y => y.EmployeeId == x.EmployeeId).Email : string.Empty
                })
                .ToList()
                .Select(x => new
                {
                    Id = x.Id,
                    EmailId = MaskEmail(x.EmailId)
                })
                .ToList();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: data);
        }

        private string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return email;
            }

            var parts = email.Split('@');
            if (parts.Length != 2)
            {
                return email;
            }

            var maskedFirstPart = parts[0].Substring(0, 2) + new string('x', parts[0].Length - 4) + parts[0].Substring(parts[0].Length - 2);
            var maskedSecondPart = parts[1].Substring(0, 2) + new string('x', parts[1].Length - 4) + parts[1].Substring(parts[1].Length - 2);
            return maskedFirstPart + "@" + maskedSecondPart;
        }

        [HttpPost]
        public ApiResponse GetRandomString(int id)
        {
            int length = 6;
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }


            UserMaster userdetails = (from a in this.entities.UserMaster
                                      join b in this.entities.TblEmployees on a.EmployeeId equals b.EmployeeId
                                      where a.EmployeeId == id
                                      select a).SingleOrDefault();

            userdetails.SecurityCode = res.ToString();

            this.entities.SaveChanges();
            var EmployeeMasterList = (from s in this.entities.UserMaster
                                          .AsEnumerable()
                                      join c
                                      in this.entities.TblEmployees on s.EmployeeId equals c.EmployeeId
                                      let TotalRecords = this.entities.TblEmployees.AsEnumerable()
                                      where c.EmployeeId == id
                                      select new
                                      {
                                          EmployeeId = s.EmployeeId,
                                          EmpName = c.FirstName,
                                          EmailId = c.Email,
                                          UserName = s.UserName,
                                          SecurityCode = s.SecurityCode,
                                          TotalRecords,

                                      }).FirstOrDefault();
            var recipientEmail = EmployeeMasterList.EmailId;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                mail.To.Add(recipientEmail);
                mail.Subject = "Reset Password";
                mail.Body = $"<html>\r\n<head>\r\n    <style>\r\n        body {{\r\n            font-family: Arial, sans-serif;\r\n            font-size: 16px;\r\n            color: #333333;\r\n            line-height: 1.5;\r\n        }}\r\n        h1 {{\r\n            font-size: 24px;\r\n            color: #333333;\r\n            margin: 0;\r\n            padding: 0;\r\n        }}\r\n        p {{\r\n            margin-top: 0;\r\n            margin-bottom: 20px;\r\n        }}\r\n        .security-code {{\r\n            font-size: 18px;\r\n            font-weight: bold;\r\n            color: #007bff;\r\n            margin: 20px 0;\r\n            padding: 10px;\r\n            border: 2px solid #007bff;\r\n            border-radius: 4px;\r\n        }}\r\n        .contact {{\r\n            margin-top: 20px;\r\n            font-size: 14px;\r\n        }}\r\n        .contact a {{\r\n            color: #007bff;\r\n            text-decoration: none;\r\n        }}\r\n        .signature {{\r\n            margin-top: 40px;\r\n            font-size: 14px;\r\n            color: #666666;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <h1>Reset Your Password</h1>\r\n    <p>Dear [{EmployeeMasterList.UserName}],</p>\r\n    <p>You recently requested to reset your password for account. To complete the process, please use the following security code:</p>\r\n    <p class=\"security-code\">[{EmployeeMasterList.SecurityCode}]</p>\r\n    <p>Please note that this security code is valid for the next 10 minutes only. If you do not use it within this time, you will need to request a new security code.</p>\r\n    <p>If you did not request to reset your password, please contact our support team immediately at ASK EHS</a>.</p>\r\n    <div class=\"contact\">\r\n        <p>Thank you,</p>\r\n        <p>[HRMS] Team</p>\r\n     </div>\r\n    <div class=\"signature\">\r\n        <p>This email was sent to you by [ASK EHS].</p>\r\n   </div>\r\n</body>\r\n</html>";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("noreplyehstesting78@gmail.com", "cntpraqgsaqjlxpi");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: Resource.Success);
        }

        /// <summary>
        /// Verify Security Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse VerifyCode(UserMaster code)
        {
            var dbUser = entities.UserMaster.Where(u => u.SecurityCode == code.SecurityCode && u.EmployeeId == code.EmployeeId).FirstOrDefault();
            if (dbUser == null)
            {
                //return new ApiResponse { Status = "Error", Message = "Invalid Security Code" };
                return this.Response(Utilities.MessageTypes.Error, responseToReturn: "Invalid Security Code");
            }
            else
            {
                return this.Response(Utilities.MessageTypes.Success, responseToReturn: "Verify Successful");
            }

        }

        //update password
        [HttpPost]
        public ApiResponse UpdatePassword(UserMaster user)
        {
            UserMaster userdetails = this.entities.UserMaster
                .Where(u => u.EmployeeId == user.EmployeeId).FirstOrDefault();

            userdetails.UserPassword = user.UserPassword;

            this.entities.SaveChanges();
            return this.Response(Utilities.MessageTypes.Success, responseToReturn: "Upadated Successful");
        }

        /// <summary>
        /// Create Session
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse CreateSession(UserContext userContext)
        {
            // Store the user's information in the session
            HttpContext.Current.Session["UserId"] = userContext.UserId;
            HttpContext.Current.Session["UserName"] = userContext.UserName;
            HttpContext.Current.Session["IsAuthenticated"] = true;

            return this.Response(Utilities.MessageTypes.Success, responseToReturn: "Upadated Successful");
        }
    }
}



