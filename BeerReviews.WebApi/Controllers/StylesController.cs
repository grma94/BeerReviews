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
        [Route("styles/post")]
        public void Post([FromBody]Style style)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                db.Styles.Add(style);
                db.SaveChanges();
            }
        }

        // PUT api/values/5
        [HttpPut]
        [Route("styles/put/")]
        public void Put([FromBody]Style style)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                db.Entry(style).State = EntityState.Modified;
                db.SaveChanges();

            }
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("styles/delete/{styleId}")]
        public void Delete(int styleId)
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {
                var style = db.Styles.Find(styleId);
                db.Styles.Remove(style);
                db.SaveChanges();
            }
        }


        [HttpGet]
        [Route("categories")]
        public IEnumerable<Category> GetCategories()
        {
            using (BeerReviewsContext db = new BeerReviewsContext())
            {

                var categoriesQuery = from s in db.Categories
                                     orderby s.Name
                                     select s;
                return categoriesQuery.ToList();
            }
        }

    }
}
