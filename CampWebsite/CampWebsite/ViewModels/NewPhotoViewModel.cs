using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CampWebsite.Models;
namespace CampWebsite.ViewModels
{
    public class NewPhotoViewModel
    {

        //public tTentPhoto tTentPhoto { get; set; }

        public string TentName { get; set; }
        public int CampsiteID { get; set; }
        public string CampsiteName { get; set; }



        [Required(ErrorMessage = "請選擇照片！")]
        [Display(Name = "住宿照片")]
        public HttpPostedFileBase[] OtherPhotos { get; set; }


    }
}