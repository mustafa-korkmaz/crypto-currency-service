namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CryptoCurrencies", "ClassName", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CryptoCurrencies", "ClassName", c => c.String(maxLength: 20, unicode: false));
        }
    }
}
