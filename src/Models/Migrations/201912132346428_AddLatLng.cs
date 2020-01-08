namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLatLng : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Address_Latitude", c => c.Single(nullable: false));
            AddColumn("dbo.Employees", "Address_Longitude", c => c.Single(nullable: false));
            AddColumn("dbo.Festivals", "Address_Latitude", c => c.Single(nullable: false));
            AddColumn("dbo.Festivals", "Address_Longitude", c => c.Single(nullable: false));
            AddColumn("dbo.Customers", "Address_Latitude", c => c.Single(nullable: false));
            AddColumn("dbo.Customers", "Address_Longitude", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Address_Longitude");
            DropColumn("dbo.Customers", "Address_Latitude");
            DropColumn("dbo.Festivals", "Address_Longitude");
            DropColumn("dbo.Festivals", "Address_Latitude");
            DropColumn("dbo.Employees", "Address_Longitude");
            DropColumn("dbo.Employees", "Address_Latitude");
        }
    }
}
