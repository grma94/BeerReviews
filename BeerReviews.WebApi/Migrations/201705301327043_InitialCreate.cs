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
                .ForeignKey("dbo.Beer", t => t.BeerID)
                .ForeignKey("dbo.Brewery", t => t.BreweryID)
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
                .ForeignKey("dbo.Style", t => t.StyleID)
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
                .ForeignKey("dbo.Beer", t => t.BeerID)
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
                .ForeignKey("dbo.Category", t => t.CategoryID)
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
                .ForeignKey("dbo.Country", t => t.CountryID)
                .Index(t => t.CountryID);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CountryID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Role", t => t.RoleId)
                .ForeignKey("dbo.User", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(maxLength: 150),
                        ClaimValue = c.String(maxLength: 500),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(maxLength: 500),
                        SecurityStamp = c.String(maxLength: 500),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        isAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Id", "dbo.User");
            DropForeignKey("dbo.UserRole", "IdentityUser_Id", "dbo.User");
            DropForeignKey("dbo.UserLogin", "IdentityUser_Id", "dbo.User");
            DropForeignKey("dbo.UserClaim", "IdentityUser_Id", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Brewery", "CountryID", "dbo.Country");
            DropForeignKey("dbo.BeerBrewery", "BreweryID", "dbo.Brewery");
            DropForeignKey("dbo.Style", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.Beer", "StyleID", "dbo.Style");
            DropForeignKey("dbo.Review", "BeerID", "dbo.Beer");
            DropForeignKey("dbo.BeerBrewery", "BeerID", "dbo.Beer");
            DropIndex("dbo.AspNetUsers", new[] { "Id" });
            DropIndex("dbo.User", "UserNameIndex");
            DropIndex("dbo.UserLogin", new[] { "IdentityUser_Id" });
            DropIndex("dbo.UserClaim", new[] { "IdentityUser_Id" });
            DropIndex("dbo.UserRole", new[] { "IdentityUser_Id" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.Role", "RoleNameIndex");
            DropIndex("dbo.Brewery", new[] { "CountryID" });
            DropIndex("dbo.Style", new[] { "CategoryID" });
            DropIndex("dbo.Review", new[] { "BeerID" });
            DropIndex("dbo.Beer", new[] { "StyleID" });
            DropIndex("dbo.BeerBrewery", new[] { "BreweryID" });
            DropIndex("dbo.BeerBrewery", new[] { "BeerID" });
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.User");
            DropTable("dbo.UserLogin");
            DropTable("dbo.UserClaim");
            DropTable("dbo.UserRole");
            DropTable("dbo.Role");
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
