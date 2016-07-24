namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class childTableBaseFieldsDropped : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "IsActive");
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "IsDeleted");
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "CreatedBy");
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "CreatedDate");
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "ModifiedBy");
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "ModifiedDate");
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "DeletedBy");
            DropColumn("dbo.NationalRechargeAPIResponseDetails", "DeletedDate");
            DropColumn("dbo.NationalRechargePaymentDetails", "IsActive");
            DropColumn("dbo.NationalRechargePaymentDetails", "IsDeleted");
            DropColumn("dbo.NationalRechargePaymentDetails", "CreatedBy");
            DropColumn("dbo.NationalRechargePaymentDetails", "CreatedDate");
            DropColumn("dbo.NationalRechargePaymentDetails", "ModifiedBy");
            DropColumn("dbo.NationalRechargePaymentDetails", "ModifiedDate");
            DropColumn("dbo.NationalRechargePaymentDetails", "DeletedBy");
            DropColumn("dbo.NationalRechargePaymentDetails", "DeletedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NationalRechargePaymentDetails", "DeletedDate", c => c.DateTime());
            AddColumn("dbo.NationalRechargePaymentDetails", "DeletedBy", c => c.Int());
            AddColumn("dbo.NationalRechargePaymentDetails", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.NationalRechargePaymentDetails", "ModifiedBy", c => c.Int());
            AddColumn("dbo.NationalRechargePaymentDetails", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.NationalRechargePaymentDetails", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.NationalRechargePaymentDetails", "IsDeleted", c => c.Boolean());
            AddColumn("dbo.NationalRechargePaymentDetails", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "DeletedDate", c => c.DateTime());
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "DeletedBy", c => c.Int());
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "ModifiedBy", c => c.Int());
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "IsDeleted", c => c.Boolean());
            AddColumn("dbo.NationalRechargeAPIResponseDetails", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
