namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changec : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Votings", "PostId", "dbo.Posts");
            AddForeignKey("dbo.Votings", "PostId", "dbo.Posts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votings", "PostId", "dbo.Posts");
            AddForeignKey("dbo.Votings", "PostId", "dbo.Posts", "Id");
        }
    }
}
