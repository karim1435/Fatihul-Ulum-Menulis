namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class slug : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SlugUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SlugUrl");
        }
    }
}
