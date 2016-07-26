using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class InternationalServiceProviders : BaseEntity
    {
        public InternationalServiceProviders()
        {

        }

        public string Name { get; set; }
        public string Code { get; set; }
        public int CountryID { get; set; }

        [ForeignKey("CountryID")]
        public Countries Country { get; set; }

    }
}