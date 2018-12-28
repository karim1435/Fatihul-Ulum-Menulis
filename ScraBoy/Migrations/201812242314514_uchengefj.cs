namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uchengefj : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Participants", "AssessmentId", "dbo.Assessments");
            DropIndex("dbo.Participants", new[] { "AssessmentId" });
            AddColumn("dbo.Participants", "Score", c => c.Int(nullable: false));
            AddColumn("dbo.Participants", "Message", c => c.String());
            DropColumn("dbo.Participants", "AssessmentId");
            DropTable("dbo.Assessments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Assessments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Participants", "AssessmentId", c => c.Int());
            DropColumn("dbo.Participants", "Message");
            DropColumn("dbo.Participants", "Score");
            CreateIndex("dbo.Participants", "AssessmentId");
            AddForeignKey("dbo.Participants", "AssessmentId", "dbo.Assessments", "Id");
        }
    }
}
