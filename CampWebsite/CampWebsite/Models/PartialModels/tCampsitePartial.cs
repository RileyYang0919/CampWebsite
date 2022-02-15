using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// tCampsite Partial
namespace CampWebsite.Models
{
    [MetadataType(typeof(tCampsiteMetadata))]
    public partial class tCampsite
    {
        //Data Annotation
        public class tCampsiteMetadata
        {
            [Required]
            [DisplayName("營區名稱")]
            public string fCampsiteName { get; set; }


            [DisplayName("連絡電話")]
            [DataType(DataType.PhoneNumber)]
            //[RegularExpression("d{}"),ErrorMessage("")]
            public string fCampsitePhone { get; set; }

            [DisplayName("地區")]
            public string fCampsiteArea { get; set; }


            [Required]
            [DisplayName("所在縣市")]
            public string fCampsiteCity { get; set; }


            [Required]
            [DisplayName("地址")]
            public string fCampsiteAddress { get; set; }


            [Required]
            [DisplayName("簡介")]
            public string fCampsiteIntroduction { get; set; }


            [DisplayName("海拔高度")]
            public string fCampsiteAltitude { get; set; }

            [DisplayName("公休日")]
            [DefaultValue("0")]
            public string fCampsiteClosedDay { get; set; }

            [DisplayName("入房時間")]
            public string fCampsiteCheckInTime { get; set; }

            [DisplayName("退房時間")]
            public string fCampsiteCheckOutTime { get; set; }
        }

        //DropDownList for CityArea

        //DropDownList for 北部
        //DropDownList for 中部
        //DropDownList for 南部
        //DropDownList for 東部

        //DropDownList for SelectCity
        public List<SelectListItem> SelectCity = new List<SelectListItem>()
            {
                new SelectListItem {Text="台北市", Value="台北市" },
                new SelectListItem {Text="新北市", Value="新北市" },
                new SelectListItem {Text="基隆市", Value="基隆市" },
                new SelectListItem {Text="桃園市", Value="桃園市" },
                new SelectListItem {Text="新竹縣", Value="新竹縣" },
                new SelectListItem {Text="苗栗縣", Value="苗栗縣" },
                new SelectListItem {Text="台中市", Value="台中市" },
                new SelectListItem {Text="彰化縣", Value="彰化縣" },
                new SelectListItem {Text="南投縣", Value="南投縣" },
                new SelectListItem {Text="雲林縣", Value="雲林縣" },
                new SelectListItem {Text="嘉義縣", Value="嘉義縣" },
                new SelectListItem {Text="台南市", Value="台南市" },
                new SelectListItem {Text="高雄市", Value="高雄市" },
                new SelectListItem {Text="屏東縣", Value="屏東縣" },
                new SelectListItem {Text="宜蘭縣", Value="宜蘭縣" },
                new SelectListItem {Text="花蓮縣", Value="花蓮縣" },
                new SelectListItem {Text="台東縣", Value="台東縣" },
            };

    }
}