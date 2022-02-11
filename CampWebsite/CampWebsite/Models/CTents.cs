using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    public class CTents
    {
        public int fTentId { get; set; }
        public string fTentName { get; set; }
        public string fTentCategory { get; set; }
        public int fTentPeople { get; set; }
        public int fTentPriceWeekday { get; set; }
        public int fTentPriceWeekend { get; set; }
    }
}