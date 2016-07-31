using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Helpers
{
    

    public enum RechargeType
    {
        Prepaid=0,
        Postpaid=1,
        Vochers=2
    }
    public enum ServiceType
    {
        National=0,
        International=1,
        DataCards=2,
        ShoppingCards = 3
    }
}