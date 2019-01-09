namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setttdkfjdk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Follows", "CMSUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Follows", "CMSUser_Id1", c => c.String(maxLength: 128));
            CreateIndex("dbo.Follows", "CMSUser_Id");
            CreateIndex("dbo.Follows", "CMSUser_Id1");
            AddForeignKey("dbo.Follows", "CMSUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Follows", "CMSUser_Id1", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follows", "CMSUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "CMSUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "CMSUser_Id1" });
            DropIndex("dbo.Follows", new[] { "CMSUser_Id" });
            DropColumn("dbo.Follows", "CMSUser_Id1");
            DropColumn("dbo.Follows", "CMSUser_Id");
        }
    }
}
