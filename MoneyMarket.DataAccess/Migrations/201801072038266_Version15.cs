namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version15 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Teams", "Channel", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Teams", "Channel", c => c.String(maxLength: 50, unicode: false));
        }
    }
}
