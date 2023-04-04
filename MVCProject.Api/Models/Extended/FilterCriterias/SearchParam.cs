using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MVCProject.Api.Models.Extended.FilterCriterias
{
    public class SearchParam
    {
       [DataMember]
        public string FirstName { get; set; }

        public int? DepartmentId { get; set; }

        public int? DesignationId { get; set; }
    }
}