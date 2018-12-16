namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bismillah : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Posts", name: "AuthorId", newName: "UserId");
            RenameIndex(table: "dbo.Posts", name: "IX_AuthorId", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Posts", name: "IX_UserId", newName: "IX_AuthorId");
            RenameColumn(table: "dbo.Posts", name: "UserId", newName: "AuthorId");
        }
    }
}
