using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Saart.Models;

namespace Saart.Areas.Admin.Controllers
{
    [Auth]
    public class AdminSubCategoryController : AsyncController
    {
        SaartEntities1 db = new SaartEntities1();
        // GET: Admin/SubCategory
        public async Task<ActionResult> Index()
        {
            return View(await db.SubCategories.ToListAsync());
        }

        public async Task<ActionResult> Create()
        {
            return View(await db.Categories.ToListAsync());
        }


        [HttpPost]
        public async Task<ActionResult> Create(string Name,int Id)
        {
            if (!String.IsNullOrEmpty(Name) && Id!=null)
            {
                SubCategory newSubCategory = new SubCategory()
                {
                    Name = Name,
                    CategoryID = Id
                };
                db.SubCategories.Add(newSubCategory);
               await db.SaveChangesAsync();
            }
            return View(await db.Categories.ToListAsync());
        }

        public async Task<ActionResult> Update(int? id)
        {
            ViewBag.Categories =await db.Categories.ToListAsync();
            var Subcategory =await db.SubCategories.FindAsync(id);
            return View(Subcategory);
          
        }

        [HttpPost]
        public async Task<ActionResult> Update(string Name, int CategoryId, int id)
        {
            ViewBag.Categories =await db.Categories.ToListAsync();
            var subCategory =await db.SubCategories.FindAsync(id);
            subCategory.Name = Name;
            subCategory.CategoryID = CategoryId;
           await db.SaveChangesAsync();
            return View(subCategory);
        }

        public async Task<ActionResult> Delete(int? id)
        {

            var subCategory =await db.SubCategories.FindAsync(id);
            if (subCategory.Status==true)
            {
                foreach (var item in subCategory.Products)
                {
                    item.Status = false;
                }
                subCategory.Status = false;
               await db.SaveChangesAsync();
            }
            else
            {
                if (subCategory.Category.Status==true)
                {
                    foreach (var item in subCategory.Products)
                    {
                        item.Status = true;
                    }
                    subCategory.Status = true;
                }

            }
           
           await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}