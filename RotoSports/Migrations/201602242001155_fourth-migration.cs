namespace RotoSports.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fourthmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lineups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Title = c.String(),
                        Sport = c.String(),
                        Details = c.String(),
                        SingleLineup = c.String(),
                        PlayerList = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Lineups");
        }
    }
}
