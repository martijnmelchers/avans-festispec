namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InspectionInstructions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlannedEvents", "Instructions", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlannedEvents", "Instructions");
        }
    }
}
