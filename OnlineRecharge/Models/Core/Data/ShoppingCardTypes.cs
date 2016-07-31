using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    /// <summary>
    /// Need to configure from db. itunes, netflix
    /// </summary>
    public class ShoppingCardTypes : BaseEntity
    {
        public ShoppingCardTypes()
        {

        }

        public string Name { get; set; }
        public string Code { get; set; }

    }
}