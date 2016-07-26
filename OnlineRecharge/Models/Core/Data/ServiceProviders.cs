using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class ServiceProviders : BaseEntity
    {
        public ServiceProviders()
        {

        }

        public string Name { get; set; }
        public string Code { get; set; }

    }
}