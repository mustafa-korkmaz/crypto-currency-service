namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version16 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TeamCryptoCurrencyBalances", new[] { "Name" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.TeamCryptoCurrencyBalances", "Name", unique: true);
        }
    }
}
