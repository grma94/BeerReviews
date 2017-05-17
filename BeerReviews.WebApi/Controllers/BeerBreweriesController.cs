using BeerReviews.WebApi.Data;
using BeerReviews.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BeerReviews.WebApi.Controllers
{
    public class BeerBreweriesController : ApiController
    {
        // POST api/values
        [HttpPost]
        [Route("beerbreweries/post")]
        public int Post([FromBody]Beer beer)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                db.Beers.Add(beer);
                db.SaveChanges();
                int a = beer.BeerID;
                return beer.BeerID;
            }
        }

        [HttpPost]
        [Route("beerbreweries/postbb")]
        public void AddBeerBrewery([FromBody]BeerBrewery bb2)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {

                if (bb2 != null) { 
                    if (db.BeerBreweries.Find(bb2.BeerID, bb2.BreweryID, bb2.isPlace) == null)
                    {
                        db.BeerBreweries.Add(bb2);
                        db.SaveChanges();
                        db.Breweries.Find(bb2.BreweryID).BeersCount++;
                        db.SaveChanges();
                    }
                }
            }
        }
        [HttpPost]
        [Route("beerbreweries/postbpb")]
        public void AddBeerPlaceBrewery([FromBody]BeerBrewery bb2)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                if (bb2 != null)
                {
                    if (db.BeerBreweries.Find(bb2.BeerID, bb2.BreweryID, bb2.isPlace) == null)
                    {
                        db.BeerBreweries.Add(bb2);
                        db.SaveChanges();
                    }
                }
            }
        }
        // PUT api/values/5
        [HttpPut]
        [Route("breweries/put/")]
        public void Put([FromBody]Brewery brewery)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                //    Brewery existingBrewery=db.Breweries.Find(breweryId);
                db.Entry(brewery).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
