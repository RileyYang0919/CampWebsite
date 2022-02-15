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
        public string CampsiteName { get; set; }
        public string TentName { get; set; }


        [Required(ErrorMessage = "請選擇檔案！")]
        //public HttpPostedFileBase[] Photoes{ get; set; }
        public HttpPostedFileBase Photo { get; set; }
    }
}