using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BeerReviews.WebApi.Models
{
    public class Beer
    {
        public int BeerID { get; set; }
        public string Name { get; set; }
        public double Abv { get; set; }
        public int? IBU { get; set; }
        public double? Gravity { get; set; }
        public string Description { get; set; }
        public bool isLocked { get; set; }
        public string ImageUrl { get; set; }
        public int StyleID { get; set; }

        public virtual Style Style { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<BeerBrewery> BeerBreweries { get; set; }

    }
}