namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pubsss : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "Published", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "Published", c => c.DateTime(nullable: false));
        }
    }
}
