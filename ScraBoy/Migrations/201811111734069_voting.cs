namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class voting : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Votings", "PostId", "dbo.Posts");
            DropIndex("dbo.Votings", new[] { "PostId" });
            AlterColumn("dbo.Votings", "PostId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Votings", "PostId");
            AddForeignKey("dbo.Votings", "PostId", "dbo.Posts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votings", "PostId", "dbo.Posts");
            DropIndex("dbo.Votings", new[] { "PostId" });
            AlterColumn("dbo.Votings", "PostId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Votings", "PostId");
            AddForeignKey("dbo.Votings", "PostId", "dbo.Posts", "Id");
        }
    }
}
