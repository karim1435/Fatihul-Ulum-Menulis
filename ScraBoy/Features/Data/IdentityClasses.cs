using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using ScraBoy.Features.CMS.User;

namespace ScraBoy.Features.Data
{
    public class CmsUserStore:UserStore<CMSUser>
    {
        public CmsUserStore():this(new CMSContext())
        {

        }
        public CmsUserStore(CMSContext context):base(context)
        {
        }
    }
    public class CmsUserManager : UserManager<CMSUser>
    {
        public CmsUserManager():this(new CmsUserStore())
        {

        }
        public CmsUserManager(UserStore<CMSUser> userStore):base(userStore)
        {

        }

     
    }
}