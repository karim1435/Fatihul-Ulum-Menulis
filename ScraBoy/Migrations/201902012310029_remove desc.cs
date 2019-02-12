namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedesc : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Imams", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Imams", "Description", c => c.String());
        }
    }
}
