using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerReviews.Database.Models
{
    public class BeerBrewery
    {
        [Key, Column(Order = 0)]
        public int BeerID { get; set;}
        [Key, Column(Order = 1)]
        public int BreweryID { get; set; }
        [Key, Column(Order = 2)]
        public bool isPlace { get; set; }

        public virtual Beer Beer { get; set; }
        public virtual Brewery Brewery { get; set; }
    }
}