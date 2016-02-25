namespace RotoSports.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sixthmigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lineups", "FileConnection", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lineups", "FileConnection");
        }
    }
}
