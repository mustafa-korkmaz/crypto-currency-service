namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        NotificationType = c.Byte(nullable: false),
                        Key = c.String(nullable: false, maxLength: 50, unicode: false),
                        TimeInterval = c.Int(nullable: false),
                        LastExecutedAt = c.DateTime(nullable: false, precision: 0, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamNotifications", "TeamId", "dbo.Teams");
            DropIndex("dbo.TeamNotifications", new[] { "TeamId" });
            DropTable("dbo.TeamNotifications");
        }
    }
}
