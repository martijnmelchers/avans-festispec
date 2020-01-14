namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                        IsNonActive = c.DateTime(),
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
                        ContactDetails_PhoneNumber = c.String(maxLength: 50),
                        ContactDetails_EmailAddress = c.String(maxLength: 50),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Address_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.Address_Id);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ZipCode = c.String(nullable: false, maxLength: 10),
                        StreetName = c.String(nullable: false, maxLength: 50),
                        HouseNumber = c.Int(),
                        Suffix = c.String(maxLength: 10),
                        City = c.String(nullable: false, maxLength: 200),
                        Country = c.String(nullable: false, maxLength: 75),
                        Latitude = c.Single(nullable: false),
                        Longitude = c.Single(nullable: false),
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
                        EndTime = c.DateTime(),
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
                        Questionnaire_Id = c.Int(),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Festivals", t => t.Festival_Id)
                .ForeignKey("dbo.Questionnaires", t => t.Questionnaire_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Festival_Id)
                .Index(t => t.Questionnaire_Id)
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
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contents = c.String(nullable: false, maxLength: 250),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Options = c.String(),
                        Minimum = c.Int(),
                        Maximum = c.Int(),
                        Unit = c.Int(),
                        LowRatingDescription = c.String(),
                        HighRatingDescription = c.String(),
                        IsMultiline = c.Boolean(),
                        PicturePath = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Category_Id = c.Int(),
                        Questionnaire_Id = c.Int(nullable: false),
                        Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionCategories", t => t.Category_Id)
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
                        CategoryName = c.String(nullable: false, maxLength: 45),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questionnaires",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 45),
                        IsComplete = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Festival_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Festivals", t => t.Festival_Id, cascadeDelete: true)
                .Index(t => t.Festival_Id);
            
            CreateTable(
                "dbo.Festivals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FestivalName = c.String(nullable: false, maxLength: 45),
                        Description = c.String(nullable: false, maxLength: 250),
                        OpeningHours_StartTime = c.Time(nullable: false, precision: 7),
                        OpeningHours_EndTime = c.Time(nullable: false, precision: 7),
                        OpeningHours_StartDate = c.DateTime(nullable: false),
                        OpeningHours_EndDate = c.DateTime(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Address_Id = c.Int(nullable: false),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.Address_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KvkNr = c.Int(nullable: false),
                        CustomerName = c.String(nullable: false, maxLength: 20),
                        ContactDetails_PhoneNumber = c.String(maxLength: 50),
                        ContactDetails_EmailAddress = c.String(maxLength: 50),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Address_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.Address_Id);
            
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
                "dbo.DistanceResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Distance = c.Double(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Destination_Id = c.Int(nullable: false),
                        Origin_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Destination_Id)
                .ForeignKey("dbo.Addresses", t => t.Origin_Id)
                .Index(t => t.Destination_Id)
                .Index(t => t.Origin_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DistanceResults", "Origin_Id", "dbo.Addresses");
            DropForeignKey("dbo.DistanceResults", "Destination_Id", "dbo.Addresses");
            DropForeignKey("dbo.Attachments", "Answer_Id", "dbo.Answers");
            DropForeignKey("dbo.Accounts", "Id", "dbo.Employees");
            DropForeignKey("dbo.PlannedEvents", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.PlannedEvents", "Questionnaire_Id", "dbo.Questionnaires");
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Questions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Questions", "Questionnaire_Id", "dbo.Questionnaires");
            DropForeignKey("dbo.Questionnaires", "Festival_Id", "dbo.Festivals");
            DropForeignKey("dbo.PlannedEvents", "Festival_Id", "dbo.Festivals");
            DropForeignKey("dbo.Festivals", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Customers", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Festivals", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Questions", "Category_Id", "dbo.QuestionCategories");
            DropForeignKey("dbo.Answers", "PlannedInspection_Id", "dbo.PlannedEvents");
            DropForeignKey("dbo.Certificates", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.Employees", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.DistanceResults", new[] { "Origin_Id" });
            DropIndex("dbo.DistanceResults", new[] { "Destination_Id" });
            DropIndex("dbo.Attachments", new[] { "Answer_Id" });
            DropIndex("dbo.Customers", new[] { "Address_Id" });
            DropIndex("dbo.Festivals", new[] { "Customer_Id" });
            DropIndex("dbo.Festivals", new[] { "Address_Id" });
            DropIndex("dbo.Questionnaires", new[] { "Festival_Id" });
            DropIndex("dbo.Questions", new[] { "Question_Id" });
            DropIndex("dbo.Questions", new[] { "Questionnaire_Id" });
            DropIndex("dbo.Questions", new[] { "Category_Id" });
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropIndex("dbo.Answers", new[] { "PlannedInspection_Id" });
            DropIndex("dbo.PlannedEvents", new[] { "Employee_Id" });
            DropIndex("dbo.PlannedEvents", new[] { "Questionnaire_Id" });
            DropIndex("dbo.PlannedEvents", new[] { "Festival_Id" });
            DropIndex("dbo.Certificates", new[] { "Employee_Id" });
            DropIndex("dbo.Employees", new[] { "Address_Id" });
            DropIndex("dbo.Accounts", new[] { "Id" });
            DropTable("dbo.DistanceResults");
            DropTable("dbo.Attachments");
            DropTable("dbo.Customers");
            DropTable("dbo.Festivals");
            DropTable("dbo.Questionnaires");
            DropTable("dbo.QuestionCategories");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
            DropTable("dbo.PlannedEvents");
            DropTable("dbo.Certificates");
            DropTable("dbo.Addresses");
            DropTable("dbo.Employees");
            DropTable("dbo.Accounts");
        }
    }
}
