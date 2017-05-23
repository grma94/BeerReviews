using BeerReviews.Database.Models;
using BeerReviews.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BeerReviews.Controllers
{
    public class BeerBreweryController : Controller
    {   
        // GET: BeerBrewery/Create
        public async Task<ActionResult> Create(int? styleID)
        {
            var httpClient = new HttpClient();
            var response1 = await httpClient.GetAsync("http://localhost:64635/styles/");
            var stylesQuery = await response1.Content.ReadAsAsync<IEnumerable<Style>>();
            ViewBag.StyleID = new SelectList(stylesQuery, "StyleID", "Name",styleID);
            return View();
        }

        // POST: BeerBrewery/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind] BeerBreweryViewModel beerBreweryVM, HttpPostedFileBase file)
        {
            try
            {
                Beer beer = new Beer();
                beer.Abv = beerBreweryVM.Abv;
                beer.Description = beerBreweryVM.Description;
                beer.Gravity = beerBreweryVM.Gravity;
                beer.IBU = beerBreweryVM.IBU;
              //  beer.ImageUrl = beerBreweryVM.ImageUrl;
                beer.Name = beerBreweryVM.Name;
                beer.StyleID = beerBreweryVM.StyleID;
                //            beer.ImageUrl=FileUpload(file);
                beer.ImageUrl = "/Content/Images/no_image.png";
                var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync("http://localhost:64635/beerbreweries/post/", beer);
                var id = await response.Content.ReadAsAsync<int>();
               // response.EnsureSuccessStatusCode();

                response = await httpClient.GetAsync("http://localhost:64635/breweries/many/" + "nil");
                var breweries = await response.Content.ReadAsAsync<IEnumerable<Brewery>>();
                foreach (var bb in beerBreweryVM.BreweriesNames)
                {

                    var bId = breweries.Where(b => b.Name == bb);
                    if (bId.Any())
                    { 
                    var bb2 = new BeerBrewery();
                    bb2.BeerID = id;
                    bb2.BreweryID = bId.First().BreweryID;
                        response = await httpClient.PostAsJsonAsync("http://localhost:64635/beerbreweries/postbb/", bb2);
                        response.EnsureSuccessStatusCode();
                    }
                }
                
                foreach (var bb in beerBreweryVM.BreweriesPlacesNames)
                {
                    var bId = breweries.Where(b => b.Name == bb);
                    if (bId.Any())
                    {
                        var bb2 = new BeerBrewery();
                        bb2.BeerID = id;
                        bb2.isPlace = true;
                        bb2.BreweryID = bId.First().BreweryID;
                        response = await httpClient.PostAsJsonAsync("http://localhost:64635/beerbreweries/postbb/", bb2);
                        response.EnsureSuccessStatusCode();
                    }
                }


                var response1 = await httpClient.GetAsync("http://localhost:64635/styles/");
                var stylesQuery = await response1.Content.ReadAsAsync<IEnumerable<Style>>();
                ViewBag.StyleID = new SelectList(stylesQuery, "StyleID", "Name", beerBreweryVM.StyleID);
                return RedirectToAction("Index", "Beer");
            }
            catch
            {
                var httpClient = new HttpClient();
                var response1 = await httpClient.GetAsync("http://localhost:64635/styles/");
                var stylesQuery = await response1.Content.ReadAsAsync<IEnumerable<Style>>();
                ViewBag.StyleID = new SelectList(stylesQuery, "StyleID", "Name", beerBreweryVM.StyleID);
                return View();
            }
        }

        [HttpPost]
        public async Task<JsonResult> Create2(string Prefix)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/breweries/many/" + "nil");
            var ObjList = await response.Content.ReadAsAsync<List<Brewery>>();

            //Searching records from list using LINQ query  
            var BreweryName = (from N in ObjList
                               where N.Name.ToLower().Contains(Prefix.ToLower())
                               select new { N.Name });
            return Json(BreweryName, JsonRequestBehavior.AllowGet);
        }

        // GET: BeerBrewery/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/beers/single/" + id);
            var beer = await response.Content.ReadAsAsync<Beer>();
            var bbvm = new BeerBreweryViewModel();
            bbvm.Abv = beer.Abv;
            bbvm.BeerID = beer.BeerID;
            List<String> listS = new List<string>();
            foreach (var b in beer.BeerBreweries.Where(z=>!z.isPlace))
            {
                listS.Add(b.Brewery.Name);
            }
            bbvm.BreweriesNames = listS;
            List<String> listPS = new List<string>();
            foreach (var b in beer.BeerBreweries.Where(z => z.isPlace))
            {
                listPS.Add(b.Brewery.Name);
            }
            bbvm.BreweriesPlacesNames = listPS;
            bbvm.Description = beer.Description;
            bbvm.Gravity = beer.Gravity;
            bbvm.IBU = beer.IBU;
            bbvm.ImageUrl = beer.ImageUrl;
            bbvm.Name = beer.Name;
            bbvm.StyleID = beer.StyleID;
            var response1 = await httpClient.GetAsync("http://localhost:64635/styles/");
            var stylesQuery = await response1.Content.ReadAsAsync<IEnumerable<Style>>();
            ViewBag.StyleID = new SelectList(stylesQuery, "StyleID", "Name", beer.StyleID);
            return View(bbvm);
        }

        // POST: BeerBrewery/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit([Bind] BeerBreweryViewModel beerBreweryVM, HttpPostedFileBase file)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:64635/beers/single/" + beerBreweryVM.BeerID);
            var beer = await response.Content.ReadAsAsync<Beer>();
            Beer tempBeer = new Beer();
            tempBeer.BeerID = beerBreweryVM.BeerID;
            tempBeer.Abv = beerBreweryVM.Abv;
            tempBeer.Description = beerBreweryVM.Description;
            tempBeer.Gravity = beerBreweryVM.Gravity;
            tempBeer.IBU = beerBreweryVM.IBU;
            tempBeer.ImageUrl = beerBreweryVM.ImageUrl;
            tempBeer.Name = beerBreweryVM.Name;
            tempBeer.StyleID = beerBreweryVM.StyleID;
         /*   if (beer.Abv != beerBreweryVM.Abv)
            {
                beer.Abv = beerBreweryVM.Abv;
            }
            if (beer.Description != beerBreweryVM.Description)
            {
                beer.Description = beerBreweryVM.Description;
            }
            if (beer.Gravity != beerBreweryVM.Gravity)
            {
                beer.Gravity = beerBreweryVM.Gravity;
            }
            if (beer.IBU != beerBreweryVM.IBU)
            {
                beer.IBU = beerBreweryVM.IBU;
            }
            if (beer.Name != beerBreweryVM.Name)
            {
                beer.Name = beerBreweryVM.Name;
            }
            if (beer.StyleID != beerBreweryVM.StyleID)
            {
                beer.StyleID = beerBreweryVM.StyleID;
            }
            if (file != null)
            {
                beer.ImageUrl = FileUpload(file);
            }*/
            var beerId = beer.BeerID;
            response = await httpClient.PutAsJsonAsync($"http://localhost:64635/beers/put/", tempBeer);
            response.EnsureSuccessStatusCode();

            response = await httpClient.GetAsync("http://localhost:64635/breweries/many/" + "nil");
            var breweries = await response.Content.ReadAsAsync<IEnumerable<Brewery>>();

            if (beerBreweryVM.BreweriesNames != null)
            {
                foreach (var bb in beerBreweryVM.BreweriesNames)
                {
                    var bId = breweries.Where(b => b.Name == bb);
                    if (bId.Any())
                    {
                        var bb2 = new BeerBrewery();
                        bb2.BeerID = beerId;
                        bb2.BreweryID = bId.First().BreweryID;
                        response = await httpClient.PostAsJsonAsync("http://localhost:64635/beerbreweries/postbb/", bb2);
                        response.EnsureSuccessStatusCode();
                    }
                }

                if (beerBreweryVM.BreweriesPlacesNames != null) {
                    foreach (var bb in beerBreweryVM.BreweriesPlacesNames)
                    {
                        var bId = breweries.Where(b => b.Name == bb);
                        if (bId.Any())
                        {
                            var bb2 = new BeerBrewery();
                            bb2.BeerID = beerId;
                            bb2.isPlace = true;
                            bb2.BreweryID = bId.First().BreweryID;
                            response = await httpClient.PostAsJsonAsync("http://localhost:64635/beerbreweries/postbb/", bb2);
                            response.EnsureSuccessStatusCode();
                        }

                    }
                }
                response = await httpClient.GetAsync("http://localhost:64635/beerbreweries/get/" + beerId);
                var beerbreweries = await response.Content.ReadAsAsync<IEnumerable<BeerBrewery>>();
                if (beerbreweries != null)
                { 
                    foreach (var bb in beerbreweries)
                    {
                        if (beerBreweryVM.BreweriesNames != null)
                        {
                            if (!bb.isPlace && !beerBreweryVM.BreweriesNames.Where(n => n == bb.Brewery.Name).Any())
                            {
                                response = await httpClient.PutAsJsonAsync($"http://localhost:64635/beerbreweries/delete/", bb);
                                response.EnsureSuccessStatusCode();
                            }
                        }
                        if (beerBreweryVM.BreweriesPlacesNames != null)
                        {
                            if (bb.isPlace && !beerBreweryVM.BreweriesPlacesNames.Where(n => n == bb.Brewery.Name).Any())
                            {
                                response = await httpClient.PutAsJsonAsync($"http://localhost:64635/beerbreweries/delete/", bb);
                                response.EnsureSuccessStatusCode();
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Beer");
        }


        public string FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string p = Path.Combine("/Content/Images/Beers", pic);
                string path = Server.MapPath(p);

                file.SaveAs(path);

                return p;
            }
            else return "/Content/Images/no_image.png";
        }
    }


}
