namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionnaireQuestions", "Questionnaire_Id", "dbo.Questionnaires");
            DropForeignKey("dbo.QuestionnaireQuestions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.OpeningHours", "Festival_Id", "dbo.Festivals");
            DropForeignKey("dbo.Questionnaires", "Id", "dbo.PlannedEvents");
            DropIndex("dbo.OpeningHours", new[] { "Festival_Id" });
            DropIndex("dbo.QuestionnaireQuestions", new[] { "Questionnaire_Id" });
            DropIndex("dbo.QuestionnaireQuestions", new[] { "Question_Id" });
            DropColumn("dbo.OpeningHours", "Id");
            RenameColumn(table: "dbo.OpeningHours", name: "Festival_Id", newName: "Id");
            DropPrimaryKey("dbo.PlannedEvents");
            DropPrimaryKey("dbo.OpeningHours");
            CreateTable(
                "dbo.FullNames",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        First = c.String(nullable: false, maxLength: 40),
                        Middle = c.String(maxLength: 40),
                        Last = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Id)
                .ForeignKey("dbo.ContactPersons", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Questions", "Question_Id", c => c.Int());
            AddColumn("dbo.Questions", "Questionnaire_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Questionnaires", "Festival_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.PlannedEvents", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.OpeningHours", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.PlannedEvents", "Id");
            AddPrimaryKey("dbo.OpeningHours", "Id");
            CreateIndex("dbo.PlannedEvents", "Id");
            CreateIndex("dbo.OpeningHours", "Id");
            CreateIndex("dbo.Questionnaires", "Festival_Id");
            CreateIndex("dbo.Questions", "Question_Id");
            CreateIndex("dbo.Questions", "Questionnaire_Id");
            AddForeignKey("dbo.PlannedEvents", "Id", "dbo.Answers", "Id");
            AddForeignKey("dbo.Questions", "Question_Id", "dbo.Questions", "Id");
            AddForeignKey("dbo.Questions", "Questionnaire_Id", "dbo.Questionnaires", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Questionnaires", "Festival_Id", "dbo.Festivals", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OpeningHours", "Id", "dbo.Festivals", "Id");
            AddForeignKey("dbo.Questionnaires", "Id", "dbo.PlannedEvents", "Id");
            DropColumn("dbo.Employees", "EmployeeName");
            DropColumn("dbo.ContactPersons", "ContactPersonName");
            DropTable("dbo.QuestionnaireQuestions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.QuestionnaireQuestions",
                c => new
                    {
                        Questionnaire_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Questionnaire_Id, t.Question_Id });
            
            AddColumn("dbo.ContactPersons", "ContactPersonName", c => c.String(nullable: false, maxLength: 45));
            AddColumn("dbo.Employees", "EmployeeName", c => c.String(nullable: false, maxLength: 45));
            DropForeignKey("dbo.Questionnaires", "Id", "dbo.PlannedEvents");
            DropForeignKey("dbo.OpeningHours", "Id", "dbo.Festivals");
            DropForeignKey("dbo.Questionnaires", "Festival_Id", "dbo.Festivals");
            DropForeignKey("dbo.Questions", "Questionnaire_Id", "dbo.Questionnaires");
            DropForeignKey("dbo.Questions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.PlannedEvents", "Id", "dbo.Answers");
            DropForeignKey("dbo.FullNames", "Id", "dbo.ContactPersons");
            DropForeignKey("dbo.FullNames", "Id", "dbo.Employees");
            DropIndex("dbo.Questions", new[] { "Questionnaire_Id" });
            DropIndex("dbo.Questions", new[] { "Question_Id" });
            DropIndex("dbo.Questionnaires", new[] { "Festival_Id" });
            DropIndex("dbo.OpeningHours", new[] { "Id" });
            DropIndex("dbo.PlannedEvents", new[] { "Id" });
            DropIndex("dbo.FullNames", new[] { "Id" });
            DropPrimaryKey("dbo.OpeningHours");
            DropPrimaryKey("dbo.PlannedEvents");
            AlterColumn("dbo.OpeningHours", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.PlannedEvents", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Questionnaires", "Festival_Id");
            DropColumn("dbo.Questions", "Questionnaire_Id");
            DropColumn("dbo.Questions", "Question_Id");
            DropTable("dbo.FullNames");
            AddPrimaryKey("dbo.OpeningHours", "Id");
            AddPrimaryKey("dbo.PlannedEvents", "Id");
            RenameColumn(table: "dbo.OpeningHours", name: "Id", newName: "Festival_Id");
            AddColumn("dbo.OpeningHours", "Id", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.QuestionnaireQuestions", "Question_Id");
            CreateIndex("dbo.QuestionnaireQuestions", "Questionnaire_Id");
            CreateIndex("dbo.OpeningHours", "Festival_Id");
            AddForeignKey("dbo.Questionnaires", "Id", "dbo.PlannedEvents", "Id");
            AddForeignKey("dbo.OpeningHours", "Festival_Id", "dbo.Festivals", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QuestionnaireQuestions", "Question_Id", "dbo.Questions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QuestionnaireQuestions", "Questionnaire_Id", "dbo.Questionnaires", "Id", cascadeDelete: true);
        }
    }
}
