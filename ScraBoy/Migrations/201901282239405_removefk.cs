namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removefk : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Translations", "KitabId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Translations", "KitabId", c => c.Int(nullable: false));
        }
    }
}
