namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDistanceResult : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DistanceResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Distance = c.Double(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Destination_Id = c.Int(nullable: false),
                        Origin_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Destination_Id)
                .ForeignKey("dbo.Addresses", t => t.Origin_Id)
                .Index(t => t.Destination_Id)
                .Index(t => t.Origin_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DistanceResults", "Origin_Id", "dbo.Addresses");
            DropForeignKey("dbo.DistanceResults", "Destination_Id", "dbo.Addresses");
            DropIndex("dbo.DistanceResults", new[] { "Origin_Id" });
            DropIndex("dbo.DistanceResults", new[] { "Destination_Id" });
            DropTable("dbo.DistanceResults");
        }
    }
}
