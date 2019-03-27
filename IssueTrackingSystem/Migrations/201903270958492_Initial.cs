namespace IssueTrackingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Spaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
                        AssignedTo_Id = c.Int(),
                        CreatedBy_Id = c.Int(),
                        Space_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.AssignedTo_Id)
                .ForeignKey("dbo.UserProfile", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Spaces", t => t.Space_Id)
                .Index(t => t.AssignedTo_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Space_Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "Space_Id", "dbo.Spaces");
            DropForeignKey("dbo.Tickets", "CreatedBy_Id", "dbo.UserProfile");
            DropForeignKey("dbo.Tickets", "AssignedTo_Id", "dbo.UserProfile");
            DropIndex("dbo.Tickets", new[] { "Space_Id" });
            DropIndex("dbo.Tickets", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Tickets", new[] { "AssignedTo_Id" });
            DropTable("dbo.UserProfile");
            DropTable("dbo.Tickets");
            DropTable("dbo.Spaces");
        }
    }
}
