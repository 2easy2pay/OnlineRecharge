namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rechargetypeforiegnkeywithnationalrecharge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NationalRecharges", "RechargeTypeID", c => c.Int(nullable: false));
            CreateIndex("dbo.NationalRecharges", "RechargeTypeID");
            AddForeignKey("dbo.NationalRecharges", "RechargeTypeID", "dbo.NationalRechargeTypes", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NationalRecharges", "RechargeTypeID", "dbo.NationalRechargeTypes");
            DropIndex("dbo.NationalRecharges", new[] { "RechargeTypeID" });
            DropColumn("dbo.NationalRecharges", "RechargeTypeID");
        }
    }
}
