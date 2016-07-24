namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedforiegnkeys : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "NationalRechargeID", c => c.Int(nullable: false));
            AddColumn("dbo.NationalRechargePaymentDetails", "NationalRechargeID", c => c.Int(nullable: false));
            AddColumn("dbo.NationalRecharges", "ServiceProviderID", c => c.Int(nullable: false));
            CreateIndex("dbo.NationalRechargeAPIResponseDetails", "NationalRechargeID");
            CreateIndex("dbo.NationalRecharges", "ServiceProviderID");
            CreateIndex("dbo.NationalRechargePaymentDetails", "NationalRechargeID");
            AddForeignKey("dbo.NationalRecharges", "ServiceProviderID", "dbo.ServiceProviders", "ID", cascadeDelete: true);
            AddForeignKey("dbo.NationalRechargeAPIResponseDetails", "NationalRechargeID", "dbo.NationalRecharges", "ID", cascadeDelete: true);
            AddForeignKey("dbo.NationalRechargePaymentDetails", "NationalRechargeID", "dbo.NationalRecharges", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NationalRechargePaymentDetails", "NationalRechargeID", "dbo.NationalRecharges");
            DropForeignKey("dbo.NationalRechargeAPIResponseDetails", "NationalRechargeID", "dbo.NationalRecharges");
            DropForeignKey("dbo.NationalRecharges", "ServiceProviderID", "dbo.ServiceProviders");
            DropIndex("dbo.NationalRechargePaymentDetails", new[] { "NationalRechargeID" });
            DropIndex("dbo.NationalRecharges", new[] { "ServiceProviderID" });
            DropIndex("dbo.NationalRechargeAPIResponseDetails", new[] { "NationalRechargeID" });
            DropColumn("dbo.NationalRecharges", "ServiceProviderID");
            DropColumn("dbo.NationalRechargePaymentDetails", "NationalRechargeID");
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "NationalRechargeID");
        }
    }
}
