namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testnew : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "CategoryId", "dbo.Categories");
            AddForeignKey("dbo.Posts", "CategoryId", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "CategoryId", "dbo.Categories");
            AddForeignKey("dbo.Posts", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
