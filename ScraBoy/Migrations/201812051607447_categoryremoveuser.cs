namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categoryremoveuser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Categories", new[] { "AuthorId" });
            DropColumn("dbo.Categories", "AuthorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Categories", "AuthorId");
            AddForeignKey("dbo.Categories", "AuthorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
