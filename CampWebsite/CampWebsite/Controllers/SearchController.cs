using CampWebsite.Models;
using CampWebsite.ViewModels;
using System;
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
        public ActionResult Search(string from, string to, string campId)
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
            if (User.Identity.Name != "")
                vw.MemberFavors = searchFactory.QueryAllFavor(Convert.ToInt32(User.Identity.Name));
            if (Convert.ToInt32(campId) > 0 && User.Identity.Name != "")
            {
                searchFactory.AddDeleteFavor(Convert.ToInt32(User.Identity.Name), Convert.ToInt32(campId));
            }
            return Json(vw, JsonRequestBehavior.AllowGet);
        }
    }
}