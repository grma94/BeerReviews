using BeerReviews.WebApi.Data;
using BeerReviews.WebApi.Models;
using BeerReviews.WebApi.ViewModels;
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
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                var categories = db.Categories.Include(c=>c.Styles).ToList();
                return categories;
            }
        }

        // GET api/values/5
        [HttpGet]
        [Route("styles/single/{styleId}")]
        public StyleNewBB GetStyle(int styleId)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                Style style = db.Styles
                    .Include(z=>z.Category)
                    .Include(z=>z.Beers.Select(bb=>bb.BeerBreweries))
                    .Include(z => z.Beers.Select(bb => bb.Reviews))
                    .SingleOrDefault(x => x.StyleID == styleId);
                var ba = style.Beers;
                var nStyle = new StyleNewBB();
                nStyle.CategoryID = style.CategoryID;
                nStyle.CategoryName = style.Category.Name;
                nStyle.StyleID = style.StyleID;
                nStyle.Name = style.Name;
                nStyle.Description = style.Description;
                var beers = new List<BeerWithAvg>();
                foreach(var beer in style.Beers)
                {
                    var onebeer = new BeerWithAvg();
                    var bbb = new List<BeerBreweryWName>();
                    foreach(var beerBrewery in beer.BeerBreweries)
                    {
                        var oneBB = new BeerBreweryWName();
                        oneBB.BreweryID = beerBrewery.BreweryID;
                      //  oneBB.BreweryName = beerBrewery.Brewery.Name;
                        oneBB.BreweryName=db.Breweries.Find(oneBB.BreweryID).Name;
                        oneBB.isPlace = beerBrewery.isPlace;
                        bbb.Add(oneBB);
                    }
                    onebeer.BeerBreweries = bbb;
                    onebeer.Abv = beer.Abv;
                    onebeer.BeerID = beer.BeerID;
                    onebeer.Gravity = beer.Gravity;
                    onebeer.ImageUrl = beer.ImageUrl;
                    onebeer.isLocked = beer.isLocked;
                    onebeer.Name = beer.Name;
                    var avg = 0.0;
                    var count = beer.Reviews.Count();
                    onebeer.ReviewsCount = count;
                    if (count > 0)
                    {
                        foreach (var r in beer.Reviews)
                        {
                            avg += r.Overall;
                        }
                        avg = avg / count;
                    }
                    onebeer.ReviewsAvg = avg;
                    beers.Add(onebeer);
                }
                nStyle.Beers = beers;
            /*    foreach (var b in style.Beers)
                {
                    List<BeerBrewery> bbb = db.BeerBreweries.Where(x => x.BeerID == b.BeerID).ToList();
                    b.BeerBreweries = bbb;
                }*/
                return nStyle;
            }
        }

        // POST api/values
        [HttpPost]
        [Route("styles/post")]
        public void Post([FromBody]Style style)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
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
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
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
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
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
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {

                var categoriesQuery = from s in db.Categories
                                     orderby s.Name
                                     select s;
                return categoriesQuery.ToList();
            }
        }

    }
}
