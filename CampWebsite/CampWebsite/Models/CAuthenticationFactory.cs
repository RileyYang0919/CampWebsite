using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CampWebsite.Models
{
    public class CAuthenticationFactory
    {
        // 身分驗證方法
        internal void SetAuthenTicket(string roles, string userID)
        {
            //宣告一個驗證票
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
            1,//版本
            userID,//使用者名稱orID，可以用User.Identity.Name取出
            DateTime.Now,//發行時間
            DateTime.Now.AddMinutes(120),//有效時間，也可以AddHours
            false,//是否將 Cookie 設定成 Session Cookie，如果是則會在瀏覽器關閉後移除。
            roles//寫入使用者角色
            );
            //加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
        }
    }
}