namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useless : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Assessments", "Score");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assessments", "Score", c => c.Int(nullable: false));
        }
    }
}
