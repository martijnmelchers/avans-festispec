namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OpeningHoursChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Festivals", "OpeningHours_StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Festivals", "OpeningHours_EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Festivals", "OpeningHours_StartTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Festivals", "OpeningHours_EndTime", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Festivals", "OpeningHours_EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Festivals", "OpeningHours_StartTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Festivals", "OpeningHours_EndDate");
            DropColumn("dbo.Festivals", "OpeningHours_StartDate");
        }
    }
}
