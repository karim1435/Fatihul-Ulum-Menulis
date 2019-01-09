namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Followers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Followers", "FollowerId", "dbo.AspNetUsers");
            DropIndex("dbo.Followers", new[] { "UserId" });
            DropIndex("dbo.Followers", new[] { "FollowerId" });
            DropTable("dbo.Followers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Followers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        FollowerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.FollowerId });
            
            CreateIndex("dbo.Followers", "FollowerId");
            CreateIndex("dbo.Followers", "UserId");
            AddForeignKey("dbo.Followers", "FollowerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Followers", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
