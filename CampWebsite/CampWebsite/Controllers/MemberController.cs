using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CampWebsite.Models;
using System.Web.Security;

namespace CampWebsite.Controllers
{
    public class MemberController : Controller
    {
        dbCampEntities db = new dbCampEntities();

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(string fName, string fEmail, string fPassword )
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }
            var member = db.tMember.Where(i => i.fEmail == fEmail).FirstOrDefault();
            if (member == null)
            {
                tMember newUser = new tMember();
                newUser.fName = fName;
                newUser.fEmail = fEmail;
                newUser.fPassword = fPassword;
                newUser.fSex = 0;
                newUser.fGroup = "gCustomer";
                newUser.fVerified = false;
                newUser.fAvailable = false;
                db.tMember.Add(newUser);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            ViewBag.Message = "帳號重複";
            return View();
        }
        // ---

        // 登入會員
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(string fEmail, string fPassword)
        {
            var member = db.tMember.Where(i => i.fEmail == fEmail && i.fPassword == fPassword).FirstOrDefault();
            //if member is null,表示沒註冊
            if (member == null)
            {
                ViewBag.Message = "帳號密碼有誤" + "\n我輸入: " + fEmail + " 密碼: " + fPassword;
                return View();
            }
            string userData = (member.fGroup).ToString() + "," + member.fName;
            string userID = (member.fMemberID).ToString();
            //Session["UserName"] = member.fName;
            SetAuthenTicket(userData, userID);
            //指定使用者帳號通過驗證(需要using System.Web.Security)
            //FormsAuthentication.RedirectFromLoginPage(member.Email, true);
            return RedirectToAction("Index", "Home");
        }

        //修改個人資料
        [Authorize]
        public ActionResult personalProfile()
        {
            int myID = Convert.ToInt32(User.Identity.Name);
            var member = db.tMember.Where(i => i.fMemberID == myID).FirstOrDefault();

            return View(member);
        }
        [HttpPost]
        public ActionResult personalProfile(tMember m)
        {
            int myID = Convert.ToInt32(User.Identity.Name);
            var temp = db.tMember.Where(i => i.fMemberID == myID).FirstOrDefault();
            temp.fName = m.fName;
            db.SaveChanges();
            return RedirectToAction("List", "Home");
        }


        // 顯示所有資料
        public ActionResult List()
        {
            var members = db.tMember.OrderBy(m => m.fMemberID).ToList();
            return View(members);
        }

        //登出
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

        [Authorize]
        [Authorize(Roles = "gVendor")]
        public ActionResult forGroup3()
        {
            return View();
        }
        [Authorize(Roles = "gCustomer")]
        public ActionResult forGroup2()
        {
            return View();
        }

        // 身分驗證方法
        void SetAuthenTicket(string roles, string userID)
        {
            //宣告一個驗證票
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
            1,//版本
            userID,//使用者名稱orID，可以用User.Identity.Name取出
            DateTime.Now,//發行時間
            DateTime.Now.AddMinutes(20),//有效時間，也可以AddHours
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