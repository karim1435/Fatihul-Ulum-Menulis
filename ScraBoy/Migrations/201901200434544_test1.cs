namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Massages", "FromId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Massages", "ToId", "dbo.AspNetUsers");
            DropIndex("dbo.Massages", new[] { "FromId" });
            DropIndex("dbo.Massages", new[] { "ToId" });
            DropTable("dbo.Massages");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Massages", "ToId");
            CreateIndex("dbo.Massages", "FromId");
            AddForeignKey("dbo.Massages", "ToId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Massages", "FromId", "dbo.AspNetUsers", "Id");
        }
    }
}
