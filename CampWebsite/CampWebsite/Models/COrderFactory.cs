using CampWebsite.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    public class COrderFactory
    {
        dbCampEntities db = new dbCampEntities();

        //將客戶想要下單的資料存成JSON送至GenerateOrderController
        //使用OrderJason2VM method將JSON轉成model list
        public List<PreOrderInfoViewModel> OrderJason2VM(string jasonString, string theUserID)
        {
            //將JSON轉成JsonModel並存進list
            var myJsonModels = JsonConvert.DeserializeObject<List<COrderJsonModel>>(jasonString);
            int userID = Convert.ToInt32(theUserID);
            //宣告newOrder model的list
            List<PreOrderInfoViewModel> newOrderList = new List<PreOrderInfoViewModel>();
            //迭代每一筆JsonModel資料，並combine到newOrder model
            foreach (var item in myJsonModels)
            {
                //initialize data from Jason
                PreOrderInfoViewModel newOrder = new PreOrderInfoViewModel();
                int theTentID = item.tentID;
                DateTime theCheckinDate = item.checkinDate;
                int thePrice = item.price;
                //combine model
                newOrder.tTent = db.tTent.Where(t => t.fTentID == theTentID).FirstOrDefault();
                newOrder.tMember = db.tMember.Where(m => m.fMemberID == userID).FirstOrDefault();
                newOrder.fCheckinStart = theCheckinDate;
                newOrder.fCheckinEnd = theCheckinDate.AddDays(1.0);
                newOrder.fOrderPrice = thePrice;
                newOrderList.Add(newOrder);
            }
            return newOrderList;
        }
        //將訂單存進DB
        public void SaveOrder2DB(IEnumerable<PreOrderInfoViewModel> newOrderList, string theUserID, string fClientName, string fClientEmail, string fClientPhone)
        {
            int userID = Convert.ToInt32(theUserID);
            string orderConfirmCode = "OD" + DateTime.Now.ToString("yyMMddff");
            //迭代每一筆newOrderList資料
            foreach (var item in newOrderList)
            {
                tOrder newOrder = new tOrder();
                newOrder.fMemberID = userID;
                newOrder.fTentID = item.tTent.fTentID;
                newOrder.fOrderConfirmCode = orderConfirmCode;
                newOrder.fClientName = fClientName;
                newOrder.fClientEmail = fClientEmail;
                newOrder.fClientPhone = fClientPhone;
                newOrder.fCheckinDate = item.fCheckinStart;
                newOrder.fOrderPrice = item.fOrderPrice;
                newOrder.fOrderComment = item.fOrderComment;
                newOrder.fOrderIsPaid = false;
                newOrder.fOrderCreatedTime = DateTime.Now;
                db.tOrder.Add(newOrder);
                db.SaveChanges();
            }
        }
    }
}