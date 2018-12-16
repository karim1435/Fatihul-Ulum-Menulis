namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Massages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromId = c.String(maxLength: 128),
                        ToId = c.String(maxLength: 128),
                        Content = c.String(),
                        Sent = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FromId)
                .ForeignKey("dbo.AspNetUsers", t => t.ToId)
                .Index(t => t.FromId)
                .Index(t => t.ToId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Massages", "ToId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Massages", "FromId", "dbo.AspNetUsers");
            DropIndex("dbo.Massages", new[] { "ToId" });
            DropIndex("dbo.Massages", new[] { "FromId" });
            DropTable("dbo.Massages");
        }
    }
}
