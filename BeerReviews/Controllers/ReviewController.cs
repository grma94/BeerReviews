using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeerReviews.Database.Models;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Threading.Tasks;

namespace BeerReviews.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        public async Task<ActionResult> Index(int? beerId, string userId, string sortParam)
        {
            var httpClient = new HttpClient();
            if (beerId == null && userId == null)
            {
                var response = await httpClient.GetAsync("http://52.178.159.188:8001/wa/reviews/many/" + "all");
                var reviews = await response.Content.ReadAsAsync<List<ReviewWName>>();
                return View(reviews);
            }
            else 
            //if (beerId != 0)
            {
                var response = await httpClient.GetAsync("http://52.178.159.188:8001/wa/reviews/many/" + beerId);
                var reviews = await response.Content.ReadAsAsync<List<ReviewWName>>();
                if (reviews.Count() == 0) return Content("No reviews of this beer found");
                return View(reviews);
            }
     /*       else
            {
                var results = db.Reviews.Where(r => r.UserName == userId);
                if (results.Count() == 0) return Content("No reviews written by this user");
                return PartialView(results);
            }*/
        }

        // GET: Review/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://52.178.159.188:8001/wa/reviews/single/" + id);
            var review = await response.Content.ReadAsAsync<ReviewWName>();
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }
        [Authorize]
        // GET: Review/Create
        public ActionResult Create(int BeerID)
        {

            Review r = new Review();
            r.BeerID = BeerID;

   //         string userName = User.Identity.GetUserName();
   //         r.UserName = userName;
            return View(r);
            
        }

        // POST: Review/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ReviewID,Aroma,Taste,Palate,Apperance,Description,Overall,ImageUrl,UserName,BeerID")] Review review, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                review.Date = DateTime.Now;
                review.UserName = User.Identity.Name;
       //         review.UserID = User.Identity.GetUserId();
                if (review.Overall == 0.0)
                {
                    review.Overall = ((double)(review.Aroma + review.Apperance + review.Palate + review.Taste)) / 7;
                }
                else
                {
                    review.Overall = review.Overall / 7;
                }
                       review.ImageUrl = FileUpload(file);

                var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync("http://beerreviewswebapi20170525061826.azurewebsites.net/reviews/post/", review);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("Details", "Beer",new { id = review.BeerID });
            }

            return View(review);
        }
        [Authorize]
        // GET: Review/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://52.178.159.188:8001/wa/reviews/single/" + id);
            var review = await response.Content.ReadAsAsync<Review>();
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Review/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ReviewID,Aroma,Taste,Palate,Apperance,Overall,Description,ImageUrl,Date,UserID,BeerID")] Review review)
        {
            if (ModelState.IsValid)
            {
                var httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PutAsJsonAsync($"http://beerreviewswebapi20170525061826.azurewebsites.net/breweries/put/", review);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Review/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://52.178.159.188:8001/wa/reviews/single/" + id);
            var review = await response.Content.ReadAsAsync<Review>();
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // DELETE: Review/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync($"http://beerreviewswebapi20170525061826.azurewebsites.net/breweries/delete/{id}");

            return RedirectToAction("Index");
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
