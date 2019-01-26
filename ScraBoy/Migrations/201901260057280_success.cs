namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class success : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chapters", "ImamId", "dbo.Imams");
            DropForeignKey("dbo.Kitabs", "ChapterId", "dbo.Chapters");
            DropIndex("dbo.Chapters", new[] { "ImamId" });
            DropIndex("dbo.Kitabs", new[] { "ChapterId" });
            AddColumn("dbo.Chapters", "Number", c => c.Int(nullable: false));
            AlterColumn("dbo.Chapters", "ImamId", c => c.Int(nullable: false));
            AlterColumn("dbo.Kitabs", "ChapterId", c => c.Int(nullable: false));
            CreateIndex("dbo.Chapters", "ImamId");
            CreateIndex("dbo.Kitabs", "ChapterId");
            AddForeignKey("dbo.Chapters", "ImamId", "dbo.Imams", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Kitabs", "ChapterId", "dbo.Chapters", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Kitabs", "ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.Chapters", "ImamId", "dbo.Imams");
            DropIndex("dbo.Kitabs", new[] { "ChapterId" });
            DropIndex("dbo.Chapters", new[] { "ImamId" });
            AlterColumn("dbo.Kitabs", "ChapterId", c => c.Int());
            AlterColumn("dbo.Chapters", "ImamId", c => c.Int());
            DropColumn("dbo.Chapters", "Number");
            CreateIndex("dbo.Kitabs", "ChapterId");
            CreateIndex("dbo.Chapters", "ImamId");
            AddForeignKey("dbo.Kitabs", "ChapterId", "dbo.Chapters", "Id");
            AddForeignKey("dbo.Chapters", "ImamId", "dbo.Imams", "Id");
        }
    }
}
