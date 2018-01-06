namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version12 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CryptoCurrencies", "UsdValue", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("dbo.TeamCryptoCurrencyBalances", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TeamCryptoCurrencyBalances", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.CryptoCurrencies", "UsdValue", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
    }
}
