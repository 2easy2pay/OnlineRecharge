namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Shoppingcardstables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShoppingCardAPIResponseDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Response = c.String(),
                        Denomination = c.Decimal(precision: 18, scale: 2),
                        OperatorName = c.String(maxLength: 20),
                        Password = c.String(maxLength: 20),
                        RechargeCode = c.String(maxLength: 50),
                        SerialNo = c.String(maxLength: 50),
                        ShoppingCardsID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ShoppingCards", t => t.ShoppingCardsID, cascadeDelete: true)
                .Index(t => t.ShoppingCardsID);
            
            CreateTable(
                "dbo.ShoppingCards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShoppingCardTypesID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        DeletedBy = c.Int(),
                        DeletedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ShoppingCardTypes", t => t.ShoppingCardTypesID, cascadeDelete: true)
                .Index(t => t.ShoppingCardTypesID);
            
            CreateTable(
                "dbo.ShoppingCardTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Code = c.String(nullable: false, maxLength: 10),
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
            
            CreateTable(
                "dbo.ShoppingCardPaymentDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PaymentID = c.String(nullable: false, maxLength: 50),
                        Result = c.String(nullable: false, maxLength: 50),
                        TrackID = c.String(nullable: false, maxLength: 50),
                        TransID = c.String(nullable: false, maxLength: 50),
                        Ref = c.String(nullable: false, maxLength: 50),
                        ShoppingCardID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ShoppingCards", t => t.ShoppingCardID, cascadeDelete: true)
                .Index(t => t.ShoppingCardID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShoppingCardPaymentDetails", "ShoppingCardID", "dbo.ShoppingCards");
            DropForeignKey("dbo.ShoppingCardAPIResponseDetails", "ShoppingCardsID", "dbo.ShoppingCards");
            DropForeignKey("dbo.ShoppingCards", "ShoppingCardTypesID", "dbo.ShoppingCardTypes");
            DropIndex("dbo.ShoppingCardPaymentDetails", new[] { "ShoppingCardID" });
            DropIndex("dbo.ShoppingCards", new[] { "ShoppingCardTypesID" });
            DropIndex("dbo.ShoppingCardAPIResponseDetails", new[] { "ShoppingCardsID" });
            DropTable("dbo.ShoppingCardPaymentDetails");
            DropTable("dbo.ShoppingCardTypes");
            DropTable("dbo.ShoppingCards");
            DropTable("dbo.ShoppingCardAPIResponseDetails");
        }
    }
}
