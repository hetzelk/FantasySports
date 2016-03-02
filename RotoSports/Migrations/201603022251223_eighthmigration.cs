namespace RotoSports.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eighthmigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CSVFiles", "GameDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CSVFiles", "GameDate");
        }
    }
}
