using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Saart.Models;

namespace Saart.ViewModel
{
    public class IndexViewModel :CardViewModel
    {
       public List<Product> LastAdd { get; set; }
    }
}