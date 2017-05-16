using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace BeerReviews.Database.Models
{
    public class Brewery
    {
        public int BreweryID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        [Required]
        public int CountryID { get; set; }
        public bool isLocked { get; set; }
        public string Url { get; set; }
        public string FbUrl { get; set; }
        public string ImageUrl { get; set; }
        public int BeersCount { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<BeerBrewery> BeerBreweries { get; set; }
    }
}