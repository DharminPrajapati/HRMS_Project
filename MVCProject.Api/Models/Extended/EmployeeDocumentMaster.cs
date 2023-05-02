

namespace MVCProject.Api.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Web;
    public partial class EmployeeDocumentMaster
    {
        [DataMember]
        public List<EmployeeDocumentMaster> Attachments { get; set; }

        [DataMember]
        public List<EmployeeDocumentMaster> DeleteAttachments { get; set; }
    }
}