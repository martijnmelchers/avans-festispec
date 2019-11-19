namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Username = c.String(nullable: false, maxLength: 45),
                        Password = c.String(nullable: false, maxLength: 100),
                        Role = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name_First = c.String(nullable: false, maxLength: 40),
                        Name_Middle = c.String(maxLength: 40),
                        Name_Last = c.String(nullable: false, maxLength: 40),
                        Iban = c.String(nullable: false, maxLength: 30),
                        Address_ZipCode = c.String(nullable: false, maxLength: 10),
                        Address_StreetName = c.String(nullable: false, maxLength: 50),
                        Address_HouseNumber = c.Int(),
                        Address_Suffix = c.String(maxLength: 10),
                        Address_City = c.String(nullable: false, maxLength: 50),
                        Address_Country = c.String(nullable: false, maxLength: 75),
                        ContactDetails_PhoneNumber = c.String(maxLength: 50),
                        ContactDetails_EmailAddress = c.String(maxLength: 50),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Certificates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CertificateTitle = c.String(nullable: false, maxLength: 45),
                        CertificationDate = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "dbo.PlannedEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        EventTitle = c.String(nullable: false, maxLength: 45),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        IsAvailable = c.Boolean(),
                        Reason = c.String(maxLength: 250),
                        WorkedHours = c.Int(),
                        WorkedHoursAccepted = c.DateTime(),
                        CancellationReason = c.String(maxLength: 250),
                        IsCancelled = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Festival_Id = c.Int(),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Festivals", t => t.Festival_Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Festival_Id)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        MultipleChoiceAnswerKey = c.Int(),
                        IntAnswer = c.Int(),
                        AnswerContents = c.String(),
                        UploadedFilePath = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        PlannedInspection_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlannedEvents", t => t.PlannedInspection_Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .Index(t => t.PlannedInspection_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilePath = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Answer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.Answer_Id, cascadeDelete: true)
                .Index(t => t.Answer_Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contents = c.String(nullable: false, maxLength: 250),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Minimum = c.Int(),
                        Maximum = c.Int(),
                        Unit = c.Int(),
                        LowRatingDescription = c.String(),
                        HighRatingDescription = c.String(),
                        IsMultiline = c.Boolean(),
                        PicturePath = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Category_Id = c.Int(nullable: false),
                        Questionnaire_Id = c.Int(nullable: false),
                        Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionCategories", t => t.Category_Id, cascadeDelete: true)
                .ForeignKey("dbo.Questionnaires", t => t.Questionnaire_Id, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.Questionnaire_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.QuestionCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(maxLength: 45),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questionnaires",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IsComplete = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Festival_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Festivals", t => t.Festival_Id, cascadeDelete: true)
                .ForeignKey("dbo.PlannedEvents", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.Festival_Id);
            
            CreateTable(
                "dbo.Festivals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FestivalName = c.String(nullable: false, maxLength: 45),
                        Description = c.String(nullable: false, maxLength: 250),
                        Address_ZipCode = c.String(nullable: false, maxLength: 10),
                        Address_StreetName = c.String(nullable: false, maxLength: 50),
                        Address_HouseNumber = c.Int(),
                        Address_Suffix = c.String(maxLength: 10),
                        Address_City = c.String(nullable: false, maxLength: 50),
                        Address_Country = c.String(nullable: false, maxLength: 75),
                        OpeningHours_StartTime = c.DateTime(nullable: false),
                        OpeningHours_EndTime = c.DateTime(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KvkNr = c.Int(nullable: false),
                        CustomerName = c.String(nullable: false, maxLength: 20),
                        Address_ZipCode = c.String(nullable: false, maxLength: 10),
                        Address_StreetName = c.String(nullable: false, maxLength: 50),
                        Address_HouseNumber = c.Int(),
                        Address_Suffix = c.String(maxLength: 10),
                        Address_City = c.String(nullable: false, maxLength: 50),
                        Address_Country = c.String(nullable: false, maxLength: 75),
                        ContactDetails_PhoneNumber = c.String(maxLength: 50),
                        ContactDetails_EmailAddress = c.String(maxLength: 50),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactPersons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role = c.String(nullable: false, maxLength: 20),
                        Name_First = c.String(nullable: false, maxLength: 40),
                        Name_Middle = c.String(maxLength: 40),
                        Name_Last = c.String(nullable: false, maxLength: 40),
                        ContactDetails_PhoneNumber = c.String(maxLength: 50),
                        ContactDetails_EmailAddress = c.String(maxLength: 50),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.ContactPersonNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Note = c.String(nullable: false, maxLength: 500),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        ContactPerson_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactPersons", t => t.ContactPerson_Id, cascadeDelete: true)
                .Index(t => t.ContactPerson_Id);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Festivals", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ReportEntries",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        GraphXAxisType = c.Int(),
                        GraphType = c.Int(),
                        XAxisLabel = c.String(),
                        YAxisLabel = c.String(),
                        Header = c.String(),
                        Text = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Report_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.Id)
                .ForeignKey("dbo.Reports", t => t.Report_Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Report_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "Id", "dbo.Employees");
            DropForeignKey("dbo.PlannedEvents", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.Questionnaires", "Id", "dbo.PlannedEvents");
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Questions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Questions", "Questionnaire_Id", "dbo.Questionnaires");
            DropForeignKey("dbo.Reports", "Id", "dbo.Festivals");
            DropForeignKey("dbo.ReportEntries", "Report_Id", "dbo.Reports");
            DropForeignKey("dbo.ReportEntries", "Id", "dbo.Questions");
            DropForeignKey("dbo.Questionnaires", "Festival_Id", "dbo.Festivals");
            DropForeignKey("dbo.PlannedEvents", "Festival_Id", "dbo.Festivals");
            DropForeignKey("dbo.Festivals", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.ContactPersonNotes", "ContactPerson_Id", "dbo.ContactPersons");
            DropForeignKey("dbo.ContactPersons", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Questions", "Category_Id", "dbo.QuestionCategories");
            DropForeignKey("dbo.Answers", "PlannedInspection_Id", "dbo.PlannedEvents");
            DropForeignKey("dbo.Attachments", "Answer_Id", "dbo.Answers");
            DropForeignKey("dbo.Certificates", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.ReportEntries", new[] { "Report_Id" });
            DropIndex("dbo.ReportEntries", new[] { "Id" });
            DropIndex("dbo.Reports", new[] { "Id" });
            DropIndex("dbo.ContactPersonNotes", new[] { "ContactPerson_Id" });
            DropIndex("dbo.ContactPersons", new[] { "Customer_Id" });
            DropIndex("dbo.Festivals", new[] { "Customer_Id" });
            DropIndex("dbo.Questionnaires", new[] { "Festival_Id" });
            DropIndex("dbo.Questionnaires", new[] { "Id" });
            DropIndex("dbo.Questions", new[] { "Question_Id" });
            DropIndex("dbo.Questions", new[] { "Questionnaire_Id" });
            DropIndex("dbo.Questions", new[] { "Category_Id" });
            DropIndex("dbo.Attachments", new[] { "Answer_Id" });
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropIndex("dbo.Answers", new[] { "PlannedInspection_Id" });
            DropIndex("dbo.PlannedEvents", new[] { "Employee_Id" });
            DropIndex("dbo.PlannedEvents", new[] { "Festival_Id" });
            DropIndex("dbo.Certificates", new[] { "Employee_Id" });
            DropIndex("dbo.Accounts", new[] { "Id" });
            DropTable("dbo.ReportEntries");
            DropTable("dbo.Reports");
            DropTable("dbo.ContactPersonNotes");
            DropTable("dbo.ContactPersons");
            DropTable("dbo.Customers");
            DropTable("dbo.Festivals");
            DropTable("dbo.Questionnaires");
            DropTable("dbo.QuestionCategories");
            DropTable("dbo.Questions");
            DropTable("dbo.Attachments");
            DropTable("dbo.Answers");
            DropTable("dbo.PlannedEvents");
            DropTable("dbo.Certificates");
            DropTable("dbo.Employees");
            DropTable("dbo.Accounts");
        }
    }
}
