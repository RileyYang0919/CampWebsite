using CampWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CampWebsite.Controllers
{
    public class HomeController : Controller
    {
        dbCampEntities db = new dbCampEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //俊長的營區搜尋
        public ActionResult search()
        {
            IEnumerable<tCampsite> products = null;
            string keyword = Request.Form["txtKeyword"];
            if (string.IsNullOrEmpty(keyword))
                products = from p in (new dbCampEntities()).tCampsite select p;
            else
                products = from p in (new dbCampEntities()).tCampsite where p.fCampsiteName.Contains(keyword) select p;
            //var todos = db.營區資料.OrderByDescending(m =>m.營區Id).ToList();

            return View(products);
        }

        //俊長的天氣API
        public ActionResult weatherTemp()
        {
            return View();
        }
    }
}