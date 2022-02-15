using CampWebsite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampWebsite.Controllers
{
    public class TempTentListController : Controller
    {
        dbCampEntities db = new dbCampEntities();
        // GET: TempTentList
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListTent()
        {
            var tent = db.tTent.OrderBy(t => t.fTentID).ToList();
            return View(tent);
        }

        public ActionResult ListOrder()
        {
            var order = db.tOrder.OrderBy(t => t.fOrderID).ToList();
            return View(order);
        }

        public ActionResult testUploadPic()
        {
            ViewBag.t1 = DateTime.Now.ToString("MM-dd-yyyy");
            ViewBag.t2 = DateTime.Now.ToString("MMddyyyyff");
            ViewBag.t3 = DateTime.Now.ToString("d");
            ViewBag.t4 = DateTime.Now.ToString("ff");


            return View();
        }
        public virtual ActionResult UploadFile()
        {



            string path = Server.MapPath("~/Images/Test");

            //## 如果有任何檔案類型才做
            if (Request.Files.AllKeys.Any())
            {
                //## 讀取指定的上傳檔案ID
                var httpPostedFile = Request.Files["UploadedImage"];

                //## 真實有檔案，進行上傳
                if (httpPostedFile != null && httpPostedFile.ContentLength != 0)
                {

                    httpPostedFile.SaveAs(path + Path.GetFileName(httpPostedFile.FileName));
                }
            }

            //## 模擬上傳的檔案內容
            //List<Test> oStr = new List<Test>();
            //oStr.Add(new Test() { ID = 1, Name = "王1" });
            //oStr.Add(new Test() { ID = 2, Name = "王2" });

            //## 將結果回傳
            return Json(new { isUploaded = true, result = "成功囉" }, "text/html");
        }
    }
}