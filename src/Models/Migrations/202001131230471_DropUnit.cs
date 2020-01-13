namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropUnit : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Questions", "Unit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "Unit", c => c.Int());
        }
    }
}
