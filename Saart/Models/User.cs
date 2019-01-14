using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Saart.Controllers;

namespace Saart.Models
{
    public class User
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string Adress { get; set; }
        
    }
}