namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imageprofile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UrlImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UrlImage");
        }
    }
}
