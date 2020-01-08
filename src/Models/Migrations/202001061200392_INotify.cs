namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class INotify : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "_question_Id", c => c.Int());
            CreateIndex("dbo.Questions", "_question_Id");
            AddForeignKey("dbo.Questions", "_question_Id", "dbo.Questions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "_question_Id", "dbo.Questions");
            DropIndex("dbo.Questions", new[] { "_question_Id" });
            DropColumn("dbo.Questions", "_question_Id");
        }
    }
}
