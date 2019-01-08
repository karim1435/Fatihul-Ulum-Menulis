namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tpsdd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Security", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Security", c => c.String());
        }
    }
}
