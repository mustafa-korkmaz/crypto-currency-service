namespace MoneyMarket.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Commands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScopeId = c.Int(nullable: false),
                        Action = c.String(nullable: false, maxLength: 20, unicode: false),
                        ResponseText = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Scopes", t => t.ScopeId, cascadeDelete: true)
                .Index(t => t.ScopeId);
            
            CreateTable(
                "dbo.Scopes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeamScopes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        ScopeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Scopes", t => t.ScopeId, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.ScopeId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SlackId = c.String(nullable: false, maxLength: 50, unicode: false),
                        BotId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 255, unicode: false),
                        BotAccessToken = c.String(nullable: false, maxLength: 255, unicode: false),
                        Language = c.Byte(nullable: false),
                        AccountType = c.Byte(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0, storeType: "datetime2"),
                        ExpiresIn = c.DateTime(nullable: false, precision: 0, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Settings", "Key", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamScopes", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.TeamScopes", "ScopeId", "dbo.Scopes");
            DropForeignKey("dbo.Commands", "ScopeId", "dbo.Scopes");
            DropIndex("dbo.TeamScopes", new[] { "ScopeId" });
            DropIndex("dbo.TeamScopes", new[] { "TeamId" });
            DropIndex("dbo.Commands", new[] { "ScopeId" });
            AlterColumn("dbo.Settings", "Key", c => c.Byte(nullable: false));
            DropTable("dbo.Teams");
            DropTable("dbo.TeamScopes");
            DropTable("dbo.Scopes");
            DropTable("dbo.Commands");
        }
    }
}
