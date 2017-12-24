namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Responses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommandId = c.Int(nullable: false),
                        Language = c.Byte(nullable: false),
                        SuccessText = c.String(maxLength: 8000, unicode: false),
                        ErrorText = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Commands", t => t.CommandId, cascadeDelete: true)
                .Index(t => t.CommandId);
            
            DropColumn("dbo.Commands", "ResponseText");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Commands", "ResponseText", c => c.String(maxLength: 8000, unicode: false));
            DropForeignKey("dbo.Responses", "CommandId", "dbo.Commands");
            DropIndex("dbo.Responses", new[] { "CommandId" });
            DropTable("dbo.Responses");
        }
    }
}
