namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forgottime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserScores", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserScores", "Updated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserScores", "Updated");
            DropColumn("dbo.UserScores", "Created");
        }
    }
}
