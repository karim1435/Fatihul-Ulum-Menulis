using PagedList;
using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.CMS.Reporting
{
    public class ViolationRepository : IViolationRepository
    {
        private readonly CMSContext db;
        private readonly int pageSize = 10;

        public ViolationRepository(CMSContext db)
        {
            this.db = db;
        }
        public ViolationRepository() : this(new CMSContext())
        {

        }
        public async Task CreateAsync(Violation model)
        {
            model.ReportedOn = DateTime.Now;
            this.db.Violation.Add(model);
            await this.db.SaveChangesAsync();
        }
        public IQueryable<Violation> GetViolation(string name)
        {
            if(!string.IsNullOrWhiteSpace(name))
            {
                return this.db.Violation.Include("User").Include("Post").Where(a => a.Post.Title.Contains(name) ||
               a.User.UserName.Equals(name));
            }
            return this.db.Violation.Include("User").Include("Post");
        }
        public List<Violation> GetViolationList(string name)
        {
            return GetViolation(name).OrderByDescending(a => a.ReportedOn).ToList();
        }
        public IPagedList<Violation> GetPagedList(string search,int page)
        {
            var model = new List<Violation>();

            model = GetViolationList(search).ToList();

            return model.ToPagedList(page,pageSize);
        }

        public async Task<Violation> GetReportAsync(int id)
        {
            return await this.db.Violation.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task Delete(Violation model)
        {
            this.db.Violation.Remove(model);
            await this.db.SaveChangesAsync();
        }
    }
}