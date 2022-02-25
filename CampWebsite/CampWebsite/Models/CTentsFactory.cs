using CampWebsite.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CampWebsite.Models
{
    public class CTentsFactory
    {
        CampsiteDetailsViewModel viewModel = new CampsiteDetailsViewModel();
        private SqlConnection ConnectSQL()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=chicken122two.database.windows.net;Initial Catalog=dbCamp;User ID=chicken;Password=P@sswo0rd;MultipleActiveResultSets=True;Application Name=EntityFramework";
            con.Open();
            return con;
        }
        private SqlCommand sqlcmd(SqlConnection con, string SQLstring, string ID)
        {
            SqlCommand cmd = new SqlCommand(SQLstring, con);
            cmd.CommandText = SQLstring;
            cmd.Parameters.AddWithValue("@fCampsiteID", Int32.Parse(ID));
            return cmd;
        }
        int StartMonth = 0;
        int StartMonthLastDate = 0;
        int EndMonth = 0;
        int EndMonthLastDate = 0;
        int StartYear = 0;
        int EndYear = 0;
        private void QueryAllTentsBookedOnCalendar(string ID, int nowYear, int nowMonth, int nowLastDate, int preMonthCount, int preMonthLastDate, int nextMonthCount)
        {
            SqlConnection con = ConnectSQL();
            string SQLstring = "select tTent.fTentName,count(tTent.fTentName) as [BookedCount],tOrder.fCheckinDate from tTent " +
                "join tOrder on tTent.fTentID = tOrder.fTentID " +
                "where tTent.fCampsiteID = @fCampsiteID and tOrder.fCheckinDate between (select convert(date,@StartDate)) and (select convert(date, @EndDate)) " +
                "group by tTent.fTentName,tOrder.fCheckinDate";
            SqlCommand cmd = sqlcmd(con, SQLstring, ID);
            //月份判斷
            #region
            StartYear = nowYear;
            EndYear = nowYear;
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
            #endregion
            cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime($"{StartYear}-{StartMonth}-{StartMonthLastDate}").ToString("yyyy-M-d"));
            cmd.Parameters.AddWithValue("@EndDate", Convert.ToDateTime($"{EndYear}-{EndMonth}-{EndMonthLastDate}").ToString("yyyy-M-d"));
            SqlDataReader reader = cmd.ExecuteReader();
            List<CTentsBooked> ThisM_TentsBooked = new List<CTentsBooked>();
            List<CTentsBooked> PreM_TentsBooked = new List<CTentsBooked>();
            List<CTentsBooked> NextM_TentsBooked = new List<CTentsBooked>();
            while (reader.Read())
            {
                CTentsBooked ctentsbooked = new CTentsBooked();
                ctentsbooked.fTentName = reader["fTentName"].ToString();
                ctentsbooked.BookedCount = Convert.ToInt32(reader["BookedCount"]);
                if (int.Parse(Convert.ToDateTime(reader["fCheckinDate"]).ToString("MM")) == nowMonth)
                {
                    ctentsbooked.fCheckinDate = int.Parse(Convert.ToDateTime(reader["fCheckinDate"]).ToString("dd"));
                    ThisM_TentsBooked.Add(ctentsbooked);
                }
                else if (int.Parse(Convert.ToDateTime(reader["fCheckinDate"]).ToString("MM")) == StartMonth)
                {
                    ctentsbooked.fCheckinDate = int.Parse(Convert.ToDateTime(reader["fCheckinDate"]).ToString("dd"));
                    PreM_TentsBooked.Add(ctentsbooked);
                }
                else
                {
                    ctentsbooked.fCheckinDate = int.Parse(Convert.ToDateTime(reader["fCheckinDate"]).ToString("dd"));
                    NextM_TentsBooked.Add(ctentsbooked);
                }
            }
            reader.Close();
            con.Close();
            viewModel.ThisM_TentsBooked = ThisM_TentsBooked;
            viewModel.NextM_TentsBooked = NextM_TentsBooked;
            viewModel.PreM_TentsBooked = PreM_TentsBooked;
        }
        public List<CTents> QueryAllTents(string ID)
        {
            SqlConnection con = ConnectSQL();
            string SQLstring = "select tTent.fTentID,tTent.fTentName,tTent.fTentCategory,tTent.fTentPeople," +
                "tTent.fTentPriceWeekday,tTent.fTentPriceWeekend,tCampsite.fCampsiteClosedDay " +
                "from tTent " +
                "join tCampsite on tTent.fCampsiteID = tCampsite.fCampsiteID " +
                "where tTent.fCampsiteID = @fCampsiteID";
            SqlCommand cmd = sqlcmd(con, SQLstring, ID);
            SqlDataReader reader = cmd.ExecuteReader();
            List<CTents> TentList = new List<CTents>();
            while (reader.Read())
            {
                CTents ctents = new CTents();
                ctents.fTentId = Convert.ToInt32(reader["fTentID"]);
                ctents.fTentName = reader["fTentName"].ToString();
                ctents.fTentCategory = reader["fTentCategory"].ToString();
                ctents.fTentPeople = Convert.ToInt32(reader["fTentPeople"]);
                ctents.fTentPriceWeekday = Convert.ToInt32(reader["fTentPriceWeekday"]);
                ctents.fTentPriceWeekend = Convert.ToInt32(reader["fTentPriceWeekend"]);
                ctents.fCampsiteClosedDay = reader["fCampsiteClosedDay"].ToString();
                TentList.Add(ctents);
            }
            reader.Close();
            con.Close();
            viewModel.tents = TentList;
            return TentList;
        }
        public List<CTentsBooked> QueryAllTentsBetweenSelectedDates(string ID, string CheckinDate, string CheckoutDate)
        {
            SqlConnection con = ConnectSQL();
            string SQLstring = "select tOrder.fTentID, tTent.fTentName,tOrder.fCheckinDate from tTent " +
                "join tOrder on tTent.fTentID = tOrder.fTentID " +
                "where tTent.fCampsiteID = @fCampsiteID and tOrder.fCheckinDate between(select convert(date,@CheckinDate)) and(select convert(date, @CheckoutDate)) " +
                "group by tOrder.fTentID,tTent.fTentName,tOrder.fCheckinDate; ";
            SqlCommand cmd = sqlcmd(con, SQLstring, ID);
            //被訂走的方案id、名字、入住日期
            cmd.Parameters.AddWithValue("@CheckinDate", Convert.ToDateTime(CheckinDate).ToString("yyyy-M-d"));
            cmd.Parameters.AddWithValue("@CheckoutDate", Convert.ToDateTime(CheckoutDate).ToString("yyyy-M-d"));
            SqlDataReader reader = cmd.ExecuteReader();
            List<CTentsBooked> ctentbooked = new List<CTentsBooked>();
            while (reader.Read())
            {
                CTentsBooked t = new CTentsBooked();
                t.fTentId = Convert.ToInt32(reader["fTentID"]);
                t.fTentName = reader["fTentName"].ToString();
                t.fCheckinDate_dt = Convert.ToDateTime(reader["fCheckinDate"]).ToString("yyyy/MM/dd");
                ctentbooked.Add(t);
            }
            reader.Close();
            con.Close();
            viewModel.tentsbooked = ctentbooked;
            return ctentbooked;
        }

        public CampsiteDetailsViewModel sendVM(string ID, int nowYear, int nowMonth, int nowLastDate, int preMonthCount, int preMonthLastDate, int nextMonthCount, string CheckinDate, string CheckoutDate)
        {
            QueryAllTentsBookedOnCalendar(ID, nowYear, nowMonth, nowLastDate, preMonthCount, preMonthLastDate, nextMonthCount);
            QueryAllTents(ID);
            QueryAllTentsBetweenSelectedDates(ID, CheckinDate, CheckoutDate);
            return viewModel;
        }
        public List<CTags> QueryAllTags(string ID) //服務設施讀取
        {
            SqlConnection con = ConnectSQL();
            string SQLstring = "select tTag.fTagName,tTag.fTagType from tCampTag " +
                "join tTag on tCampTag.fTagID = tTag.fTagID " +
                "where tCampTag.fCampsiteID = @fCampsiteID";
            SqlCommand cmd = sqlcmd(con, SQLstring, ID);
            SqlDataReader reader = cmd.ExecuteReader();
            List<CTags> tags = new List<CTags>();
            while (reader.Read())
            {
                CTags tag = new CTags()
                {
                    fTagName = Convert.ToString(reader["fTagName"]),
                    fTagType = Convert.ToString(reader["fTagType"]),
                };
                tags.Add(tag);
            }
            reader.Close();
            con.Close();
            return tags;
        }
        public List<tComment> QueryAllReviews(int ID)
        {
            dbCampEntities c = new dbCampEntities();
            //評價讀取
            var reviews = from rev in c.tComment
                          join m in c.tMember on rev.fMemberID equals m.fMemberID
                          where rev.fCampsiteID == ID
                          orderby rev.fCommentTime descending
                          select rev;
            return reviews.ToList();
        }
        public bool QueryIsFavor(string MemberId, string CampId)
        {
            SqlConnection con = ConnectSQL();
            string SQLstring = "select * from tCampFavorite where fMemberID = @MemberId and fCampsiteID = @fCampsiteID";
            SqlCommand cmd = sqlcmd(con, SQLstring, CampId);
            cmd.Parameters.AddWithValue("@MemberId", MemberId);
            SqlDataReader reader = cmd.ExecuteReader();
            bool isFavored = false;
            if (reader.Read())
            {
                isFavored = true;
            }
            reader.Close();
            con.Close();
            return isFavored;
        }
        public List<tTentPhoto> QueryCampPhotos(string campid)
        {
            string photoUrl = $"/Images/Campsites/Campsite{campid}";
            SqlConnection con = ConnectSQL();
            StringBuilder sb = new StringBuilder();
            sb.Append(" and fTentPhotoURL like N'%'+@fTentPhotoURL+'%'");
            string SQLstring = "select * from tTentPhoto where fCampsiteID = @fCampsiteID" + sb;
            SqlCommand cmd = sqlcmd(con, SQLstring, campid);
            cmd.Parameters.AddWithValue("@CampsiteID", campid);
            cmd.Parameters.AddWithValue("@fTentPhotoURL", photoUrl);
            SqlDataReader reader = cmd.ExecuteReader();
            List<tTentPhoto> photoList = new List<tTentPhoto>();
            while (reader.Read())
            {
                tTentPhoto photo = new tTentPhoto();
                photo.fTentPhotoURL = reader["fTentPhotoURL"].ToString();
                photoList.Add(photo);
            }
            reader.Close();
            con.Close();
            return photoList;
        }
        public List<tTentPhoto> QueryTentPhoto(string campid, string tentName)
        {
            string photoUrl = $"/Images/Campsites/Campsite{campid}/{tentName}_";
            SqlConnection con = ConnectSQL();
            StringBuilder sb = new StringBuilder();
            sb.Append(" and fTentPhotoURL like N'%'+@fTentPhotoURL+'%'");
            string SQLstring = "select * from tTentPhoto where fCampsiteID = @fCampsiteID" + sb;
            SqlCommand cmd = sqlcmd(con, SQLstring, campid);
            cmd.Parameters.AddWithValue("@CampsiteID", campid);
            cmd.Parameters.AddWithValue("@fTentPhotoURL", photoUrl);
            SqlDataReader reader = cmd.ExecuteReader();
            List<tTentPhoto> photoList = new List<tTentPhoto>();
            while (reader.Read())
            {
                tTentPhoto photo = new tTentPhoto();
                photo.fTentPhotoURL = reader["fTentPhotoURL"].ToString();
                photoList.Add(photo);
            }
            reader.Close();
            con.Close();
            return photoList;
        }
    }
}