namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceProviderTableUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceProviders", "Code", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceProviders", "Code");
        }
    }
}
