namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NationalRechargeAPIResponseDetailsModified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "Denomination", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "OperatorName", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "Password", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "RechargeCode", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "SerialNo", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "ResponseDescription", c => c.String(maxLength: 1000));
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "PaymentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "PaymentRef", c => c.String(maxLength: 300));
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "PaymentRef", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "PaymentID", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "ResponseDescription", c => c.String(nullable: false, maxLength: 1000));
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "SerialNo");
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "RechargeCode");
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "Password");
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "OperatorName");
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "Denomination");
        }
    }
}
