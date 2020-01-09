﻿using System.Data.Entity.Migrations;

namespace Festispec.Models.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Accounts",
                    c => new
                    {
                        Id = c.Int(false),
                        Username = c.String(false, 45),
                        Password = c.String(false, 100),
                        IsNonActive = c.DateTime(),
                        Role = c.Int(false),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                    "dbo.Employees",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name_First = c.String(false, 40),
                        Name_Middle = c.String(maxLength: 40),
                        Name_Last = c.String(false, 40),
                        Iban = c.String(false, 30),
                        ContactDetails_PhoneNumber = c.String(maxLength: 50),
                        ContactDetails_EmailAddress = c.String(maxLength: 50),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        Address_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.Address_Id);

            CreateTable(
                    "dbo.Addresses",
                    c => new
                    {
                        Id = c.Int(false, true),
                        ZipCode = c.String(false, 10),
                        StreetName = c.String(false, 50),
                        HouseNumber = c.Int(),
                        Suffix = c.String(maxLength: 10),
                        City = c.String(false, 200),
                        Country = c.String(false, 75),
                        Latitude = c.Single(false),
                        Longitude = c.Single(false)
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.Certificates",
                    c => new
                    {
                        Id = c.Int(false, true),
                        CertificateTitle = c.String(false, 45),
                        CertificationDate = c.DateTime(false),
                        ExpirationDate = c.DateTime(false),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        Employee_Id = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, true)
                .Index(t => t.Employee_Id);

            CreateTable(
                    "dbo.PlannedEvents",
                    c => new
                    {
                        Id = c.Int(false, true),
                        StartTime = c.DateTime(false),
                        EndTime = c.DateTime(false),
                        EventTitle = c.String(false, 45),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        IsAvailable = c.Boolean(),
                        Reason = c.String(maxLength: 250),
                        WorkedHours = c.Int(),
                        WorkedHoursAccepted = c.DateTime(),
                        CancellationReason = c.String(maxLength: 250),
                        IsCancelled = c.DateTime(),
                        Discriminator = c.String(false, 128),
                        Festival_Id = c.Int(),
                        Questionnaire_Id = c.Int(),
                        Employee_Id = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Festivals", t => t.Festival_Id)
                .ForeignKey("dbo.Questionnaires", t => t.Questionnaire_Id, true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, true)
                .Index(t => t.Festival_Id)
                .Index(t => t.Questionnaire_Id)
                .Index(t => t.Employee_Id);

            CreateTable(
                    "dbo.Answers",
                    c => new
                    {
                        Id = c.Int(false, true),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        MultipleChoiceAnswerKey = c.Int(),
                        IntAnswer = c.Int(),
                        AnswerContents = c.String(),
                        UploadedFilePath = c.String(),
                        Discriminator = c.String(false, 128),
                        PlannedInspection_Id = c.Int(false),
                        Question_Id = c.Int(false)
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
                        Id = c.Int(false, true),
                        FilePath = c.String(false),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        Answer_Id = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.Answer_Id, true)
                .Index(t => t.Answer_Id);

            CreateTable(
                    "dbo.Questions",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Contents = c.String(false, 250),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        Options = c.String(),
                        Minimum = c.Int(),
                        Maximum = c.Int(),
                        Unit = c.Int(),
                        LowRatingDescription = c.String(),
                        HighRatingDescription = c.String(),
                        IsMultiline = c.Boolean(),
                        PicturePath = c.String(),
                        Discriminator = c.String(false, 128),
                        Category_Id = c.Int(),
                        Questionnaire_Id = c.Int(false),
                        Question_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionCategories", t => t.Category_Id)
                .ForeignKey("dbo.Questionnaires", t => t.Questionnaire_Id, true)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.Questionnaire_Id)
                .Index(t => t.Question_Id);

            CreateTable(
                    "dbo.QuestionCategories",
                    c => new
                    {
                        Id = c.Int(false, true),
                        CategoryName = c.String(false, 45),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false)
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.Questionnaires",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(false, 45),
                        IsComplete = c.DateTime(),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        Festival_Id = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Festivals", t => t.Festival_Id, true)
                .Index(t => t.Festival_Id);

            CreateTable(
                    "dbo.Festivals",
                    c => new
                    {
                        Id = c.Int(false, true),
                        FestivalName = c.String(false, 45),
                        Description = c.String(false, 250),
                        OpeningHours_StartTime = c.Time(false, 7),
                        OpeningHours_EndTime = c.Time(false, 7),
                        OpeningHours_StartDate = c.DateTime(false),
                        OpeningHours_EndDate = c.DateTime(false),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        Address_Id = c.Int(false),
                        Customer_Id = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id, true)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, true)
                .Index(t => t.Address_Id)
                .Index(t => t.Customer_Id);

            CreateTable(
                    "dbo.Customers",
                    c => new
                    {
                        Id = c.Int(false, true),
                        KvkNr = c.Int(false),
                        CustomerName = c.String(false, 20),
                        ContactDetails_PhoneNumber = c.String(maxLength: 50),
                        ContactDetails_EmailAddress = c.String(maxLength: 50),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        Address_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.Address_Id);

            CreateTable(
                    "dbo.ContactPersons",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Role = c.String(false, 20),
                        Name_First = c.String(false, 40),
                        Name_Middle = c.String(maxLength: 40),
                        Name_Last = c.String(false, 40),
                        ContactDetails_PhoneNumber = c.String(maxLength: 50),
                        ContactDetails_EmailAddress = c.String(maxLength: 50),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        Customer_Id = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, true)
                .Index(t => t.Customer_Id);

            CreateTable(
                    "dbo.ContactPersonNotes",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Note = c.String(false, 500),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        ContactPerson_Id = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactPersons", t => t.ContactPerson_Id, true)
                .Index(t => t.ContactPerson_Id);

            CreateTable(
                    "dbo.Reports",
                    c => new
                    {
                        Id = c.Int(false),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Festivals", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                    "dbo.ReportEntries",
                    c => new
                    {
                        Id = c.Int(false),
                        Order = c.Int(false),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        GraphXAxisType = c.Int(),
                        GraphType = c.Int(),
                        XAxisLabel = c.String(),
                        YAxisLabel = c.String(),
                        Header = c.String(),
                        Text = c.String(),
                        Discriminator = c.String(false, 128),
                        Report_Id = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.Id)
                .ForeignKey("dbo.Reports", t => t.Report_Id, true)
                .Index(t => t.Id)
                .Index(t => t.Report_Id);

            CreateTable(
                    "dbo.DistanceResults",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Distance = c.Double(false),
                        CreatedAt = c.DateTime(false),
                        UpdatedAt = c.DateTime(false),
                        Destination_Id = c.Int(false),
                        Origin_Id = c.Int(false)
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
            DropForeignKey("dbo.Accounts", "Id", "dbo.Employees");
            DropForeignKey("dbo.PlannedEvents", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.PlannedEvents", "Questionnaire_Id", "dbo.Questionnaires");
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
            DropForeignKey("dbo.Customers", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Festivals", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Questions", "Category_Id", "dbo.QuestionCategories");
            DropForeignKey("dbo.Answers", "PlannedInspection_Id", "dbo.PlannedEvents");
            DropForeignKey("dbo.Attachments", "Answer_Id", "dbo.Answers");
            DropForeignKey("dbo.Certificates", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.Employees", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.DistanceResults", new[] {"Origin_Id"});
            DropIndex("dbo.DistanceResults", new[] {"Destination_Id"});
            DropIndex("dbo.ReportEntries", new[] {"Report_Id"});
            DropIndex("dbo.ReportEntries", new[] {"Id"});
            DropIndex("dbo.Reports", new[] {"Id"});
            DropIndex("dbo.ContactPersonNotes", new[] {"ContactPerson_Id"});
            DropIndex("dbo.ContactPersons", new[] {"Customer_Id"});
            DropIndex("dbo.Customers", new[] {"Address_Id"});
            DropIndex("dbo.Festivals", new[] {"Customer_Id"});
            DropIndex("dbo.Festivals", new[] {"Address_Id"});
            DropIndex("dbo.Questionnaires", new[] {"Festival_Id"});
            DropIndex("dbo.Questions", new[] {"Question_Id"});
            DropIndex("dbo.Questions", new[] {"Questionnaire_Id"});
            DropIndex("dbo.Questions", new[] {"Category_Id"});
            DropIndex("dbo.Attachments", new[] {"Answer_Id"});
            DropIndex("dbo.Answers", new[] {"Question_Id"});
            DropIndex("dbo.Answers", new[] {"PlannedInspection_Id"});
            DropIndex("dbo.PlannedEvents", new[] {"Employee_Id"});
            DropIndex("dbo.PlannedEvents", new[] {"Questionnaire_Id"});
            DropIndex("dbo.PlannedEvents", new[] {"Festival_Id"});
            DropIndex("dbo.Certificates", new[] {"Employee_Id"});
            DropIndex("dbo.Employees", new[] {"Address_Id"});
            DropIndex("dbo.Accounts", new[] {"Id"});
            DropTable("dbo.DistanceResults");
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
            DropTable("dbo.Addresses");
            DropTable("dbo.Employees");
            DropTable("dbo.Accounts");
        }
    }
}