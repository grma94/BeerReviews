﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeerReviews.Database.Models;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Net.Http;

namespace BeerReviews.Controllers
{
    public class BeerController : Controller
    {
        // GET: Beer
        public async Task<ActionResult> Index(string sortOrder, int? breweryId, bool? isPlace, int? styleId, bool? p)
        {
            string address = "http://52.178.159.188:8001/";
            string address2 = "http://52.169.111.92:8001/";
            System.Random r = new System.Random();
            bool a=(r.NextDouble() > 0.5);
            address = a ? address2 : address; 
            var httpClient = new HttpClient();
            var response1 = await httpClient.GetAsync(address+"wa/styles/");
            var stylesQuery = await response1.Content.ReadAsAsync<IEnumerable<Style>>();
            ViewBag.StyleID = new SelectList(stylesQuery, "StyleID", "Name", null);
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.GravitySortParm = sortOrder == "gravity" ? "gravity_desc" : "gravity";
            ViewBag.IbuSortParm = sortOrder == "ibu" ? "ibu_desc" : "ibu";
            ViewBag.AbvSortParm = sortOrder == "abv" ? "abv_desc" : "abv";
            ViewBag.ReviewsSortParm = sortOrder == "rc" ? "rc_desc" : "rc";
            ViewBag.AvgSortParm = sortOrder == "avg" ? "avg_desc" : "avg";
            if (sortOrder == null) { sortOrder = "name"; }
            if (breweryId == null && styleId == null)
            {
                response1 = await httpClient.GetAsync(address + "wa/beers/many/" + "all/" + sortOrder);
                var beers = await response1.Content.ReadAsAsync<List<BeerWithAvg>>();

                return View(beers);
                //      return View(Sort(sortOrder, beers));
            }
            else
            {
                response1 = await httpClient.GetAsync("http://52.178.159.188:8001/wa/beers/many/" + styleId + "/" + sortOrder);
                var beers = await response1.Content.ReadAsAsync<IEnumerable<BeerWithAvg>>();
                ViewBag.Style = styleId;
                return View(beers);
                // return View(Sort(sortOrder, beers.ToList()));
            }
        }

        // GET: Beer/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://52.178.159.188:8001/wa/beers/single/" + id);
            var beer = await response.Content.ReadAsAsync<BeerFDetails>();

            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // GET: Beer/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://52.178.159.188:8001/wa/beers/single/" + id);
            var beer = await response.Content.ReadAsAsync<BeerFDetails>();
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // POST: Beer/Delete/5
        [Authorize]
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
            HttpResponseMessage response = await httpClient.DeleteAsync($"http://beerreviewswebapi20170525061826.azurewebsites.net/beers/delete/{id}");
            return RedirectToAction("Index");
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