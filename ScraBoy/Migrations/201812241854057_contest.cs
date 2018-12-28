namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Participants", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.Participants", "Title", c => c.String(nullable: false));
            DropColumn("dbo.Participants", "FileUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Participants", "FileUrl", c => c.String());
            AlterColumn("dbo.Participants", "Title", c => c.String());
            DropColumn("dbo.Participants", "Content");
        }
    }
}
