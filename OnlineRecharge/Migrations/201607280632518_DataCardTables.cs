namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataCardTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataCardRechargeAPIResponseDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Response = c.String(nullable: false, maxLength: 20),
                        ResponseDescription = c.String(maxLength: 1000),
                        PaymentID = c.String(maxLength: 20),
                        PaymentRef = c.String(maxLength: 300),
                        Date = c.DateTime(),
                        Denomination = c.Decimal(precision: 18, scale: 2),
                        OperatorName = c.String(maxLength: 20),
                        Password = c.String(maxLength: 20),
                        RechargeCode = c.String(maxLength: 50),
                        SerialNo = c.String(maxLength: 50),
                        DataCardRechargeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DataCardRecharges", t => t.DataCardRechargeID, cascadeDelete: true)
                .Index(t => t.DataCardRechargeID);
            
            CreateTable(
                "dbo.DataCardRecharges",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        MobileNumber = c.String(nullable: false, maxLength: 20),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ServiceProviderID = c.Int(nullable: false),
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
                .ForeignKey("dbo.ServiceProviders", t => t.ServiceProviderID, cascadeDelete: true)
                .Index(t => t.ServiceProviderID);
            
            CreateTable(
                "dbo.DataCardRechargePaymentDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PaymentID = c.String(nullable: false, maxLength: 50),
                        Result = c.String(nullable: false, maxLength: 50),
                        TrackID = c.String(nullable: false, maxLength: 50),
                        TransID = c.String(nullable: false, maxLength: 50),
                        Ref = c.String(nullable: false, maxLength: 50),
                        DataCardRechargeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DataCardRecharges", t => t.DataCardRechargeID, cascadeDelete: true)
                .Index(t => t.DataCardRechargeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DataCardRechargePaymentDetails", "DataCardRechargeID", "dbo.DataCardRecharges");
            DropForeignKey("dbo.DataCardRechargeAPIResponseDetails", "DataCardRechargeID", "dbo.DataCardRecharges");
            DropForeignKey("dbo.DataCardRecharges", "ServiceProviderID", "dbo.ServiceProviders");
            DropIndex("dbo.DataCardRechargePaymentDetails", new[] { "DataCardRechargeID" });
            DropIndex("dbo.DataCardRecharges", new[] { "ServiceProviderID" });
            DropIndex("dbo.DataCardRechargeAPIResponseDetails", new[] { "DataCardRechargeID" });
            DropTable("dbo.DataCardRechargePaymentDetails");
            DropTable("dbo.DataCardRecharges");
            DropTable("dbo.DataCardRechargeAPIResponseDetails");
        }
    }
}
