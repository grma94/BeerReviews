using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace BeerReviews.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        [Range(0,10)]
        public int? Aroma { get; set; }
        [Range(0, 15)]
        public int? Taste { get; set; }
        [Range(0, 5)]
        public int? Palate { get; set; }
        [Range(0, 5)]
        public int? Apperance { get; set; }
        public double Overall { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public int BeerID { get; set; }

        public virtual Beer Beer { get; set; }
    //    public virtual ApplicationUser User { get; set; }
    }
}