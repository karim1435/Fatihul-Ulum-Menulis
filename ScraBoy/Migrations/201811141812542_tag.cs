namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tag : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "CombinedTags", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "CombinedTags", c => c.String());
        }
    }
}
