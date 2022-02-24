using CampWebsite.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    public class CCampFavoriteFactory
    {
        //查詢資料-單筆
        public List<MyCampFavoriteViewModel> QueryByFid(int fId)
        {
            string sql = "SELECT tCampFavorite.fMemberID,tCampFavorite.fCampsiteID, fCampsiteName, fCampsiteCity "
                        + "FROM tCampFavorite "
                        + "LEFT JOIN tCampsite "
                        + "ON tCampFavorite.fCampsiteID = tCampsite.fCampsiteID "
                        + "WHERE tCampFavorite.fMemberID=@K_fMemberID";

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_fMemberID", (object)fId));
            List<MyCampFavoriteViewModel> myFavorites = QueryBySql(sql, paras);
            if (myFavorites.Count == 0)
                return null;            
            return myFavorites;
        }

        //QueryBySql將查詢結果用List傳回
        private List<MyCampFavoriteViewModel> QueryBySql(string sql, List<SqlParameter> paras)
        {
            List<MyCampFavoriteViewModel> myFavorites = new List<MyCampFavoriteViewModel>();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source = chicken122two.database.windows.net; Initial Catalog = dbCamp; Persist Security Info = True; User ID = chicken; Password = P@sswo0rd; MultipleActiveResultSets = True; Application Name = EntityFramework";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null)
            {
                foreach (SqlParameter par in paras)
                    cmd.Parameters.Add(par);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int CampID = (int)reader["fCampsiteID"];
                myFavorites.Add(new MyCampFavoriteViewModel()
                {
                    fCampsiteID = CampID,
                    fCampsiteName = reader["fCampsiteName"].ToString(),
                    fCity = reader["fCampsiteCity"].ToString(),
                    fScore = 2,
                    fPhotoUrl = "/Images/Campsites/Campsite" + CampID + "/Cover.jpg",
                });
            }
            con.Close();
            foreach(var camp in myFavorites)
            {
                //評價讀取
                var reviews = new CTentsFactory().QueryAllReviews(camp.fCampsiteID);
                //計算總平均評論分數
                double score = 0;
                if (reviews.Count != 0)
                {//如果評論數不是0才計算否則傳到view的總平均為0
                    foreach (var review in reviews)
                        score += (double)review.fCommentScore;
                    score = Math.Round(score / reviews.Count(), 1);
                }
                camp.fScore = score;
            }
            return myFavorites;
        }
    }
}