namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class slugurl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chapters", "SlugUrl", c => c.String());
            AddColumn("dbo.Imams", "SlugUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Imams", "SlugUrl");
            DropColumn("dbo.Chapters", "SlugUrl");
        }
    }
}
