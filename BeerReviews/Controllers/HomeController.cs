using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeerReviews.Data;
using BeerReviews.Models;

namespace BeerReviews.Controllers
{
    public class HomeController : Controller
    {
        private BeerReviewsContext db = new BeerReviewsContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult SearchResults(string searchString)
        {
            var rBeers = db.Beers.Where(b => b.Name.Contains(searchString)).ToList();
            var rBreweries= db.Breweries.Where(b => b.Name.Contains(searchString)).ToList();
            var results = new Tuple<List<Beer>, List<Brewery>>(rBeers, rBreweries);
            return View(results);
        }
    }
}