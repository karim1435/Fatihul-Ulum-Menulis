using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraBoy.Features.Hadist.Hadis
{
    public interface IimamRepository
    {
        Task<IEnumerable<Imam>> GetAll();
        Task Create(Imam model);
        Task Edit(Imam model,int id);
        Task Delete(Imam model);
        Task<Imam> FindByName(string name);
        Task<Imam> FindById(int id);
        Task<Imam> FindBySlug(string slugUrl);
        Task<IPagedList> GetPageImam(string name,int currentPage);
    }
}
