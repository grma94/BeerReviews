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
using Microsoft.AspNet.Identity;

namespace BeerReviews.Controllers
{
    public class ReviewController : Controller
    {
        private BeerReviewsContext db = new BeerReviewsContext();

        // GET: Review
        public ActionResult Index(int? beerId, string userId)
        {
            if (beerId == null && userId == null)
            {
                return View(db.Reviews.ToList());
            }
            else if (beerId != 0)
            {
                var results = db.Reviews.Where(r => r.BeerID == beerId);
                if (results.Count() == 0) return Content("No reviews of this beer found");
                return PartialView(results);
            }
            else
            {
                var results = db.Reviews.Where(r => r.UserName == userId);
                if (results.Count() == 0) return Content("No reviews written by this user");
                return PartialView(results);
            }
        }

        // GET: Review/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Review/Create
        public ActionResult Create(int BeerID)
        {

            Review r = new Review();
            r.BeerID = BeerID;

            string userName = User.Identity.GetUserName();
            r.UserName = userName;
            return View(r);
            
        }

        // POST: Review/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReviewID,Aroma,Taste,Palate,Apperance,Description,ImageUrl,UserName,BeerID")] Review review, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                review.Date = DateTime.Now;
                review.Overall = ((double)(review.Aroma + review.Apperance + review.Palate + review.Taste))/7;
                review.ImageUrl = FileUpload(file);
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Details", "Beer",new { id = review.BeerID });
            }

            return View(review);
        }

        // GET: Review/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Review/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReviewID,Aroma,Taste,Palate,Apperance,Overall,Description,ImageUrl,Date,UserID,BeerID")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Review/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Review/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            var path = review.ImageUrl;
            if (path != "/Content/Images/no_image.png")
            {
                var filePath = Server.MapPath(review.ImageUrl);
                System.IO.File.Delete(filePath);
            }
            db.Reviews.Remove(review);
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
                string pic = System.IO.Path.GetFileName(file.FileName);
                string p = System.IO.Path.Combine("/Content/Images/Reviews", pic);
                string path = Server.MapPath(p);

                file.SaveAs(path);

                return p;
            }
            else return "/Content/Images/no_image.png";
        }
    }
}
