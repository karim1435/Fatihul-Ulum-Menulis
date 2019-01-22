namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hadisttest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Kitabs", "ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.Kitabs", "ImamId", "dbo.Imams");
            DropIndex("dbo.Kitabs", new[] { "ChapterId" });
            DropIndex("dbo.Kitabs", new[] { "ImamId" });
            DropColumn("dbo.Kitabs", "ChapterId");
            DropColumn("dbo.Kitabs", "ImamId");
            DropTable("dbo.Chapters");
            DropTable("dbo.Imams");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Imams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Kitabs", "ImamId", c => c.Int());
            AddColumn("dbo.Kitabs", "ChapterId", c => c.Int());
            CreateIndex("dbo.Kitabs", "ImamId");
            CreateIndex("dbo.Kitabs", "ChapterId");
            AddForeignKey("dbo.Kitabs", "ImamId", "dbo.Imams", "Id");
            AddForeignKey("dbo.Kitabs", "ChapterId", "dbo.Chapters", "Id");
        }
    }
}
