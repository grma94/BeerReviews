using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeerReviews.Data;
using BeerReviews.Models;
using System.IO;

namespace BeerReviews.Controllers
{
    public class BreweryController : Controller
    {
        private BeerReviewsContext db = new BeerReviewsContext();

        // GET: Brewery
        public ActionResult Index(int? CountryID, string sortOrder)
        {

            PopulateCountriesDropDownList();
            if (CountryID == null)
            { 
            return View(Sort(sortOrder,db.Breweries.ToList()));
            }
            var results = db.Breweries.Where(b => b.CountryID == CountryID);
            return View(Sort(sortOrder,results.ToList()));
        }

        // GET: Brewery/Details/5
        public ActionResult Details(int? id, string sortOrder)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brewery brewery = db.Breweries.Find(id);
            if (brewery == null)
            {
                return HttpNotFound();
            }
            return View(brewery);
        }

        // GET: Brewery/Create
        public ActionResult Create()
        {
            PopulateCountriesDropDownList();
            return View();
        }

        // POST: Brewery/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Phone,Description,Street,PostalCode,City,CountryID")] Brewery brewery, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                brewery.ImageUrl = FileUpload(file);
                db.Breweries.Add(brewery);
                db.SaveChanges();
                PopulateCountriesDropDownList(brewery.CountryID);
                return RedirectToAction("Index");
            }
            PopulateCountriesDropDownList(brewery.CountryID);
            return View(brewery);
        }

        // GET: Brewery/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brewery brewery = db.Breweries.Find(id);
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
        public ActionResult Edit([Bind(Include = "BreweryID,Name,Phone,Description,Street,PostalCode,City, CountryID")] Brewery brewery, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    brewery.ImageUrl = db.Breweries.AsNoTracking().Where(b=>b.BreweryID==brewery.BreweryID).First().ImageUrl;
                }else
                {
                    brewery.ImageUrl = FileUpload(file);
                }
                db.Entry(brewery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brewery);
        }

        // GET: Brewery/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brewery brewery = db.Breweries.Find(id);
            if (brewery == null)
            {
                return HttpNotFound();
            }
            return View(brewery);
        }

        // POST: Brewery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Brewery brewery = db.Breweries.Find(id);
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
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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

        private void PopulateCountriesDropDownList(object selectedCountry = null)
        {
            var countriesQuery = from s in db.Countries
                              orderby s.Name
                              select s;
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
    }
}
