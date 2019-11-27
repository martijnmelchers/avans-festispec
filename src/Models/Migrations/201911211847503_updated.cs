namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Address_City", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.QuestionCategories", "CategoryName", c => c.String(nullable: false, maxLength: 45));
            AlterColumn("dbo.Festivals", "Address_City", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Customers", "Address_City", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "Address_City", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Festivals", "Address_City", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.QuestionCategories", "CategoryName", c => c.String(maxLength: 45));
            AlterColumn("dbo.Employees", "Address_City", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
