namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commnetsystem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "ReplyId", c => c.Int());
            CreateIndex("dbo.Comments", "ReplyId");
            AddForeignKey("dbo.Comments", "ReplyId", "dbo.Comments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "ReplyId", "dbo.Comments");
            DropIndex("dbo.Comments", new[] { "ReplyId" });
            DropColumn("dbo.Comments", "ReplyId");
        }
    }
}
