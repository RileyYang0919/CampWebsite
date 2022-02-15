using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampWebsite.Models
{
    [MetadataType(typeof(tTentMetadata))]

    public partial class tTent
    {
        //Data Annotation
        public class tTentMetadata
        {

            [Required]
            [DisplayName("選項名稱")]
            [MinLength(3, ErrorMessage = "最少三個字唷")]
            [MaxLength(12, ErrorMessage = "最多12個字聽不懂是不是")]
            public string fTentName { get; set; }


            [DisplayName("選項分類")]
            public string fTentCategory { get; set; }


            //room for ...?
            [Required]
            [DisplayName("容納人數")]
            public int fTentPeople { get; set; }


            [Required]
            [DisplayName("平日價格")]
            public int fTentPriceWeekday { get; set; }


            [Required]
            [DisplayName("例假日價格")]
            public int fTentPriceWeekend { get; set; }

        }

    }
}