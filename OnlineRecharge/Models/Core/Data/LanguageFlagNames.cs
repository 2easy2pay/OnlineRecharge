using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class LanguageFlagNames
    {
        public LanguageFlagNames()
        {

        }
        public int ID { get; set; }
        public string Name { get; set; }
        public Languages Language { get; set; }
    }
}