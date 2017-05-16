using BeerReviews.WebApi.Data;
using BeerReviews.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BeerReviews.WebApi.Controllers
{
    public class StylesController : ApiController
    {
        [HttpGet]
        [Route("styles/many/")]
        public IEnumerable<Category> GetStyles()
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                var categories = db.Categories.Include(c=>c.Styles).ToList();
                return categories;
            }
        }

        // GET api/values/5
        [HttpGet]
        [Route("styles/single/{styleId}")]
        public Style GetStyle(int styleId)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                Style style = db.Styles
                    .Include(z=>z.Category)
                    .Include(z=>z.Beers.Select(bb=>bb.BeerBreweries))
                    .Include(z => z.Beers.Select(bb => bb.Reviews))
                    .SingleOrDefault(x => x.StyleID == styleId);
                return style;
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

    }
}
