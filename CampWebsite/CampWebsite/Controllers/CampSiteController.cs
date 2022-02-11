using CampWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using CampWebsite.ViewModels;

namespace CampWebsite.Controllers
{
    public class CampSiteController : Controller
    {
        CTentsFactory cTentsFactory = new CTentsFactory();
        // GET: CampSite
        public ActionResult List()
        {
            var camps = from c in new dbCampEntities().tCampsite select c;
            return View(camps);
        }
        public ActionResult Details(int? id)
        {
            dbCampEntities c = new dbCampEntities();
            //營地資料讀取
            tCampsite camp = c.tCampsite.FirstOrDefault((r => r.fCampsiteID == id));
            if (camp == null)
                return RedirectToAction("List");
            //服務設施讀取
            List<CTags> tags = cTentsFactory.QueryAllTags(id.ToString());

            //評價讀取
            var reviews = cTentsFactory.QueryAllReviews((int)id);
            //計算總平均評論分數
            double score = 0;
            if (reviews.Count != 0)
            {//如果評論數不是0才計算否則傳到view的總平均為0
                foreach (var review in reviews)
                    score += (double)review.fCommentScore;
                ViewBag.reviews_score = Math.Round(score / reviews.Count(), 1);
            }
            else
                ViewBag.reviews_score = 0.0;
            return View(Tuple.Create(camp, reviews.AsEnumerable(), tags));
        }

        [HttpPost]
        public ActionResult Details(string ID, int nowYear, int nowMonth, int nowLastDate, int preMonthCount, int preMonthLastDate, int nextMonthCount, string CheckinDate, string CheckoutDate)
        {
            CampsiteDetailsViewModel viewModel = cTentsFactory.sendVM(ID, nowYear, nowMonth, nowLastDate, preMonthCount, preMonthLastDate, nextMonthCount, CheckinDate, CheckoutDate);
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        //blue
        //AddNewCampsite
        public ActionResult AddNewCampsite()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewCampsite(tCampsite campsite)
        {
            dbCampEntities dbCampEntities = new dbCampEntities();
            campsite.fMemberID = Convert.ToInt32(User.Identity.Name);
            dbCampEntities.tCampsite.Add(campsite);
            dbCampEntities.SaveChanges();
            //return RedirectToAction("List");
            return RedirectToAction("Details");
        }
        //blue end
    }
}