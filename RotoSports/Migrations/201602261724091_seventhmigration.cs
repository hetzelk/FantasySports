namespace RotoSports.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seventhmigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CSVFiles", "BaseTitleList", c => c.String());
            AddColumn("dbo.Lineups", "BaseTitleList", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lineups", "BaseTitleList");
            DropColumn("dbo.CSVFiles", "BaseTitleList");
        }
    }
}
