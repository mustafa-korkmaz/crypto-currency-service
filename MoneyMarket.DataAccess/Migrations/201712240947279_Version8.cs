namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Commands", "Text", c => c.String(nullable: false, maxLength: 255, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Commands", "Text");
        }
    }
}
