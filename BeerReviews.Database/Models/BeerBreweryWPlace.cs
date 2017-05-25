using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerReviews.Database.Models
{
    public class BeerBreweryWPlace
    {
        public int BreweryID { get; set; }
        public bool isPlace { get; set; }
        public string BreweryName { get; set; }
        public string CountryName { get; set; }
        public string City { get; set; }
    }
}
