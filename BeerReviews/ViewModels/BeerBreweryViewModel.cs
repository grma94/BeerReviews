using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BeerReviews.Models;
using System.ComponentModel.DataAnnotations;

namespace BeerReviews.ViewModels
{
    public class BeerBreweryViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0,100)]
        public double Abv { get; set; }
        [Range(0, 10000)]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public int? IBU { get; set; }
        [Range(0,100)]
        public double? Gravity { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public int StyleID { get; set; }

        public int BeerID { get; set; }
        //  public List<int> BreweriesID { get; set; }
        [Required]
          public List<string> BreweriesNames { get; set; }
        public List<string> BreweriesPlacesNames { get; set; }
        //   public int Brewery { get; set; }
    }
}