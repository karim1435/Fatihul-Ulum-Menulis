namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class statuscontest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competitions", "StatusContest", c => c.Int(nullable: false));
            DropColumn("dbo.Competitions", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Competitions", "Active", c => c.Boolean(nullable: false));
            DropColumn("dbo.Competitions", "StatusContest");
        }
    }
}
