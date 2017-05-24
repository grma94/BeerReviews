using BeerReviews.WebApi.Data;
using BeerReviews.WebApi.Models;
using BeerReviews.WebApi.ViewModels;
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
        public IEnumerable<BreweryNewBB> GetBreweries(int? countryID)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                         var breweries =
                         countryID.HasValue
                         ? db.Breweries.Where(b => b.CountryID == countryID).Include(b=>b.Country).ToList()
                         : db.Breweries.Include(b => b.Country).ToList();
                var bnbb = new List<BreweryNewBB>();
                foreach(var b in breweries)
                {
                    var bnbb1 = new BreweryNewBB();
                    bnbb1.BeersCount = b.BeersCount;
                    bnbb1.BreweryID = b.BreweryID;
                    bnbb1.City = b.City;
                    bnbb1.CountryName = b.Country.Name;
                    bnbb1.Name = b.Name;
                    bnbb1.ImageUrl = b.ImageUrl;
                    bnbb.Add(bnbb1);
                }
                return bnbb;
            }
        }

        // GET api/values/5
        [HttpGet]
        [Route("breweries/single/{breweryId}")]
        public BreweryNewBB GetBrewery(int breweryId)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
                var brewery = db.Breweries
                    .Include(b => b.Country)
                    .Include(bb => bb.BeerBreweries.Select(b => b.Beer.Reviews))
                    .Include(bb => bb.BeerBreweries.Select(b => b.Beer.Style))
                    .Include(bb => bb.BeerBreweries.Select(b => b.Beer.BeerBreweries.Select(c => c.Brewery)))
                    .SingleOrDefault(x => x.BreweryID == breweryId);

                var bnbb = new BreweryNewBB();
                var bbwn = new List<BeerBreweryWName>();
                foreach (var bb in brewery.BeerBreweries)
                {
                    var bbwn1 = new BeerBreweryWName();
                    bbwn1.BreweryID = bb.BreweryID;
                    bbwn1.BreweryName = bb.Brewery.Name;
                    bbwn1.isPlace = bb.isPlace;
                    bbwn1.BeerID = bb.BeerID;
                    bbwn1.BeerName = bb.Beer.Name;

                    var a = new BeerWithAvg();
                    a.Abv = bb.Beer.Abv;
                    a.BeerID = bb.Beer.BeerID;
                    a.Gravity = bb.Beer.Gravity;
                    a.IBU = bb.Beer.IBU;
                    a.ImageUrl = bb.Beer.ImageUrl;
                    a.isLocked = bb.Beer.isLocked;
                    a.Name = bb.Beer.Name;
                    a.StyleID = bb.Beer.StyleID;
                    a.StyleName = bb.Beer.Style.Name;
                    var bbbbwn = new List<BeerBreweryWName>();
                    foreach (var bbbb in bb.Beer.BeerBreweries)
                    {
                        var bbbbwn1 = new BeerBreweryWName();
                        bbbbwn1.BreweryID = bbbb.BreweryID;
                        bbbbwn1.BreweryName = bbbb.Brewery.Name;
                        bbbbwn1.isPlace = bbbb.isPlace;
                        bbbbwn.Add(bbbbwn1);
                    }
                    a.BeerBreweries = bbbbwn;


                    var avg = 0.0;
                    var count = bb.Beer.Reviews.Count();
                    a.ReviewsCount = count;
                    if (count > 0)
                    {
                        foreach (var r in bb.Beer.Reviews)
                        {
                            avg += r.Overall;
                        }
                        avg = avg / count;
                    }
                    a.ReviewsAvg = avg;
                    bbwn1.Beer = a;
                    bbwn.Add(bbwn1);
                }
                bnbb.BeerBreweries = bbwn;
                bnbb.BeersCount = brewery.BeersCount;
                bnbb.BreweryID = brewery.BreweryID;
                bnbb.City = brewery.City;
                bnbb.CountryName = brewery.Country.Name;
                bnbb.CountryID = brewery.CountryID;
                bnbb.Description = brewery.Description;
                bnbb.FbUrl = bnbb.FbUrl;
                bnbb.ImageUrl = brewery.ImageUrl;
                bnbb.isLocked = brewery.isLocked;
                bnbb.Name = brewery.Name;
                bnbb.Phone = brewery.Phone;
                bnbb.PostalCode = brewery.PostalCode;
                bnbb.Street = brewery.Street;
                bnbb.Url = brewery.Url;


                return bnbb;
            }
        }

        // POST api/values
        [HttpPost]
        [Route("breweries/post")]
        public string Post([FromBody]Brewery brewery)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {
               db.Breweries.Add(brewery);
               db.SaveChanges();
                string bID = brewery.BreweryID.ToString();
               return bID;
            }
        }

        // PUT api/values/5
        [HttpPut]
        [Route("breweries/put/")]
        public void Put([FromBody]Brewery brewery)
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
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
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
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
                if (brewery.BeerBreweries != null)
                {
                    foreach (var bb in brewery.BeerBreweries)
                    {
                        db.BeerBreweries.Remove(bb);
                        db.SaveChanges();
                    }
                }
                db.Breweries.Remove(brewery);
                db.SaveChanges();
            }
        }

        [HttpGet]
        [Route("countries")]
        public IEnumerable<Country> GetCountries()
        {
            using (BeerReviewsContext2 db = new BeerReviewsContext2())
            {

                var countriesQuery = from s in db.Countries
                                     orderby s.Name
                                     select s;
                return countriesQuery.ToList();
            }
        }
    }
}
