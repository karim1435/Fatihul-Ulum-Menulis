namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class publicb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Private", c => c.Boolean(nullable: false));
            DropColumn("dbo.Posts", "Public");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "Public", c => c.Boolean(nullable: false));
            DropColumn("dbo.Posts", "Private");
        }
    }
}
