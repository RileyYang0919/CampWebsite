using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    public class CTents
    {
        public string fTentName { get; set; }
        public string fTentCategory { get; set; }
        public int fTentPeople { get; set; }
        public int fTentQuantity { get; set; }
        public int fTentPriceWeekday { get; set; }
        public int fTentPriceWeekend { get; set; }
        public int fTentPriceHoliday { get; set; }
    }
}