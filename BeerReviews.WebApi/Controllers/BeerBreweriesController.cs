using System;
using System.Collections.Generic;
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
    }
}
