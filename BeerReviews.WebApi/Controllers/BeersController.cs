using BeerReviews.WebApi.Data;
using BeerReviews.WebApi.ViewModels;
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
        [Route("beers/many/{styleId?}/{sortOrder}")]
        public List<BeerWithAvg> GetBeers(int? styleId, string sortOrder)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                var beers =
                styleId.HasValue
                ? db.Beers.Where(b => b.StyleID == styleId).Include(b => b.Style).Include(b => b.BeerBreweries).Include(b=>b.Reviews).ToList()
                : db.Beers.Include(b => b.Style).Include(b=>b.BeerBreweries.Select(bbb=>bbb.Brewery)).Include(b => b.Reviews).ToList();
                List<BeerWithAvg> bwa=new List<BeerWithAvg>();
                foreach (var b in beers)
                {
                    var a = new BeerWithAvg();
                    a.Abv = b.Abv;
                    var bbwn = new List<BeerBreweryWName>();
                    foreach (var bb in b.BeerBreweries)
                    {
                        var bbwn1 = new BeerBreweryWName();
                        bbwn1.BreweryID = bb.BreweryID;
                        bbwn1.BreweryName = bb.Brewery.Name;
                        bbwn1.isPlace = bb.isPlace;
                        bbwn.Add(bbwn1);
                    }
                    a.BeerBreweries = bbwn;
                    a.BeerID = b.BeerID;
                    a.Gravity = b.Gravity;
                    a.IBU = b.IBU;
                    a.ImageUrl = b.ImageUrl;
                    a.isLocked = b.isLocked;
                    a.Name = b.Name;
                    a.StyleID = b.StyleID;
                    a.StyleName = b.Style.Name;
                    var avg = 0.0;
                    var count =b.Reviews.Count();
                    a.ReviewsCount = count;
                    if (count>0) {
                        foreach (var r in b.Reviews)
                        {
                            avg += r.Overall;
                        }
                        avg = avg / count;
                     }
                    a.ReviewsAvg = avg;
                    bwa.Add(a);
                }
                var bwaS = Sort(sortOrder,bwa);
                return bwaS;
            }
        }

        // GET api/values/5
        [HttpGet]
        [Route("beers/single/{beerId}")]
        public BeerFDetails GetBeer(int beerId)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                var beer = db.Beers
                    .Include(b => b.Style)
                    .Include(b=>b.Reviews)
                    .Include(bb => bb.BeerBreweries.Select(b => b.Brewery).Select(c=>c.Country))
                    .SingleOrDefault(x => x.BeerID == beerId);

                var beerFDetails = new BeerFDetails();
                beerFDetails.Abv = beer.Abv;
                beerFDetails.BeerID = beer.BeerID;
                beerFDetails.Description = beer.Description;
                beerFDetails.Gravity = beer.Gravity;
                beerFDetails.IBU = beer.IBU;
                beerFDetails.ImageUrl = beer.ImageUrl;
                beerFDetails.isLocked = beer.isLocked;
                beerFDetails.Name = beer.Name;
                var revs = new List<Review>();
                foreach(var r in beer.Reviews)
                {
                    var rev1 = new Review();
                    rev1.Apperance = r.Apperance;
                    rev1.Aroma = r.Aroma;
                    rev1.Date = r.Date;
                    rev1.Description = r.Description;
                    rev1.ImageUrl = r.ImageUrl;
                    rev1.Overall = r.Overall;
                    rev1.Palate = r.Palate;
                    rev1.ReviewID = r.ReviewID;
                    rev1.Taste = r.Taste;
                    rev1.UserName = r.UserName;
                    revs.Add(rev1);
                }
                beerFDetails.Reviews = revs;
                beerFDetails.StyleID = beer.StyleID;
                beerFDetails.StyleName = beer.Style.Name;
                var BBs = new List<BeerBreweryWPlace>();
                foreach(var bb in beer.BeerBreweries)
                {
                    var bbwp = new BeerBreweryWPlace();
                    bbwp.BreweryID = bb.BreweryID;
                    bbwp.BreweryName = bb.Brewery.Name;
                    bbwp.City = bb.Brewery.City;
                    bbwp.CountryName = bb.Brewery.Country.Name;
                    bbwp.isPlace = bb.isPlace;
                    BBs.Add(bbwp);
                }
                beerFDetails.BeerBreweries = BBs;

                return beerFDetails;
            }
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("beers/delete/{beerId}")]
        public void Delete(int beerId)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                var beer = db.Beers.Find(beerId);
                //       var path = brewery.ImageUrl;
                //       if (path != "/Content/Images/no_image.png")
                //        {
                //                  var filePath = Server.MapPath(brewery.ImageUrl);
                //                  System.IO.File.Delete(filePath);
                //         }
                if (beer.BeerBreweries != null) { 
                    foreach (var bb in beer.BeerBreweries)
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
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {

                var stylesQuery = from s in db.Styles
                                     orderby s.Name
                                     select s;
                return stylesQuery.ToList();
            }
        }
        private List<BeerWithAvg> Sort(string sortOrder, List<BeerWithAvg> unsorted)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Name).ToList();
                    break;
                case "gravity":
                    unsorted = unsorted.OrderBy(b => b.Gravity).ToList();
                    break;
                case "gravity_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Gravity).ToList();
                    break;
                case "ibu":
                    unsorted = unsorted.OrderBy(b => b.IBU).ToList();
                    break;
                case "ibu_desc":
                    unsorted = unsorted.OrderByDescending(b => b.IBU).ToList();
                    break;
                case "abv":
                    unsorted = unsorted.OrderBy(b => b.Abv).ToList();
                    break;
                case "abv_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Abv).ToList();
                    break;
                case "rc":
                    unsorted = unsorted.OrderBy(b => b.ReviewsCount).ToList();
                    break;
                case "rc_desc":
                    unsorted = unsorted.OrderByDescending(b => b.ReviewsCount).ToList();
                    break;
                case "avg":
                    unsorted = unsorted.OrderBy(b => b.ReviewsAvg).ToList();
                    break;
                case "avg_desc":
                    unsorted = unsorted.OrderByDescending(b => b.ReviewsAvg).ToList();
                    break;
                default:
                    unsorted = unsorted.OrderBy(b => b.Name).ToList();
                    break;
            }
            return unsorted;
        }
    }
}
