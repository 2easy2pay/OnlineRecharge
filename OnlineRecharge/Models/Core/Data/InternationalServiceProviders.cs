using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Foreign Key
        /// </summary>
        public virtual Countries Country { get; set; }
    }
}