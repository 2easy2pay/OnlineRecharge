namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LanguageDirections",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Direction = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        DeletedBy = c.Int(),
                        DeletedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LanguageDirections", t => t.ID)
                .ForeignKey("dbo.LanguageFlagNames", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.LanguageFlagNames",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Languages", "ID", "dbo.LanguageFlagNames");
            DropForeignKey("dbo.Languages", "ID", "dbo.LanguageDirections");
            DropIndex("dbo.Languages", new[] { "ID" });
            DropTable("dbo.LanguageFlagNames");
            DropTable("dbo.Languages");
            DropTable("dbo.LanguageDirections");
        }
    }
}
