using BeerReviews.WebApi.Data;
using BeerReviews.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeerReviews.WebApi.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                var breweries = db.Breweries;

            }
            return View();
        }
    }
}