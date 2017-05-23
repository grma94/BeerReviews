using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerReviews.WebApi.ViewModels
{
    public class BeerBreweryWName
    {
        public int BeerID { get; set; }
        public int BreweryID { get; set; }
        public bool isPlace { get; set; }

        public string BeerName { get; set; } 
        public string BreweryName { get; set; }
        public virtual BeerWithAvg Beer { get; set; }
    }
}