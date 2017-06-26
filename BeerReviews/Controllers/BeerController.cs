using System;
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
     //       var firstTime = DateTime.Now;
            string address1 = "http://52.178.159.188:8001/";
        /*      string address2 = "http://52.169.111.92:8001/";
              System.Random r = new System.Random();
              bool a=(r.NextDouble() > 0.5);
              string address = a ? address2 : address1;
            address2 = a ? address1 : address2;
            */

            
            var httpClient = new HttpClient();
            var client2 = new HttpClient();
     
           
     //       var response = await httpClient.GetAsync(address2 + "wa/styles/");
            //  var timeE1 = (DateTime.Now - firstTime).ToString();
            if (sortOrder == null) { sortOrder = "name"; }

       //     var response1 = await client2.GetAsync(address + "wa/beers/many/" + "all/" + sortOrder);
            //      var timeE1 = (DateTime.Now - firstTime).TotalMilliseconds.ToString();
            /*     var tasks = new List<Task>();
                 tasks.Add(response);
                 tasks.Add(response1);
                 var tasksT = tasks.ToArray();
              */
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.GravitySortParm = sortOrder == "gravity" ? "gravity_desc" : "gravity";
            ViewBag.IbuSortParm = sortOrder == "ibu" ? "ibu_desc" : "ibu";
            ViewBag.AbvSortParm = sortOrder == "abv" ? "abv_desc" : "abv";
            ViewBag.ReviewsSortParm = sortOrder == "rc" ? "rc_desc" : "rc";
            ViewBag.AvgSortParm = sortOrder == "avg" ? "avg_desc" : "avg";



    //        var timeE2 = (DateTime.Now - firstTime).TotalMilliseconds.ToString();

            //    var beers = await response1.Content.ReadAsAsync<List<BeerWithAvg>>();
            //        var styles = stylesQuery.Result;
            //       var beersW = beers.Result;


            if (breweryId == null && styleId == null)
            {
                Label:
                try
                {
                    var response = httpClient.GetAsync(address1 + "wa/styles/");
                    var response1 = client2.GetAsync(address1 + "wa/beers/many/" + "all/" + sortOrder);
                await Task.WhenAll(response, response1);
                var stylesQuery = response.Result.Content.ReadAsAsync<IEnumerable<Style>>().Result;
                    var beers = response1.Result.Content.ReadAsAsync<List<BeerWithAvg>>().Result;
                    if (stylesQuery.First().Beers != null || beers.First().BeerBreweries == null)
                    {
                        goto Label;
                    }
                    ViewBag.StyleID = new SelectList(stylesQuery, "StyleID", "Name", null);
                    return View(beers);
                }
                                 catch (HttpRequestException e)
                {
                    goto Label;
                }


                //  var response1 = await httpClient.GetAsync(address + "wa/beers/many/" + "all/" + sortOrder);
                //   var beers = await response1.Content.ReadAsAsync<List<BeerWithAvg>>();
                //    var timeE3 = (DateTime.Now - firstTime).TotalMilliseconds.ToString();
                /*         beers.First().ImageUrl = timeE1;
                         beers.First().Name = timeE2;
                         beers.First().StyleName = timeE3;*/

                //      return View(Sort(sortOrder, beers));
            }
            else
            {
                var response = httpClient.GetAsync(address1 + "wa/styles/");
                var response2 = client2.GetAsync(address1+"wa/beers/many/" + "styleID="+styleId + "/" + sortOrder);
         //       Task.WaitAll(response, response2);
                var stylesQuery = response.Result.Content.ReadAsAsync<IEnumerable<Style>>().Result;
                ViewBag.StyleID = new SelectList(stylesQuery, "StyleID", "Name", null);
                var beers = response2.Result.Content.ReadAsAsync<IEnumerable<BeerWithAvg>>().Result;
               
                ViewBag.Style = styleId;
                return View(beers);
                // return View(Sort(sortOrder, beers.ToList()));
            }
        }

        async Task<int> ProcessURLAsync(string url, HttpClient client)
        {
            var byteArray = await client.GetByteArrayAsync(url);
            return byteArray.Length;
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