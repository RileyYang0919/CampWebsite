using CampWebsite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    public class CMemberProfileFactory
    {
        dbCampEntities db = new dbCampEntities();
        /// <summary>
        /// 會員歷史訂單區塊
        /// </summary>
        public List<MyOrdersViewModel> getMyOrdersList(int myID)
        {
            List<MyOrdersViewModel> myOrderList = new List<MyOrdersViewModel>(); //訂單卡片的集合

            //myOrderCode 選出用戶的每一筆訂單編號(不重複)
            var myOrderCode = (from c in db.tOrder
                               where c.fMemberID == myID
                               select c.fOrderConfirmCode).Distinct().ToList();
            //使用訂單編號去抓訂單內容
            foreach (var code in myOrderCode)
            {
                //myOrderCodeDetail 訂單內容
                var everyOrder = getSingleOrder(code);
                myOrderList.Add(everyOrder);
            }
            return myOrderList;
        }
        /// <summary>
        /// 訂單的概略，給歷史訂單卡片用的
        /// </summary>
        public MyOrdersViewModel getSingleOrder(string orderCode)
        {
            var myOrderCodeDetail = (from x in db.tOrder
                                     where x.fOrderConfirmCode == orderCode
                                     join t in db.tTent on x.fTentID equals t.fTentID
                                     join c in db.tCampsite on t.fCampsiteID equals c.fCampsiteID
                                     orderby x.fTentID
                                     orderby x.fCheckinDate
                                     select new
                                     {
                                         //CampsiteID = c.fCampsiteID,
                                         CampsiteName = c.fCampsiteName,
                                         TentID = x.fTentID,
                                         CheckinDate = x.fCheckinDate,
                                         Price = x.fOrderPrice
                                     }).ToList();
            DateTime firstDay = myOrderCodeDetail.First().CheckinDate;
            DateTime lastDay = myOrderCodeDetail.Last().CheckinDate.AddDays(1);
            string scheduleFirst = firstDay.ToLongDateString().ToString();
            string scheduleLast = lastDay.ToLongDateString().ToString();
            int countDownDays = firstDay.Subtract(DateTime.Today).Days;
            int priceSum = 0;
            int tentSum = 0;
            List<int> tentIDs = new List<int>();
            //去抓訂單內容裡面的總價、帳篷數量
            foreach (var one in myOrderCodeDetail)
            {
                if (!tentIDs.Contains(one.TentID))
                {
                    tentIDs.Add(one.TentID);
                }
                priceSum += one.Price;
            }
            tentSum = tentIDs.Count();
            MyOrdersViewModel getSingleOrder = new MyOrdersViewModel(); //everyOrder 每一筆訂單卡片
            getSingleOrder.fOrderCode = orderCode;
            getSingleOrder.fCampsiteName = myOrderCodeDetail.First().CampsiteName;
            getSingleOrder.fCheckinStart = scheduleFirst;
            getSingleOrder.fCheckinEnd = scheduleLast;
            getSingleOrder.fTentCount = tentSum;
            getSingleOrder.fTotalPrice = priceSum;
            getSingleOrder.fZDay = countDownDays;
            return getSingleOrder;
        }
        /// <summary>
        /// 單筆訂單內的詳細資訊
        /// </summary>
        public MyOrderDetailViewModel getSingleOrderDetail(string orderCode)
        {
            var myOrders = (from x in db.tOrder
                            where x.fOrderConfirmCode == orderCode
                            join t in db.tTent on x.fTentID equals t.fTentID
                            join c in db.tCampsite on t.fCampsiteID equals c.fCampsiteID
                            orderby x.fTentID
                            orderby x.fCheckinDate
                            select new
                            {
                                CampsiteID = c.fCampsiteID,
                                tTent = t,
                                tOrder = x,
                                CampsiteName = c.fCampsiteName,
                                CampsitePhone = c.fCampsitePhone,
                                CampsiteAddress = c.fCampsiteAddress,
                                CheckinDate = x.fCheckinDate,
                                Price = x.fOrderPrice,
                            }).ToList();
            int CampsiteID = myOrders[0].CampsiteID;
            string fCampsiteName = myOrders[0].CampsiteName;
            string fCampsitePhone = myOrders[0].CampsitePhone;
            string fCampsiteAddress = myOrders[0].CampsiteAddress;
            string scheduleFirst = myOrders.First().CheckinDate.ToLongDateString().ToString();
            DateTime lastDay = myOrders.Last().CheckinDate;
            string scheduleLast = lastDay.AddDays(1).ToLongDateString().ToString();
            int stayNight = Convert.ToInt32(myOrders.Last().CheckinDate.AddDays(1).Subtract(myOrders.First().CheckinDate).Days);
            int priceSum = 0;
            int tentSum = 0;
            List<int> tentIDs = new List<int>();
            List<tOrder> tOrders = new List<tOrder>();
            List<tTent> tTents = new List<tTent>();
            //去抓訂單內容裡面的總價、帳篷數量
            foreach (var one in myOrders)
            {
                if (!tentIDs.Contains(one.tOrder.fTentID))
                {
                    tentIDs.Add(one.tOrder.fTentID);
                }
                tOrders.Add(one.tOrder);
                tTents.Add(one.tTent);
                priceSum += one.Price;
            }
            tentSum = tentIDs.Count();
            // userComment 抓取留言，只要一筆
            var userComment = (from x in db.tComment
                               where x.fOrderConfirmCode == orderCode
                               select x).FirstOrDefault();
            MyOrderDetailViewModel getTheOrderDetails = new MyOrderDetailViewModel(); //everyOrder 每一筆訂單卡片
            getTheOrderDetails.fCampsiteID = CampsiteID;
            getTheOrderDetails.tComment = userComment;
            getTheOrderDetails.fOrderCode = orderCode;
            getTheOrderDetails.fCampsiteName = fCampsiteName;
            getTheOrderDetails.fCampsitePhone = fCampsitePhone;
            getTheOrderDetails.fCampsiteAddress = fCampsiteAddress;
            getTheOrderDetails.fCheckinStart = scheduleFirst;
            getTheOrderDetails.fCheckinEnd = scheduleLast;
            getTheOrderDetails.fTentCount = tentSum;
            getTheOrderDetails.fTotalPrice = priceSum;
            getTheOrderDetails.fStayNight = stayNight;
            getTheOrderDetails.tOrders = tOrders;
            return getTheOrderDetails;
        }
    }
}