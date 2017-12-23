namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version7 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Teams", "SlackId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Teams", new[] { "SlackId" });
        }
    }
}
