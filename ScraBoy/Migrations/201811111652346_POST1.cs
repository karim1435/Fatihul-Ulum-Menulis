namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class POST1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Post_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Comments", "Post_Id");
            AddForeignKey("dbo.Comments", "Post_Id", "dbo.Posts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropIndex("dbo.Comments", new[] { "Post_Id" });
            DropColumn("dbo.Comments", "Post_Id");
        }
    }
}
