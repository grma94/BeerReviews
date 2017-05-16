using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeerReviews.Data;
using BeerReviews.Database.Models;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace BeerReviews.Controllers
{
    public class BreweryController : Controller
    {
    //    private BeerReviewsContext db = new BeerReviewsContext();

        // GET: Brewery
        public async Task<ActionResult> Index(int? CountryID, string sortOrder)
        {

    //        PopulateCountriesDropDownList();
            var httpClient = new HttpClient();
            var response1 = await httpClient.GetAsync("http://localhost:64635/countries/");
            var countriesQuery = await response1.Content.ReadAsAsync<IEnumerable<Country>>();
            ViewBag.CountryID = new SelectList(countriesQuery, "CountryID", "Name", null);
    //        var httpClient = new HttpClient();
    //        httpClient.DefaultRequestHeaders.Accept.Clear();
   //         httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (CountryID == null)
            {
                var response = await httpClient.GetAsync("http://localhost:64635/breweries/many/" + "all");
                var breweries = await response.Content.ReadAsAsync<IEnumerable<Brewery>>();
                ViewBag.Country = CountryID;
                return View(Sort(sortOrder, breweries.ToList()));
            }
            else
            {
                var response = await httpClient.GetAsync("http://localhost:64635/breweries/many/" + CountryID);
                var breweries = await response.Content.ReadAsAsync<IEnumerable<Brewery>>();
                ViewBag.Country = CountryID;
                return View(Sort(sortOrder, breweries.ToList()));
            }
            //      var data = JsonConvert.DeserializeObject<Root>(response.Content.ReadAsStringAsync().ToString()).Data;
            
         //   var brew=breweries.ToList();

        }

        // GET: Brewery/Details/5
        public async Task<ActionResult> Details(int? id, string sortOrder, bool? place)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                     httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.GetAsync("http://localhost:64635/breweries/single/" + id);
            var brewery = await response.Content.ReadAsAsync<Brewery>();

        //    Brewery brewery = db.Breweries.Find(id);
            if (brewery == null)
            {
                return HttpNotFound();
            }
            if (place != null)
            { 
      //      brewery.BeerBreweries = SortBeers(sortOrder, brewery.BeerBreweries.ToList(), (bool)place);
            }

            return View(brewery);
        }

        // GET: Brewery/Create
        public async Task<ActionResult> Create()
        {
            //      PopulateCountriesDropDownList();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/countries/");
            var countriesQuery = await response.Content.ReadAsAsync<IEnumerable<Country>>();
            ViewBag.CountryID = new SelectList(countriesQuery, "CountryID", "Name", null);
            return View();
        }

        // POST: Brewery/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Phone,Description,Street,PostalCode,City,CountryID")] Brewery brewery, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                brewery.ImageUrl = FileUpload(file);

                var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync("http://localhost:64635/breweries/post/", brewery);
                response.EnsureSuccessStatusCode();

             //   PopulateCountriesDropDownList(brewery.CountryID);
                return RedirectToAction("Index");
            }
            PopulateCountriesDropDownList(brewery.CountryID);
            return View(brewery);
        }
        [HttpGet]
        // GET: Brewery/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/breweries/single/" + id);
            var brewery = await response.Content.ReadAsAsync<Brewery>();
            if (brewery == null)
            {
                return HttpNotFound();
            }
            return View(brewery);
        }

        // POST: Brewery/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BreweryID,Name,Phone,Description,Street,PostalCode,City, CountryID")] Brewery brewery, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var httpClient = new HttpClient();
                if (file == null)
                {   
                    var response1 = await httpClient.GetAsync("http://localhost:64635/breweries/single/" + brewery.BreweryID);
                    var breweryI = await response1.Content.ReadAsAsync<Brewery>();
                    brewery.ImageUrl = breweryI.ImageUrl;
   //                 brewery.ImageUrl = db.Breweries.AsNoTracking().Where(b=>b.BreweryID==brewery.BreweryID).First().ImageUrl;
                }
                else
                {
                    brewery.ImageUrl = FileUpload(file);
                }
                var breweryID = brewery.BreweryID;
                HttpResponseMessage response = await httpClient.PutAsJsonAsync($"http://localhost:64635/breweries/put/", brewery);
                response.EnsureSuccessStatusCode();
     //           db.Entry(brewery).State = EntityState.Modified;
     //           db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brewery);
        }

        // GET: Brewery/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/breweries/single/" + id);
            var brewery = await response.Content.ReadAsAsync<Brewery>();
            if (brewery == null)
            {
                return HttpNotFound();
            }
            return View(brewery);
        }

        // POST: Brewery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //           var httpClient = new HttpClient();
            //           var response = await httpClient.GetAsync("localhost:64635/breweries/single" + id);
            //           var brewery = await response.Content.ReadAsAsync<Brewery>();
            /*          Brewery brewery = db.Breweries.Find(id);
                      var path = brewery.ImageUrl;
                      if (path != "/Content/Images/no_image.png")
                      { 
                      var filePath = Server.MapPath(brewery.ImageUrl);
                          System.IO.File.Delete(filePath);
                      }
                      foreach (var bb in brewery.BeerBreweries.ToList())
                      {
                          db.BeerBreweries.Remove(bb);
                          db.SaveChanges();
                      }

                      db.Breweries.Remove(brewery);
                      db.SaveChanges();*/
            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync($"http://localhost:64635/breweries/delete/{id}");
     //       return response.StatusCode;

            return RedirectToAction("Index");
        }

 /*       protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
*/
        public string FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = Path.GetFileName(file.FileName);
                string p = Path.Combine("/Content/Images/Breweries", pic);
                string path = Server.MapPath(p);

                file.SaveAs(path);

                return p;
            }
            else return "/Content/Images/no_image.png";
        }

        private async void PopulateCountriesDropDownList(object selectedCountry = null)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/countries/");
            var countriesQuery = await response.Content.ReadAsAsync<IEnumerable<Country>>();
   //         var countriesQuery = from s in db.Countries
  //                            orderby s.Name
  //                            select s;
            ViewBag.CountryID = new SelectList(countriesQuery, "CountryID", "Name", selectedCountry);
        }

        private List<Brewery> Sort(string sortOrder, List<Brewery> unsorted)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CountrySortParm = sortOrder == "country" ? "country_desc" : "country";
            ViewBag.CitySortParm = sortOrder == "city" ? "city_desc" : "city";
            ViewBag.BeersSortParm = sortOrder == "bc" ? "bc_desc" : "bc";

            switch (sortOrder)
            {
                case "name_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Name).ToList();
                    break;
                case "country":
                    unsorted = unsorted.OrderBy(b => b.Country.Name).ToList();
                    break;
                case "country_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Country.Name).ToList();
                    break;
                case "city":
                    unsorted = unsorted.OrderBy(b => b.City).ToList();
                    break;
                case "city_desc":
                    unsorted = unsorted.OrderByDescending(b => b.City).ToList();
                    break;
                case "bc":
                    unsorted = unsorted.OrderBy(b => b.BeersCount).ToList();
                    break;
                case "bc_desc":
                    unsorted = unsorted.OrderByDescending(b => b.BeersCount).ToList();
                    break;
                default:
                    unsorted = unsorted.OrderBy(b => b.Name).ToList();
                    break;
            }
            return unsorted;
        }


        private List<BeerBrewery> SortBeers(string sortOrder, List<BeerBrewery> unsorted, bool place)
        {

        List<BeerBrewery> unsortedPart=unsorted.Where(bb => bb.isPlace != place).ToList();
        unsorted = unsorted.Where(bb => bb.isPlace == place).ToList();

        ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.GravitySortParm = sortOrder == "gravity" ? "gravity_desc" : "gravity";
            ViewBag.IbuSortParm = sortOrder == "ibu" ? "ibu_desc" : "ibu";
            ViewBag.AbvSortParm = sortOrder == "abv" ? "abv_desc" : "abv";
            ViewBag.ReviewsSortParm = sortOrder == "rc" ? "rc_desc" : "rc";
            ViewBag.AvgSortParm = sortOrder == "avg" ? "avg_desc" : "avg";
            ViewBag.StyleSortParm = sortOrder == "style" ? "style_desc" : "style";

            switch (sortOrder)
            {
                case "name_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Beer.Name).ToList();
                    break;
                case "gravity":
                    unsorted = unsorted.OrderBy(b => b.Beer.Gravity).ToList();
                    break;
                case "gravity_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Beer.Gravity).ToList();
                    break;
                case "ibu":
                    unsorted = unsorted.OrderBy(b => b.Beer.IBU).ToList();
                    break;
                case "ibu_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Beer.IBU).ToList();
                    break;
                case "abv":
                    unsorted = unsorted.OrderBy(b => b.Beer.Abv).ToList();
                    break;
                case "abv_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Beer.Abv).ToList();
                    break;
                case "rc":
                    unsorted = unsorted.OrderBy(b => b.Beer.Abv).ToList();
                    break;
                case "rc_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Beer.Abv).ToList();
                    break;
                case "avg":
                    unsorted = unsorted.OrderBy(b => b.Beer.Abv).ToList();
                    break;
                case "avg_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Beer.Abv).ToList();
                    break;
                case "style":
                    unsorted = unsorted.OrderBy(b => b.Beer.Style.Name).ToList();
                    break;
                case "style_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Beer.Style.Name).ToList();
                    break;
                default:
                    unsorted = unsorted.OrderBy(b => b.Beer.Name).ToList();
                    break;
            }
            return unsorted=unsorted.Union(unsortedPart).ToList();
        }
    }
}
