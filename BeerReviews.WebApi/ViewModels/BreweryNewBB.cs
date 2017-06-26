using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerReviews.WebApi.ViewModels
{
    public class BreweryNewBB
    {
        public int BreweryID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public int CountryID { get; set; }
        public bool isLocked { get; set; }
        public string Url { get; set; }
        public string FbUrl { get; set; }
        public string ImageUrl { get; set; }
        public int BeersCount { get; set; }
        public double ReviewsAvg { get; set; }
        public string CountryName { get; set; }
        public virtual ICollection<BeerBreweryWName> BeerBreweries { get; set; }
    }
}