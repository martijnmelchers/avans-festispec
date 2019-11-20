namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequiredQuestionCategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "Category_Id", "dbo.QuestionCategories");
            DropIndex("dbo.Questions", new[] { "Category_Id" });
            AlterColumn("dbo.Questions", "Category_Id", c => c.Int());
            CreateIndex("dbo.Questions", "Category_Id");
            AddForeignKey("dbo.Questions", "Category_Id", "dbo.QuestionCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "Category_Id", "dbo.QuestionCategories");
            DropIndex("dbo.Questions", new[] { "Category_Id" });
            AlterColumn("dbo.Questions", "Category_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Questions", "Category_Id");
            AddForeignKey("dbo.Questions", "Category_Id", "dbo.QuestionCategories", "Id", cascadeDelete: true);
        }
    }
}
