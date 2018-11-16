namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Updated", c => c.DateTime(nullable: false));
            DropColumn("dbo.Posts", "UpdatedAt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "UpdatedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.Posts", "Updated");
        }
    }
}
