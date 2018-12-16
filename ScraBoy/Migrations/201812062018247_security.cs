namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class security : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Security", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Security");
        }
    }
}
