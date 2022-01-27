using CampWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace CampWebsite.Controllers
{
    public class CampListController : Controller
    {
        // GET: CampList
        public ActionResult List()
        {
            var camps = from c in new dbCampEntities().tCampsite select c;
            return View(camps);
        }
        int pageSize = 3;
        public ActionResult Details(int? id, int page = 1)
        {
            //營地資料表
            dbCampEntities c = new dbCampEntities();
            tCampsite camp = c.tCampsite.FirstOrDefault((r => r.fCampsiteID == id));
            if (camp == null)
            {
                return RedirectToAction("List");
            }


            //服務設施讀取
            #region
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=chicken122.database.windows.net;Initial Catalog=dbCamp;User ID=chicken;Password=P@sswo0rd;MultipleActiveResultSets=True;Application Name=EntityFramework";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText =
                "select tTag.fTagName,tTag.fTagType from tCampTag " +
                "join tTag on tCampTag.fTagID = tTag.fTagID " +
                "where tCampTag.fCampsiteID = @fCampsiteID";
            cmd.Parameters.AddWithValue("@fCampsiteID", id);
            SqlDataReader reader = cmd.ExecuteReader();
            CTags ctags = new CTags();
            ctags.f設施標籤 = new List<string>();
            ctags.f服務標籤 = new List<string>();
            ctags.f特色標籤 = new List<string>();
            ViewBag.facility_count = 0;
            ViewBag.service_count = 0;
            ViewBag.feature_count = 0;
            while (reader.Read())
            {
                if (Convert.ToString(reader["fTagType"]) == "設施")
                {
                    ctags.f設施標籤.Add(Convert.ToString(reader["fTagName"]));
                    ViewBag.facility = ctags.f設施標籤;
                    ViewBag.facility_count++;
                }
                else if (Convert.ToString(reader["fTagType"]) == "服務")
                {
                    ctags.f服務標籤.Add(Convert.ToString(reader["fTagName"]));
                    ViewBag.service = ctags.f服務標籤;
                    ViewBag.service_count++;
                }
                else if (Convert.ToString(reader["fTagType"]) == "特色")
                {
                    ctags.f特色標籤.Add(Convert.ToString(reader["fTagName"]));
                    ViewBag.feature = ctags.f特色標籤;
                    ViewBag.feature_count++;
                }
            }
            #endregion
            #region
            //SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Data Source=DESKTOP-5T348TE;Initial Catalog=Camp;Integrated Security=True";
            //con.Open();
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = con;
            //cmd.CommandText =
            //    "select tag.f標籤內容,tag.f標籤類型 from [t服務設施標籤中介表] as [interm] " +
            //    "join[t服務設施標籤] as [tag] " +
            //    "on interm.f服務設施標籤Id = tag.f服務設施標籤Id where interm.f營地Id = @fId";
            //cmd.Parameters.AddWithValue("@fId", id);
            //SqlDataReader reader = cmd.ExecuteReader();
            //CTags tags = new CTags();
            //tags.f設施標籤 = new List<string>();
            //tags.f服務標籤 = new List<string>();
            //ViewBag.tags_f設施標籤_count = 0;
            //ViewBag.tags_f服務標籤_count = 0;
            //while (reader.Read())
            //{
            //    if (Convert.ToString(reader["f標籤類型"]) == "設施")
            //    {
            //        tags.f設施標籤.Add(Convert.ToString(reader["f標籤內容"]));
            //        ViewBag.fa = tags.f設施標籤;
            //        ViewBag.tags_f設施標籤_count++;
            //    }
            //    if (Convert.ToString(reader["f標籤類型"]) == "服務")
            //    {
            //        tags.f服務標籤.Add(Convert.ToString(reader["f標籤內容"]));
            //        ViewBag.se = tags.f服務標籤;
            //        ViewBag.tags_f服務標籤_count++;
            //    }
            //}
            #endregion
            //方案讀取
            var tents = from t in new dbCampEntities().tTent where t.fCampsiteID == id select t;

            if (tents == null)
            {
                return RedirectToAction("List");
            }
            //方案數量讀取



            //評價讀取
            int currentPage = page < 1 ? 1 : page;
            //把所有(等於回傳回來的id參數值)的評論找出來
            var reviews = from rev in c.tComment
                          join m in c.tMember on rev.fMemberID equals m.fMemberID
                          where rev.fCampsiteID == id
                          orderby rev.fCommentTime descending
                          select rev;
            var results = reviews.ToList().ToPagedList(currentPage, pageSize);
            //計算總共有幾則評論放到viewbag
            ViewBag.reviews_count = reviews.Count();
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
            return View(Tuple.Create(camp, tents.AsEnumerable(), results));
        }
    }
}