namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RechargeParameters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RechargeParameters",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ServiceType = c.String(nullable: false, maxLength: 30),
                        RechargeType = c.String(nullable: false, maxLength: 30),
                        OperatorCode = c.String(nullable: false, maxLength: 30),
                        MobileNumber = c.String(nullable: false, maxLength: 30),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        DeletedBy = c.Int(),
                        DeletedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RechargeParameters");
        }
    }
}
