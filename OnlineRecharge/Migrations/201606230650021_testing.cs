namespace OnlineRecharge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LanguageDirections", "test", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LanguageDirections", "test");
        }
    }
}
