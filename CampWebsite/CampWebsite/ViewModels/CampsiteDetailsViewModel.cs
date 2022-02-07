using CampWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.ViewModels
{
    public class CampsiteDetailsViewModel
    {
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
    }
}