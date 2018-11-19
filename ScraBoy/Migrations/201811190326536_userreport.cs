namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userreport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "ReportType", c => c.Int(nullable: false));
            AddColumn("dbo.Reports", "Fixed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Reports", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Reports", "UserId");
            AddForeignKey("dbo.Reports", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Reports", new[] { "UserId" });
            DropColumn("dbo.Reports", "UserId");
            DropColumn("dbo.Reports", "Fixed");
            DropColumn("dbo.Reports", "ReportType");
        }
    }
}
