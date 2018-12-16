namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class smaia : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reports", "UserId", "dbo.AspNetUsers");
            AddForeignKey("dbo.Reports", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "UserId", "dbo.AspNetUsers");
            AddForeignKey("dbo.Reports", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
