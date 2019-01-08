namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Bonus", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Security", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Security", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "Bonus");
        }
    }
}
