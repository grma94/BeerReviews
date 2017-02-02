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
            foreach (var bb in beer.BeerBreweries.ToList())
            {
                db.BeerBreweries.Remove(bb);
                db.SaveChanges();
            }


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
