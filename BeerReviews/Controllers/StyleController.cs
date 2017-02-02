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

namespace BeerReviews.Controllers
{
    public class StyleController : Controller
    {
        private BeerReviewsContext db = new BeerReviewsContext();

        // GET: Style
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Style/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return View(style);
        }

        // GET: Style/Create
        public ActionResult Create()
        {
            PopulateCategoriesDropDownList();
            return View();
        }

        // POST: Style/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StyleID,Name,Description,CategoryID")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Add(style);
                db.SaveChanges();
                PopulateCategoriesDropDownList(style.CategoryID);
                return RedirectToAction("Index");
            }
            PopulateCategoriesDropDownList(style.CategoryID);
            return View(style);
        }

        // GET: Style/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            PopulateCategoriesDropDownList(style.CategoryID);
            return View(style);
        }

        // POST: Style/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StyleID,Name,Description,CategoryID")] Style style)
        {
            if (ModelState.IsValid)
            {
                PopulateCategoriesDropDownList(style.CategoryID);
                db.Entry(style).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(style);
        }

        // GET: Style/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return View(style);
        }

        // POST: Style/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Style style = db.Styles.Find(id);
            db.Styles.Remove(style);
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

        private void PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var categoriesQuery = from s in db.Categories
                                 orderby s.Name
                                 select s;
            ViewBag.CategoryID = new SelectList(categoriesQuery, "CategoryID", "Name", selectedCategory);
        }
    }
}
