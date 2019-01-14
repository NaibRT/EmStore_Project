using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Saart.ViewModel;
using Saart.Models;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Saart.Controllers
{
    public class CheckoutController : AsyncController
    {
        SaartEntities1 db = new SaartEntities1();
        // GET: Checkout

      
        public async Task<ActionResult> Checkout(int? id)
        {
            if (id != null)
            {

                CardViewModel newmodel = new CardViewModel();
                newmodel.Categories =await db.Categories.Where(x=>x.Status==true).ToListAsync();
                newmodel.Products =await db.Products.Where(x => x.Id == id).ToListAsync();

                return View(newmodel);
            }
            else
	        {
                List<string> ProductsId = Request.Cookies["Cards"].Value.Split(' ').ToList();
                CardViewModel newmodel = new CardViewModel()
                {
                    Categories =await db.Categories.Where(x=>x.Status==true).ToListAsync(),
                    Products = CardController.FindCookieProduct(ProductsId,await db.Products.ToListAsync())

                };
                return View(newmodel);

            }


        }



        public async Task<JsonResult> SendMessage(ProductJson[] arr,User user)
        {
            ValidationContext context = new ValidationContext(user);
            var list = new List<ValidationResult>();
           var result =Validator.TryValidateObject(user, context, list);
            if (arr!=null&&arr.Length>0&& result)
            {

                var email =await db.AdminInformations.Where(x => x.Name == "Email").SingleOrDefaultAsync();
                var password =await db.AdminInformations.Where(x => x.Name == "EmailPassword").SingleOrDefaultAsync();
                string userText = $"Sifarişçinin Adi: {user.Name},/n Soyadı: {user.Surname},/n  Telefonu:{user.Number},/n Çatdırılma Ünvanı:{user.Adress}";
                var message = FindProduct(arr);
                List<Product> Products = new List<Product>();
                
                try
                {
                    
                    MailMessage mail = new MailMessage(email.Value,email.Value);
                    mail.Subject = "Sifariş";
                    mail.Body = userText+"<br>"+message;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential
                         (email.Value,password.Value); // use valid credentials
                    smtp.Port = 587;
                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                    foreach (var item in arr)
                    {
                        foreach (var itemP in db.Products)
                        {
                            if (item.Id == itemP.Id)
                            {
                                if (itemP.Count>0)
                                {
                                    itemP.Count -= item.Count;
                                    itemP.SoldCount = item.Count;
                                }
                                else
                                {
                                    itemP.Status = false;
                                }

                            }
                        }
                        db.SaveChanges();
                    }
                    Request.Cookies["Cards"].Value = null;
                    Response.Cookies.Add(Request.Cookies["Cards"]);
                    return Json(new { boolvalue=true});
                    
                }
                catch (Exception)
                {
                    return Json(new { boolvalue =false,validatioList=list });

                }

            }
            else
            {
                return Json(new { boolvalue = false, validatioList = list });
            }


          
        }


        private  string  FindProduct(ProductJson[] ar)
        {
            List<Product> products = new List<Product>();
            string messageText=null;
            foreach (var item1 in ar)
            {
                foreach (var item in db.Products)
                {
                    if (item.Id==item1.Id)
                    {
                        products.Add(item);
                        messageText += $"<br/> Mehsulun Kodu:{item.Id};,Mehsulun Adi:{item.Name};,Sifaris Sayi:{item1.Count};";
                    }
                }
            }
            return messageText;
        }
    }
}