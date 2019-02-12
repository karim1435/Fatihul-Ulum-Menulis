namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class worng : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Translations", "TranslationId", "dbo.Kitabs");
            RenameColumn(table: "dbo.Translations", name: "TranslationId", newName: "KitabId");
            RenameIndex(table: "dbo.Translations", name: "IX_TranslationId", newName: "IX_KitabId");
            DropPrimaryKey("dbo.Translations");
            AddColumn("dbo.Translations", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Translations", "Id");
            AddForeignKey("dbo.Translations", "KitabId", "dbo.Kitabs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Translations", "KitabId", "dbo.Kitabs");
            DropPrimaryKey("dbo.Translations");
            DropColumn("dbo.Translations", "Id");
            AddPrimaryKey("dbo.Translations", "TranslationId");
            RenameIndex(table: "dbo.Translations", name: "IX_KitabId", newName: "IX_TranslationId");
            RenameColumn(table: "dbo.Translations", name: "KitabId", newName: "TranslationId");
            AddForeignKey("dbo.Translations", "TranslationId", "dbo.Kitabs", "Id");
        }
    }
}
