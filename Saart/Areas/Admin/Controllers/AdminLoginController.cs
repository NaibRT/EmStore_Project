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
    public class AdminLoginController : AsyncController
    {
        SaartEntities1 db = new SaartEntities1();
        // GET: Admin/Login
        [HttpPost]
        public async Task<ActionResult> Index(string username, string password)
        {
            if (username != null && password != null)
            {
                var userName =await db.AdminInformations.Where(x=>x.Name== "UserName").SingleOrDefaultAsync();
                var Password =await db.AdminInformations.Where(x=>x.Name== "UserPassword").SingleOrDefaultAsync();
                if (username == userName .Value&& password == Password.Value)
                {
                    Session["Admin"] = true;
                    return RedirectToAction("Index","AdminProduct");
                }
                else
                {
                    return Content("Bele istifadeci tapilamiyor");
                }
            }
            else
            {
                return Content("Boshlug ola bilmez");
            }
            //return View();
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


    }
}