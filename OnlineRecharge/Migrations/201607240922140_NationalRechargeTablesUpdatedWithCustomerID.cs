namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NationalRechargeTablesUpdatedWithCustomerID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NationalRecharges", "CustomerID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NationalRecharges", "CustomerID");
        }
    }
}
