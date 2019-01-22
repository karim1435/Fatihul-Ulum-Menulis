namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class burung1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Quotes", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Quotes", new[] { "AuthorId" });
            DropTable("dbo.Quotes");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Quotes", "AuthorId");
            AddForeignKey("dbo.Quotes", "AuthorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
