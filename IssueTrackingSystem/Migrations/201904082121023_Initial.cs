namespace IssueTrackingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        Ticket_Id = c.Int(),
                        CreatedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.Ticket_Id)
                .ForeignKey("dbo.UserProfile", t => t.CreatedBy_Id)
                .Index(t => t.Ticket_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        Name = c.String(),
                        Lastname = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Status = c.Int(nullable: false),
                        Description = c.String(),
                        Eta = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy_Id = c.Int(nullable: false),
                        AssignedTo_Id = c.Int(nullable: false),
                        Space_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.AssignedTo_Id)
                .ForeignKey("dbo.UserProfile", t => t.CreatedBy_Id, cascadeDelete: true)
                .ForeignKey("dbo.Spaces", t => t.Space_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.AssignedTo_Id)
                .Index(t => t.Space_Id);
            
            CreateTable(
                "dbo.Spaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpaceUserProfiles",
                c => new
                    {
                        Space_Id = c.Int(nullable: false),
                        UserProfile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Space_Id, t.UserProfile_Id })
                .ForeignKey("dbo.Spaces", t => t.Space_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_Id, cascadeDelete: true)
                .Index(t => t.Space_Id)
                .Index(t => t.UserProfile_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "CreatedBy_Id", "dbo.UserProfile");
            DropForeignKey("dbo.Tickets", "Space_Id", "dbo.Spaces");
            DropForeignKey("dbo.SpaceUserProfiles", "UserProfile_Id", "dbo.UserProfile");
            DropForeignKey("dbo.SpaceUserProfiles", "Space_Id", "dbo.Spaces");
            DropForeignKey("dbo.Tickets", "CreatedBy_Id", "dbo.UserProfile");
            DropForeignKey("dbo.Comments", "Ticket_Id", "dbo.Tickets");
            DropForeignKey("dbo.Tickets", "AssignedTo_Id", "dbo.UserProfile");
            DropIndex("dbo.SpaceUserProfiles", new[] { "UserProfile_Id" });
            DropIndex("dbo.SpaceUserProfiles", new[] { "Space_Id" });
            DropIndex("dbo.Tickets", new[] { "Space_Id" });
            DropIndex("dbo.Tickets", new[] { "AssignedTo_Id" });
            DropIndex("dbo.Tickets", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Comments", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Comments", new[] { "Ticket_Id" });
            DropTable("dbo.SpaceUserProfiles");
            DropTable("dbo.Spaces");
            DropTable("dbo.Tickets");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Comments");
        }
    }
}
