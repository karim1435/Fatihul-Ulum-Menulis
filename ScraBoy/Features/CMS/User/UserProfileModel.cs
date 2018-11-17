using ScraBoy.Features.CMS.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.CMS.User
{
    public class UserProfileModel
    {
        public UserProfileModel()
        {
            User = new CMSUser();
            Posts = new List<Post>();
        }
        public IEnumerable<Post> Posts { get; set; }
        public CMSUser User { get; set; }
        public string Role { get; set; }
    }
}