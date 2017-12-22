namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DropExceptions", newName: "CryptoCurrencyExceptions");
            AddColumn("dbo.CryptoCurrencies", "ClassName", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CryptoCurrencies", "ClassName");
            RenameTable(name: "dbo.CryptoCurrencyExceptions", newName: "DropExceptions");
        }
    }
}
