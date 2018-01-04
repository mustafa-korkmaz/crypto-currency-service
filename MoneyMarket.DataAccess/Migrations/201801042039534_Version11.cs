namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TeamCryptoCurrencyBalances", "CryptoCurrencyId", "dbo.CryptoCurrencies");
            DropIndex("dbo.TeamCryptoCurrencyBalances", new[] { "CryptoCurrencyId" });
            AddColumn("dbo.TeamCryptoCurrencyBalances", "Currency", c => c.Byte(nullable: false));
            CreateIndex("dbo.TeamCryptoCurrencyBalances", "Name", unique: true);
            DropColumn("dbo.TeamCryptoCurrencyBalances", "CryptoCurrencyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TeamCryptoCurrencyBalances", "CryptoCurrencyId", c => c.Int(nullable: false));
            DropIndex("dbo.TeamCryptoCurrencyBalances", new[] { "Name" });
            DropColumn("dbo.TeamCryptoCurrencyBalances", "Currency");
            CreateIndex("dbo.TeamCryptoCurrencyBalances", "CryptoCurrencyId");
            AddForeignKey("dbo.TeamCryptoCurrencyBalances", "CryptoCurrencyId", "dbo.CryptoCurrencies", "Id", cascadeDelete: true);
        }
    }
}
