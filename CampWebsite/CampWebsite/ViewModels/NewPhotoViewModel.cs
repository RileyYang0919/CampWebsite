using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampWebsite.ViewModels
{
    public class NewPhotoViewModel
    {
        //public tTentPhoto tTentPhoto { get; set; }

        //hidden field 
        public int TentID { get; set; }
        public int CampsiteID { get; set; }
        public string CampsiteName { get; set; }
        public string TentName { get; set; }

        //[Display(Name ="封面照片")]
        //[Required(ErrorMessage ="請選擇封面照片！")]
        //public HttpPostedFileBase  CoverPhoto { get; set; }

        [Display(Name = "其他照片")]
        public HttpPostedFileBase[] OtherPhotos { get; set; }
    }
}