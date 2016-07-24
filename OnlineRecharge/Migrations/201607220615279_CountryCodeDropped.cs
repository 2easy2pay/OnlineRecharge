namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountryCodeDropped : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Countries", "Code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Countries", "Code", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
