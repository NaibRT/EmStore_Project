using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Saart.Models;

namespace Saart.Areas.Admin.Controllers
{
    [Auth]
    public class AdminSoldProductController : Controller
    {
        SaartEntities1 db = new SaartEntities1();
        // GET: Admin/SoldProduct
        public ActionResult Index()
        {
            
            var products = db.Products.Where(x => x.SoldCount != null && x.SoldCount != 0);
            return View(products);
        }
        public ActionResult Create()
        {
            return View();
        }
    }
}