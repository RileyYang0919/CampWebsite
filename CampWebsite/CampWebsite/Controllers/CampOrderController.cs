using CampWebsite.Models;
using CampWebsite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampWebsite.Controllers
{
    public class CampOrderController : Controller
    {
        dbCampEntities db = new dbCampEntities();
        // GET: CampOrder
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult GenerateOrder(int TentID)
        {
            PreOrderInfoViewModel newOrder = new PreOrderInfoViewModel();
            newOrder.tTent = db.tTent.Where(t => t.fTentID == TentID).FirstOrDefault();
            int userID = Convert.ToInt32(User.Identity.Name);
            newOrder.tMember = db.tMember.Where(m => m.fMemberID == userID).FirstOrDefault();
            DateTime CheckinDate = DateTime.Today;
            newOrder.fCheckinDateBegin = CheckinDate;
            return View(newOrder);
        }
        [HttpPost]
        public ActionResult GenerateOrder(PreOrderInfoViewModel VM)
        {
            tOrder newOrder = new tOrder();
            newOrder.fMemberID = Convert.ToInt32(User.Identity.Name);
            newOrder.fTentID = VM.tTent.fTentID;
            newOrder.fClinetName = VM.tMember.fName;
            newOrder.fClinetEmail = VM.tMember.fEmail;
            newOrder.fClinetPhone = VM.tMember.fPhoneNumber;
            //newOrder.fCheckinDate = VM.fCheckinDateBegin;
            newOrder.fCheckinDate = VM.fCheckinDateBegin;
            newOrder.fOrderPrice = VM.fOrderPrice;
            newOrder.fOrderComment = VM.fOrderComment;
            newOrder.fOrderIsPaid = false;
            newOrder.fOrderCreatedTime = DateTime.Now;
            db.tOrder.Add(newOrder);
            db.SaveChanges();

            return RedirectToAction("personalProfile", "Member");
        }
    }
}