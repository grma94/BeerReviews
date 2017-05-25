namespace BeerReviews.WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BeerBrewery",
                c => new
                    {
                        BeerID = c.Int(nullable: false),
                        BreweryID = c.Int(nullable: false),
                        isPlace = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.BeerID, t.BreweryID, t.isPlace })
                .ForeignKey("dbo.Beer", t => t.BeerID, cascadeDelete: true)
                .ForeignKey("dbo.Brewery", t => t.BreweryID, cascadeDelete: true)
                .Index(t => t.BeerID)
                .Index(t => t.BreweryID);
            
            CreateTable(
                "dbo.Beer",
                c => new
                    {
                        BeerID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Abv = c.Double(nullable: false),
                        IBU = c.Int(),
                        Gravity = c.Double(),
                        Description = c.String(),
                        isLocked = c.Boolean(nullable: false),
                        ImageUrl = c.String(),
                        StyleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BeerID)
                .ForeignKey("dbo.Style", t => t.StyleID, cascadeDelete: true)
                .Index(t => t.StyleID);
            
            CreateTable(
                "dbo.Review",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        Aroma = c.Int(),
                        Taste = c.Int(),
                        Palate = c.Int(),
                        Apperance = c.Int(),
                        Overall = c.Double(nullable: false),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        Date = c.DateTime(nullable: false),
                        UserName = c.String(),
                        BeerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewID)
                .ForeignKey("dbo.Beer", t => t.BeerID, cascadeDelete: true)
                .Index(t => t.BeerID);
            
            CreateTable(
                "dbo.Style",
                c => new
                    {
                        StyleID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StyleID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Brewery",
                c => new
                    {
                        BreweryID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Phone = c.String(),
                        Description = c.String(),
                        Street = c.String(),
                        PostalCode = c.String(),
                        City = c.String(),
                        CountryID = c.Int(nullable: false),
                        isLocked = c.Boolean(nullable: false),
                        Url = c.String(),
                        FbUrl = c.String(),
                        ImageUrl = c.String(),
                        BeersCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BreweryID)
                .ForeignKey("dbo.Country", t => t.CountryID, cascadeDelete: true)
                .Index(t => t.CountryID);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CountryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Brewery", "CountryID", "dbo.Country");
            DropForeignKey("dbo.BeerBrewery", "BreweryID", "dbo.Brewery");
            DropForeignKey("dbo.Style", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.Beer", "StyleID", "dbo.Style");
            DropForeignKey("dbo.Review", "BeerID", "dbo.Beer");
            DropForeignKey("dbo.BeerBrewery", "BeerID", "dbo.Beer");
            DropIndex("dbo.Brewery", new[] { "CountryID" });
            DropIndex("dbo.Style", new[] { "CategoryID" });
            DropIndex("dbo.Review", new[] { "BeerID" });
            DropIndex("dbo.Beer", new[] { "StyleID" });
            DropIndex("dbo.BeerBrewery", new[] { "BreweryID" });
            DropIndex("dbo.BeerBrewery", new[] { "BeerID" });
            DropTable("dbo.Country");
            DropTable("dbo.Brewery");
            DropTable("dbo.Category");
            DropTable("dbo.Style");
            DropTable("dbo.Review");
            DropTable("dbo.Beer");
            DropTable("dbo.BeerBrewery");
        }
    }
}
