
namespace MVCProject.Api.Controllers.Dashboard
{
    #region NameSapces
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.EntityClient;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    #endregion
    public class DashboardController : BaseController
    {
        private MVCProjectEntities entities;

        public DashboardController()
        {
            this.entities = new MVCProjectEntities();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse CountEmployees()
        {
            var records = this.entities.sp_hrms_countemployees();

            return this.Response(Utilities.MessageTypes.Success, string.Empty, records);
        }

        /// <returns></returns>
        [HttpGet]
        public ApiResponse GenderChart()
        {
            try
            {
                var records = this.entities.sp_hrms_genderchart();
                return this.Response(Utilities.MessageTypes.Success, string.Empty, records);
            }
            catch
            {
                return this.Response(Utilities.MessageTypes.Error);
            }
        }

        [HttpGet]
        public ApiResponse RoleChart()
        {
            try
            {
                var records = this.entities.sp_hrms_rolechart();
                return this.Response(Utilities.MessageTypes.Success, string.Empty, records);
            }
            catch
            {
                return this.Response(Utilities.MessageTypes.Error);
            }
        }
    }
}
