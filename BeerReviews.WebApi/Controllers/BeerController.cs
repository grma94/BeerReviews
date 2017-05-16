using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeerReviews.WebApi.Data;
using BeerReviews.WebApi.Models;
using System.Data.Entity.Infrastructure;

namespace BeerReviews.WebApi.Controllers
{
    public class BeerController : Controller
    {
        private BeerReviewsContext db = new BeerReviewsContext();

        // GET: Beer
        public ActionResult Index(string sortOrder, int? breweryId, bool? isPlace, int?styleId, bool? p)
        {

            if (breweryId == null && styleId==null)
            {
/*                if (p != null)
                {
                    return PartialView(Sort(sortOrder, db.Beers.ToList()));
                }
*/
                return View(Sort(sortOrder,db.Beers.ToList()));
            }
/*           else if (breweryId != null)
                       {
                           var r = new List<Beer>();
                           foreach (var it in db.BeerBreweries.Where(bb => bb.BreweryID == breweryId && bb.isPlace==isPlace))
                           {
                               r.Add(it.Beer);
                           }
                           if (r.Count == 0) return Content("No beers found");

                           return PartialView(Sort(sortOrder,r));
                       }
*/
            else
            {

                var r = db.Beers.Where(b => b.StyleID == styleId).ToList();
/*                if (p != null)
                {
                    if (r.Count == 0) return Content("No beers found");
                    {
                        return PartialView(Sort(sortOrder, r));
                    }
                }
*/
                ViewBag.Style = styleId;
                return View(Sort(sortOrder, r));
            }
        }

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


    }
}
