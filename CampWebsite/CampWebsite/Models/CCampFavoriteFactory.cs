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
        public List<MyCampFavoriteViewModel> QueryByFid(int fId = 1009)
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
                    fPhotoUrl = "/Images/Campsites/Campsite" + CampID + "/" + CampID + "Cover.jpg",
                });
            }
            con.Close();
            return myFavorites;
        }
    }
}