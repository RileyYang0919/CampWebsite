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
            public string fCampsiteClosedDay { get; set; }

            [DisplayName("入房時間")]
            public string fCampsiteCheckInTime { get; set; }

            [DisplayName("退房時間")]
            public string fCampsiteCheckOutTime { get; set; }
        }
    }
}