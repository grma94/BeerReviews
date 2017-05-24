using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BeerReviews.WebApi.Data;
using BeerReviews.WebApi.Models;
using System.Data.Entity;

namespace BeerReviews.WebApi.Controllers
{
    public class ReviewsController : ApiController
    {
        [HttpGet]
        [Route("reviews/many/{beerId?}")]
        public IEnumerable<Review> GetReviews(int? beerId)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                var reviews =
                 beerId.HasValue
                ? db.Reviews.Where(r => r.BeerID == beerId).Include(r => r.Beer.Style).Include(r=>r.Beer.BeerBreweries.Select(bb=>bb.Brewery)).ToList()
                : db.Reviews.Include(r => r.Beer.Style).Include(r => r.Beer.BeerBreweries.Select(bb => bb.Brewery)).ToList();
                return reviews;
            }
        }

        // GET api/values/5
        [HttpGet]
        [Route("reviews/single/{reviewId}")]
        public Review GetReview(int reviewId)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                Review review = db.Reviews
                    .Include(z => z.Beer.Style)
                    .Include(z => z.Beer.BeerBreweries.Select(bb=>bb.Brewery))
                    .SingleOrDefault(x => x.ReviewID == reviewId);
                return review;
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
