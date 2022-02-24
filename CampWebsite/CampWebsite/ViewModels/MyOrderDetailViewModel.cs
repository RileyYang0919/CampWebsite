using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CampWebsite.Models;

namespace CampWebsite.ViewModels
{
    public class MyOrderDetailViewModel
    {
        public int fCampsiteID { get; set; }
        public string fOrderCode { get; set; }
        public string fCampsiteName { get; set; }
        public string fCampsitePhone { get; set; }
        public string fCampsiteAddress { get; set; }
        public string fCheckinStart { get; set; }
        public string fCheckinEnd { get; set; }
        public int fTentCount { get; set; }
        public int fStayNight { get; set; }
        public int fTotalPrice { get; set; }
        public List<tOrder> tOrders { get; set; }
        public tComment tComment { get; set; }
    }
}