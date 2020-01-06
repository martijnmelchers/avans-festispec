namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class beschikbaarheid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PlannedEvents", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PlannedEvents", "EndTime", c => c.DateTime());
        }
    }
}
