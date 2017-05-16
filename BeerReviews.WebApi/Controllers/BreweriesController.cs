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
                var brewery = db.Breweries.Find(breweryId);
               
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
            }
        }

        // PUT api/values/5
        [HttpPut]
        [Route("breweries/put/{breweryId}")]
        public void Put(int id, [FromBody]Brewery brewery)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                Brewery existingBrewery=db.Breweries.Find(id);
       //         modify.
                db.Entry(existingBrewery).State = EntityState.Modified;
                db.SaveChangesAsync();
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
