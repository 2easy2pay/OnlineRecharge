namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removesforiegnkeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NationalRechargePaymentDetails", "ID", "dbo.NationalRecharges");
            DropForeignKey("dbo.NationalRecharges", "ID", "dbo.NationalRechargeTypes");
            DropForeignKey("dbo.NationalRecharges", "ID", "dbo.ServiceProviders");
            DropForeignKey("dbo.NationalRechargeAPIResponseDetails", "ID", "dbo.NationalRecharges");
            DropIndex("dbo.NationalRechargeAPIResponseDetails", new[] { "ID" });
            DropIndex("dbo.NationalRecharges", new[] { "ID" });
            DropIndex("dbo.NationalRechargePaymentDetails", new[] { "ID" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.NationalRechargePaymentDetails", "ID");
            CreateIndex("dbo.NationalRecharges", "ID");
            CreateIndex("dbo.NationalRechargeAPIResponseDetails", "ID");
            AddForeignKey("dbo.NationalRechargeAPIResponseDetails", "ID", "dbo.NationalRecharges", "ID");
            AddForeignKey("dbo.NationalRecharges", "ID", "dbo.ServiceProviders", "ID");
            AddForeignKey("dbo.NationalRecharges", "ID", "dbo.NationalRechargeTypes", "ID");
            AddForeignKey("dbo.NationalRechargePaymentDetails", "ID", "dbo.NationalRecharges", "ID");
        }
    }
}
