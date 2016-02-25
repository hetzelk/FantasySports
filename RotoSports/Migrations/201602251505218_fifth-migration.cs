namespace RotoSports.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fifthmigration : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.NBAs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.NBAs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Position = c.String(),
                        Name = c.String(),
                        Salary = c.String(),
                        GameInfo = c.String(),
                        AvgPointsPerGame = c.String(),
                        teamAbbrev = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
