using Saart.Models;
using Saart.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Saart.Controllers
{
    public class IndexController : AsyncController
    {
        SaartEntities1 db = new SaartEntities1();

       
        // GET: Index
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var count = db.Products.ToList().Count;
            if (count%16==0)
            {
                ViewBag.count = (count / 16);
            }
            else
            {
                ViewBag.count = (count / 16)+1;
            }
           // ViewBag.Productes = db.Products.Where(x=>x.Status==true&&x.Count!=0).ToList();
            
            
            IndexViewModel newmodel =  new IndexViewModel()
            {
                Categories =await db.Categories.Where(x=>x.Status==true).ToListAsync(),
                LastAdd = await db.Products.Where(x=>x.Count!=0&&x.Status==true).OrderByDescending(x => x.Id).Take(3).ToListAsync(),
                Products=await db.Products.Where(x => x.Status == true && x.Count != 0).OrderBy(x=>x.Price).Take(16).ToListAsync()
            };

            return  View(newmodel);
        }

        
        public async Task<ActionResult> PostIndex(int id,string name)
        {
            IndexViewModel newmodel = new IndexViewModel();
            if (id!=0&&!String.IsNullOrEmpty(name))
            {


                newmodel.Categories =await db.Categories.Where(x => x.Status == true).ToListAsync();
                newmodel.LastAdd =await db.Products.Where(x => x.Count != 0 && x.Status == true).OrderByDescending(x => x.Id).Take(3).ToListAsync();
                newmodel.Products = await FindCat_SubCat_Products(id, name);

                
            }

            return View("Index",newmodel);
        }

        [HttpGet]
        public async Task<ActionResult> IndexProductList()
        {
            var products =await db.Products.Where(x=>x.Count>0 && x.Status == true).OrderBy(x=>x.Count).Take(16).ToListAsync();
            return PartialView("IndexProductListPartial", products);
        }

        private async Task<List<Product>> FindCat_SubCat_Products(int id,string name)
        {

            List<Product> products = new List<Product>();
            if (name == "cat")
            {
                var cat = await db.Categories.FindAsync(id);

                foreach (var item in cat.SubCategories)
                {
                    if (item.Status==true)
                    {
                        foreach (var itemP in item.Products)
                        {
                            if (itemP.Status==true&&itemP.Count!=0)
                            {
                                products.Add(itemP);
                            }
                           
                        }
                    
                    }

                }
            }
            else if (name == "subcat")
            {
                products =await db.Products.Where(x => x.SubCategoryId == id && x.SubCategory.Status == true && x.Status == true&&x.Count!=0).ToListAsync();
            }
           
            return products;
        }

        [HttpPost]
        public async Task<ActionResult> IndexProductList(int id, string name)
        {

            return PartialView("IndexProductListPartial", await FindCat_SubCat_Products(id, name));
        }

        
        [HttpPost]
        public async Task<ActionResult> ProductSearch(string value)
        {
            List<Product> product = new List<Product>();
            if (!String.IsNullOrEmpty(value))
            {
                if (value == "default")
                {
                     product =await db.Products.Where(x => x.Count != 0 && x.Status == true).OrderByDescending(x=>x.SoldCount).ToListAsync();
                    return PartialView("IndexProductListPartial", product);
;
                }

                else if (value == "cheap")
                {
                     product = await db.Products.Where(x =>x.Count != 0 && x.Status == true).OrderByDescending(x => x.Price).ToListAsync();
                    return PartialView("IndexProductListPartial", product.ToList());
                }

                else if (value == "expensive")
                {
                     product = await db.Products.Where(x =>  x.Count != 0 && x.Status == true).OrderBy(x => x.Price).ToListAsync();
                    return PartialView("IndexProductListPartial", product.ToList());
                   
                }
                else if (value== "discount")
                {
                     product = await db.Products.Where(x => x.Discount >0&&x.Count!=0&& x.Status == true).OrderBy(x => x.Name).ToListAsync();
                    return PartialView("IndexProductListPartial", product.ToList());
                }
               
                else if (value == "productName")
                {
                     product = await db.Products.Where(x =>  x.Count != 0 && x.Status == true).OrderBy(x => x.Name).ToListAsync();
                    return PartialView("IndexProductListPartial", product.ToList());
                   
                }
                else
                {
                     product = await db.Products.Where(x => (x.Title.Contains(value)||x.Name.Contains(value)||x.Description.Contains(value))&& x.Count != 0&&  x.Status == true).ToListAsync();
                   
                    if (product!=null&&product.Count()>0)
                    {
                        return  PartialView("IndexProductListPartial", product);

                    }
                    else
                    {
                       
                        return PartialView("IndexProductListPartial", product);
                    }


                }

            }
            return PartialView("IndexProductListPartial",product );
        }


        public async Task<ActionResult> Pagination(int count)
        {
            var products =await db.Products.Where(x => x.Count > 0 && x.Status == true).OrderBy(x => x.Count).Skip(count * 16).Take(16).ToListAsync();
            return PartialView("IndexProductListPartial", products);
        }

    }
}