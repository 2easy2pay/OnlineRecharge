using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class Languages : BaseEntity
    {
        public Languages()
        {

        }

        public string Code { get; set; }
        public string Name { get; set; }

        public virtual LanguageFlagNames Flag { get; set; }
        public virtual LanguageDirections Directions { get; set; }
    }
}