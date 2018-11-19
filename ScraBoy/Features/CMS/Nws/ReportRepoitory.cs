using PagedList;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.CMS.Nws
{
    public class ReportRepoitory : IReportRepoitory
    {
        private readonly CMSContext context;
        private readonly int pageSize = 10;

        public ReportRepoitory(CMSContext db)
        {
            this.context = db;
        }
        public ReportRepoitory() : this(new CMSContext())
        {

        }
        public async Task CreateReportAsync(Report model)
        {
            if(model.ReportType.Equals(ReportType.Bug))
            {
                model.Fixed = false;
            }
            else
            {
                model.Fixed = true;
            }

            model.ReportedOn = DateTime.Now;
            this.context.Report.Add(model);
            await this.context.SaveChangesAsync();
        }
        public IQueryable<Report> GetReports(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                return this.context.Report.Where(m => m.Title.Contains(name));
            }
            return this.context.Report;
        }

        public List<Report> GetReportList(string name)
        {
            return this.GetReports(name).OrderByDescending(a => a.ReportedOn).ToList();
        }

        public IPagedList<Report> GetPagedList(string search,int currentPage)
        {
            var model = new List<Report>();

            model = GetReportList(search);

            model = model.OrderByDescending(s => s.ReportedOn).ToList();
            return model.ToPagedList(currentPage,pageSize);
        }
        public async Task DeleteReportAsync(Report model)
        {
            this.context.Report.Remove(model);
            await this.context.SaveChangesAsync();
        }

        public async Task<Report> GetByIdAsync(int id)
        {
            return await this.context.Report.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateReportAsync(Report model,int id)
        {
            var report = await this.GetByIdAsync(id);

            report.Title = model.Title;
            report.Description = model.Description;
            report.ReportType = model.ReportType;
            report.Fixed = model.Fixed;
            await this.context.SaveChangesAsync();
        }
    }
}