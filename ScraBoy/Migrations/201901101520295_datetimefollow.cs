namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datetimefollow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Follows", "FollowedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Follows", "FollowedOn");
        }
    }
}
