namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class begintoaddforum : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Threads", t => t.ThreadId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ThreadId)
                .Index(t => t.UserId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.TopicId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedOn = c.DateTime(nullable: false),
                        AuthorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .Index(t => t.AuthorId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Threads", t => t.ThreadId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ThreadId)
                .Index(t => t.UserId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Threads", t => t.ThreadId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ThreadId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ThreadStars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Like = c.Boolean(nullable: false),
                        ThreadId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Threads", t => t.ThreadId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ThreadId)
                .Index(t => t.UserId);
            
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
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Threads", t => t.ThreadId)
                .Index(t => t.ThreadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ThreadViews", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.ThreadStars", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ThreadStars", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Pools", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Pools", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Answers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answers", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Alerts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Alerts", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Threads", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.Topics", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Topics", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Threads", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.ThreadViews", new[] { "ThreadId" });
            DropIndex("dbo.ThreadStars", new[] { "UserId" });
            DropIndex("dbo.ThreadStars", new[] { "ThreadId" });
            DropIndex("dbo.Pools", new[] { "UserId" });
            DropIndex("dbo.Pools", new[] { "ThreadId" });
            DropIndex("dbo.Answers", new[] { "UserId" });
            DropIndex("dbo.Answers", new[] { "ThreadId" });
            DropIndex("dbo.Subjects", new[] { "AuthorId" });
            DropIndex("dbo.Topics", new[] { "AuthorId" });
            DropIndex("dbo.Topics", new[] { "SubjectId" });
            DropIndex("dbo.Threads", new[] { "TopicId" });
            DropIndex("dbo.Threads", new[] { "AuthorId" });
            DropIndex("dbo.Alerts", new[] { "UserId" });
            DropIndex("dbo.Alerts", new[] { "ThreadId" });
            DropTable("dbo.ThreadViews");
            DropTable("dbo.ThreadStars");
            DropTable("dbo.Pools");
            DropTable("dbo.Answers");
            DropTable("dbo.Subjects");
            DropTable("dbo.Topics");
            DropTable("dbo.Threads");
            DropTable("dbo.Alerts");
        }
    }
}
