namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testingcoy : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Description", c => c.String(maxLength: 1000));
        }
    }
}
