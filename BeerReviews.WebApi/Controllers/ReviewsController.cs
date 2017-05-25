using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BeerReviews.WebApi.Data;
using BeerReviews.WebApi.Models;
using BeerReviews.WebApi.ViewModels;
using System.Data.Entity;

namespace BeerReviews.WebApi.Controllers
{
    public class ReviewsController : ApiController
    {
        [HttpGet]
        [Route("reviews/many/{beerId?}")]
        public IEnumerable<ReviewWName> GetReviews(int? beerId)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                var reviews =
                 beerId.HasValue
//                ? db.Reviews.Where(r => r.BeerID == beerId).Include(r => r.Beer.Style).Include(r=>r.Beer.BeerBreweries.Select(bb=>bb.Brewery)).ToList()
                ? db.Reviews.Where(r => r.BeerID == beerId).Include(r => r.Beer).ToList()
                : db.Reviews.Include(r => r.Beer).ToList();
                var treviews = new List<ReviewWName>();
                foreach (var r in reviews)
                {
                    var rev1 = new ReviewWName();
                    rev1.Apperance = r.Apperance;
                    rev1.Aroma = r.Aroma;
                    rev1.BeerName = r.Beer.Name;
                    rev1.BeerID = r.BeerID;
                    rev1.Date = r.Date;
                    rev1.Description = r.Description;
                    rev1.ImageUrl = r.ImageUrl;
                    rev1.Overall = r.Overall;
                    rev1.Palate = r.Palate;
                    rev1.ReviewID = r.ReviewID;
                    rev1.Taste = r.Taste;
                    rev1.UserName = r.UserName;
                    treviews.Add(rev1);
                }
                
                return treviews;
            }
        }

        // GET api/values/5
        [HttpGet]
        [Route("reviews/single/{reviewId}")]
        public ReviewWName GetReview(int reviewId)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                Review r = db.Reviews
                    .Include(z => z.Beer.Style)
                    .Include(z => z.Beer.BeerBreweries.Select(bb=>bb.Brewery))
                    .SingleOrDefault(x => x.ReviewID == reviewId);

                var rev1 = new ReviewWName();
                rev1.Apperance = r.Apperance;
                rev1.Aroma = r.Aroma;
                rev1.BeerName = r.Beer.Name;
                rev1.BeerID = r.BeerID;
                rev1.Date = r.Date;
                rev1.Description = r.Description;
                rev1.ImageUrl = r.ImageUrl;
                rev1.Overall = r.Overall;
                rev1.Palate = r.Palate;
                rev1.ReviewID = r.ReviewID;
                rev1.Taste = r.Taste;
                rev1.UserName = r.UserName;
                var lBB = new List<BeerBreweryWName>();
                foreach(var bb in r.Beer.BeerBreweries)
                {
                    var lbb1 = new BeerBreweryWName();
                    lbb1.BreweryID = bb.BreweryID;
                    lbb1.BreweryName = bb.Brewery.Name;
                    lbb1.isPlace = bb.isPlace;
                    lBB.Add(lbb1);
                }
                rev1.BeerBreweries = lBB;
                return rev1;
            }
        }

        // POST api/values
        [HttpPost]
        [Route("reviews/post")]
        public void Post([FromBody]Review review)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                db.Reviews.Add(review);
                db.SaveChanges();
            }
        }

        // PUT api/values/5
        [HttpPut]
        [Route("reviews/put/")]
        public void Put([FromBody]Review review)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();

            }
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("reviews/delete/{styleId}")]
        public void Delete(int reviewId)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                var review = db.Reviews.Find(reviewId);
                db.Reviews.Remove(review);
                db.SaveChanges();
            }
        }


    }
}
