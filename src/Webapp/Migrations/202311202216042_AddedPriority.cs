namespace Webapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPriority : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Priorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tasks", "PriorityId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tasks", "PriorityId");
            AddForeignKey("dbo.Tasks", "PriorityId", "dbo.Priorities", "Id", cascadeDelete: true);
            DropColumn("dbo.Tasks", "HighPriority");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "HighPriority", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Tasks", "PriorityId", "dbo.Priorities");
            DropIndex("dbo.Tasks", new[] { "PriorityId" });
            DropColumn("dbo.Tasks", "PriorityId");
            DropTable("dbo.Priorities");
        }
    }
}
