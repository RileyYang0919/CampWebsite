using CampWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.ViewModels
{
    public class Member3in1ViewModel
    {
        public tMember tMember { get; set; }
        public List<MyOrdersViewModel> myOrderList { get; set; }
        public List<MyCampFavoriteViewModel> myCampFavorites { get; set; }
    }
}