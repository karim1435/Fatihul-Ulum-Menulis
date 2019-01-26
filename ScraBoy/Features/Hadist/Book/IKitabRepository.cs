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
        Task GetDataFromWeb(int chapterId,int form, int to);
        Task Edit(int id,Kitab model);
        Task Delete(Kitab model);
        Task<IPagedList> GetPagedListKitab(string imam,string name,int currentPage);
        Task<IPagedList> GetPageByChapter(string imam,string name,string chapterId,int currentPage);
    }
}