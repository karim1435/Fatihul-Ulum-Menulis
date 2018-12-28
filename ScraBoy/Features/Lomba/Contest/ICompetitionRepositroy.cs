using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Lomba.Contest
{
    public interface ICompetitionRepositroy
    {
        Task Create(Competition model);
        Task<IEnumerable<Competition>> GetAll();
        Task<Competition> GetByUrl(string slugUrl);
        Task Edit(string slugUrl,Competition model);
        Task Delete(string slugIrl);
    }
}