using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Saart.Models
{

    [System.AttributeUsage(AttributeTargets.Property| AttributeTargets.Property, Inherited = true, AllowMultiple = false)]

    sealed class EmailAdressAttribute : RegularExpressionAttribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236


        // This is a positional argument

        public EmailAdressAttribute() : base(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?") { }



    }

  
   
}