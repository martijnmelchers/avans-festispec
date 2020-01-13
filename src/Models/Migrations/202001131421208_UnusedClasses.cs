namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnusedClasses : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "Category_Id", "dbo.QuestionCategories");
            DropForeignKey("dbo.Attachments", "Answer_Id", "dbo.Answers");
            DropIndex("dbo.Questions", new[] { "Category_Id" });
            DropIndex("dbo.Attachments", new[] { "Answer_Id" });
            DropColumn("dbo.Questions", "Category_Id");
            DropTable("dbo.QuestionCategories");
            DropTable("dbo.Attachments");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
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
            
            AddColumn("dbo.Questions", "Category_Id", c => c.Int());
            CreateIndex("dbo.Attachments", "Answer_Id");
            CreateIndex("dbo.Questions", "Category_Id");
            AddForeignKey("dbo.Attachments", "Answer_Id", "dbo.Answers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Questions", "Category_Id", "dbo.QuestionCategories", "Id");
        }
    }
}
