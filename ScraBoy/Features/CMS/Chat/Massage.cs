using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.CMS.Chat
{
    public class Massage
    {
        public int Id { get; set; }
        public string FromId { get; set; }
        [ForeignKey("FromId")]
        public CMSUser From { get; set; }
        public string ToId { get; set; }
        [ForeignKey("ToId")]
        public CMSUser To { get; set; }
        public string Content { get; set; }
        public DateTime Sent { get; set; }
       
        [NotMapped]
        public MessageType MassageType { get; set; }

    }
}