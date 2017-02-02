using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BeerReviews.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BeerReviews.Data
{
    public class BeerReviewsContext : DbContext
    {
        public BeerReviewsContext() : base("BeerReviewsContext") { }

        public DbSet<Beer> Beers { get; set; }
        public DbSet<BeerBrewery> BeerBreweries { get; set; }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<Country> Countries { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}