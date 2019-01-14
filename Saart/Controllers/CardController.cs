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
    public class CardController : AsyncController
    {
        SaartEntities1 db = new SaartEntities1();

        // GET: Card
        public async Task<ActionResult> Card()
        {
            // var X= db.Products.ToList();
            CardViewModel newmodel = new CardViewModel();
            newmodel.Categories = await db.Categories.Where(x => x.Status == true).ToListAsync();
            newmodel.BestSeller = await db.Products.Where(x => x.Status == true).OrderByDescending(x => x.SoldCount).Take(3).ToListAsync();
            if (Request.Cookies["Cards"] != null && !string.IsNullOrEmpty(Request.Cookies["Cards"].Value))
            {
                List<string> ProductsId = Request.Cookies["Cards"].Value.Split(' ').ToList();
                newmodel.Products = FindCookieProduct(ProductsId, await db.Products.ToListAsync());
            }

            return View(newmodel);

        }

        [HttpGet]
        public async Task<ActionResult> CardPartialView()
        {

            List<string> ProductsId = Request.Cookies["Cards"].Value.Split(' ').ToList();
            var selectedProducts = FindCookieProduct(ProductsId, await db.Products.ToListAsync());
            return PartialView("CardPartialView", selectedProducts);
        }

        [HttpPost]
        public async Task<ActionResult> CardPartialView(int? id)
        {
            List<string> ProductsId = Request.Cookies["Cards"].Value.Split(' ').ToList();
            if (id != null)
            {
                ProductsId.Remove(id.ToString());
                if (ProductsId.Any())
                {
                    Request.Cookies["Cards"].Value = string.Join(" ", ProductsId);
                    Response.Cookies.Add(Request.Cookies["Cards"]);
                }
                else
                {
                    Request.Cookies["Cards"].Value = null;
                    Response.Cookies.Add(Request.Cookies["Cards"]);
                }

                var selectedProducts = FindCookieProduct(ProductsId, await db.Products.ToListAsync());
                return PartialView("CardPartialView", selectedProducts);
            }

            else
            {
                var selectedProducts = FindCookieProduct(ProductsId, await db.Products.ToListAsync());
                return PartialView("CardPartialView", selectedProducts);
            }


        }


        //--------------Find Products that id exist in Cookie------------------------------------
        public static List<Product> FindCookieProduct(List<string> arr, List<Product> products)
        {
            List<Product> selectProducts = new List<Product>();
            foreach (var itemA in arr)
            {
                foreach (var itemP in products)
                {
                    if ((Convert.ToInt32(itemA) == itemP.Id))
                    {
                        selectProducts.Add(itemP);
                    }
                }
            }
            return selectProducts;
        }

        public ActionResult Hello()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProductToCookie(int id)
        {


            if (Request.Cookies["Cards"] == null)
            {
                HttpCookie cookie = new HttpCookie("Cards");
                cookie.Value = id.ToString();
                cookie.Expires = DateTime.Now.AddHours(24);
                Response.Cookies.Add(cookie);
                List<string> co = Request.Cookies["Cards"].Value.Split(' ').ToList();
                ViewBag.cardcount = co.Count;
                var x = co[0];
                return PartialView("HeaderPartialView");

            }
            else
            {
                if (String.IsNullOrEmpty(Request.Cookies["Cards"].Value))
                {
                    Request.Cookies["Cards"].Value = id.ToString();
                }
                else
                {
                    Request.Cookies["Cards"].Value += ' ' + id.ToString();
                }
                Response.Cookies.Add(Request.Cookies["Cards"]);
                List<string> co = Request.Cookies["Cards"].Value.Split(' ').ToList();
                ViewBag.cardcount = co.Count;
                return PartialView("HeaderPartialView");
            }

        }
        [HttpGet]
        public ActionResult AddProductToCookie()
        {

            if (Request.Cookies["Cards"] == null || String.IsNullOrEmpty(Request.Cookies["Cards"].Value))
            {
                ViewBag.cardcount = 0;
                return PartialView("HeaderPartialView");
            }
            else
            {
                List<string> coo = Request.Cookies["Cards"].Value.Split(' ').ToList();
                ViewBag.cardcount = coo.Count;
                return PartialView("HeaderPartialView");
            }
        }
    }
}
