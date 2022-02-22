using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace CampWebsite.Models
{
    public class CMailVerifyFactory
    {
        public void SendVerificationMail(string userEmail)
        {
            try
            {
                //  MailMessage ( FromEmailAddress, ToEmailAddress)
                MailMessage message = new MailMessage("<killmemvc@hotmail.com>", userEmail);//MailMessage(寄信者, 收信者)
                message.IsBodyHtml = true;
                message.BodyEncoding = System.Text.Encoding.UTF8;//E-mail編碼
                message.SubjectEncoding = System.Text.Encoding.UTF8;//E-mail編碼
                message.Priority = MailPriority.Normal;//設定優先權
                message.Subject = "請驗證電子信箱"; //E-mail主旨

                message.Body = $"請進行郵箱驗證來完成您註冊的最後一步,點擊下面的連結啟動您的帳號：<br> <a href=https://localhost:44358/Member/Em?email={ userEmail }>返回主頁</a>"; //郵件內容
                SmtpClient MySmtp = new SmtpClient("smtp-mail.outlook.com");//設定gmail的smtp
                                                                            //System.Net.NetworkCredential(帳號,密碼)，hotmail的帳號是整的Email，不是只有@前面的
                MySmtp.Credentials = new System.Net.NetworkCredential("killmemvc@hotmail.com", "123qweasdzxc");
                MySmtp.EnableSsl = true;//開啟ssl
                MySmtp.Send(message);
                MySmtp = null;
                message.Dispose();
            }
            catch { }
        }
    }
}