using CampWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.ViewModels
{
    public class PreOrderInfoViewModel
    {
        public tMember tMember { get; set; }
        public tTent tTent { get; set; }
        public System.DateTime fCheckinStart { get; set; }
        public System.DateTime fCheckinEnd { get; set; }
        public string fOrderComment { get; set; }
        public int fOrderPrice { get; set; }
    }
}