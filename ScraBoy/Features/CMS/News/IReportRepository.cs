using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.CMS.Nws
{
    public interface IReportRepoitory
    {
        Task CreateReportAsync(Report model);
        Task UpdateReportAsync(Report model, int id);
        Task<Report> GetByIdAsync(int id);
        Task DeleteReportAsync(Report model);
        IPagedList<Report> GetPagedList(string search,int currentPage);
    }

}