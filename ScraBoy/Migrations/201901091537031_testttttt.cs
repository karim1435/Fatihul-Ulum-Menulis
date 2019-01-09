namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testttttt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Follows", "CMSUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Follows", "CMSUser_Id");
            AddForeignKey("dbo.Follows", "CMSUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follows", "CMSUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "CMSUser_Id" });
            DropColumn("dbo.Follows", "CMSUser_Id");
        }
    }
}
