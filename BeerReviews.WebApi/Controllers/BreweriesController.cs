using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BeerReviews.WebApi.Controllers
{
    public class BreweriesController : ApiController
    {
        // GET api/values/5
        [HttpGet]
        [Route("breweries/{countryId?}")]
        public IEnumerable<Brewery> GetBreweries(int? countryId)
        {
            var breweries = 
                countryId.HasValue
                ? db.Breweries.Where(b => b.CountryID == CountryID).ToList()
                : db.Breweries.ToList();

            return breweries;
        }

        // GET api/values/5
        [HttpGet]
        [Route("breweries/single/{breweryId}")]
        public Brewery GetBrewery(int breweryId)
        {
            var breweries =
                countryId.HasValue
                ? db.Breweries.Where(b => b.CountryID == CountryID).ToList()
                : db.Breweries.ToList();

            return breweries;
        }

        // POST api/values
        [HttpPost]
        [Route("breweries/single")]
        public void Post([FromBody]Brewery brewery)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
