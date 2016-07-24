using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class NationalRechargeTypes:BaseEntity
    {
        public NationalRechargeTypes()
        {

        }

        public string Name { get; set; }

        public NationalRecharges NationalRecharge { get; set; }
    }
}