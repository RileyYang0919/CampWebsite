using CampWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampWebsite.ViewModels
{
    public class NewTentViewModel
    {
        public int CampsiteID { get; set; }
        public string CampsiteName { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public tTent tTent { get; set; }

        //房型選擇
        public List<SelectListItem> Categories = new List<SelectListItem>()
            {
                new SelectListItem {Text="Villa", Value="Villa" },
                new SelectListItem {Text="小帳棚", Value="小帳篷" },
                new SelectListItem {Text="大帳篷", Value="大帳篷" },
                new SelectListItem {Text="露營車", Value="露營車" },
                new SelectListItem {Text="小木屋", Value="小木屋" },
                new SelectListItem {Text="露營區域", Value="露營區域" }
            };
    }
}