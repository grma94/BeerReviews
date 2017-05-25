using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerReviews.WebApi.Models
{
    public class BeerFDetails
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
        public string StyleName { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<BeerBreweryWPlace> BeerBreweries { get; set; }
    }
}
