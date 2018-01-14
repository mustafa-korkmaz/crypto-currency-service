namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamInvestments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        Currency = c.Byte(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamInvestments", "TeamId", "dbo.Teams");
            DropIndex("dbo.TeamInvestments", new[] { "TeamId" });
            DropTable("dbo.TeamInvestments");
        }
    }
}
