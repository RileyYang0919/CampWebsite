using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    public class COrderJsonModel
    {
        public int tentID { get; set; }
        public DateTime checkinDate { get; set; }
        public int price { get; set; }
    }
}