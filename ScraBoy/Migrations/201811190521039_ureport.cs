namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ureport : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reports", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Reports", new[] { "UserId" });
            AlterColumn("dbo.Reports", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Reports", "UserId");
            AddForeignKey("dbo.Reports", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Reports", new[] { "UserId" });
            AlterColumn("dbo.Reports", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Reports", "UserId");
            AddForeignKey("dbo.Reports", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
