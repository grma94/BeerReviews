using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerReviews.Database.Models
{
    public class ReviewWName
    {
        public int ReviewID { get; set; }
        public int? Aroma { get; set; }
        public int? Taste { get; set; }
        public int? Palate { get; set; }
        public int? Apperance { get; set; }
        public double Overall { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public int BeerID { get; set; }

        public string BeerName { get; set; }
        public virtual ICollection<BeerBreweryWName> BeerBreweries { get; set; }
    }
}