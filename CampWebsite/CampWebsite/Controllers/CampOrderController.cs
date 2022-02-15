using CampWebsite.Models;
using CampWebsite.ViewModels;
using Newtonsoft.Json;
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
        public ActionResult GenerateOrder(string orderJson)
        {
            //var jasonString = fakeJSON(); //this is fake data.                
            List<PreOrderInfoViewModel> newOrderList = new COrderFactory().OrderJason2VM(orderJson, User.Identity.Name);
            return View(newOrderList.AsEnumerable());
        }
        [HttpPost]
        [Authorize]
        public ActionResult GenerateOrder(IEnumerable<PreOrderInfoViewModel> newOrderList, string fClientName, string fClientEmail, string fClientPhone)
        {
            new COrderFactory().SaveOrder2DB(newOrderList, User.Identity.Name, fClientName, fClientEmail, fClientPhone);
            return RedirectToAction("ListOrder","TempTentList" );
        }

    }
}