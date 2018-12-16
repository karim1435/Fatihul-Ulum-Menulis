namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bismillahlagi : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Posts", name: "UserId", newName: "AuthorId");
            RenameIndex(table: "dbo.Posts", name: "IX_UserId", newName: "IX_AuthorId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Posts", name: "IX_AuthorId", newName: "IX_UserId");
            RenameColumn(table: "dbo.Posts", name: "AuthorId", newName: "UserId");
        }
    }
}
