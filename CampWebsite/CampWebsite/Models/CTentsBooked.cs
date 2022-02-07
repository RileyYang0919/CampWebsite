using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    public class CTentsBooked
    {
        public string fTentName { get; set; }
        public int BookedCount { get; set; }

        //返回哪一日
        public int fCheckinDate { get; set; }
    }
}