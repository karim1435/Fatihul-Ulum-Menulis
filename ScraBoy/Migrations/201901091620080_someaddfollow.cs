namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class someaddfollow : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Follows", "CMSUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "CMSUser_Id1", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "CMSUser_Id" });
            DropIndex("dbo.Follows", new[] { "CMSUser_Id1" });
            DropColumn("dbo.Follows", "CMSUser_Id");
            DropColumn("dbo.Follows", "CMSUser_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Follows", "CMSUser_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.Follows", "CMSUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Follows", "CMSUser_Id1");
            CreateIndex("dbo.Follows", "CMSUser_Id");
            AddForeignKey("dbo.Follows", "CMSUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Follows", "CMSUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
