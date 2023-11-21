namespace Webapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TasksAddDueDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "DateDue", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "DateDue");
        }
    }
}
