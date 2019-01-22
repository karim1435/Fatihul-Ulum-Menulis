namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quotesfeature : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        Published = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        StatusColor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quotes", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Quotes", new[] { "AuthorId" });
            DropTable("dbo.Quotes");
        }
    }
}
