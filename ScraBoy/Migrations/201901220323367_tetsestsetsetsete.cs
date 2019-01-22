namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tetsestsetsetsete : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Imams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Kitabs", "ChapterId", c => c.Int());
            AddColumn("dbo.Kitabs", "ImamId", c => c.Int());
            CreateIndex("dbo.Kitabs", "ChapterId");
            CreateIndex("dbo.Kitabs", "ImamId");
            AddForeignKey("dbo.Kitabs", "ChapterId", "dbo.Chapters", "Id");
            AddForeignKey("dbo.Kitabs", "ImamId", "dbo.Imams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Kitabs", "ImamId", "dbo.Imams");
            DropForeignKey("dbo.Kitabs", "ChapterId", "dbo.Chapters");
            DropIndex("dbo.Kitabs", new[] { "ImamId" });
            DropIndex("dbo.Kitabs", new[] { "ChapterId" });
            DropColumn("dbo.Kitabs", "ImamId");
            DropColumn("dbo.Kitabs", "ChapterId");
            DropTable("dbo.Imams");
            DropTable("dbo.Chapters");
        }
    }
}
