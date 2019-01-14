using Saart.Models;
using Saart.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Saart.Controllers
{
    public class ProductController : AsyncController
    {
        SaartEntities1 db = new SaartEntities1();
        // GET: Product
        public async Task<ActionResult> Product(int? id)
        {

            var selectedProduct =await db.Products.FindAsync(id);
            var products = await db.Products.Where(x => x.SubCategoryId == selectedProduct.SubCategoryId&&x.Status==true&&x.Count>0).ToListAsync();
            var bestSeller = await db.Products.Where(x=>x.Status==true&&x.Count>0).OrderByDescending(x => x.SoldCount).Take(3).ToListAsync();
            ProductControllerViewModel newProductControllerViewModel = new ProductControllerViewModel()
            {
                Product = selectedProduct,
                Products = products,
                BestSeller = bestSeller,
                Categories = db.Categories.Where(x=>x.Status==true).ToList()
            };
            return View(newProductControllerViewModel);
        }

    }
}