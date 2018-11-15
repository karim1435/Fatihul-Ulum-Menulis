using PagedList;
using ScraBoy.Features.CMS.Tag;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.CMS.Tag
{
    public class TagRepository : ITagRepostory
    {
        private readonly int pageSize = 10;
        public void Delete(string tag)
        {
            using(var db = new CMSContext())
            {
                var posts = db.Post.Where(post => post.CombinedTags.Contains(tag)).ToList();

                posts = posts.Where(pos =>
                pos.Tags.Contains(tag,StringComparer.CurrentCultureIgnoreCase)).
                ToList();

                if(!posts.Any())
                {
                    throw new KeyNotFoundException("The tag" + tag + " does not exist");
                }

                foreach(var post in posts)
                {
                    post.Tags.Remove(tag);
                }

                db.SaveChanges();
            }
        }

        public void Edit(string existingTag,string newTag)
        {
            using(var db = new CMSContext())
            {
                var posts = db.Post.Where(post => post.CombinedTags.Contains(existingTag)).ToList();

                posts = posts.Where(pos =>
                pos.Tags.Contains(existingTag,StringComparer.CurrentCultureIgnoreCase)).
                ToList();

                if(!posts.Any())
                {
                    throw new KeyNotFoundException("The tag" + existingTag + " does not exist");
                }

                foreach(var post in posts)
                {
                    post.Tags.Remove(existingTag);
                    post.Tags.Add(newTag);
                }
                db.SaveChanges();

            }
        }
        public string Get(string tag)
        {
            using(var db = new CMSContext())
            {
                var posts = db.Post.Where(post => post.CombinedTags.Contains(tag)).ToList();

                posts = posts.Where(pos =>
                pos.Tags.Contains(tag,StringComparer.CurrentCultureIgnoreCase)).
                ToList();

                if(!posts.Any())
                {
                    throw new KeyNotFoundException("The tag" + tag + " does not exist");
                }

                return tag.ToLower();
            }
        }

        public IPagedList<string> GetPagedList(string search,int currentPage)
        {
            var model = GetTags(search);

            return model.ToPagedList(currentPage,pageSize);
        }
        public List<string> GetTags(string name)
        {
            using(var db = new CMSContext())
            {
                var tagList = GetAll().ToList();

                if(!string.IsNullOrEmpty(name))
                {
                    tagList = GetAll().Where(a => a.Contains(name)).ToList();
                }
                return tagList;
            }
        }
        public IEnumerable<string> GetAll()
        {
            using(var db = new CMSContext())
            {
                var tagsList = db.Post.Select(p => p.CombinedTags).ToList();
                return string.Join(",",tagsList).Split(',').Distinct();
            }
        }
    }
}