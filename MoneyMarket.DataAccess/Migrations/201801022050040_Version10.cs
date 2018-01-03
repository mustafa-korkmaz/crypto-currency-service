namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamCryptoCurrencyBalances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        CryptoCurrencyId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CryptoCurrencies", t => t.CryptoCurrencyId, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.CryptoCurrencyId);
            
            AddColumn("dbo.Responses", "Depth", c => c.Byte(nullable: false));
            AddColumn("dbo.Teams", "MainCurrency", c => c.Byte(nullable: false));
            AddColumn("dbo.Teams", "Provider", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamCryptoCurrencyBalances", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.TeamCryptoCurrencyBalances", "CryptoCurrencyId", "dbo.CryptoCurrencies");
            DropIndex("dbo.TeamCryptoCurrencyBalances", new[] { "CryptoCurrencyId" });
            DropIndex("dbo.TeamCryptoCurrencyBalances", new[] { "TeamId" });
            DropColumn("dbo.Teams", "Provider");
            DropColumn("dbo.Teams", "MainCurrency");
            DropColumn("dbo.Responses", "Depth");
            DropTable("dbo.TeamCryptoCurrencyBalances");
        }
    }
}
