using CampWebsite.Models;
using CampWebsite.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace CampWebsite.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string from, string to)
        {
            CSearchFactory searchFactory = new CSearchFactory();
            SearchSearchViewModel vw = new SearchSearchViewModel();
            if (from != "" && to != "")
            {
                vw.TentsBooked = searchFactory.QueryAllBooked(from, to);
            }
            vw.CampTents = searchFactory.QueryAllCampsite();
            vw.Tents = searchFactory.QueryCampsiteAllTents();
            vw.Tags = searchFactory.QueryTags();
            return Json(vw, JsonRequestBehavior.AllowGet);
        }
    }
}