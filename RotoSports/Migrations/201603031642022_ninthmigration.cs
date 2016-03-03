namespace RotoSports.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ninthmigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lineups", "RequiredPositions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lineups", "RequiredPositions");
        }
    }
}
