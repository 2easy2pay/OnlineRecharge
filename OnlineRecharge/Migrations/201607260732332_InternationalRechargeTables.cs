namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InternationalRechargeTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InternationalServiceProviders", "ID", "dbo.Country");
            DropIndex("dbo.InternationalServiceProviders", new[] { "ID" });
            CreateTable(
                "dbo.InternationalRechargeAPIResponseDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Response = c.String(nullable: false, maxLength: 20),
                        ResponseDescription = c.String(nullable: false, maxLength: 1000),
                        PaymentID = c.String(nullable: false, maxLength: 20),
                        PaymentRef = c.String(nullable: false, maxLength: 300),
                        Date = c.DateTime(nullable: false),
                        InternationalRechargeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.InternationalRecharges", t => t.InternationalRechargeID, cascadeDelete: true)
                .Index(t => t.InternationalRechargeID);
            
            CreateTable(
                "dbo.InternationalRecharges",
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
                .ForeignKey("dbo.InternationalServiceProviders", t => t.ServiceProviderID, cascadeDelete: true)
                .Index(t => t.ServiceProviderID);
            
            CreateTable(
                "dbo.InternationalRechargePaymentDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PaymentID = c.String(nullable: false, maxLength: 50),
                        Result = c.String(nullable: false, maxLength: 50),
                        TrackID = c.String(nullable: false, maxLength: 50),
                        TransID = c.String(nullable: false, maxLength: 50),
                        Ref = c.String(nullable: false, maxLength: 50),
                        InternationalRechargeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.InternationalRecharges", t => t.InternationalRechargeID, cascadeDelete: true)
                .Index(t => t.InternationalRechargeID);
            
            AddColumn("dbo.InternationalServiceProviders", "CountryID", c => c.Int(nullable: false));
            CreateIndex("dbo.InternationalServiceProviders", "CountryID");
            AddForeignKey("dbo.InternationalServiceProviders", "CountryID", "dbo.Country", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InternationalServiceProviders", "CountryID", "dbo.Country");
            DropForeignKey("dbo.InternationalRechargePaymentDetails", "InternationalRechargeID", "dbo.InternationalRecharges");
            DropForeignKey("dbo.InternationalRechargeAPIResponseDetails", "InternationalRechargeID", "dbo.InternationalRecharges");
            DropForeignKey("dbo.InternationalRecharges", "ServiceProviderID", "dbo.InternationalServiceProviders");
            DropIndex("dbo.InternationalRechargePaymentDetails", new[] { "InternationalRechargeID" });
            DropIndex("dbo.InternationalServiceProviders", new[] { "CountryID" });
            DropIndex("dbo.InternationalRecharges", new[] { "ServiceProviderID" });
            DropIndex("dbo.InternationalRechargeAPIResponseDetails", new[] { "InternationalRechargeID" });
            DropColumn("dbo.InternationalServiceProviders", "CountryID");
            DropTable("dbo.InternationalRechargePaymentDetails");
            DropTable("dbo.InternationalRecharges");
            DropTable("dbo.InternationalRechargeAPIResponseDetails");
            CreateIndex("dbo.InternationalServiceProviders", "ID");
            AddForeignKey("dbo.InternationalServiceProviders", "ID", "dbo.Country", "ID");
        }
    }
}
