namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixPlannedInspections : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questionnaires", "Id", "dbo.PlannedEvents");
            DropForeignKey("dbo.Questions", "Questionnaire_Id", "dbo.Questionnaires");
            DropIndex("dbo.Questionnaires", new[] { "Id" });
            DropPrimaryKey("dbo.Questionnaires");
            AddColumn("dbo.PlannedEvents", "Questionnaire_Id", c => c.Int());
            AlterColumn("dbo.Questionnaires", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Questionnaires", "Id");
            CreateIndex("dbo.PlannedEvents", "Questionnaire_Id");
            AddForeignKey("dbo.PlannedEvents", "Questionnaire_Id", "dbo.Questionnaires", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Questions", "Questionnaire_Id", "dbo.Questionnaires", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "Questionnaire_Id", "dbo.Questionnaires");
            DropForeignKey("dbo.PlannedEvents", "Questionnaire_Id", "dbo.Questionnaires");
            DropIndex("dbo.PlannedEvents", new[] { "Questionnaire_Id" });
            DropPrimaryKey("dbo.Questionnaires");
            AlterColumn("dbo.Questionnaires", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.PlannedEvents", "Questionnaire_Id");
            AddPrimaryKey("dbo.Questionnaires", "Id");
            CreateIndex("dbo.Questionnaires", "Id");
            AddForeignKey("dbo.Questions", "Questionnaire_Id", "dbo.Questionnaires", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Questionnaires", "Id", "dbo.PlannedEvents", "Id");
        }
    }
}
