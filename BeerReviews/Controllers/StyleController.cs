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
using System.Net.Http;
using System.Threading.Tasks;

namespace BeerReviews.Controllers
{
    public class StyleController : Controller
    {
        private BeerReviewsContext db = new BeerReviewsContext();

        // GET: Style
        public async Task<ActionResult> Index()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/styles/many/");
            var styles = await response.Content.ReadAsAsync<IEnumerable<Category>>();
            return View(styles.ToList());
        }

        // GET: Style/Details/5
        public async Task<ActionResult> Details(int? id, string sortOrder)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/styles/single/"+id);
            var style = await response.Content.ReadAsAsync<Style>();
            if (style == null)
            {
                return HttpNotFound();
            }

            style.Beers=Sort(sortOrder,style.Beers.ToList());

            return View(style);
        }

        // GET: Style/Create
        public async Task<ActionResult> Create()
        {
            //PopulateCategoriesDropDownList();
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/categories/");
            var categoriesQuery = await response.Content.ReadAsAsync<IEnumerable<Category>>();

            ViewBag.CategoryID = new SelectList(categoriesQuery, "CategoryID", "Name", null);
            return View();
        }

        // POST: Style/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "StyleID,Name,Description,CategoryID")] Style style)
        {
            if (ModelState.IsValid)
            {
                var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync("http://localhost:64635/styles/post/", style);
                response.EnsureSuccessStatusCode();

                PopulateCategoriesDropDownList(style.CategoryID);
                return RedirectToAction("Index");
            }
            PopulateCategoriesDropDownList(style.CategoryID);
            return View(style);
        }

        // GET: Style/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/styles/single/" + id);
            var style = await response.Content.ReadAsAsync<Style>();
            if (style == null)
            {
                return HttpNotFound();
            }


            response = await httpClient.GetAsync("http://localhost:64635/categories/");
            var categoriesQuery = await response.Content.ReadAsAsync<IEnumerable<Category>>();

            ViewBag.CategoryID = new SelectList(categoriesQuery, "CategoryID", "Name", style.CategoryID);

          //  PopulateCategoriesDropDownList(style.CategoryID);
            return View(style);
        }

        // POST: Style/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "StyleID,Name,Description,CategoryID")] Style style)
        {
            if (ModelState.IsValid)
            {
                var httpClient = new HttpClient();

                PopulateCategoriesDropDownList(style.CategoryID);


                var styleID = style.StyleID;
                HttpResponseMessage response = await httpClient.PutAsJsonAsync($"http://localhost:64635/styles/put/", style);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            return View(style);
        }

        // GET: Style/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/styles/single/" + id);
            var style = await response.Content.ReadAsAsync<Style>();
            if (style == null)
            {
                return HttpNotFound();
            }
            return View(style);
        }

        // POST: Style/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync($"http://localhost:64635/styles/delete/{id}");
      //      Style style = db.Styles.Find(id);
      //      db.Styles.Remove(style);
      //      db.SaveChanges();
            return RedirectToAction("Index");
        }

  /*      protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/

        private async void PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/categories/");
            var categoriesQuery = await response.Content.ReadAsAsync<IEnumerable<Category>>();

            ViewBag.CategoryID = new SelectList(categoriesQuery, "CategoryID", "Name", selectedCategory);
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
