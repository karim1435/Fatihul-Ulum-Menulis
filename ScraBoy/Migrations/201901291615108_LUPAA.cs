namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LUPAA : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Chapters", "SlugUrl", c => c.String());
            AlterColumn("dbo.Imams", "SlugUrl", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Imams", "SlugUrl", c => c.String(nullable: false));
            AlterColumn("dbo.Chapters", "SlugUrl", c => c.String(nullable: false));
        }
    }
}