namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quotes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Content = c.String(nullable: false),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            AddColumn("dbo.Follows", "FollowedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quotes", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Quotes", new[] { "AuthorId" });
            DropColumn("dbo.Follows", "FollowedOn");
            DropTable("dbo.Quotes");
        }
    }
}
