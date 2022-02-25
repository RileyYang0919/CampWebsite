using CampWebsite.Models;
using CampWebsite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace CampWebsite.Controllers
{
    public class CampSiteController : Controller
    {
        CTentsFactory cTentsFactory = new CTentsFactory();
        // GET: CampSite
        public ActionResult Details(int? id)
        {
            dbCampEntities c = new dbCampEntities();
            //營地資料讀取
            tCampsite camp = c.tCampsite.FirstOrDefault((r => r.fCampsiteID == id));
            if (camp == null)
                return RedirectToAction("Search", "Search");
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

            //讀取照片
            List<tTentPhoto> photos = cTentsFactory.QueryCampPhotos(id.ToString());

            return View(Tuple.Create(camp, reviews.AsEnumerable(), tags, photos));
        }

        [HttpPost]
        public ActionResult Details(string ID, int nowYear, int nowMonth, int nowLastDate, int preMonthCount, int preMonthLastDate, int nextMonthCount, string CheckinDate, string CheckoutDate)
        {
            CampsiteDetailsViewModel viewModel = cTentsFactory.sendVM(ID, nowYear, nowMonth, nowLastDate, preMonthCount, preMonthLastDate, nextMonthCount, CheckinDate, CheckoutDate);
            viewModel.isFavored = cTentsFactory.QueryIsFavor(User.Identity.Name, ID);
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddDeleteFavor(string campId)
        {
            CSearchFactory searchFactory = new CSearchFactory();
            if (Convert.ToInt32(campId) > 0 && User.Identity.Name != "")//收藏功能
            {
                searchFactory.AddDeleteFavor(Convert.ToInt32(User.Identity.Name), Convert.ToInt32(campId));
            }
            bool isDone = true;
            return Json(isDone, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ShowTentPhoto(string campid, string tentName)
        {
            if (campid == null || tentName == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.tentName = tentName;
                List<tTentPhoto> photoList = cTentsFactory.QueryTentPhoto(campid, tentName);
                return View(photoList);
            }
        }
    }
}