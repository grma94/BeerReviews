using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using BeerReviews.Data;
using BeerReviews.Database.Models;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Net.Http;

namespace BeerReviews.Controllers
{
    public class BeerController : Controller
    {
        // GET: Beer
        public async Task<ActionResult> Index(string sortOrder, int? breweryId, bool? isPlace, int?styleId, bool? p)
        {
            // PopulateStylesDropDownList();
            var httpClient = new HttpClient();
            var response1 = await httpClient.GetAsync("http://localhost:64635/styles/");
            var stylesQuery = await response1.Content.ReadAsAsync<IEnumerable<Style>>();
            ViewBag.StyleID = new SelectList(stylesQuery, "StyleID", "Name", null);
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.GravitySortParm = sortOrder == "gravity" ? "gravity_desc" : "gravity";
            ViewBag.IbuSortParm = sortOrder == "ibu" ? "ibu_desc" : "ibu";
            ViewBag.AbvSortParm = sortOrder == "abv" ? "abv_desc" : "abv";
            ViewBag.ReviewsSortParm = sortOrder == "rc" ? "rc_desc" : "rc";
            ViewBag.AvgSortParm = sortOrder == "avg" ? "avg_desc" : "avg";
            if (sortOrder == null) { sortOrder = "name"; }
            if (breweryId == null && styleId==null)
            {
                response1 = await httpClient.GetAsync("http://localhost:64635/beers/many/" + "all/"+sortOrder);
                var beers = await response1.Content.ReadAsAsync<List<BeerWithAvg>>();

                return View(beers);
          //      return View(Sort(sortOrder, beers));
            }
            else
            {
                response1 = await httpClient.GetAsync("http://localhost:64635/beers/many/" + styleId + "/" + sortOrder);
                var beers = await response1.Content.ReadAsAsync<IEnumerable<BeerWithAvg>>();
                ViewBag.Style = styleId;
                return View(beers);
                // return View(Sort(sortOrder, beers.ToList()));
            }
        }

     /*   public ActionResult BeerStyleList(string sortOrder, ICollection<Beer>beers, int?styleId)
        {
            List<Beer> b;
            if (beers == null)
            {
               b = db.Beers.Where(c => c.StyleID == styleId).ToList();
            }
            else
            {
               b = beers.ToList();
            }
            return PartialView(Sort(sortOrder,b));
        }
        */

        // GET: Beer/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/beers/single/" + id);
            var beer = await response.Content.ReadAsAsync<Beer>();

            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // GET: Beer/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/beers/single/" + id);
            var beer = await response.Content.ReadAsAsync<Beer>();
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // POST: Beer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            /*            var path = beer.ImageUrl;
                        if (path != "/Content/Images/no_image.png")
                        {
                   //         var filePath = Server.MapPath(beer.ImageUrl);
                   //         System.IO.File.Delete(filePath);
                        }
                        */
            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync($"http://localhost:64635/beers/delete/{id}");
            return RedirectToAction("Index");
        }

        /*
        public ActionResult BeerList(string sortOrder, int? breweryId, bool? isPlace, int? styleId)
        {
            PopulateStylesDropDownList();

            if (breweryId == null && styleId == null)
            {

                return PartialView(Sort(sortOrder, db.Beers.ToList()));

            }
            else if (breweryId != null)
            {
                var r = new List<Beer>();
                foreach (var it in db.BeerBreweries.Where(bb => bb.BreweryID == breweryId && bb.isPlace == isPlace))
                {
                    r.Add(it.Beer);
                }
                if (r.Count == 0) return Content("No beers found");

                return PartialView(Sort(sortOrder, r));
            }
            else
            {
                var r = db.Beers.Where(b => b.StyleID == styleId).ToList();
                if (r.Count == 0) return Content("No beers found");
                {
                    return PartialView(Sort(sortOrder, r));
                }
            }
        }
*/
     /*   protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/

        private List<Beer> Sort(string sortOrder, List<Beer> unsorted)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.GravitySortParm = sortOrder == "gravity" ? "gravity_desc" : "gravity";
            ViewBag.IbuSortParm = sortOrder == "ibu" ? "ibu_desc" : "ibu";
            ViewBag.AbvSortParm = sortOrder == "abv" ? "abv_desc" : "abv";
            ViewBag.ReviewsSortParm = sortOrder == "rc" ? "rc_desc" : "rc";
            ViewBag.AvgSortParm = sortOrder == "avg" ? "avg_desc" : "avg";

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
                    unsorted = unsorted.OrderBy(b => b.Abv).ToList();
                    break;
                case "rc_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Abv).ToList();
                    break;
                case "avg":
                    unsorted = unsorted.OrderBy(b => b.Abv).ToList();
                    break;
                case "avg_desc":
                    unsorted = unsorted.OrderByDescending(b => b.Abv).ToList();
                    break;
                default:
                    unsorted = unsorted.OrderBy(b => b.Name).ToList();
                    break;
            }
            return unsorted;
        }

 /*       private void PopulateStylesDropDownList(object selectedStyle = null)
        {
            var stylesQuery = from s in db.Styles
                              orderby s.Name
                              select s;
            ViewBag.StyleID = new SelectList(stylesQuery, "StyleID", "Name", selectedStyle);
        }*/
    }
}
