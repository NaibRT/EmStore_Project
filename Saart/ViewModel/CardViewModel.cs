using Saart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saart.ViewModel
{
    public class CardViewModel
    {
       public  List<Category> Categories { get; set; }
        public List<Product> BestSeller { get; set; }
        public List<Product> Products { get; set; }
    }
}