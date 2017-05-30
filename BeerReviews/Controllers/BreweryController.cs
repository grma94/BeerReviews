using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
        // GET: Brewery
        public async Task<ActionResult> Index(int? CountryID, string sortOrder)
        {

            var httpClient = new HttpClient();
            var response1 = await httpClient.GetAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/countries/");
            var countriesQuery = await response1.Content.ReadAsAsync<IEnumerable<Country>>();
            ViewBag.CountryID = new SelectList(countriesQuery, "CountryID", "Name", null);
            if (CountryID == null)
            {
                var response = await httpClient.GetAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/breweries/many/" + "all");
                var breweries = await response.Content.ReadAsAsync<IEnumerable<Brewery>>();
                ViewBag.Country = CountryID;
                return View(Sort(sortOrder, breweries.ToList()));
            }
            else
            {
                var response = await httpClient.GetAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/breweries/many/" + CountryID);
                var breweries = await response.Content.ReadAsAsync<IEnumerable<Brewery>>();
                ViewBag.Country = CountryID;
                return View(Sort(sortOrder, breweries.ToList()));
            }
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
            var response = await httpClient.GetAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/breweries/single/" + id);
            var brewery = await response.Content.ReadAsAsync<Brewery>();

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
            if (User.Identity.IsAuthenticated)
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/countries/");
                var countriesQuery = await response.Content.ReadAsAsync<IEnumerable<Country>>();
                ViewBag.CountryID = new SelectList(countriesQuery, "CountryID", "Name", null);
                return View();
            }
            return RedirectToAction("Index");
        }
        

        // POST: Brewery/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Phone,Description,Street,PostalCode,City,CountryID")] Brewery brewery, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                brewery.ImageUrl = FileUpload(file);

                var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/breweries/post/", brewery);
        //        string bID=await response.Content.ReadAsStringAsync();
        //        int beID = Convert.ToInt32(bID);
         //       response.EnsureSuccessStatusCode();
             //   return RedirectToAction("Details", new { breweryID = beID });
                return RedirectToAction("Index");
            }
            PopulateCountriesDropDownList(brewery.CountryID);
            return View(brewery);
        }
        [HttpGet]
        // GET: Brewery/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/breweries/single/" + id);
                var brewery = await response.Content.ReadAsAsync<Brewery>();
                if (brewery == null)
                {
                    return HttpNotFound();
                }
                return View(brewery);
            }
            return RedirectToAction("Index");
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
                    var response1 = await httpClient.GetAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/breweries/single/" + brewery.BreweryID);
                    var breweryI = await response1.Content.ReadAsAsync<Brewery>();
                    brewery.ImageUrl = breweryI.ImageUrl;
   //                 brewery.ImageUrl = db.Breweries.AsNoTracking().Where(b=>b.BreweryID==brewery.BreweryID).First().ImageUrl;
                }
                else
                {
                    brewery.ImageUrl = FileUpload(file);
                }
                var breweryID = brewery.BreweryID;
                HttpResponseMessage response = await httpClient.PutAsJsonAsync($"http://beerreviewswebapi20170525061826.azurewebsites.net/breweries/put/", brewery);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            return View(brewery);
        }

        // GET: Brewery/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/breweries/single/" + id);
                var brewery = await response.Content.ReadAsAsync<Brewery>();
                if (brewery == null)
                {
                    return HttpNotFound();
                }
                return View(brewery);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        // POST: Brewery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync($"http://beerreviewswebapi20170525061826.azurewebsites.net/breweries/delete/{id}");
     //       return response.StatusCode;
            return RedirectToAction("Index");
        }

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
            var response = await httpClient.GetAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/countries/");
            var countriesQuery = await response.Content.ReadAsAsync<IEnumerable<Country>>();
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
                    unsorted = unsorted.OrderBy(b => b.CountryName).ToList();
                    break;
                case "country_desc":
                    unsorted = unsorted.OrderByDescending(b => b.CountryName).ToList();
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


  /*      private List<BeerBrewery> SortBeers(string sortOrder, List<BeerBrewery> unsorted, bool place)
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
        }*/
    }
}
