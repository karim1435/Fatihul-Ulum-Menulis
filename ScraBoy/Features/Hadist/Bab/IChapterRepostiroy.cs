using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Hadist.Bab
{
    public interface IChapterRepository
    {
        Task Create(Chapter model);
        Task Edit(Chapter model,int id);
        Task Delete(Chapter model);
        Task<IEnumerable<Chapter>> GetAll();
        Task<Chapter> GetOne(int id);
        Task<Chapter> FindBySlug(string slugUrl);
        Task<IEnumerable<Chapter>> FindByImam(string url);
        Task<IPagedList> GetPageChapter(string name,string imam,int currenPage);
    }

}