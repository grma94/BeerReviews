using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeerReviews.Database.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace BeerReviews.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public async Task<ActionResult> SearchResults(string searchString)
        {
            if (searchString != "")
            {
                var httpClient = new HttpClient();
                var response1 = await httpClient.GetAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/searchbe/" + searchString);
                var beers = await response1.Content.ReadAsAsync<List<Beer>>();
                response1 = await httpClient.GetAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/searchbr/" + searchString);
                var breweries = await response1.Content.ReadAsAsync<List<Brewery>>();
                var results = new Tuple<List<Beer>, List<Brewery>>(beers, breweries);
                return View(results);
            }
            return View();
        }
    }
}