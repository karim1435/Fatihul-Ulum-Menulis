namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wrongmodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Kitabs", "ImamId", "dbo.Imams");
            DropIndex("dbo.Kitabs", new[] { "ImamId" });
            AddColumn("dbo.Chapters", "ImamId", c => c.Int());
            CreateIndex("dbo.Chapters", "ImamId");
            AddForeignKey("dbo.Chapters", "ImamId", "dbo.Imams", "Id");
            DropColumn("dbo.Kitabs", "ImamId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Kitabs", "ImamId", c => c.Int());
            DropForeignKey("dbo.Chapters", "ImamId", "dbo.Imams");
            DropIndex("dbo.Chapters", new[] { "ImamId" });
            DropColumn("dbo.Chapters", "ImamId");
            CreateIndex("dbo.Kitabs", "ImamId");
            AddForeignKey("dbo.Kitabs", "ImamId", "dbo.Imams", "Id");
        }
    }
}
