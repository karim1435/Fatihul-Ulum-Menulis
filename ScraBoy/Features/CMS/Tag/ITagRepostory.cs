
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraBoy.Features.CMS.Tag
{
    public interface ITagRepostory
    {
        IEnumerable<string> GetAll();
        string Get(string tag);
        void Edit(string existingTag,string newTag);
        void Delete(string tag);
    }
}
