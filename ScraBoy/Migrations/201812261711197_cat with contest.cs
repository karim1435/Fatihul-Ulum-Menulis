namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class catwithcontest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Competitions", "EventId", "dbo.Events");
            DropIndex("dbo.Competitions", new[] { "EventId" });
            AddColumn("dbo.Competitions", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Competitions", "CategoryId");
            AddForeignKey("dbo.Competitions", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            DropColumn("dbo.Competitions", "EventId");
            DropTable("dbo.Events");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Competitions", "EventId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Competitions", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Competitions", new[] { "CategoryId" });
            DropColumn("dbo.Competitions", "CategoryId");
            CreateIndex("dbo.Competitions", "EventId");
            AddForeignKey("dbo.Competitions", "EventId", "dbo.Events", "Id", cascadeDelete: true);
        }
    }
}
