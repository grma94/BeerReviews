using System;
using System.Collections.Generic;


namespace BeerReviews.Database.Models
{
    public class BeerWithAvg
    {
        public int BeerID { get; set; }
        public string Name { get; set; }
        public double Abv { get; set; }
        public int? IBU { get; set; }
        public double? Gravity { get; set; }
        public bool isLocked { get; set; }
        public string ImageUrl { get; set; }
        public int StyleID { get; set; }
        public string StyleName { get; set; }
        public double ReviewsAvg { get; set; }
        public int ReviewsCount { get; set; }
        public virtual ICollection<BeerBreweryWName> BeerBreweries { get; set; }
    }
}
