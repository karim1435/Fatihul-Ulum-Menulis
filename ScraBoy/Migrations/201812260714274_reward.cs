namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reward : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competitions", "Reward", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Competitions", "Reward");
        }
    }
}
