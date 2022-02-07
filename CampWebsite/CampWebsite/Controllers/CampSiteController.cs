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
            List<CTags> tags = null;
            if (id != null)
            {
                tags = new CTagsFactory().QueryAllTags((int)id);
                ViewBag.tagsCount = tags.Count;
            }

            //評價讀取
            #region
            //把所有(等於回傳回來的id參數值)的評論找出來
            var reviews = from rev in c.tComment
                          join m in c.tMember on rev.fMemberID equals m.fMemberID
                          where rev.fCampsiteID == id
                          orderby rev.fCommentTime descending
                          select rev;
            var results = reviews.ToList();
            //計算總平均評論分數
            double score = 0;
            if (reviews.Count() != 0)
            {//如果評論數不是0才計算否則傳到view的總平均為0
                foreach (var review in reviews)
                    score += (double)review.fCommentScore;
                ViewBag.reviews_score = Math.Round(score / reviews.Count(), 1);
            }
            else
                ViewBag.reviews_score = 0.0;
            #endregion
            return View(Tuple.Create(camp, results.AsEnumerable(), tags));
        }

        [HttpPost]
        public ActionResult Details(string ID, int nowYear, int nowMonth, int nowLastDate, int preMonthCount, int preMonthLastDate, int nextMonthCount)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=chicken122two.database.windows.net;Initial Catalog=dbCamp;User ID=chicken;Password=P@sswo0rd;MultipleActiveResultSets=True;Application Name=EntityFramework";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select tTent.fTentName,count(tTent.fTentName) as [BookedCount],tOrder.fCheckinDate from tTent " +
                "join tOrder on tTent.fTentID = tOrder.fTentID " +
                "where tTent.fCampsiteID = @fCampsiteID and tOrder.fCheckinDate between (select convert(date,@StartDate)) and (select convert(date, @EndDate)) " +
                "group by tTent.fTentName,tOrder.fCheckinDate";
            cmd.Parameters.AddWithValue("@fCampsiteID", Int32.Parse(ID));
            //月份判斷
            int StartMonth = 0;
            int StartMonthLastDate = 0;
            int EndMonth = 0;
            int EndMonthLastDate = 0;
            int StartYear = nowYear;
            int EndYear = nowYear;
            //上月
            if (nowMonth == 1 && preMonthCount != 0)
            {
                StartMonth = 12;
                StartMonthLastDate = preMonthLastDate - preMonthCount + 1;
                StartYear = nowYear - 1;
            }
            else if (nowMonth != 1 && preMonthCount != 0)
            {
                StartMonth = nowMonth - 1;
                StartMonthLastDate = preMonthLastDate - preMonthCount + 1;
            }
            else if (preMonthCount == 0)
            {
                StartMonth = nowMonth;
                StartMonthLastDate = 1;
            }
            //下月
            if (nowMonth == 12 && nextMonthCount != 0)
            {
                EndMonth = 1;
                EndMonthLastDate = nextMonthCount;
                EndYear = nowYear + 1;
            }
            else if (nowMonth != 12 && nextMonthCount != 0)
            {
                EndMonth = nowMonth + 1;
                EndMonthLastDate = nextMonthCount;
            }
            else if (nextMonthCount == 0)
            {
                EndMonth = 12;
                EndMonthLastDate = nowLastDate;
            }
            cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime($"{StartYear}-{StartMonth}-{StartMonthLastDate}").ToString("yyyy-M-d"));
            cmd.Parameters.AddWithValue("@EndDate", Convert.ToDateTime($"{EndYear}-{EndMonth}-{EndMonthLastDate}").ToString("yyyy-M-d"));
            SqlDataReader reader = cmd.ExecuteReader();
            List<CTentsBooked> ThisM_TentsBooked = new List<CTentsBooked>();
            List<CTentsBooked> LastM_TentsBooked = new List<CTentsBooked>();
            List<CTentsBooked> NextM_TentsBooked = new List<CTentsBooked>();
            while (reader.Read())
            {
                CTentsBooked ctentsbooked = new CTentsBooked();
                ctentsbooked.fTentName = reader["fTentName"].ToString();
                ctentsbooked.BookedCount = Convert.ToInt32(reader["BookedCount"]);
                if (int.Parse(Convert.ToDateTime(reader["fCheckinDate"]).ToString("MM")) == StartMonth)
                {
                    ctentsbooked.fCheckinDate = int.Parse(Convert.ToDateTime(reader["fCheckinDate"]).ToString("dd"));
                    LastM_TentsBooked.Add(ctentsbooked);
                }
                else if (int.Parse(Convert.ToDateTime(reader["fCheckinDate"]).ToString("MM")) == nowMonth)
                {
                    ctentsbooked.fCheckinDate = int.Parse(Convert.ToDateTime(reader["fCheckinDate"]).ToString("dd"));
                    ThisM_TentsBooked.Add(ctentsbooked);
                }
                else
                {
                    ctentsbooked.fCheckinDate = int.Parse(Convert.ToDateTime(reader["fCheckinDate"]).ToString("dd"));
                    NextM_TentsBooked.Add(ctentsbooked);
                }
            }
            reader.Close();
            //每天各方案有幾個
            cmd.CommandText =
    "select fTentName,fTentCategory,fTentPeople,fTentPriceWeekday,fTentPriceWeekend,fTendPriceHoliday,COUNT(fTentName) as fTentQuantity " +
    "from tTent where fCampsiteID = @fCampsiteID " +
    "group by fTentName,fTentCategory,fTentPeople,fTentPriceWeekday,fTentPriceWeekend,fTendPriceHoliday";
            SqlDataReader reader1 = cmd.ExecuteReader();
            List<CTents> TentList = new List<CTents>();
            while (reader1.Read())
            {
                CTents ctents = new CTents();
                ctents.fTentName = reader1["fTentName"].ToString();
                ctents.fTentCategory = reader1["fTentCategory"].ToString();
                ctents.fTentPeople = Convert.ToInt32(reader1["fTentPeople"]);
                ctents.fTentPriceWeekday = Convert.ToInt32(reader1["fTentPriceWeekday"]);
                ctents.fTentPriceWeekend = Convert.ToInt32(reader1["fTentPriceWeekend"]);
                ctents.fTentPriceHoliday = Convert.ToInt32(reader1["fTendPriceHoliday"]);
                ctents.fTentQuantity = Convert.ToInt32(reader1["fTentQuantity"]);
                TentList.Add(ctents);
            }
            reader1.Close();
            CampsiteDetailsViewModel viewModel = new CampsiteDetailsViewModel();
            viewModel.tents = TentList;
            viewModel.ThisM_TentsBooked = ThisM_TentsBooked;
            viewModel.PreM_TentsBooked = LastM_TentsBooked;
            viewModel.NextM_TentsBooked = NextM_TentsBooked;
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