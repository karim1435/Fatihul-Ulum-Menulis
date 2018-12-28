namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forgotevent : DbMigration
    {
        public override void Up()
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
            CreateIndex("dbo.Competitions", "EventId");
            AddForeignKey("dbo.Competitions", "EventId", "dbo.Events", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Competitions", "EventId", "dbo.Events");
            DropIndex("dbo.Competitions", new[] { "EventId" });
            DropColumn("dbo.Competitions", "EventId");
            DropTable("dbo.Events");
        }
    }
}
