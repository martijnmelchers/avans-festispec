namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Notes", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Notes");
        }
    }
}
