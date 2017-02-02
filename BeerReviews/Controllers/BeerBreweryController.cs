using BeerReviews.Data;
using BeerReviews.Models;
using BeerReviews.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeerReviews.Controllers
{
    public class BeerBreweryController : Controller
    {
        private BeerReviewsContext db = new BeerReviewsContext();
     
        // GET: BeerBrewery/Create
        public ActionResult Create()
        {
            PopulateStylesDropDownList();
            return View();
        }

        // POST: BeerBrewery/Create
        [HttpPost]
        public ActionResult Create([Bind] BeerBreweryViewModel beerBreweryVM, HttpPostedFileBase file)
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
                beer.ImageUrl=FileUpload(file);

                    db.Beers.Add(beer);
                    db.SaveChanges();



                foreach(var bb in beerBreweryVM.BreweriesNames)
                {
                    var bId = db.Breweries.Where(b => b.Name == bb);
                    if (bId.Any())
                    { 
                    var bb2 = new BeerBrewery();
                    bb2.BeerID = beer.BeerID;
                    bb2.BreweryID = bId.First().BreweryID;
                    db.BeerBreweries.Add(bb2);
                    db.SaveChanges();
                    }
                }
                
                foreach (var bb in beerBreweryVM.BreweriesPlacesNames)
                {
                    var bId = db.Breweries.Where(b => b.Name == bb);
                    if (bId.Any())
                    {
                        var bb2 = new BeerBrewery();
                        bb2.BeerID = beer.BeerID;
                        bb2.isPlace = true;
                        bb2.BreweryID = bId.First().BreweryID;
                        db.BeerBreweries.Add(bb2);
                        db.SaveChanges();
                    }
                }
                //     var id = beerBreweryVM.BreweriesNames.First();
                //     beerBrewery.BreweryID=db.Breweries.Where(b => b.Name == id).First().BreweryID;

                //     db.BeerBreweries.Add(beerBrewery);


                PopulateStylesDropDownList(beerBreweryVM.StyleID);
                return RedirectToAction("Index", "Beer");
            }
            catch
            {
                PopulateStylesDropDownList(beerBreweryVM.StyleID);
                return View();
            }
        }

        [HttpPost]
        public JsonResult Create2(string Prefix)
        {
            //Note : you can bind same list from database  
            List<Brewery> ObjList = db.Breweries.ToList();

            //Searching records from list using LINQ query  
            var BreweryName = (from N in ObjList
                               where N.Name.ToLower().Contains(Prefix.ToLower())
                               select new { N.Name });
            //     var BreweryName = db.Beers.Where(b => b.Name.StartsWith(Prefix)).ToList();
            //                        where N.Name.StartsWith(Prefix)
            //                         select new { N.Name });
            return Json(BreweryName, JsonRequestBehavior.AllowGet);
        }

        // GET: BeerBrewery/Edit/5
        public ActionResult Edit(int id)
        {
            var beer=db.Beers.Find(id);
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
            PopulateStylesDropDownList(beer.StyleID);
            return View(bbvm);
        }

        // POST: BeerBrewery/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind] BeerBreweryViewModel beerBreweryVM, HttpPostedFileBase file)
        {
            Beer beer = db.Beers.Find(beerBreweryVM.BeerID);
            if(beer.Abv != beerBreweryVM.Abv)
            {
                beer.Abv = beerBreweryVM.Abv;
            }
            if(beer.Description != beerBreweryVM.Description)
            {
                beer.Description = beerBreweryVM.Description;
            }
            if(beer.Gravity != beerBreweryVM.Gravity)
            {
                beer.Gravity = beerBreweryVM.Gravity;
            }
            if(beer.IBU != beerBreweryVM.IBU)
            {
                beer.IBU = beerBreweryVM.IBU;
            }
           if(beer.Name != beerBreweryVM.Name)
            {
                beer.Name = beerBreweryVM.Name;
            }
            if(beer.StyleID != beerBreweryVM.StyleID)
            {
                beer.StyleID = beerBreweryVM.StyleID;
            }
            if (file != null)
            {
                beer.ImageUrl = FileUpload(file);
            }

            //         db.Beers.Add(beer);

            db.SaveChanges();


            foreach (var bb in beerBreweryVM.BreweriesNames)
            {
                var bId = db.Breweries.Where(b => b.Name == bb);
                if (bId.Any())
                {
                    var bb2 = new BeerBrewery();
                    bb2.BeerID = beer.BeerID;
                    bb2.BreweryID = bId.First().BreweryID;
                    if (db.BeerBreweries.Find(bb2.BeerID, bb2.BreweryID, bb2.isPlace) == null)
                    {
                        db.BeerBreweries.Add(bb2);
                        db.SaveChanges();
                    }
                }
            }

            foreach (var bb in beerBreweryVM.BreweriesPlacesNames)
            {
                var bId = db.Breweries.Where(b => b.Name == bb);
                if (bId.Any())
                {
                    var bb2 = new BeerBrewery();
                    bb2.BeerID = beer.BeerID;
                    bb2.isPlace = true;
                    bb2.BreweryID = bId.First().BreweryID;
                    if (db.BeerBreweries.Find(bb2.BeerID, bb2.BreweryID, bb2.isPlace) == null)
                    {
                        db.BeerBreweries.Add(bb2);
                        db.SaveChanges();
                    }
                }
                
            }
            foreach (var bb in db.Beers.Find(beer.BeerID).BeerBreweries.ToList())
            {
                if(!bb.isPlace && !beerBreweryVM.BreweriesNames.Where(n=>n == bb.Brewery.Name).Any())
                {
                    db.BeerBreweries.Remove(bb);
                    db.SaveChanges();
                }
                if (bb.isPlace && !beerBreweryVM.BreweriesPlacesNames.Where(n => n == bb.Brewery.Name).Any())
                {
                    db.BeerBreweries.Remove(bb);
                    db.SaveChanges();
                }
            }



            /*

                       try
                       {
                           // TODO: Add update logic here
                           db.Entry(brewery).State = EntityState.Modified;
                           db.SaveChanges();
                           return RedirectToAction("Index","Beer");
                       }
                       catch
                       {
                           return View();
                       }*/
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

        private void PopulateStylesDropDownList(object selectedStyle = null)
        {
            var stylesQuery = from s in db.Styles
                              orderby s.Name
                              select s;
            ViewBag.StyleID = new SelectList(stylesQuery, "StyleID", "Name", selectedStyle);
        }
    }


}
