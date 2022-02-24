using CampWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.ViewModels
{
    public class SearchSearchViewModel
    {
        public List<CTentCamp> CampTents { get; set; }
        public List<CTags> Tags { get; set; }
        public List<CTentsBooked> TentsBooked { get; set; }
        public List<CTents> Tents { get; set; }
        public List<CMemberFavor> MemberFavors { get; set; }
    }
}