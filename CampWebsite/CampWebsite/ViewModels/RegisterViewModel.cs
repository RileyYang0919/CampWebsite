using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampWebsite.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "會員姓名")]
        [Required(ErrorMessage = "您必須輸入姓名！")]
        public string fName { get; set; }
        [Display(Name = "會員E-mail(帳號)")]
        [EmailAddress]
        [Required(ErrorMessage = "您必須輸入Email，此Email作為後續登入的帳號")]
        public string fEmail { get; set; }
        [Display(Name = "會員密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "您必須輸入密碼！")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "請設定8~16字元密碼")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$", ErrorMessage = "須包含大小寫英文字母與數字")]
        public string fPassword { get; set; }

        [Display(Name = "再次確認密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "請您再次輸入密碼！")]
        //與Password做比對，再次確認使用者輸入的密碼
        //會使用System.Web.Mvc.Compare，是因為引入System.ComponentModel.DataAnnotations時會有衝突的產生
        [Compare("fPassword", ErrorMessage = "兩次輸入的密碼必須相符！")]
        public string ConfirmPassword { get; set; }
        public string fGroup { get; set; }
    }
}