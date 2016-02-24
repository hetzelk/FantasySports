namespace RotoSports.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirdmigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CSVFiles", "Details", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CSVFiles", "Details");
        }
    }
}
