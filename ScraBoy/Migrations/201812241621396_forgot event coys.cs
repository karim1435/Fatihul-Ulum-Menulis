namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forgoteventcoys : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Competitions", "UrlImage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Competitions", "UrlImage", c => c.String(nullable: false));
        }
    }
}
