namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scoreuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Bonus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Bonus");
        }
    }
}
