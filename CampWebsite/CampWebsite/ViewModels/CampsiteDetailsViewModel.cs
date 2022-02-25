using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CampWebsite.Models;

namespace CampWebsite.ViewModels
{
    public class CampsiteDetailsViewModel
    {
        public bool isFavored { get; set; }
        public List<CTents> tents
        {
            get; set;
        }
        public List<CTentsBooked> ThisM_TentsBooked
        {
            get; set;
        }
        public List<CTentsBooked> PreM_TentsBooked
        {
            get; set;
        }
        public List<CTentsBooked> NextM_TentsBooked
        {
            get; set;
        }
        public List<CTentsBooked> tentsbooked
        {
            get; set;
        }
    }
}