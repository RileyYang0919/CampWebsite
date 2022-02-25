using CampWebsite.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    public class CSearchFactory
    {
        private SqlConnection ConnectSQL()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=chicken122two.database.windows.net;Initial Catalog=dbCamp;User ID=chicken;Password=P@sswo0rd;MultipleActiveResultSets=True;Application Name=EntityFramework";
            con.Open();
            return con;
        }
        private SqlCommand sqlcmd(SqlConnection con, string SQLstring)
        {
            SqlCommand cmd = new SqlCommand(SQLstring, con);
            cmd.CommandText = SQLstring;
            return cmd;
        }
        //所有找營區圖片
        private List<tTentPhoto> QueryCampCovers()
        {
            SqlConnection con = ConnectSQL();
            string SQLstring =
                "select photo.fCampsiteID,photo.fTentPhotoURL " +
                "from tCampsite as [camp] " +
                "join tTentPhoto as [photo] on camp.fCampsiteID = photo.fCampsiteID " +
                "where photo.fTentPhotoURL like N'/Images/Campsites/%/Cover%'";
            SqlCommand cmd = sqlcmd(con, SQLstring);
            SqlDataReader reader = cmd.ExecuteReader();
            List<tTentPhoto> photoList = new List<tTentPhoto>();
            while (reader.Read())
            {
                tTentPhoto photo = new tTentPhoto();
                photo.fCampsiteID = Convert.ToInt32(reader["fCampsiteID"]);
                photo.fTentPhotoURL = reader["fTentPhotoURL"].ToString();
                photoList.Add(photo);
            }
            reader.Close();
            con.Close();
            return photoList;
        }
        //找所有營區
        public List<CTentCamp> QueryAllCampsite()
        {
            SqlConnection con = ConnectSQL();
            string SQLstring =
                "select " +
                "camp.fCampsiteID,camp.fCampsiteName,camp.fCampsiteArea,camp.fCampsiteCity,camp.fCampsiteClosedDay,camp.fCampsiteAltitude," +
                "round(avg(convert(float, comment.fCommentScore)), 1) as [fAvgComment] " +
                "from tCampsite as [camp] " +
                "left join tComment as [comment] on camp.fCampsiteID = comment.fCampsiteID " +
                "group by camp.fCampsiteID,camp.fCampsiteName,camp.fCampsiteArea,camp.fCampsiteCity,camp.fCampsiteClosedDay,camp.fCampsiteAltitude";
            SqlCommand cmd = sqlcmd(con, SQLstring);
            SqlDataReader reader = cmd.ExecuteReader();
            List<CTentCamp> campList = new List<CTentCamp>();
            while (reader.Read())
            {
                List<tTentPhoto> photoList = QueryCampCovers();
                CTentCamp tentcamp = new CTentCamp();
                tentcamp.fCampsiteID = Convert.ToInt32(reader["fCampsiteID"]);
                tentcamp.fCampsiteName = reader["fCampsiteName"].ToString();
                tentcamp.fCampsiteArea = reader["fCampsiteArea"].ToString();
                tentcamp.fCampsiteCity = reader["fCampsiteCity"].ToString();
                tentcamp.fCampsiteClosedDay = reader["fCampsiteClosedDay"].ToString();
                tentcamp.fCampsiteAltitude = reader["fCampsiteAltitude"].ToString();
                if (photoList.Count > 0)
                {
                    for (int i = 0; i < photoList.Count; i++)
                    {
                        if (photoList[i].fCampsiteID == Convert.ToInt32(reader["fCampsiteID"]))
                            tentcamp.fTentPhotoURL = photoList[i].fTentPhotoURL;
                    }
                }
                else
                    break;
                if (!Convert.IsDBNull(reader["fAvgComment"])) { tentcamp.fAvgComment = Convert.ToDouble(reader["fAvgComment"]) * 1.0; }
                campList.Add(tentcamp);
            }
            reader.Close();
            con.Close();
            return campList;
        }
        //找各營區的個別營地
        public List<CTents> QueryCampsiteAllTents()
        {
            SqlConnection con = ConnectSQL();
            string SQLstring =
                "select " +
                "camp.fCampsiteID,tent.fTentName,tent.fTentPeople,count(tent.fTentName) as [fTentQuantity] " +
                "from tCampsite as [camp] " +
                "join tTent as [tent] on tent.fCampsiteID = camp.fCampsiteID " +
                "group by tent.fTentName,tent.fTentPeople,camp.fCampsiteID " +
                "order by camp.fCampsiteID,tent.fTentName";
            SqlCommand cmd = sqlcmd(con, SQLstring);
            SqlDataReader reader = cmd.ExecuteReader();
            List<CTents> tentList = new List<CTents>();
            while (reader.Read())
            {
                CTents tent = new CTents();
                tent.fCampsiteID = Convert.ToInt32(reader["fCampsiteID"]);
                if (!Convert.IsDBNull(reader["fTentName"])) { tent.fTentName = reader["fTentName"].ToString(); };
                if (!Convert.IsDBNull(reader["fTentPeople"])) { tent.fTentPeople = Convert.ToInt32(reader["fTentPeople"]); };
                tent.fTentQuantity = Convert.ToInt32(reader["fTentQuantity"]);
                tentList.Add(tent);
            }
            reader.Close();
            con.Close();
            return tentList;
        }
        //找tagName
        public List<CTags> QueryTags()
        {
            SqlConnection con = ConnectSQL();
            string SQLstring =
                "select " +
                "camp.fCampsiteID,tag.fTagName " +
                "from tCampsite as [camp] " +
                "join tCampTag on camp.fCampsiteID = tCampTag.fCampsiteID " +
                "join tTag as [tag] on tCampTag.fTagID = tag.fTagID";
            SqlCommand cmd = sqlcmd(con, SQLstring);
            SqlDataReader reader = cmd.ExecuteReader();
            List<CTags> tagList = new List<CTags>();
            while (reader.Read())
            {
                CTags tag = new CTags();
                tag.fCampsiteID = Convert.ToInt32(reader["fCampsiteID"]);
                tag.fTagName = reader["fTagName"].ToString();
                tagList.Add(tag);
            }
            reader.Close();
            con.Close();
            return tagList;
        }
        public List<CTentsBooked> QueryAllBooked(string from, string to)
        {
            SqlConnection con = ConnectSQL();
            string SQLstring =
                    "select distinct tent.fTentName,count(tent.fTentName) as [BookedCount],tOrder.fCheckinDate,tCampsite.fCampsiteID " +
                    "from tTent as [tent] " +
                    "join tOrder on tent.fTentID = tOrder.fTentID " +
                    "join tCampsite on tCampsite.fCampsiteID = tent.fCampsiteID " +
                    "where tOrder.fCheckinDate between(select convert(date, @CheckinDate)) and(select convert(date, @CheckoutDate)) " +
                    "group by tent.fTentName,tOrder.fCheckinDate,tCampsite.fCampsiteID " +
                    "order by tOrder.fCheckinDate,tent.fTentName";
            SqlCommand cmd = sqlcmd(con, SQLstring);
            cmd.Parameters.AddWithValue("@CheckinDate", Convert.ToDateTime(from).ToString("yyyy-M-d"));
            cmd.Parameters.AddWithValue("@CheckoutDate", Convert.ToDateTime(to).ToString("yyyy-M-d"));
            SqlDataReader reader = cmd.ExecuteReader();
            List<CTentsBooked> tentsBookedList = new List<CTentsBooked>();
            while (reader.Read())
            {
                CTentsBooked tentbooked = new CTentsBooked();
                tentbooked.fCampsiteID = Convert.ToInt32(reader["fCampsiteID"]);
                tentbooked.fTentName = reader["fTentName"].ToString();
                tentbooked.BookedCount = Convert.ToInt32(reader["BookedCount"]);
                tentbooked.fCheckinDate_dt = Convert.ToDateTime(reader["fCheckinDate"]).ToString("yyyy/MM/dd");
                tentsBookedList.Add(tentbooked);
            }
            reader.Close();
            con.Close();
            return tentsBookedList;
        }
        public List<CMemberFavor> QueryAllFavor(int MemberId)
        {
            SqlConnection con = ConnectSQL();
            string SQLstring = "select * from tCampFavorite where fMemberID = @MemberId";
            SqlCommand cmd = sqlcmd(con, SQLstring);
            cmd.Parameters.AddWithValue("@MemberId", MemberId);
            SqlDataReader reader = cmd.ExecuteReader();
            List<CMemberFavor> favors = new List<CMemberFavor>();
            while (reader.Read())
            {
                CMemberFavor favor = new CMemberFavor();
                favor.fMemberID = Convert.ToInt32(reader["fMemberID"]);
                favor.fCampsiteID = Convert.ToInt32(reader["fCampsiteID"]);
                favors.Add(favor);
            }
            reader.Close();
            con.Close();
            return favors;
        }
        public void AddDeleteFavor(int MemberId, int CampId)
        {
            SqlConnection con = ConnectSQL();
            string SQLstring = "select * from tCampFavorite where fMemberID = @MemberId and fCampsiteID = @CampId";
            SqlCommand cmd = sqlcmd(con, SQLstring);
            cmd.Parameters.AddWithValue("@MemberId", MemberId);
            cmd.Parameters.AddWithValue("@CampId", CampId);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                SQLstring = "Delete from tCampFavorite where fCampsiteID = @CampId and fMemberID = @MemberId";
                cmd = sqlcmd(con, SQLstring);
                cmd.Parameters.AddWithValue("@MemberId", MemberId);
                cmd.Parameters.AddWithValue("@CampId", CampId);
                cmd.ExecuteNonQuery();
            }
            else
            {
                SQLstring =
                    "insert into tCampFavorite(fMemberID,fCampsiteID) " +
                    "values(@MemberId, @CampId)";
                cmd = sqlcmd(con, SQLstring);
                cmd.Parameters.AddWithValue("@MemberId", MemberId);
                cmd.Parameters.AddWithValue("@CampId", CampId);
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }
    }
}