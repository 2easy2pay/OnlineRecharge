using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class LanguageDirections
    {
        public LanguageDirections()
        {

        }
        public int ID { get; set; }
        public string Direction { get; set; }
        public Languages Language { get; set; }
    }
}