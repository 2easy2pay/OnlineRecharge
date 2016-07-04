namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletetestcolumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.LanguageDirections", "test");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LanguageDirections", "test", c => c.String());
        }
    }
}
