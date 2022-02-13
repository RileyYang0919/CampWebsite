using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CampWebsite.Models;
using System.Web.Security;
using CampWebsite.ViewModels;

namespace CampWebsite.Controllers
{
    public class MemberController : Controller
    {
        dbCampEntities db = new dbCampEntities();

        public ActionResult Register()
        {
            RegisterViewModel newMember = new RegisterViewModel();
            return View(newMember);
        }
        [HttpPost]
        public ActionResult Register( RegisterViewModel newMember )
        {
            //string fName, string fEmail, string fPassword
            if (ModelState.IsValid == false)
            {
                return View(newMember);
            }
            var member = db.tMember.Where(i => i.fEmail == newMember.fEmail).FirstOrDefault();
            if (member == null)
            {
                try
                {
                    tMember newUser = new tMember();
                    newUser.fName = newMember.fName;
                    newUser.fEmail = newMember.fEmail;
                    newUser.fPassword = newMember.fPassword;
                    newUser.fSex = 0;
                    newUser.fGroup = "gCustomer";
                    newUser.fVerified = false;
                    newUser.fAvailable = false; //此欄位之後要刪除
                    db.tMember.Add(newUser);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
                catch
                {
                    return View();  //在這邊返回View
                }
                
            }
            ViewBag.Message = "帳號重複";
            return View();
        }
        // ---

        // 登入會員
        public ActionResult Login()
        {
            //得到原先頁面的完整URL
            string previousUrl = (Request.UrlReferrer == null) ? "" : Request.UrlReferrer.ToString();
            ViewData["returnUrl"] = previousUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string fEmail, string fPassword, string returnUrl)
        {
            var member = db.tMember.Where(i => i.fEmail == fEmail && i.fPassword == fPassword).FirstOrDefault();
            //if member is null,表示沒註冊
            if (member == null)
            {
                ViewBag.Message = "帳號密碼有誤" + "\n我輸入: " + fEmail + " 密碼: " + fPassword;
                ViewData["returnUrl"] = returnUrl;
                return View();
            }
            string userData = (member.fGroup).ToString() + "," + member.fName;
            string userID = (member.fMemberID).ToString();
            new CAuthenticationFactory().SetAuthenTicket(userData, userID);
            //另一種驗證方式FormsAuthentication.RedirectFromLoginPage(member.Email, true);
            return Redirect(returnUrl);
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
            return Redirect("/Home/Index");
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
    }
}