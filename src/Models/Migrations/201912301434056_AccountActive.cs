namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "IsNonActive", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "IsNonActive");
        }
    }
}
