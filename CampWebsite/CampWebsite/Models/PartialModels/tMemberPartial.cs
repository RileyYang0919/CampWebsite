using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    [MetadataType(typeof(tMemberMetadata))]
    public partial class tMember
    {
        public class tMemberMetadata
        {
            [DisplayName("會員姓名")]
            [Required(ErrorMessage = "姓名為必填欄位！")]
            public string fName { get; set; }

            [DisplayName("會員E-mail")]
            [Required(ErrorMessage = "Email為必填欄位！")]
            [EmailAddress(ErrorMessage = "Email格式錯誤！")]
            public string fEmail { get; set; }

            [DisplayName("會員密碼")]
            [Required(ErrorMessage = "密碼為必填欄位！")]            
            public string fPassword { get; set; }
            [DataType(DataType.Date)]
            public Nullable<System.DateTime> fBirthday { get; set; }
        }            
    }
}