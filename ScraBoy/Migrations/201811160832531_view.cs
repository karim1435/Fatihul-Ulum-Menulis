namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class view : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ViewPosts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ViewId = c.String(nullable: false),
                        Count = c.Int(nullable: false),
                        PostId = c.String(maxLength: 128),
                        LastViewed = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ViewPosts", "PostId", "dbo.Posts");
            DropIndex("dbo.ViewPosts", new[] { "PostId" });
            DropTable("dbo.ViewPosts");
        }
    }
}
