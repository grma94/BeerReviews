﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BeerReviews.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BeerReviews.Data
{
    public class BeerReviewsContext : IdentityDbContext<ApplicationUser>
    {
        public BeerReviewsContext() : base("BeerReviewsContext") { }

        public DbSet<Beer> Beers { get; set; }
        public DbSet<BeerBrewery> BeerBreweries { get; set; }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<IdentityUserLogin> UserLogins { get; set; }
        public DbSet<IdentityUserClaim> UserClaims { get; set; }
        public DbSet<IdentityUserRole> UserRoles { get; set; }

        public static BeerReviewsContext Create()
        {
            return new BeerReviewsContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Configure Asp Net Identity Tables
            modelBuilder.Entity<IdentityUser>().ToTable("User");
            modelBuilder.Entity<IdentityUser>().Property(u => u.PasswordHash).HasMaxLength(500);
            modelBuilder.Entity<IdentityUser>().Property(u => u.SecurityStamp).HasMaxLength(500);
         //   modelBuilder.Entity<IdentityUser>().Property(u => u.PhoneNumber).HasMaxLength(50);

            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserClaim>().Property(u => u.ClaimType).HasMaxLength(150);
            modelBuilder.Entity<IdentityUserClaim>().Property(u => u.ClaimValue).HasMaxLength(500);
        }
    }
}