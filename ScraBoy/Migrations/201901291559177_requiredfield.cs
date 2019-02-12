namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredfield : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Chapters", "SlugUrl", c => c.String(nullable: false));
            AlterColumn("dbo.Chapters", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Imams", "SlugUrl", c => c.String(nullable: false));
            AlterColumn("dbo.Imams", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Kitabs", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.Translations", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Translations", "Content", c => c.String());
            AlterColumn("dbo.Kitabs", "Content", c => c.String());
            AlterColumn("dbo.Imams", "Name", c => c.String());
            AlterColumn("dbo.Imams", "SlugUrl", c => c.String());
            AlterColumn("dbo.Chapters", "Name", c => c.String());
            AlterColumn("dbo.Chapters", "SlugUrl", c => c.String());
        }
    }
}
