using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.ViewModels
{
    public class MyCampFavoriteViewModel
    {
        public int fCampsiteID { get; set; }
        public string fCampsiteName { get; set; }
        public string fCity { get; set; }
        public double fScore { get; set; }
        public string fPhotoUrl { get; set; }
    }
}