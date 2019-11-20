namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedToVragenlijstBeheren : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Answer1", c => c.String());
            AddColumn("dbo.Questions", "Answer2", c => c.String());
            AddColumn("dbo.Questions", "Answer3", c => c.String());
            AddColumn("dbo.Questions", "Answer4", c => c.String());
            AddColumn("dbo.Questionnaires", "Name", c => c.String(nullable: false, maxLength: 45));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questionnaires", "Name");
            DropColumn("dbo.Questions", "Answer4");
            DropColumn("dbo.Questions", "Answer3");
            DropColumn("dbo.Questions", "Answer2");
            DropColumn("dbo.Questions", "Answer1");
        }
    }
}
