﻿using Microsoft.AspNet.Identity.EntityFramework;
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
using ScraBoy.Features.CMS.Nws;
using ScraBoy.Features.CMS.Reporting;
using ScraBoy.Features.Lomba.Audience;
using ScraBoy.Features.Lomba.Contest;
using ScraBoy.Features.CMS.PointScore;
using ScraBoy.Features.CMS.Following;
using ScraBoy.Features.Hadist.Book;
using Microsoft.AspNet.SignalR.Messaging;
using ScraBoy.Features.CMS.Chat;
using ScraBoy.Features.Hadist.Bab;
using ScraBoy.Features.Hadist.Hadis;
using ScraBoy.Features.Hadist.Meaning;

namespace ScraBoy.Features.Data
{
    public class CMSContext : IdentityDbContext<CMSUser>
    {

        public CMSContext() : base("DefaultConnection")
        { }

        #region RankingGame
        public DbSet<CreatorModel> Creator { get; set; }
        public DbSet<GameModel> Game { get; set; }
        public DbSet<RankingModel> Ranking { get; set; }
        public DbSet<ShopModel> Shop { get; set; }
        public DbSet<TypeModel> Type { get; set; }
        public DbSet<ProductModel> Product { get; set; }
        public DbSet<InventoryModel> Inventory { get; set; }

        public DbSet<Participant> Participant { get; set; }
        public DbSet<Competition> Competiton { get; set; }
        public DbSet<UserScore> UserScore { get; set; }

        #endregion

        #region CMS
        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Voting> Voting { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ViewPost> ViewPost { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<Violation> Violation { get; set; }
        public DbSet<Follow> Follow { get; set; }
        public DbSet<Massage> Message { get; set; }
        public DbSet<Kitab> Kitab { get; set; }
        public DbSet<Chapter> Chapter { get; set; }
        public DbSet<Imam> Imam { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Translation> Translation { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region CMS FluenApi
            modelBuilder.Entity<Post>().HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Post>().HasRequired(e => e.Category);

            modelBuilder.Entity<Comment>()
                .HasRequired(s => s.Post)
                .WithMany(g => g.Comments)
                .HasForeignKey<string>(s => s.PostId).WillCascadeOnDelete(true);



            modelBuilder.Entity<Report>()
               .HasRequired(s => s.User)
               .WithMany(g => g.Reports)
               .HasForeignKey<string>(s => s.UserId).WillCascadeOnDelete(true);


            modelBuilder.Entity<Post>()
              .HasRequired(s => s.Author)
              .WithMany(g => g.Posts)
              .HasForeignKey<string>(s => s.AuthorId).WillCascadeOnDelete(false);


            modelBuilder.Entity<Comment>()
              .HasRequired(s => s.User)
              .WithMany(g => g.Comments)
              .HasForeignKey<string>(s => s.UserId).WillCascadeOnDelete(true);

            modelBuilder.Entity<Voting>()
                .HasRequired(s => s.User)
                .WithMany(g => g.Votings)
                .HasForeignKey<string>(s => s.UserId).WillCascadeOnDelete(true);

            modelBuilder.Entity<Post>()
            .HasRequired(s => s.Category)
            .WithMany(g => g.Posts)
            .HasForeignKey<int>(s => s.CategoryId).WillCascadeOnDelete(false);

            //thi

            modelBuilder.Entity<Voting>()
           .HasRequired(s => s.Post)
           .WithMany(g => g.Votings)
           .HasForeignKey<string>(s => s.PostId).WillCascadeOnDelete(true);

            modelBuilder.Entity<ViewPost>()
            .HasRequired(s => s.Post)
            .WithMany(g => g.ViewPosts)
            .HasForeignKey<string>(s => s.PostId).WillCascadeOnDelete(true);

            modelBuilder.Entity<Violation>()
                .HasRequired(s => s.User)
                .WithMany(g => g.Violations)
                .HasForeignKey<string>(s => s.UserId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Violation>()
                .HasRequired(s => s.Post)
                .WithMany(g => g.Violations)
                .HasForeignKey<string>(s => s.PostId).WillCascadeOnDelete(true);

            modelBuilder.Entity<Competition>()
             .HasRequired(s => s.Creator)
             .WithMany(g => g.Competitions)
             .HasForeignKey<string>(s => s.CreatorId).WillCascadeOnDelete(true);

            modelBuilder.Entity<Participant>()
            .HasRequired(s => s.Competition)
            .WithMany(g => g.Participants)
            .HasForeignKey<int>(s => s.CompetitionId).WillCascadeOnDelete(true);

            modelBuilder.Entity<Competition>()
            .HasRequired(s => s.Category)
            .WithMany(g => g.Competitions)
            .HasForeignKey<int>(s => s.CategoryId).WillCascadeOnDelete(true);

            modelBuilder.Entity<UserScore>()
            .HasRequired(s => s.Author)
            .WithMany(g => g.UserScores)
            .HasForeignKey<string>(s => s.AuthorId).WillCascadeOnDelete(true);

            modelBuilder.Entity<Follow>()
            .HasRequired(s => s.Follower)
            .WithMany(g => g.Following)
            .HasForeignKey<string>(s => s.FollowerId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Follow>()
            .HasRequired(s => s.Followed)
            .WithMany(g => g.Followers)
            .HasForeignKey<string>(s => s.FollowedId).WillCascadeOnDelete(false);



            modelBuilder.Entity<Chapter>()
            .HasRequired(s => s.Imam)
            .WithMany(g => g.Chapters)
            .HasForeignKey<int>(s => s.ImamId).WillCascadeOnDelete(true);

            modelBuilder.Entity<Kitab>()
            .HasRequired(s => s.Chapter)
            .WithMany(g => g.Kitabs)
            .HasForeignKey<int>(s => s.ChapterId).WillCascadeOnDelete(true);

            
            //#region Forum FluentApi
            //modelBuilder.Entity<Thread>().HasKey(e => e.Id)
            //   .Property(e => e.Id)
            //   .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //modelBuilder.Entity<Thread>().HasRequired(e => e.Author);
            //modelBuilder.Entity<Thread>().HasRequired(e => e.Topic);
            #endregion
        }

    }
}