namespace BeerReviews.WebApi.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BeerReviews.WebApi.Data.BeerReviewsContext2>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BeerReviews.WebApi.Data.BeerReviewsContext2";
        }

        protected override void Seed(BeerReviews.WebApi.Data.BeerReviewsContext2 context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
