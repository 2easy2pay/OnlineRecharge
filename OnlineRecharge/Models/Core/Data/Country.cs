using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class Country : BaseEntity
    {
        public Country()
        {

        }
      
        public string Name { get; set; }
    }
    
}