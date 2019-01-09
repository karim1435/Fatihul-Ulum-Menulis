namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bismillahsuccess : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Follows", new[] { "FollowerId" });
            DropIndex("dbo.Follows", new[] { "FollowedId" });
            AlterColumn("dbo.Follows", "FollowerId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Follows", "FollowedId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Follows", "FollowerId");
            CreateIndex("dbo.Follows", "FollowedId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Follows", new[] { "FollowedId" });
            DropIndex("dbo.Follows", new[] { "FollowerId" });
            AlterColumn("dbo.Follows", "FollowedId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Follows", "FollowerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Follows", "FollowedId");
            CreateIndex("dbo.Follows", "FollowerId");
        }
    }
}
