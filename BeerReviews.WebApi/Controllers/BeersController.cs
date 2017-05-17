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
    public class BeersController : ApiController
    {
        // GET api/values
        [HttpGet]
        [Route("beers/many/{styleId?}")]
        public IEnumerable<Beer> GetBeers(int? styleId)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                var beers =
                styleId.HasValue
                ? db.Beers.Where(b => b.StyleID == styleId).Include(b => b.Style).Include(b => b.BeerBreweries).Include(b=>b.Reviews).ToList()
                : db.Beers.Include(b => b.Style).Include(b=>b.BeerBreweries.Select(bbb=>bbb.Brewery)).Include(b => b.Reviews).ToList();
                return beers;
            }
        }

        // GET api/values/5
        [HttpGet]
        [Route("beers/single/{beerId}")]
        public Beer GetBeer(int beerId)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                var beer = db.Beers
                    .Include(b => b.Style)
                    .Include(b=>b.Reviews)
                    .Include(bb => bb.BeerBreweries.Select(b => b.Brewery).Select(c=>c.Country))
                    .SingleOrDefault(x => x.BeerID == beerId);
                return beer;
            }
        }
/*
        // POST api/values
        [HttpPost]
        [Route("breweries/post")]
        public void Post([FromBody]Brewery brewery)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                db.Breweries.Add(brewery);
                db.SaveChanges();
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
        */
        // DELETE api/values/5
        [HttpDelete]
        [Route("beers/delete/{beerId}")]
        public void Delete(int beerId)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                var beer = db.Beers.Find(beerId);
                //       var path = brewery.ImageUrl;
                //       if (path != "/Content/Images/no_image.png")
                //        {
                //                  var filePath = Server.MapPath(brewery.ImageUrl);
                //                  System.IO.File.Delete(filePath);
                //         }
                if (beer.BeerBreweries != null) { 
                    foreach (var bb in beer.BeerBreweries.ToList())
                    {
                        db.BeerBreweries.Remove(bb);
                        db.SaveChanges();
                    }
                }
                db.Beers.Remove(beer);
                db.SaveChanges();
            }
        }
        [HttpGet]
        [Route("styles")]
        public IEnumerable<Style> GetStyles()
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {

                var stylesQuery = from s in db.Styles
                                     orderby s.Name
                                     select s;
                return stylesQuery.ToList();
            }
        }
    }
}
