using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class Countries : BaseEntity
    {
        public Countries()
        {

        }

        public string Code { get; set; }
        public string Name { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public InternationalServiceProviders ServiceProvider { get; set; }

    }
}