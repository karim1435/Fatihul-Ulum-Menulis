using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using ScraBoy.Features.Creator;
using ScraBoy.Features.Game;
using ScraBoy.Features.Ranking;
using ScraBoy.Features.Shop;
using ScraBoy.Features.Product;
using ScraBoy.Features.Type;
using ScraBoy.Features.Item;
using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.Comments;
using ScraBoy.Features.CMS.Interest;
using ScraBoy.Features.CMS.Topic;
using ScraBoy.Features.CMS.User;
using System.Collections;

namespace ScraBoy.Features.Data
{
    public class CMSContext : IdentityDbContext<CMSUser>
    {

        public CMSContext() : base("DefaultConnection")
        { }
        public DbSet<CreatorModel> Creator { get; set; }
        public DbSet<GameModel> Game { get; set; }
        public DbSet<RankingModel> Ranking { get; set; }
        public DbSet<ShopModel> Shop { get; set; }
        public DbSet<TypeModel> Type { get; set; }
        public DbSet<ProductModel> Product { get; set; }
        public DbSet<InventoryModel> Inventory { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Voting> Voting { get; set; }
        public DbSet<Category> Category { get; set; }
        public IEnumerable CMSUsers { get; internal set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Post>().HasRequired(e => e.Author);
            modelBuilder.Entity<Post>().HasRequired(e => e.Category);

            modelBuilder.Entity<Comment>()
                .HasRequired(s => s.Post)
                .WithMany(g => g.Comments)
                .HasForeignKey<string>(s => s.PostId).WillCascadeOnDelete(true);


            modelBuilder.Entity<Voting>()
            .HasRequired(s => s.Post)
            .WithMany(g => g.Votings)
            .HasForeignKey<string>(s => s.PostId).WillCascadeOnDelete(true);
        }
    }
}