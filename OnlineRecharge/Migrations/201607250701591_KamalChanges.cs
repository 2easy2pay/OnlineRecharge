namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KamalChanges : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Countries", newName: "InternationalServiceProviders");
            CreateTable(
                "dbo.Country",
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
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.InternationalServiceProviders", "Code", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.InternationalServiceProviders", "ID");
            AddForeignKey("dbo.InternationalServiceProviders", "ID", "dbo.Country", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InternationalServiceProviders", "ID", "dbo.Country");
            DropIndex("dbo.InternationalServiceProviders", new[] { "ID" });
            DropColumn("dbo.InternationalServiceProviders", "Code");
            DropTable("dbo.Country");
            RenameTable(name: "dbo.InternationalServiceProviders", newName: "Countries");
        }
    }
}
