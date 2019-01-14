using Saart.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Saart.Areas.Admin.Controllers
{
    [Auth]
    public class AdminProductController : AsyncController
    {
        SaartEntities1 db = new SaartEntities1();
        // GET: Admin/Product
        public async Task<ActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }

        public async Task<ActionResult> Create()
        {
            return View(await db.SubCategories.ToListAsync());

        }

        [HttpPost]

        public async Task<ActionResult> Create(string Name, decimal Price,decimal? Discount, string Gender, string Description, int SubCategoryId, string Title, int Count, IEnumerable<HttpPostedFileBase> img)
        {
            if (!String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(Description) && !String.IsNullOrEmpty(Gender) && !String.IsNullOrEmpty(Title) && SubCategoryId != null && Price != null && Count != 0 && img.First()!=null)
            {

                Product product = new Product()
                {
                    Name = Name,
                    Price = Price,
                    Discount = Price - Convert.ToDecimal((Price * Discount) / 100),
                    Gender = Gender,
                    Description = Description,
                    SubCategoryId = SubCategoryId,
                    Title = Title,
                    Count = Count
                };
                db.Products.Add(product);
               await db.SaveChangesAsync();


                foreach (var item in img)
                {

                    if (item.ContentLength <= 20 * 1024 * 1024)
                    {
                        if (item.ContentType.ToLower() == "image/jpg" || item.ContentType.ToLower() == "image/jpeg" || item.ContentType.ToLower() == "image/png")
                        {
                            var originalName = Path.GetFileName(item.FileName);
                            string fieldID = Guid.NewGuid().ToString().Replace("-", "");
                            string filename = fieldID + originalName;
                            var path = Path.Combine(Server.MapPath("~/Areas/Admin/IMAGES/"), filename);
                            item.SaveAs(path);
                            Models.File photo = new Models.File()
                            {
                                Sources = filename,
                                ProductId = product.Id
                            };
                            db.Files.Add(photo);
                            db.SaveChanges();


                        }

                        else
                        {
                            return Content("Image is not upload");
                        }
                    }

                    else
                    {
                        return Content("Image size is not valid");
                    }

                }


            }
            return View( await db.SubCategories.ToListAsync());
        }


        public async Task<ActionResult> Update(int? id)
        {
            ViewBag.SubCategories =await db.SubCategories.ToListAsync();
            var product =await db.Products.FindAsync(id);
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Update(int? Id, string Name, decimal Price,decimal? Discount, string Gender, string Description, int SubCategoryId, string Title, int Count, HttpPostedFileBase[] img)
        {

            ViewBag.SubCategories =await db.SubCategories.ToListAsync();
            var product = db.Products.Find(Id);
            product.Name = Name;
            product.Price = Price;
            product.Discount =Price- Convert.ToDecimal((Price * Discount) / 100);
            product.Gender = Gender;
            product.Description = Description;
            product.SubCategoryId = SubCategoryId;
            product.Title = Title;
            product.Count = Count;
            var files = db.Files.Where(x => x.ProductId == Id);
            var count = img.Length;

            //--------------------------------------Create new Files------------------------------------
            if (img.First()!=null)
            {
                foreach (var item in files)
                {
                    string path = Server.MapPath("~/Areas/Admin/IMAGES/" + item.Sources);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    db.Files.Remove(item);
                }
              await  db.SaveChangesAsync();
                foreach (var item in img)
                { 
                    if (item.ContentLength <= 2 * 1024 * 1024)
                    {
                        if (item.ContentType.ToLower() == "image/jpg" || item.ContentType.ToLower() == "image/jpeg" || item.ContentType.ToLower() == "image/png")
                        {
                            var originalName = Path.GetFileName(item.FileName);
                            string fieldID = Guid.NewGuid().ToString().Replace("-", "");
                            string filename = fieldID + originalName;
                            var path = Path.Combine(Server.MapPath("~/Areas/Admin/IMAGES/"), filename);
                            item.SaveAs(path);
                            Models.File photo = new Models.File()
                            {
                                Sources = filename,
                                ProductId = product.Id
                            };
                            db.Files.Add(photo);
                           await db.SaveChangesAsync();
                        }

                        else
                        {
                            return Content("Image is not upload");
                        }
                    }

                    else
                    {
                        return Content("Image size is not valid");
                    }

                }
            }
          await  db.SaveChangesAsync();

            return View(product);
        }


        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product =await db.Products.FindAsync(id);
            var files = db.Files.Where(x => x.ProductId == product.Id);
            

            foreach (var item in files)
            {
                string path = Server.MapPath("~/Areas/Admin/IMAGES/" + item.Sources);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                db.Files.Remove(item);
            }
            db.Products.Remove(product);
           await db.SaveChangesAsync();


            if (product == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Active_DeActive_Product(int id)
        {
            Product product;
            if (id!=0)
            {
                product =await db.Products.FindAsync(id);
                if (product!=null)
                {
                    if (product.Status==true)
                    {
                        product.Status = false;
                       await db.SaveChangesAsync();
                        return View("Index",await db.Products.ToListAsync());
                    }
                    else
                    {
                        if (product.SubCategory.Status==true)
                        {
                            product.Status = true;
                        }
                       
                       await db.SaveChangesAsync();
                        return View("Index",await db.Products.ToListAsync());
                    }
                }
            }


            return View("Index",await db.Products.ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }
}