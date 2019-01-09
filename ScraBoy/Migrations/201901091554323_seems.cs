namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seems : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Follows", "CMSUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "FollowerId", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "FollowerId" });
            DropIndex("dbo.Follows", new[] { "CMSUser_Id" });
            DropColumn("dbo.Follows", "FollowerId");
            RenameColumn(table: "dbo.Follows", name: "CMSUser_Id", newName: "FollowerId");
            AlterColumn("dbo.Follows", "FollowerId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Follows", "FollowerId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Follows", "FollowerId");
            AddForeignKey("dbo.Follows", "FollowerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follows", "FollowerId", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "FollowerId" });
            AlterColumn("dbo.Follows", "FollowerId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Follows", "FollowerId", c => c.String(maxLength: 128));
            RenameColumn(table: "dbo.Follows", name: "FollowerId", newName: "CMSUser_Id");
            AddColumn("dbo.Follows", "FollowerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Follows", "CMSUser_Id");
            CreateIndex("dbo.Follows", "FollowerId");
            AddForeignKey("dbo.Follows", "FollowerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Follows", "CMSUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
