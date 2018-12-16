namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _public : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Public", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Public");
        }
    }
}
