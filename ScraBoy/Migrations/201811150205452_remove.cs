namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Description", c => c.String());
            DropColumn("dbo.AspNetUsers", "DisplayName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DisplayName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.AspNetUsers", "Description");
        }
    }
}
