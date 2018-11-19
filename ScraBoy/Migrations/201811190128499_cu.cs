namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cu : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Votings", new[] { "UserId" });
            AlterColumn("dbo.Comments", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Votings", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Comments", "UserId");
            CreateIndex("dbo.Votings", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Votings", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            AlterColumn("dbo.Votings", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Comments", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Votings", "UserId");
            CreateIndex("dbo.Comments", "UserId");
        }
    }
}
