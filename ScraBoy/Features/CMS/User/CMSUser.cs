using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.CMS.User
{
    public class CMSUser:IdentityUser
    {
        public string DisplayName { get; set; }
    }
}