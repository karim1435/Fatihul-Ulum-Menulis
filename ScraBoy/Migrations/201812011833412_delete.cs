namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Threads", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Topics", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subjects", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Topics", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Threads", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.Alerts", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Alerts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answers", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Answers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Pools", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Pools", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ThreadStars", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.ThreadStars", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ThreadViews", "ThreadId", "dbo.Threads");
            DropIndex("dbo.Alerts", new[] { "ThreadId" });
            DropIndex("dbo.Alerts", new[] { "UserId" });
            DropIndex("dbo.Threads", new[] { "AuthorId" });
            DropIndex("dbo.Threads", new[] { "TopicId" });
            DropIndex("dbo.Topics", new[] { "SubjectId" });
            DropIndex("dbo.Topics", new[] { "AuthorId" });
            DropIndex("dbo.Subjects", new[] { "AuthorId" });
            DropIndex("dbo.Answers", new[] { "ThreadId" });
            DropIndex("dbo.Answers", new[] { "UserId" });
            DropIndex("dbo.Pools", new[] { "ThreadId" });
            DropIndex("dbo.Pools", new[] { "UserId" });
            DropIndex("dbo.ThreadStars", new[] { "ThreadId" });
            DropIndex("dbo.ThreadStars", new[] { "UserId" });
            DropIndex("dbo.ThreadViews", new[] { "ThreadId" });
            DropTable("dbo.Alerts");
            DropTable("dbo.Threads");
            DropTable("dbo.Topics");
            DropTable("dbo.Subjects");
            DropTable("dbo.Answers");
            DropTable("dbo.Pools");
            DropTable("dbo.ThreadStars");
            DropTable("dbo.ThreadViews");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ThreadViews",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ViewId = c.String(nullable: false),
                        Count = c.Int(nullable: false),
                        ThreadId = c.String(maxLength: 128),
                        LastViewed = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ThreadStars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Like = c.Boolean(nullable: false),
                        ThreadId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pools",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Voting = c.Boolean(nullable: false),
                        LikedOn = c.DateTime(nullable: false),
                        ThreadId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false, maxLength: 4000),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        ThreadId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedOn = c.DateTime(nullable: false),
                        AuthorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedOn = c.DateTime(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        AuthorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Threads",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 80),
                        Content = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        CombinedLabels = c.String(nullable: false),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        TopicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Alerts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportType = c.Int(nullable: false),
                        Reason = c.String(nullable: false),
                        ThreadId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                        ReportedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ThreadViews", "ThreadId");
            CreateIndex("dbo.ThreadStars", "UserId");
            CreateIndex("dbo.ThreadStars", "ThreadId");
            CreateIndex("dbo.Pools", "UserId");
            CreateIndex("dbo.Pools", "ThreadId");
            CreateIndex("dbo.Answers", "UserId");
            CreateIndex("dbo.Answers", "ThreadId");
            CreateIndex("dbo.Subjects", "AuthorId");
            CreateIndex("dbo.Topics", "AuthorId");
            CreateIndex("dbo.Topics", "SubjectId");
            CreateIndex("dbo.Threads", "TopicId");
            CreateIndex("dbo.Threads", "AuthorId");
            CreateIndex("dbo.Alerts", "UserId");
            CreateIndex("dbo.Alerts", "ThreadId");
            AddForeignKey("dbo.ThreadViews", "ThreadId", "dbo.Threads", "Id");
            AddForeignKey("dbo.ThreadStars", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ThreadStars", "ThreadId", "dbo.Threads", "Id");
            AddForeignKey("dbo.Pools", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Pools", "ThreadId", "dbo.Threads", "Id");
            AddForeignKey("dbo.Answers", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Answers", "ThreadId", "dbo.Threads", "Id");
            AddForeignKey("dbo.Alerts", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Alerts", "ThreadId", "dbo.Threads", "Id");
            AddForeignKey("dbo.Threads", "TopicId", "dbo.Topics", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Topics", "SubjectId", "dbo.Subjects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Subjects", "AuthorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Topics", "AuthorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Threads", "AuthorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
