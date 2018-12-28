namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rewardrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Competitions", "Reward", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Competitions", "Reward", c => c.String());
        }
    }
}
