using CampWebsite.Models;
using CampWebsite.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        // Campsite

        [Authorize(Roles = "gVendor")]
        public ActionResult FindMyCampsites()
        {
            dbCampEntities campEntities = new dbCampEntities();
            int myID = Convert.ToInt32(User.Identity.Name);
            var myCampsites = campEntities.tCampsite.Where(c => c.fMemberID == myID).ToList();
            return View(myCampsites);
        }

        [Authorize(Roles = "gVendor")]
        public ActionResult NewCampsite()
        {
            tCampsite c = new tCampsite();
            ViewBag.SelectList = c.SelectCity.AsEnumerable();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCampsite(tCampsite campsite)
        {
            //refactory here
            if (!ModelState.IsValid)
            {
                tCampsite c = new tCampsite();
                ViewBag.SelectList = c.SelectCity.AsEnumerable();
                return View(campsite);
            }
            else
            {
                switch (campsite.fCampsiteCity)
                {
                    case "台北市":
                    case "新北市":
                    case "基隆市":
                    case "桃園市":
                    case "新竹縣":
                    case "苗栗縣":
                        campsite.fCampsiteArea = "北部";
                        break;
                    case "台中市":
                    case "彰化縣":
                    case "南投縣":
                    case "雲林縣":
                        campsite.fCampsiteArea = "中部";
                        break;
                    case "嘉義縣":
                    case "台南市":
                    case "高雄市":
                    case "屏東縣":
                        campsite.fCampsiteArea = "南部";
                        break;
                    case "宜蘭縣":
                    case "花蓮縣":
                    case "台東縣":
                        campsite.fCampsiteArea = "東部";
                        break;
                }
                dbCampEntities campEntities = new dbCampEntities();
                campsite.fMemberID = Convert.ToInt32(User.Identity.Name);
                campEntities.tCampsite.Add(campsite);
                campEntities.SaveChanges();
                //return RedirectToAction("Details", new { id = campsite.fCampsiteID });
                return RedirectToAction("List");
            }
        }



        [Authorize(Roles = "gVendor")]
        public ActionResult NewTent(int ID, string name)
        {
            NewTentViewModel vm = new NewTentViewModel();
            vm.CampsiteID = ID;
            vm.CampsiteName = name;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewTent(NewTentViewModel newTent)
        {
            if (!ModelState.IsValid)
            {
                return View(newTent);
            }
            else
            {
                tTent myTent = new tTent();
                dbCampEntities campEntities = new dbCampEntities();
                newTent.tTent.fCampsiteID = newTent.CampsiteID;
                myTent = newTent.tTent;
                for (int i = 0; i < newTent.Quantity; i++)
                {
                    campEntities.tTent.Add(myTent);
                }
                campEntities.SaveChanges();
                return RedirectToAction("TentsInCampsite", new { campsiteID = newTent.CampsiteID });
            }
        }


        [Authorize(Roles = "gVendor")]
        public ActionResult TentsInCampsite(int campsiteID)
        {
            dbCampEntities campEntities = new dbCampEntities();
            var Tents = from t in campEntities.tTent
                        where t.fCampsiteID == campsiteID
                        select t;
            return View(Tents);
        }


        //[Authorize(Roles = "gVendor")]
        public ActionResult NewPhoto(int tID, string tName, string cName)
        {
            NewPhotoViewModel vm = new NewPhotoViewModel
            {
                TentID = tID,
                TentName = tName,
                CampsiteName = cName,
                //TentID = 101,
                //TentName = "temp Name for tent",
                //CampsiteName = "temp Name for Campsite"/*campsiteName*/
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPhoto(NewPhotoViewModel photoVM)
        {

            if (!ModelState.IsValid/*newPhotoVM.UploadedPhoto != null*/)
            {
                return View();
            }
            else
            {
                if (photoVM.Photo != null)
                {
                    var pName = Path.GetFileName(photoVM.Photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/Campsites/") + photoVM.CampsiteName + "/" + pName);
                    tTentPhoto ttp = new tTentPhoto
                    {
                        fTentID = photoVM.TentID,
                        fTentPhotoURL = path
                    };
                    dbCampEntities campEntities = new dbCampEntities();
                    campEntities.tTentPhoto.Add(ttp);
                    campEntities.SaveChanges();
                    photoVM.Photo.SaveAs(path);
                    ViewBag.UploadStat = true;
                    return View(photoVM);
                }
                //foreach (photoVM.HttpPostdFileBase[] f in newPhotoVM.UploadedPhoto)
                //{
                //    var path = "";

                //    if (photoVM.Photoes.ContentLength > 0)
                //    {
                //        var fileType = Path.GetExtension(photoVM.Photoes.FileName).ToLower();

                //        if (fileType == ".jpg" || fileType == ".jpeg" || fileType == ".png" || fileType == ".gif")
                //        {
                //            path = Path.Combine(Server.MapPath("~/Images/tentPhotos"), newPhotoVM.UploadedPhoto.FileName);
                //            //newPhotoVM.UploadedPhoto.SaveAs(path);
                //            ViewBag.UploadSuccess = true;

                //            dbCampEntities campEntities = new dbCampEntities();
                //            tTentPhoto newPhoto = new tTentPhoto
                //            {
                //                //fTentID = newPhotoVM.TentID
                //                fTentID = 101,
                //                fTentPhotoURL = path,
                //            };
                //        }

                //    }

                //}
            }
            return Content("sldkfj");
        }

        //This controller ends here
    }
}