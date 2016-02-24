namespace RotoSports.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secondmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CSVFiles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Title = c.String(),
                        Sport = c.String(),
                        File = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
        
        public override void Down()
        {
            DropTable("dbo.NBAs");
            DropTable("dbo.CSVFiles");
        }
    }
}
