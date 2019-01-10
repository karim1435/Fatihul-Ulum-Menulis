namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removefollowdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Follows", "FollowedOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Follows", "FollowedOn", c => c.DateTime(nullable: false));
        }
    }
}
