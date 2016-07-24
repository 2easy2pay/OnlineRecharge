namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NationalRechargeTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NationalRecharges",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MobileNumber = c.String(nullable: false, maxLength: 20),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                .ForeignKey("dbo.NationalRechargeTypes", t => t.ID)
                .ForeignKey("dbo.ServiceProviders", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.NationalRechargeAPIResponseDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Response = c.String(nullable: false, maxLength: 20),
                        ResponseDescription = c.String(nullable: false, maxLength: 1000),
                        PaymentID = c.String(nullable: false, maxLength: 20),
                        PaymentRef = c.String(nullable: false, maxLength: 300),
                        Date = c.DateTime(nullable: false),
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
                .ForeignKey("dbo.NationalRecharges", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.NationalRechargePaymentDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PaymentID = c.String(nullable: false, maxLength: 20),
                        Result = c.String(nullable: false, maxLength: 20),
                        TrackID = c.String(nullable: false, maxLength: 20),
                        TransID = c.String(nullable: false, maxLength: 20),
                        Ref = c.String(nullable: false, maxLength: 20),
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
                .ForeignKey("dbo.NationalRecharges", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.NationalRechargeTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        DeletedBy = c.Int(),
                        DeletedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NationalRecharges", "ID", "dbo.ServiceProviders");
            DropForeignKey("dbo.NationalRecharges", "ID", "dbo.NationalRechargeTypes");
            DropForeignKey("dbo.NationalRechargePaymentDetails", "ID", "dbo.NationalRecharges");
            DropForeignKey("dbo.NationalRechargeAPIResponseDetails", "ID", "dbo.NationalRecharges");
            DropIndex("dbo.NationalRechargePaymentDetails", new[] { "ID" });
            DropIndex("dbo.NationalRechargeAPIResponseDetails", new[] { "ID" });
            DropIndex("dbo.NationalRecharges", new[] { "ID" });
            DropTable("dbo.NationalRechargeTypes");
            DropTable("dbo.NationalRechargePaymentDetails");
            DropTable("dbo.NationalRechargeAPIResponseDetails");
            DropTable("dbo.NationalRecharges");
        }
    }
}
