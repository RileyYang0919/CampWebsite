using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.ViewModels
{
    public class ReceiveFormViewModel
    {
        public HttpPostedFileBase CoverPhoto { get; set; }
        //public CTagsSelect firstTag { get; set; }
        public List<string> SelectTags { get; set; }
        public List<string> DayOffs { get; set; }
        public string SelectArea { get; set; }
        public string SelectCity { get; set; }
        public string campName { get; set; }
        public string campIntro { get; set; }
        public string campAddress { get; set; }
        public string campPhone { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string campAltitude { get; set; }
        public bool withoutAltitude { get; set; }
    }
}