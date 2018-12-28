namespace ScraBoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contestfeature : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assessments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                        Winner = c.Boolean(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Competitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        SlugUrl = c.String(),
                        MinimumWord = c.Int(nullable: false),
                        MaximumWord = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        StartedOn = c.DateTime(nullable: false),
                        EndOn = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreatorId = c.String(nullable: false, maxLength: 128),
                        UrlImage = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Participants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        FileUrl = c.String(),
                        ProposedOn = c.DateTime(nullable: false),
                        CompetitionId = c.Int(nullable: false),
                        AuthorId = c.String(maxLength: 128),
                        AssessmentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assessments", t => t.AssessmentId)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .Index(t => t.CompetitionId)
                .Index(t => t.AuthorId)
                .Index(t => t.AssessmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Participants", "CompetitionId", "dbo.Competitions");
            DropForeignKey("dbo.Participants", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Participants", "AssessmentId", "dbo.Assessments");
            DropForeignKey("dbo.Competitions", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Participants", new[] { "AssessmentId" });
            DropIndex("dbo.Participants", new[] { "AuthorId" });
            DropIndex("dbo.Participants", new[] { "CompetitionId" });
            DropIndex("dbo.Competitions", new[] { "CreatorId" });
            DropTable("dbo.Participants");
            DropTable("dbo.Competitions");
            DropTable("dbo.Assessments");
        }
    }
}
