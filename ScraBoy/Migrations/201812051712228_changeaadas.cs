namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeaadas : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Votings", "UserId", "dbo.AspNetUsers");
            AddForeignKey("dbo.Votings", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votings", "UserId", "dbo.AspNetUsers");
            AddForeignKey("dbo.Votings", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
