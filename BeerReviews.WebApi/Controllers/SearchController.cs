using BeerReviews.WebApi.Data;
using BeerReviews.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace BeerReviews.WebApi.Controllers
{
    public class SearchController : ApiController
    {
        [HttpGet]
        [Route("searchbe/{searchString}")]
        public List<Beer> SearchResults(string searchString)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                var rBeers = db.Beers
                    .Include(bb=>bb.Style)
                    .Include(bb=>bb.BeerBreweries.Select(bbb=>bbb.Brewery))
                    .Where(b => b.Name.Contains(searchString)).ToList();
                return rBeers;
            }
        }

        [HttpGet]
        [Route("searchbr/{searchString}")]
        public List<Brewery> SearchResultsBrewery(string searchString)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                var rBreweries = db.Breweries
                    .Include(bb=>bb.Country)
                    .Where(b => b.Name.Contains(searchString)).ToList();
                return rBreweries;
            }
        }
    }
}
