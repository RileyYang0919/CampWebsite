using CampWebsite.Models;
using CampWebsite.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CampWebsite.Controllers
{
    public class CampOwnerController : Controller
    {
        // GET: CampOwner
        public ActionResult Index()
        {
            return View();
        }



        //
        // Looking for something?

        [Authorize(Roles = "1")]
        public ActionResult FindMyCampsites()
        {
            dbCampEntities camp = new dbCampEntities();
            int OwnerID = Convert.ToInt32(User.Identity.Name);
            var myCampsites =
                from t in camp.tCampsite
                where t.fMemberID == OwnerID
                orderby t.fCampsiteID
                select t;
            return View(myCampsites);
        }

        [Authorize(Roles = "1")]
        public ActionResult TentsInCampsite(int cID, string cName)
        {
            ViewBag.cName = cName;
            dbCampEntities camp = new dbCampEntities();
            var Tents =
                from t in camp.tTent
                where t.fCampsiteID == cID
                orderby t.fTentName
                select t;
            if (Tents.Count() != 0)
            {
                return View(Tents);
            }
            else
            {
                return Json(null);
            }
        }

        [Authorize(Roles = "1")]
        public ActionResult HistoryOrders(int cID, string cName)
        {
            ViewBag.cName = cName;
            dbCampEntities camp = new dbCampEntities();
            var historyOrders =
                from o in camp.tOrder
                join t in camp.tTent on o.fTentID equals t.fTentID
                where t.fCampsiteID == cID
                select o;
            if (historyOrders.Count() != 0)
            {
                return View(historyOrders);
            }
            else
            {
                return Json(null);
            }
        }

        [Authorize(Roles = "1")]
        public ActionResult FutureOrdersForTent(int tID, string tName)
        {
            ViewBag.tName = tName;
            DateTime today = DateTime.Now.Date;
            dbCampEntities camp = new dbCampEntities();
            var futureOrders =
                from o in camp.tOrder
                where o.fTentID == tID
                where o.fCheckinDate >= today
                select o;

            if (futureOrders.Count() != 0)
            {
                return View(futureOrders);
            }
            else
            {
                return Json(null);
            }
        }



        //
        // New  Campsite

        [Authorize(Roles = "1")]
        public ActionResult NewCampsite()
        {
            NewCampsiteViewModel vm = new NewCampsiteViewModel();
            return View(vm);
        }

        //refactory here
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCampsite(NewCampsiteViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            else
            {

                tCampsite c = vm.tCampsite;
                c.fMemberID = Convert.ToInt32(User.Identity.Name);
                c.fCampsiteCheckInTime = Regex.Replace(c.fCampsiteCheckInTime, ":", String.Empty);
                c.fCampsiteCheckOutTime = Regex.Replace(c.fCampsiteCheckOutTime, ":", String.Empty);

                if (vm.withoutAltitude)
                    c.fCampsiteAltitude = "無資料";

                c.fCampsiteClosedDay = "0";
                if (vm.DayOffs[7].Checked)
                {
                    c.fCampsiteClosedDay = "0";
                }
                else
                {
                    c.fCampsiteClosedDay = "";
                    foreach (var item in vm.DayOffs)
                    {
                        if (item.Checked)
                        {
                            c.fCampsiteClosedDay += item.Value;
                        }
                    }
                }
                if (c.fCampsiteClosedDay == "")
                {
                    c.fCampsiteClosedDay = "0";
                }

                dbCampEntities camp = new dbCampEntities();
                camp.tCampsite.Add(c);
                camp.SaveChanges();

                //Images/Campsites/Campsite27/Cover
                //find campsite and use it's id 
                var newSite =
                    from nSite in camp.tCampsite
                    where nSite.fCampsiteName == vm.tCampsite.fCampsiteName
                    select nSite;

                int newID = newSite.FirstOrDefault().fCampsiteID;
                string virtualPath = "~/Images/Campsites/Campsite" + newID.ToString();
                string physicalPath = Server.MapPath(virtualPath);

                tTentPhoto tp = new tTentPhoto();
                tp.fTentPhotoURL = "/Images/Campsites/Campsite" + newID + "/Cover.jpg";
                tp.fCampsiteID = newID;
                camp.tTentPhoto.Add(tp);
                camp.SaveChanges();

                // find  or create Directory
                if (!Directory.Exists(virtualPath))
                    Directory.CreateDirectory(physicalPath);

                vm.CoverPhoto.SaveAs(Server.MapPath(virtualPath + "/" + "Cover.jpg"));
                return RedirectToAction("FindMyCampsites");
            }
        }
        [HttpPost]
        public JsonResult GetCities(string area)
        {
            List<KeyValuePair<string, string>> cityPair = new List<KeyValuePair<string, string>>();

            if (!string.IsNullOrWhiteSpace(area))
            {
                var cityList = this.GetCityList(area);
                if (cityList.Count() > 0)
                {
                    foreach (var c in cityList)
                    {
                        cityPair.Add(new KeyValuePair<string, string>(c.Text, c.Value));
                    }
                }
            }

            return this.Json(cityPair);
        }
        private List<SelectListItem> GetCityList(string area)
        {
            List<SelectListItem> Cities = new List<SelectListItem>();
            if (area == "北部")
                Cities = (new NewCampsiteViewModel()).SelectCityNorth;
            if (area == "中部")
                Cities = (new NewCampsiteViewModel()).SelectCityCenter;
            if (area == "南部")
                Cities = (new NewCampsiteViewModel()).SelectCitySouth;
            if (area == "東部")
                Cities = (new NewCampsiteViewModel()).SelectCityEast;
            return Cities;
        }



        //
        // New Tents

        [Authorize(Roles = "1")]
        public ActionResult NewTent(int cID, string cName)
        {
            NewTentViewModel vm = new NewTentViewModel
            {
                CampsiteID = cID,
                CampsiteName = cName
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewTent(NewTentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            else
            {
                dbCampEntities camp = new dbCampEntities();
                tTent myTent = new tTent();
                myTent = vm.tTent;
                myTent.fCampsiteID = vm.CampsiteID;

                for (int i = 0; i < vm.Quantity; i++)
                {
                    camp.tTent.Add(myTent);
                }
                camp.SaveChanges();
                return RedirectToAction("TentsInCampsite", new { cID = vm.CampsiteID, cName = vm.CampsiteName });
            }
        }



        //
        // New Photo

        [Authorize(Roles = "1")]
        public ActionResult NewPhoto(string tName, int cID, string cName)
        {
            NewPhotoViewModel vm = new NewPhotoViewModel
            {
                TentName = tName,
                CampsiteID = cID,
                CampsiteName = cName,
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPhoto(NewPhotoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            else
            {
                //Images/Campsites/Campsite27/tName_fileName
                string virtualPath = "~/Images/Campsites/Campsite" + vm.CampsiteID.ToString();
                string physicalPath = Server.MapPath(virtualPath);
                string tentName = vm.TentName;

                // find  or create Directory
                if (!Directory.Exists(virtualPath))
                    Directory.CreateDirectory(physicalPath);

                dbCampEntities camp = new dbCampEntities();

                foreach (HttpPostedFileBase photo in vm.OtherPhotos)
                {
                    string localFileName = tentName + "_" + Path.GetFileName(photo.FileName);
                    string path = virtualPath + "/" + localFileName;
                    photo.SaveAs(Server.MapPath(path));
                    tTentPhoto ttp = new tTentPhoto
                    {
                        fCampsiteID = vm.CampsiteID,
                        fTentPhotoURL = path.TrimStart('~')
                    };
                    camp.tTentPhoto.Add(ttp);
                    camp.SaveChanges();
                }
            }
            return RedirectToAction("TentsInCampsite", new { cID = vm.CampsiteID, cName = vm.CampsiteName });
        }
    }
}

