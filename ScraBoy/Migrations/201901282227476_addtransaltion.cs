namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtransaltion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Translations",
                c => new
                    {
                        TranslationId = c.Int(nullable: false),
                        Content = c.String(),
                        TranslatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        KitabId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TranslationId)
                .ForeignKey("dbo.Kitabs", t => t.TranslationId)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.TranslationId)
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Translations", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Translations", "TranslationId", "dbo.Kitabs");
            DropIndex("dbo.Translations", new[] { "LanguageId" });
            DropIndex("dbo.Translations", new[] { "TranslationId" });
            DropTable("dbo.Languages");
            DropTable("dbo.Translations");
        }
    }
}
