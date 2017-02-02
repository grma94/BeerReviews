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
using System.Data.Entity.Infrastructure;

namespace BeerReviews.Controllers
{
    public class BeerController : Controller
    {
        private BeerReviewsContext db = new BeerReviewsContext();

        // GET: Beer
        public ActionResult Index(int? breweryId, bool? isPlace, int?styleId, bool? p)
        {
            PopulateStylesDropDownList();
            if (breweryId == null && styleId==null)
            {
                return View(db.Beers.ToList());
            }
            else if (breweryId != null)
            {
                var r = new List<Beer>();
                foreach (var it in db.BeerBreweries.Where(bb => bb.BreweryID == breweryId && bb.isPlace==isPlace))
                {
                    r.Add(it.Beer);
                }
                if (r.Count == 0) return Content("No beers found");
                return PartialView(r);
            }
            else {
                var r = db.Beers.Where(b => b.StyleID == styleId).ToList();
                if (p != null)
                {
                    if (r.Count == 0) return Content("No beers found");
                    {
                        return PartialView(r);
                    }
                }
                return View(r);
            }
        }

        // GET: Beer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // GET: Beer/Create
        public ActionResult Create(int? breweryId)
        {
            if (breweryId == null)
            {
                PopulateStylesDropDownList();
                return View();
            }
                       
            return View(breweryId);
        }

        // POST: Beer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BeerID,Name,Abv,IBU,Gravity,Description,isLocked,ImageUrl,StyleID")] Beer beer)
        {
            if (ModelState.IsValid)
            {
                db.Beers.Add(beer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateStylesDropDownList(beer.StyleID);
            return View(beer);
        }

        [HttpPost]
        public JsonResult Create2(string Prefix)
        {
            //Note : you can bind same list from database  
            List<Brewery> ObjList = db.Breweries.ToList();

            //Searching records from list using LINQ query  
                     var BreweryName = (from N in ObjList
                                      where N.Name.ToLower().Contains(Prefix.ToLower())
                                      select new { N.Name });
       //     var BreweryName = db.Beers.Where(b => b.Name.StartsWith(Prefix)).ToList();
       //                        where N.Name.StartsWith(Prefix)
      //                         select new { N.Name });
            return Json(BreweryName, JsonRequestBehavior.AllowGet);
        }

        // GET: Beer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // POST: Beer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BeerID,Name,Abv,IBU,Gravity,Description,isLocked,ImageUrl,StyleID")] Beer beer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beer);
        }

        // GET: Beer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // POST: Beer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Beer beer = db.Beers.Find(id);
            db.Beers.Remove(beer);
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

        private void PopulateStylesDropDownList(object selectedStyle = null)
        {
            var stylesQuery = from s in db.Styles
                              orderby s.Name
                              select s;
            ViewBag.StyleID = new SelectList(stylesQuery, "StyleID", "Name", selectedStyle);
        }
    }
}
