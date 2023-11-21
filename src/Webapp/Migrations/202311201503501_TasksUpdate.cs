namespace Webapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TasksUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TasksStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tasks", "TaskStatusId", c => c.Int());
            AlterColumn("dbo.Tasks", "HighPriority", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Tasks", "TaskStatusId");
            AddForeignKey("dbo.Tasks", "TaskStatusId", "dbo.TasksStatus", "Id");
            DropColumn("dbo.Tasks", "CompletedYN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "CompletedYN", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Tasks", "TaskStatusId", "dbo.TasksStatus");
            DropIndex("dbo.Tasks", new[] { "TaskStatusId" });
            AlterColumn("dbo.Tasks", "HighPriority", c => c.Boolean());
            DropColumn("dbo.Tasks", "TaskStatusId");
            DropTable("dbo.TasksStatus");
        }
    }
}
