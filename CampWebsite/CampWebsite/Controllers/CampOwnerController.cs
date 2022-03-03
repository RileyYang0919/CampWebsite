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
            vm.DayOffs = new List<DayOff>()
            {
                new DayOff { Day = "星期一", Value = "1", IsChecked = false },
                new DayOff { Day = "星期二", Value = "2", IsChecked = false },
                new DayOff { Day = "星期三", Value = "3", IsChecked = false },
                new DayOff { Day = "星期四", Value = "4", IsChecked = false },
                new DayOff { Day = "星期五", Value = "5", IsChecked = false },
                new DayOff { Day = "星期六", Value = "6", IsChecked = false },
                new DayOff { Day = "星期七", Value = "7", IsChecked = false },
                new DayOff { Day = "無公休日", Value = "0", IsChecked = false }
            };
            return View(vm);
        }

        //refactory here
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCampsite(ReceiveFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            else
            {
                //
                // save campsite
                dbCampEntities camp = new dbCampEntities();
                tCampsite c = new tCampsite();
                c.fCampsiteName = vm.campName;
                c.fMemberID = Convert.ToInt32(User.Identity.Name);
                c.fCampsiteAddress = vm.campAddress;
                c.fCampsiteArea = vm.SelectArea;
                c.fCampsiteCity = vm.SelectCity;
                c.fCampsiteCheckInTime = Regex.Replace(vm.CheckInTime, ":", String.Empty);
                c.fCampsiteCheckOutTime = Regex.Replace(vm.CheckOutTime, ":", String.Empty);
                c.fCampsiteIntroduction = vm.campIntro;
                c.fCampsitePhone = vm.campPhone;
                if (vm.withoutAltitude)
                {
                    c.fCampsiteAltitude = "無資料";
                }
                else
                {
                    c.fCampsiteAltitude = vm.campAltitude;
                }
                c.fCampsiteClosedDay = "0";
                if (vm.DayOffs.Count != 0)
                {
                    c.fCampsiteClosedDay = "";
                    foreach (var d in vm.DayOffs)
                    {
                        c.fCampsiteClosedDay += d;
                    }
                }
                else
                {
                    c.fCampsiteClosedDay = "0";
                }

                camp.tCampsite.Add(c);
                camp.SaveChanges();

                var newSite =
                    from nSite in camp.tCampsite
                    where nSite.fCampsiteName == vm.campName
                    select nSite;
                int campID = newSite.FirstOrDefault().fCampsiteID;

                //find campsite and use it's id 
                //Images/Campsites/Campsite27/Cover
                string virtualPath = "~/Images/Campsites/Campsite" + campID.ToString();
                string physicalPath = Server.MapPath(virtualPath);

                tTentPhoto tp = new tTentPhoto();
                tp.fTentPhotoURL = "/Images/Campsites/Campsite" + campID + "/Cover.jpg";
                tp.fCampsiteID = campID;
                camp.tTentPhoto.Add(tp);
                camp.SaveChanges();


                if (!Directory.Exists(virtualPath))  // find  or create Directory
                    Directory.CreateDirectory(physicalPath);
                vm.CoverPhoto.SaveAs(Server.MapPath(virtualPath + "/" + "Cover.jpg"));


                //tag
                /*
                foreach (var selectedTag in vm.SelectTags)
                {
                    var tag =
                        from t in camp.tTag
                        where t.fTagName == selectedTag
                        select t;
                    int tagID = tag.FirstOrDefault().fTagID;
                    tCampTag ct = new tCampTag()
                    {
                        fCampsiteID = campID,
                        fTagID = tagID,
                    };
                    camp.tCampTag.Add(ct);
                    camp.SaveChanges();

                }*/

                //return RedirectToAction("FindMyCampsites");
                return RedirectToAction("Details", "Campsite", campID);
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
                    camp.SaveChanges();
                }
                
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

