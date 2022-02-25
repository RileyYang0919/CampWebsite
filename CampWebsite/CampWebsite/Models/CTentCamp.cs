using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    public class CTentCamp
    {
        public int fCampsiteID { get; set; }
        public string fCampsiteName { get; set; }
        public string fCampsiteArea { get; set; }
        public string fCampsiteCity { get; set; }
        public string fCampsiteClosedDay { get; set; }
        public string fCampsiteAltitude { get; set; }
        public double fAvgComment { get; set; }
        public string fTentPhotoURL { get; set; }

    }
}