namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CATS : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "ParentId", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "ParentId" });
            AddColumn("dbo.Categories", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Categories", "AuthorId");
            AddForeignKey("dbo.Categories", "AuthorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Categories", "ParentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "ParentId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Categories", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Categories", new[] { "AuthorId" });
            DropColumn("dbo.Categories", "AuthorId");
            CreateIndex("dbo.Categories", "ParentId");
            AddForeignKey("dbo.Categories", "ParentId", "dbo.Categories", "Id");
        }
    }
}
