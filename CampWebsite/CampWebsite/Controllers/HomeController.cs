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
    }
}