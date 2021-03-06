﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class DataCardRecharges : BaseEntity
    {
        public DataCardRecharges()
        {

        }
        public int CustomerID { get; set; }
        public string MobileNumber { get; set; }
        public decimal amount { get; set; }
        

        public int ServiceProviderID { get; set; }
        [ForeignKey("ServiceProviderID")]
        public ServiceProviders ServiceProvider { get; set; }

    }
}