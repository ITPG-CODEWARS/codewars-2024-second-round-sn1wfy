namespace ShortIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShortenedURLs",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        OwnerId = c.Guid(nullable: false),
                        Url = c.String(),
                        MaxUses = c.Int(nullable: false),
                        OpenedTimes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Username = c.String(),
                        Password = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.ShortenedURLs");
        }
    }
}
