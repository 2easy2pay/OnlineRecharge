namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreasedSizeOfNationalRechargePaymentDetails : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NationalRechargePaymentDetails", "PaymentID", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.NationalRechargePaymentDetails", "Result", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.NationalRechargePaymentDetails", "TrackID", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.NationalRechargePaymentDetails", "TransID", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.NationalRechargePaymentDetails", "Ref", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NationalRechargePaymentDetails", "Ref", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.NationalRechargePaymentDetails", "TransID", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.NationalRechargePaymentDetails", "TrackID", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.NationalRechargePaymentDetails", "Result", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.NationalRechargePaymentDetails", "PaymentID", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
