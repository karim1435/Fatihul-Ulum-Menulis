using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Hadist.Meaning
{
    public interface ITranslationRepository
    {
        Task<IEnumerable<Language>> GetAllBahasa();
        Task Create(Translation model);
        Task Edit(Translation model,int id);
        Task Delete(Translation model);
        Task<Translation> FindById(int Id);
        Task<IEnumerable<Translation>> GetAll();
    }
}