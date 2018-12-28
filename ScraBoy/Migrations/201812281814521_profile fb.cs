namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profilefb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FbProfile", c => c.String());
            AddColumn("dbo.AspNetUsers", "InstagramProfile", c => c.String());
            AddColumn("dbo.AspNetUsers", "TwitterProfile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TwitterProfile");
            DropColumn("dbo.AspNetUsers", "InstagramProfile");
            DropColumn("dbo.AspNetUsers", "FbProfile");
        }
    }
}
