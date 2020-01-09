namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EndDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PlannedEvents", "EndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PlannedEvents", "EndTime", c => c.DateTime(nullable: false));
        }
    }
}
