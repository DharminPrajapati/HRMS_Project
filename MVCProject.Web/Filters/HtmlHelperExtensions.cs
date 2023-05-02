// -----------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace System.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using MVCProject;
    using MVCProject.ViewModel;
    using System.Web.Routing;
    using MVCProject.Common.Resources;

    /// <summary>
    /// Html Helper Extensions
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Get HTML Menu
        /// </summary>
        /// <param name="helper">Html Helper</param>
        /// <param name="permission">Page Permission</param>
        /// <returns>MVC Html String</returns>
        public static MvcHtmlString Menu(this HtmlHelper helper, UserContext userContext)
        {
            //if (permission != null)
            //{
                StringBuilder menuString = new StringBuilder();
                menuString.Append("<div id='sidebar-menu' class='main_menu_side hidden-print main_menu'>");
                menuString.Append("<div class='menu_section'>");
                menuString.Append("<ul class='nav side-menu'>");
            if (userContext.RoleId == 1)
            {


                menuString.Append("<li>");
                menuString.AppendFormat(GetParentMenu(Resource.Configuration, "fa fa-gears"));
                menuString.Append("<ul class='nav child_menu'>");

                menuString.Append(GetSubMenu(Resource.Designations, "", "/Configuration/Designation"));
                menuString.Append(GetSubMenu(Resource.Department, "", "/Configuration/Department"));
                //menuString.Append(GetSubMenu(Resource.Salary, "", "/Configuration/Salary"));
                menuString.Append(GetSubMenu(Resource.User_Master, "", "/Configuration/UserMaster"));
                menuString.Append(GetSubMenu(Resource.DocumentMaster, "", "/Configuration/DocumentMaster"));
                // menuString.Append(GetSubMenu(Resource.Allowance_Master, "", "/Configuration/AllowanceMaster"));
                //menuString.Append(GetSubMenu(Resource.DeductionMaster, "", "/Configuration/DeductionMaster"));
                menuString.Append(GetSubMenu(Resource.Company_Master, "", "/Configuration/CompanyMaster"));
                menuString.Append("</ul></li>");

                menuString.Append("<li>");
                menuString.AppendFormat(GetParentMenu(Resource.Salary_Configuration, "fa fa-gears"));
                menuString.Append("<ul class='nav child_menu'>");
                //menuString.Append(GetSubMenu(Resource.Salary, "", "/Configuration/Salary"));
                menuString.Append(GetSubMenu(Resource.Allowance_Master, "", "/Configuration/AllowanceMaster"));
                menuString.Append(GetSubMenu(Resource.Deduction_Master, "", "/Configuration/DeductionMaster"));
                menuString.Append("</ul></li>");

                menuString.Append("<li>");
                menuString.AppendFormat(GetParentMenu(Resource.EmployeeManagement, "fa fa-user"));
                menuString.Append("<ul class='nav child_menu'>");
                menuString.Append(GetSubMenu(Resource.Employee_Details, "", "/EmployeeManagement/Employee"));
                menuString.Append(GetSubMenu(Resource.Employee_Search, "", "/EmployeeManagement/SearchEmployee"));
                menuString.Append("</ul></li>");

                menuString.Append("<li>");
                menuString.AppendFormat(GetParentMenu(Resource.Salary_Management, "fa fa-inr"));
                menuString.Append("<ul class='nav child_menu'>");
                //menuString.Append(GetSubMenu(Resource.PayRoll, "", "/Salary/Salary/Payroll"));
                menuString.Append(GetSubMenu(Resource.Add_Salary, "", "/Salary/Salary/Index"));
               // menuString.Append(GetSubMenu(Resource.SalaryDetails, "", "/Salary/Salary/SalaryDetails"));
                menuString.Append("</ul></li>");

                menuString.Append("<li>");
                menuString.AppendFormat(GetParentMenu(Resource.Reports, "fa fa-file-text"));
                menuString.Append("<ul class='nav child_menu'>");
                menuString.Append(GetSubMenu(Resource.Employee_Report, "", "/Reports/Reports"));
                menuString.Append(GetSubMenu(Resource.Salary_Report, "", "/Salary/Salary/SalaryDetails"));

                menuString.Append("</ul></li>");

                menuString.Append("<li>");
                menuString.AppendFormat(GetParentMenu(Resource.Attendance_Management, "fa fa-check-square"));
                menuString.Append("<ul class='nav child_menu'>");
                menuString.Append(GetSubMenu(Resource.EmployeeAttendance, "", "/Attendance/Attendance/EmployeeAttendanceView"));
                menuString.Append(GetSubMenu(Resource.Attendance, "", "/Attendance/Attendance/HRAttendanceView"));
                menuString.Append("</ul></li>");

                menuString.Append("<li>");
                menuString.AppendFormat(GetParentMenu(Resource.DocumentManagement, "fa fa-check-square"));
                menuString.Append("<ul class='nav child_menu'>");
                menuString.Append(GetSubMenu(Resource.Document, "", "/Documents/Document"));
                menuString.Append("</ul></li>");
                menuString.Append("</ul></div></div>");

            }
            if (userContext.RoleId == 2)
            {
                menuString.Append("<li>");
                menuString.AppendFormat(GetParentMenu(Resource.Attendance_Management, "fa fa-check-square"));
                menuString.Append("<ul class='nav child_menu'>");
                menuString.Append(GetSubMenu(Resource.EmployeeAttendance, "", "/Attendance/Attendance/EmployeeAttendanceView"));
               // menuString.Append(GetSubMenu(Resource.Attendance, "", "/Attendance/Attendance/HRAttendanceView"));
                menuString.Append("</ul></li>");
                menuString.Append("</ul></div></div>");

            }

            string menu = menuString.ToString();
                return new MvcHtmlString(menu);
            
        }

        /// <summary>
        /// Get Widget Menu
        /// </summary>
        /// <param name="helper">Html Helper</param>
        /// <param name="permission">Page Permission</param>
        /// <returns>Widget Menu String</returns>
        public static MvcHtmlString WidgetMenu(this HtmlHelper helper, List<UserContext.PagePermission> permission)
        {
            StringBuilder menuString = new StringBuilder();

           

            string menu = menuString.ToString();
            return new MvcHtmlString(menu);
        }

        /// <summary>
        /// Get Role Array
        /// </summary>
        /// <param name="helper">Html Helper</param>
        /// <param name="roles">Role List</param>
        /// <returns>MVC Html String</returns>
        public static MvcHtmlString RoleJson(this HtmlHelper helper, List<UserContext.Role> roles)
        {
            string roleString = JsonConvert.SerializeObject(roles);
            return new MvcHtmlString(roleString);
        }

        /// <summary>
        /// Get Main Menu
        /// </summary>
        /// <param name="title">Menu title</param>
        /// <param name="icon">Menu icon</param>
        /// <param name="url">Menu URL</param>
        /// <returns>Menu string</returns>
        private static string GetMainMenu(string title, string icon, string url)
        {
            return string.Format("<li><a href='{2}' title='{0}'><i class='{1}'></i><span class='nav-label'>{0}</span></a></li>", title, icon, url);
        }

        /// <summary>
        /// Get Parent Menu
        /// </summary>
        /// <param name="title">Menu title</param>
        /// <param name="icon">Menu icon</param>
        /// <returns>Menu string</returns>
        private static string GetParentMenu(string title, string icon)
        {
            return string.Format("<a title='{0}'><i class='{1}'></i><span class='nav-label'>{0}</span><span class='fa fa-chevron-right'></span></a>", title, icon);
        }

        /// <summary>
        /// Get Sub Menu
        /// </summary>
        /// <param name="title">Menu title</param>
        /// <param name="icon">Menu icon</param>
        /// <param name="url">Menu URL</param>
        /// <returns>Menu string</returns>
        private static string GetSubMenu(string title, string icon, string url)
        {
            return string.Format("<li><a href='{2}'><i class='{1}'></i><span>{0}</span><div class='c'></div></a><div class='c'></div></li>", title, icon, url);
        }

        /// <summary>
        /// Get Widget Menu
        /// </summary>
        /// <param name="title">Widget Menu title</param>
        /// <param name="id">Widget id</param>
        /// <returns>Widget Menu string</returns>
        private static string GetWidgetMenu(string title, int id)
        {
            return string.Format("<li class='dropdown-header'><a href='#' ng-click='addWidget({1})'>{0}</a></li>", title, id);
        }
    }
}
