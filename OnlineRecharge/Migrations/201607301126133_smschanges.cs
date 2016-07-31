namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class smschanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SmsDetails",
                c => new
                    {
                        SmsDetailsId = c.Int(nullable: false, identity: true),
                        CountryCode = c.String(maxLength: 20),
                        PhoneNumber = c.String(),
                        Date = c.DateTime(nullable: false),
                        DeviceType = c.String(),
                        Status = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.SmsDetailsId);
            
            CreateTable(
                "dbo.SmsGatewayDetails",
                c => new
                    {
                        SmsGatewayDetailsId = c.Int(nullable: false, identity: true),
                        SmsGateway = c.String(nullable: false),
                        SmsGatewayCode = c.Int(nullable: false),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        ApiId = c.String(maxLength: 50),
                        CreatedDate = c.DateTime(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.SmsGatewayDetailsId);
            
            AddColumn("dbo.ServiceProviders", "Dialer", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceProviders", "Dialer");
            DropTable("dbo.SmsGatewayDetails");
            DropTable("dbo.SmsDetails");
        }
    }
}
