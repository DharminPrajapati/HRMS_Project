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
                userContext.RoleId = (int)emp.UserRole.FirstOrDefault().RoleId;
                userContext.Ticks = DateTime.Now.Ticks;
                userContext.TimeZoneMinutes = 330;
                userContext.Token = SecurityUtility.GetToken(userContext);

                //return this.Response(Utilities.MessageTypes.Success, string.Empty);

                return this.Response(Utilities.MessageTypes.Success, responseToReturn: userContext);

                //return new ApiResponse { Status = "Success", Message = "login sucessfull." };

            }
        }

    }
}



