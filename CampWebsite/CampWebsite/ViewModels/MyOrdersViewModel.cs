using CampWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.ViewModels
{
    public class MyOrdersViewModel
    {
        public string fOrderCode { get; set; }
        public string fCampsiteName { get; set; }
        public string fCheckinStart { get; set; }
        public string fCheckinEnd { get; set; }
        public int fTentCount { get; set; }
        public int fTotalPrice { get; set; }
        public int fZDay { get; set; }
    }
}