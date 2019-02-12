namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class biography : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Imams", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Imams", "Description");
        }
    }
}
