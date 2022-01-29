using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    public class AdoQueryFactorySample
    {
        //查詢資料-單筆
        public AdoMemberModelSample QueryByFid(int fId)
        {
            string sql = "SELECT * FROM tMember WHERE fMemberID=@K_fMemberID";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_fMemberID", (object)fId));
            List<AdoMemberModelSample> members = QueryBySql(sql, paras);
            if (members.Count == 0)
                return null;
            return members[0];
        }
        //查詢資料-全部
        public List<AdoMemberModelSample> QueryAll()
        {
            return QueryBySql("SELECT * FROM tMember", null);
        }
        //新增資料
        public void Create(AdoMemberModelSample p)
        {
            //宣告一個List來存放多個變數
            List<SqlParameter> paras = new List<SqlParameter>();
            //建立SQL指令
            //使用@參數來避免SQL injection
            //使用if判斷，如果是空欄位，就不加上該行的指令
            //在指令末端加上判斷，刪除前後空白(.Trim())、刪除最後一位的逗號
            string sql = "INSERT INTO tMember(";
            if (!string.IsNullOrEmpty(p.fName))
                sql += "fName,";
            if (!string.IsNullOrEmpty(p.fEmail))
                sql += "fEmail,";
            if (!string.IsNullOrEmpty(p.fPassword))
                sql += "fPassword,";
            if (sql.Trim().Substring(sql.Trim().Length - 1, 1) == ",")
                sql = sql.Trim().Substring(0, sql.Length - 1);
            sql += ")VALUES(";
            if (!string.IsNullOrEmpty(p.fName))
            {
                sql += "@K_FNAME,";
                paras.Add(new SqlParameter(("K_FNAME"), (object)p.fName));
            }
            if (!string.IsNullOrEmpty(p.fEmail))
            {
                sql += "@K_fEmail,";
                paras.Add(new SqlParameter(("K_fEmail"), (object)p.fEmail));
            }
            if (!string.IsNullOrEmpty(p.fPassword))
            {
                sql += "@K_fPassword,";
                paras.Add(new SqlParameter(("K_fPassword"), (object)p.fPassword));
            }
            if (sql.Trim().Substring(sql.Trim().Length - 1, 1) == ",")
                sql = sql.Trim().Substring(0, sql.Length - 1);
            sql += ")";
            ExecuteSql(sql, paras);
        }
        //刪除資料
        public void Delete(int p)
        {
            string sql = "DELETE FROM tMember WHERE fMemberID=@K_fMemberID";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter(("K_fMemberID"), (object)p));
            ExecuteSql(sql, paras);
        }
        //修改、更新資料
        public void Update(AdoMemberModelSample p)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            string sql = "UPDATE tMember SET ";
            if (!string.IsNullOrEmpty(p.fName))
            {
                sql += "fName=@K_FNAME,";
                paras.Add(new SqlParameter(("K_FNAME"), (object)p.fName));
            }
            if (!string.IsNullOrEmpty(p.fEmail))
            {
                sql += "fEmail=@K_fEmail,";
                paras.Add(new SqlParameter(("K_fEmail"), (object)p.fEmail));
            }
            if (!string.IsNullOrEmpty(p.fPassword))
            {
                sql += "fPassword=@K_fPassword,";
                paras.Add(new SqlParameter(("K_fPassword"), (object)p.fPassword));
            }
            if (sql.Trim().Substring(sql.Trim().Length - 1, 1) == ",")
                sql = sql.Trim().Substring(0, sql.Length - 1);
            sql += " WHERE fMemberID = @K_fMemberID";
            paras.Add(new SqlParameter(("K_fMemberID"), (object)p.fMemberID));
            ExecuteSql(sql, paras);
        }

        //------------------
        //----連接資料庫----
        //------------------

        //ExecuteSql只有執行，沒有回傳值
        private void ExecuteSql(string sql, List<SqlParameter> paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=chicekserver.database.windows.net;Initial Catalog=dbChicken22;User ID=chicken;Password=P@ssw0rd-iii";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null)
            {
                foreach (SqlParameter par in paras)
                    cmd.Parameters.Add(par);
            }
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //QueryBySql將查詢結果用List傳回
        private List<AdoMemberModelSample> QueryBySql(string sql, List<SqlParameter> paras)
        {
            List<AdoMemberModelSample> AdoMemberModelSamples = new List<AdoMemberModelSample>();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=chicekserver.database.windows.net;Initial Catalog=dbChicken22;User ID=chicken;Password=P@ssw0rd-iii";

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
                AdoMemberModelSamples.Add(new AdoMemberModelSample()
                {
                    fMemberID = (int)reader["fMemberID"],
                    fName = reader["fName"].ToString(),
                    fEmail = reader["fEmail"].ToString(),
                    fPassword = reader["fPassword"].ToString(),
                    fSex = (int)reader["fSex"],
                    fBirthday = (DateTime)reader["fBirthday"],
                    fPhoto = reader["fPhoto"].ToString(),
                    fGroup = reader["fGroup"].ToString(),
                    fVerified = (Boolean)reader["fVerified"],
                    fAvailable = (bool)reader["fAvailable"],
                });
            }
            con.Close();
            return AdoMemberModelSamples;
        }
    }
}