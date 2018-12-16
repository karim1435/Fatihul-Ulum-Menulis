namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class violation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Violations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reason = c.String(nullable: false, maxLength: 2000),
                        ViolationType = c.Int(nullable: false),
                        ReportedOn = c.DateTime(nullable: false),
                        PostId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Violations", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Violations", "PostId", "dbo.Posts");
            DropIndex("dbo.Violations", new[] { "UserId" });
            DropIndex("dbo.Violations", new[] { "PostId" });
            DropTable("dbo.Violations");
        }
    }
}
