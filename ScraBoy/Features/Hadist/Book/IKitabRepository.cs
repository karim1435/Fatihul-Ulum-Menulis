using PagedList;
using ScraBoy.Features.Hadist.Bab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Hadist.Book
{
    public interface IKitabRepository
    {
        Task Create(Kitab model);
        Task<IEnumerable<Kitab>> GetAll();
        Task<Kitab> GetById(int id);
        Task<Kitab> GetByNumber(int number);
        Task GetDataFromWeb(string url);
        Task<IEnumerable<Kitab>> FindByChapter(int chapterId);
        Task Edit(int id,Kitab model);
        Task Delete(Kitab model);
        Task<IPagedList> GetPageByChapter(string name,int chapterId,int currentPage);
    }
}