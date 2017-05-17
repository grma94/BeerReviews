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
    public class BreweriesController : ApiController
    {
        // GET api/values
        [HttpGet]
        [Route("breweries/many/{countryID?}")]
        public IEnumerable<Brewery> GetBreweries(int? countryID)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                         var breweries =
                         countryID.HasValue
                         ? db.Breweries.Where(b => b.CountryID == countryID).Include(b=>b.Country).ToList()
                         : db.Breweries.Include(b => b.Country).ToList();
             //   var breweries = db.Breweries.Include(b => b.Country).ToList();
                return breweries;
            }
        }

        // GET api/values/5
        [HttpGet]
        [Route("breweries/single/{breweryId}")]
        public Brewery GetBrewery(int breweryId)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                var brewery = db.Breweries
                    .Include(b=>b.Country)
                    .Include(bb=>bb.BeerBreweries.Select(b=>b.Beer.Reviews))
      //              .Select(r=> r.Reviews))
                    .Include(bb => bb.BeerBreweries.Select(b => b.Beer.Style))
      //              .Include(bb => bb.BeerBreweries.Select(b => b.Beer.BeerBreweries))
                    .Include(bb => bb.BeerBreweries.Select(b => b.Beer.BeerBreweries.Select(c=>c.Brewery)))
                    .SingleOrDefault(x=>x.BreweryID==breweryId);
                return brewery;
            }
        }

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

        // DELETE api/values/5
        [HttpDelete]
        [Route("breweries/delete/{breweryId}")]
        public void Delete(int breweryId)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                var brewery = db.Breweries.Find(breweryId);
                var path = brewery.ImageUrl;
                if (path != "/Content/Images/no_image.png")
                {
  //                  var filePath = Server.MapPath(brewery.ImageUrl);
  //                  System.IO.File.Delete(filePath);
                }
       /*         foreach (var bb in brewery.BeerBreweries.ToList())
                {
                    db.BeerBreweries.Remove(bb);
                    db.SaveChanges();
                }*/
                db.Breweries.Remove(brewery);
                db.SaveChanges();
            }
        }

        [HttpGet]
        [Route("countries")]
        public IEnumerable<Country> GetCountries()
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {

                var countriesQuery = from s in db.Countries
                                     orderby s.Name
                                     select s;
                return countriesQuery.ToList();
            }
        }
    }
}
