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
        public ActionResult GenerateOrder(JsonResult test)
        {
            var jasonString = fakeJSON(); //this is fake data.                
            List<PreOrderInfoViewModel> newOrderList = new COrderFactory().OrderJason2VM(jasonString, User.Identity.Name);
            return View(newOrderList.AsEnumerable());
        }
        [HttpPost]
        [Authorize]
        public ActionResult GenerateOrder(IEnumerable<PreOrderInfoViewModel> newOrderList, string fClientName, string fClientEmail, string fClientPhone)
        {
            new COrderFactory().SaveOrder2DB(newOrderList, User.Identity.Name, fClientName, fClientEmail, fClientPhone);
            return RedirectToAction("ListOrder","TempTentList" );
        }

        public string fakeJSON()
        {
            List<COrderJsonModel> allBooks = new List<COrderJsonModel>();
            allBooks.Add(new COrderJsonModel()
            {
                tentID = 101,
                checkinDate = new DateTime(2022, 3, 18),
                price = 111
            });
            allBooks.Add(new COrderJsonModel()
            {
                tentID = 102,
                checkinDate = new DateTime(2022, 3, 18),
                price = 2222
            });
            allBooks.Add(new COrderJsonModel()
            {
                tentID = 101,
                checkinDate = new DateTime(2022, 3, 19),
                price = 2111
            });
            allBooks.Add(new COrderJsonModel()
            {
                tentID = 102,
                checkinDate = new DateTime(2022, 3, 19),
                price = 3222
            });
            string jsonString = JsonConvert.SerializeObject(allBooks);

            return jsonString;
        }
    }
}