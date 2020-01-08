namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressToSeperateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ZipCode = c.String(nullable: false, maxLength: 10),
                        StreetName = c.String(nullable: false, maxLength: 50),
                        HouseNumber = c.Int(),
                        Suffix = c.String(maxLength: 10),
                        City = c.String(nullable: false, maxLength: 200),
                        Country = c.String(nullable: false, maxLength: 75),
                        Latitude = c.Single(nullable: false),
                        Longitude = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Employees", "Address_Id", c => c.Int());
            AddColumn("dbo.Festivals", "Address_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "Address_Id", c => c.Int());
            CreateIndex("dbo.Employees", "Address_Id");
            CreateIndex("dbo.Festivals", "Address_Id");
            CreateIndex("dbo.Customers", "Address_Id");
            AddForeignKey("dbo.Employees", "Address_Id", "dbo.Addresses", "Id");
            AddForeignKey("dbo.Festivals", "Address_Id", "dbo.Addresses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Customers", "Address_Id", "dbo.Addresses", "Id");
            DropColumn("dbo.Employees", "Address_ZipCode");
            DropColumn("dbo.Employees", "Address_StreetName");
            DropColumn("dbo.Employees", "Address_HouseNumber");
            DropColumn("dbo.Employees", "Address_Suffix");
            DropColumn("dbo.Employees", "Address_City");
            DropColumn("dbo.Employees", "Address_Country");
            DropColumn("dbo.Employees", "Address_Latitude");
            DropColumn("dbo.Employees", "Address_Longitude");
            DropColumn("dbo.Festivals", "Address_ZipCode");
            DropColumn("dbo.Festivals", "Address_StreetName");
            DropColumn("dbo.Festivals", "Address_HouseNumber");
            DropColumn("dbo.Festivals", "Address_Suffix");
            DropColumn("dbo.Festivals", "Address_City");
            DropColumn("dbo.Festivals", "Address_Country");
            DropColumn("dbo.Festivals", "Address_Latitude");
            DropColumn("dbo.Festivals", "Address_Longitude");
            DropColumn("dbo.Customers", "Address_ZipCode");
            DropColumn("dbo.Customers", "Address_StreetName");
            DropColumn("dbo.Customers", "Address_HouseNumber");
            DropColumn("dbo.Customers", "Address_Suffix");
            DropColumn("dbo.Customers", "Address_City");
            DropColumn("dbo.Customers", "Address_Country");
            DropColumn("dbo.Customers", "Address_Latitude");
            DropColumn("dbo.Customers", "Address_Longitude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Address_Longitude", c => c.Single(nullable: false));
            AddColumn("dbo.Customers", "Address_Latitude", c => c.Single(nullable: false));
            AddColumn("dbo.Customers", "Address_Country", c => c.String(nullable: false, maxLength: 75));
            AddColumn("dbo.Customers", "Address_City", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.Customers", "Address_Suffix", c => c.String(maxLength: 10));
            AddColumn("dbo.Customers", "Address_HouseNumber", c => c.Int());
            AddColumn("dbo.Customers", "Address_StreetName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Customers", "Address_ZipCode", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Festivals", "Address_Longitude", c => c.Single(nullable: false));
            AddColumn("dbo.Festivals", "Address_Latitude", c => c.Single(nullable: false));
            AddColumn("dbo.Festivals", "Address_Country", c => c.String(nullable: false, maxLength: 75));
            AddColumn("dbo.Festivals", "Address_City", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.Festivals", "Address_Suffix", c => c.String(maxLength: 10));
            AddColumn("dbo.Festivals", "Address_HouseNumber", c => c.Int());
            AddColumn("dbo.Festivals", "Address_StreetName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Festivals", "Address_ZipCode", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Employees", "Address_Longitude", c => c.Single(nullable: false));
            AddColumn("dbo.Employees", "Address_Latitude", c => c.Single(nullable: false));
            AddColumn("dbo.Employees", "Address_Country", c => c.String(nullable: false, maxLength: 75));
            AddColumn("dbo.Employees", "Address_City", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.Employees", "Address_Suffix", c => c.String(maxLength: 10));
            AddColumn("dbo.Employees", "Address_HouseNumber", c => c.Int());
            AddColumn("dbo.Employees", "Address_StreetName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Employees", "Address_ZipCode", c => c.String(nullable: false, maxLength: 10));
            DropForeignKey("dbo.Customers", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Festivals", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Employees", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.Customers", new[] { "Address_Id" });
            DropIndex("dbo.Festivals", new[] { "Address_Id" });
            DropIndex("dbo.Employees", new[] { "Address_Id" });
            DropColumn("dbo.Customers", "Address_Id");
            DropColumn("dbo.Festivals", "Address_Id");
            DropColumn("dbo.Employees", "Address_Id");
            DropTable("dbo.Addresses");
        }
    }
}
