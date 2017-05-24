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
        [HttpGet]
        [Route("beerbreweries/get/{beerId}")]
        public IEnumerable<BeerBrewery> GetBBs(int beerId)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                List < BeerBrewery > beerbreweries= null;
                if (db.BeerBreweries.Where(bb => bb.BeerID == beerId) != null)
                { 
                beerbreweries = db.BeerBreweries.Where(bb=>bb.BeerID==beerId).Include(bb=>bb.Brewery).ToList();
                }
                return beerbreweries;
            }
        }
        // POST api/values
        [HttpPost]
        [Route("beerbreweries/post")]
        public int Post([FromBody]Beer beer)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
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
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
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
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
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
        [Route("beers/put/")]
        public void Put([FromBody]Beer beerBreweryVM)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                var beer=db.Beers.Find(beerBreweryVM.BeerID);
                if (beer.Abv != beerBreweryVM.Abv)
                {
                    beer.Abv = beerBreweryVM.Abv;
                }
                if (beer.Description != beerBreweryVM.Description)
                {
                    beer.Description = beerBreweryVM.Description;
                }
                if (beer.Gravity != beerBreweryVM.Gravity)
                {
                    beer.Gravity = beerBreweryVM.Gravity;
                }
                if (beer.IBU != beerBreweryVM.IBU)
                {
                    beer.IBU = beerBreweryVM.IBU;
                }
                if (beer.Name != beerBreweryVM.Name)
                {
                    beer.Name = beerBreweryVM.Name;
                }
                if (beer.StyleID != beerBreweryVM.StyleID)
                {
                    beer.StyleID = beerBreweryVM.StyleID;
                }
  //              db.Entry(beer).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // DELETE beerBrewery/bb
        [HttpPut]
        [Route("beerbreweries/delete/")]
        public void Delete([FromBody]BeerBrewery beerBrewery)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                if (!beerBrewery.isPlace)
                { 
                db.Breweries.Find(beerBrewery.BreweryID).BeersCount--;
                db.SaveChanges();
                }
                var bbd=db.BeerBreweries.SingleOrDefault(x => x.BeerID == beerBrewery.BeerID && x.BreweryID == beerBrewery.BreweryID && x.isPlace == beerBrewery.isPlace);
                db.BeerBreweries.Remove(bbd);
                db.SaveChanges();

            }
        }
    }
}
