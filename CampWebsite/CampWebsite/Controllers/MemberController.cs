using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CampWebsite.Models;
using System.Web.Security;
using CampWebsite.ViewModels;
using System.IO;

namespace CampWebsite.Controllers
{
    public class MemberController : Controller
    {
        dbCampEntities db = new dbCampEntities();
        /// <summary>
        /// 註冊會員
        /// </summary>
        public ActionResult Register()
        {
            RegisterViewModel newMember = new RegisterViewModel();
            return View(newMember);
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel newMember, int fGroup)
        {
            //string fName, string fEmail, string fPassword
            if (ModelState.IsValid == false)
            {
                return View();
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
                    newUser.fGroup = fGroup;
                    newUser.fVerified = false;
                    db.tMember.Add(newUser);
                    db.SaveChanges();
                    string userData = (newUser.fGroup).ToString() + "," + newUser.fName;
                    string userID = (newUser.fMemberID).ToString();
                    new CAuthenticationFactory().SetAuthenTicket(userData, userID);
                    newUser.fPhoto = "/Images/Members/user" + userID + ".jpg"; //拿到userID後，產生一組photo url存進資料庫
                    db.SaveChanges();

                    new CMailVerifyFactory().SendVerificationMail(newUser.fEmail); //發送驗證信給new user, function by 俊丞

                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    ViewBag.Message = "Something wrong!";
                    return View();  //在這邊返回View
                }
            }
            ViewBag.Message = "此Email帳號已被註冊";
            return View();
        }
        /// <summary>
        /// 會員點擊email驗證連結後的頁面
        /// </summary>
        [HttpGet]
        public ActionResult Em()
        {
            string fem = Request.QueryString["email"];
            var member = db.tMember.Where(i => i.fEmail == fem).FirstOrDefault();
            if (member.fVerified == false)
            {
                member.fVerified = true;
                db.SaveChanges();
            }
            return View();
        }

        /// <summary>
        /// 登入會員
        /// </summary>
        public ActionResult Login()
        {
            //得到原先頁面的完整URL
            string previousUrl = (Request.UrlReferrer == null) ? "" : Request.UrlReferrer.ToString();
            ViewData["returnUrl"] = previousUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string fEmail, string fPassword, string returnUrl, bool fRemember)
        {
            var member = db.tMember.Where(i => i.fEmail == fEmail && i.fPassword == fPassword).FirstOrDefault();
            //if member is null,表示沒註冊
            if (member == null)
            {
                ViewBag.Message = "該帳號不存在";
                ViewData["returnUrl"] = returnUrl;
                return View();
            }
            string userData = (member.fGroup).ToString() + "," + member.fName;
            string userID = (member.fMemberID).ToString();
            new CAuthenticationFactory().SetAuthenTicket(userData, userID, fRemember);
            //另一種驗證方式FormsAuthentication.RedirectFromLoginPage(member.Email, true);
            return Redirect(returnUrl);
        }
        ////////////////////////////////////////////////////////////////////////////////////////會員資料頁面///
        /// <summary>
        /// 會員資料、營區收藏、歷史訂單
        /// </summary>
        [Authorize]
        public ActionResult personalProfile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult personalProfile(tMember tMember)
        {
            int myID = Convert.ToInt32(User.Identity.Name);
            var editMember = db.tMember.Where(i => i.fMemberID == myID).FirstOrDefault();
            editMember.fName = tMember.fName;
            editMember.fPassword = tMember.fPassword;
            editMember.fPhoneNumber = tMember.fPhoneNumber;
            editMember.fSex = tMember.fSex;
            editMember.fBirthday = tMember.fBirthday;
            db.SaveChanges();
            //更新身分憑證
            string userData = (editMember.fGroup).ToString() + "," + editMember.fName;
            string userID = (editMember.fMemberID).ToString();
            new CAuthenticationFactory().SetAuthenTicket(userData, userID);
            return RedirectToAction("Index", "Home");
        }
        // MyProfile 用戶的個人資料
        public ActionResult MyProfilePartialView()
        {
            int myID = Convert.ToInt32(User.Identity.Name);
            var member = db.tMember.Where(i => i.fMemberID == myID).FirstOrDefault();
            if (member.fBirthday.HasValue)
            {
                DateTime myBirthday = member.fBirthday.HasValue ? member.fBirthday.Value : DateTime.MinValue;
                ViewBag.Birthday = myBirthday.ToString("yyyy-MM-dd");
            }
            return PartialView(member);
        }
        // MyOrders 用戶的歷史訂單(卡片)
        public ActionResult MyOrdersPartialView()
        {
            int myID = Convert.ToInt32(User.Identity.Name);
            var myOrderList = new CMemberProfileFactory().getMyOrdersList(myID);
            return PartialView(myOrderList);
        }
        //查詢我的收藏
        public ActionResult MyFavoritePartialView()
        {
            int myID = Convert.ToInt32(User.Identity.Name);
            var myFavorites = new CCampFavoriteFactory().QueryByFid(myID);
            return PartialView(myFavorites);
        }
        /// <summary>
        /// 上傳會員圖片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult uploadPersonalPic()
        {
            string path = Server.MapPath("~/Images/Members/");
            //## 如果有任何檔案類型才做
            if (Request.Files.AllKeys.Any())
            {
                //## 讀取指定的上傳檔案ID
                var httpPostedFile = Request.Files["UploadedImage"];
                int myID = Convert.ToInt32(User.Identity.Name);
                //## 真實有檔案，進行上傳
                if (httpPostedFile != null && httpPostedFile.ContentLength != 0)
                {
                    httpPostedFile.SaveAs(path + Path.GetFileName("user" + myID + ".jpg"));
                }
            }
            return Json(new { isUploaded = true, result = "成功囉" }, "text/html");
        }
        ////////////////////////////////////////////////////////////////////////////////////////會員資料頁面///結束
        ///
        /// <summary>
        /// 點選訂單卡片後，進入OrderDetail回傳訂單細節
        /// </summary>
        public ActionResult OrderDetail(string OrderCode)
        {
            if (OrderCode == "" || OrderCode == null)
            {
                return View();
            }
            var myOrderDetail = new CMemberProfileFactory().getSingleOrderDetail(OrderCode);

            return View(myOrderDetail);
        }
        /// <summary>
        /// 會員的營區留言功能
        /// </summary>
        public ActionResult SaveUserComments(tComment userComment)
        {
            if (userComment == null)
            {
                return Json(null);
            }
            else if (string.IsNullOrEmpty(userComment.fComment))
            {
                return Json("msg:'No Comment'");
            }
            userComment.fMemberID = Convert.ToInt32(User.Identity.Name);
            userComment.fCommentTime = DateTime.Now;
            db.tComment.Add(userComment);
            db.SaveChanges();
            return RedirectToAction("Details", "CampSite", new { id = userComment.fCampsiteID });
        }
        //登出
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return Redirect("/Home/Index");
        }


        // 顯示所有會員-限開發用
        public ActionResult List()
        {
            var members = db.tMember.OrderBy(m => m.fMemberID).ToList();
            return View(members);
        }
        //身分群組測試頁面
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