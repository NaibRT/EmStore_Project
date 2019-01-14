using Saart.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Saart.Areas.Admin.Controllers
{
    [Auth]
    public class AdminCategoryController : AsyncController
    {
        SaartEntities1 db = new SaartEntities1();
       
        // GET: Admin/Category
        public async Task<ActionResult> Index()
        {
            return View(await db.Categories.ToListAsync());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string Name)
        {
            SaartEntities1 db = new SaartEntities1();
            if (!String.IsNullOrEmpty(Name))
            {
                Category newCategory = new Category() {
                    Name = Name
                };

                db.Categories.Add(newCategory);
               await db.SaveChangesAsync();
            }
           

            return View();
        }

        public async Task<ActionResult> Update(int? Id)
        {
            
            var category =await db.Categories.FindAsync(Id);
            return View(category);
        }
        [HttpPost]
        public async Task<ActionResult> Update(int? Id , string Name)
        {
            
            var categories=await db.Categories.FindAsync(Id);
            categories.Name = Name;
           await db.SaveChangesAsync();
            return View();
        }


 

      
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var category =await db.Categories.FindAsync(id);
                if (category.Status==true)
                {
                    if (category == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                       

                        foreach (var item in category.SubCategories)
                        {
                            var products = db.Products.Where(x => x.SubCategoryId == item.Id);
                            item.Status = false;
                            foreach (var itemp in products)
                            {
                                itemp.Status = false;
                            }
                        }

                        category.Status = false;
                    }
                }
                else
                {
                    if (category == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {

                        foreach (var item in category.SubCategories)
                        {
                            var products = db.Products.Where(x => x.SubCategoryId == item.Id);
                            item.Status = true;
                            foreach (var itemp in products)
                            {
                                itemp.Status = true;
                            }
                        }

                        category.Status = true;
                    }
                }
                
               await db.SaveChangesAsync();
            }
            
            return RedirectToAction("Index");
        }

    }
}