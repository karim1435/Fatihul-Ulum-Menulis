namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class desc : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Description", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Description", c => c.String());
        }
    }
}
