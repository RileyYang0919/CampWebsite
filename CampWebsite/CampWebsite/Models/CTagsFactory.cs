using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    public class CTagsFactory
    {
        public List<CTags> QueryAllTags(int fCampsiteId)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=chicken122two.database.windows.net;Initial Catalog=dbCamp;User ID=chicken;Password=P@sswo0rd;MultipleActiveResultSets=True;Application Name=EntityFramework";
            con.Open();
            string SQLstring =
                "select tTag.fTagName,tTag.fTagType from tCampTag " +
                "join tTag on tCampTag.fTagID = tTag.fTagID " +
                "where tCampTag.fCampsiteID = @fCampsiteID";
            SqlCommand cmd = new SqlCommand(SQLstring, con);
            cmd.Parameters.AddWithValue("@fCampsiteID", fCampsiteId);
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

            if (tags.Count == 0)
                return null;
            return tags;
        }
    }
}