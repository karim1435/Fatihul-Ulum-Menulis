namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserScores", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.UserScores", new[] { "AuthorId" });
            DropTable("dbo.UserScores");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserScores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                        Note = c.String(nullable: false),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UserScores", "AuthorId");
            AddForeignKey("dbo.UserScores", "AuthorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
