using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class ShoppingCards : BaseEntity
    {
        public ShoppingCards()
        {

        }
        public int CustomerID { get; set; }
        public decimal amount { get; set; }

       

        public int ShoppingCardTypesID { get; set; }
        [ForeignKey("ShoppingCardTypesID")]
        public ShoppingCardTypes ServiceProvider { get; set; }

    }
}