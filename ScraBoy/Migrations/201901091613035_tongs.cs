namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tongs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Follows", "FollowerId", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "FollowerId" });
            AlterColumn("dbo.Follows", "FollowerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Follows", "FollowerId");
            AddForeignKey("dbo.Follows", "FollowerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follows", "FollowerId", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "FollowerId" });
            AlterColumn("dbo.Follows", "FollowerId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Follows", "FollowerId");
            AddForeignKey("dbo.Follows", "FollowerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
