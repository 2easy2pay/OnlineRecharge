namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NationalRechargeAPIResponseDetailsModified3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "OperatorName", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NationalRechargeAPIResponseDetails", "OperatorName", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
