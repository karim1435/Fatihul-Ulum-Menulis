namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class viewposts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ViewPosts", "PostId", "dbo.Posts");
            DropIndex("dbo.ViewPosts", new[] { "PostId" });
            AlterColumn("dbo.ViewPosts", "PostId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.ViewPosts", "PostId");
            AddForeignKey("dbo.ViewPosts", "PostId", "dbo.Posts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ViewPosts", "PostId", "dbo.Posts");
            DropIndex("dbo.ViewPosts", new[] { "PostId" });
            AlterColumn("dbo.ViewPosts", "PostId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ViewPosts", "PostId");
            AddForeignKey("dbo.ViewPosts", "PostId", "dbo.Posts", "Id");
        }
    }
}
