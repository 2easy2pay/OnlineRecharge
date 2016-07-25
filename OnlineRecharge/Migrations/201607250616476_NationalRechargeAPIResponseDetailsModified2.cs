namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NationalRechargeAPIResponseDetailsModified2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "Password", c => c.String(maxLength: 20));
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "RechargeCode", c => c.String(maxLength: 50));
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "SerialNo", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "SerialNo", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "RechargeCode", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "Password", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
