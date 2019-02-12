namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class translation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Translations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Content = c.String(),
                        TranslatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        LanguageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.Kitabs", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        KeyCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Kitabs", "TranslationId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Translations", "Id", "dbo.Kitabs");
            DropForeignKey("dbo.Translations", "LanguageId", "dbo.Languages");
            DropIndex("dbo.Translations", new[] { "LanguageId" });
            DropIndex("dbo.Translations", new[] { "Id" });
            DropColumn("dbo.Kitabs", "TranslationId");
            DropTable("dbo.Languages");
            DropTable("dbo.Translations");
        }
    }
}
