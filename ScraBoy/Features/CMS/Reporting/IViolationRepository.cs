using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraBoy.Features.CMS.Reporting
{
    public interface IViolationRepository
    {
        Task CreateAsync(Violation model);
        IPagedList<Violation> GetPagedList(string search,int page);
        Task<Violation> GetReportAsync(int id);
        Task Delete(Violation model);
    }
}
