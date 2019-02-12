namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Translations", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Translations", "Id", "dbo.Kitabs");
            DropIndex("dbo.Translations", new[] { "Id" });
            DropIndex("dbo.Translations", new[] { "LanguageId" });
            DropColumn("dbo.Kitabs", "TranslationId");
            DropTable("dbo.Translations");
            DropTable("dbo.Languages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        KeyCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Kitabs", "TranslationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Translations", "LanguageId");
            CreateIndex("dbo.Translations", "Id");
            AddForeignKey("dbo.Translations", "Id", "dbo.Kitabs", "Id");
            AddForeignKey("dbo.Translations", "LanguageId", "dbo.Languages", "Id", cascadeDelete: true);
        }
    }
}
