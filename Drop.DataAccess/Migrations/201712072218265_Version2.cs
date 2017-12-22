namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CryptoCurrencies", "UsdValue", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CryptoCurrencies", "UsdValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
