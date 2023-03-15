using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MVCProject.Api.Models
{
    public partial class TblEmployee
    {
        // public List<Attachments> Attachments { get; set; }
        [DataMember]
        public AttachmentMaster Attachment { get; set; }
        
    }
}

