namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tossssss : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Bonus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Bonus", c => c.Int(nullable: false));
        }
    }
}
