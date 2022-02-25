using CampWebsite.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;


namespace CampWebsite.ViewModels
{
    public class NewCampsiteViewModel
    {

        // Entity

        public bool withoutAltitude { get; set; }
        public tCampsite tCampsite { get; set; }


        [Display(Name = "封面照片")]
        [Required(ErrorMessage = "請選擇封面照片！")]
        public HttpPostedFileBase CoverPhoto { get; set; }


        //
        // DropDownList => CityArea
        public List<SelectListItem> SelectArea = new List<SelectListItem>()
        {
            new SelectListItem {Text="北部", Value="北部" },
            new SelectListItem {Text="中部", Value="中部" },
            new SelectListItem {Text="南部", Value="南部" },
            new SelectListItem {Text="東部", Value="東部" },
            new SelectListItem {Text="離島", Value="離島" },
        };

        // DropDownList => SelectCity
        public List<SelectListItem> SelectCityNorth = new List<SelectListItem>()
            {
                new SelectListItem {Text="台北市", Value="台北市" },
                new SelectListItem {Text="新北市", Value="新北市" },
                new SelectListItem {Text="基隆市", Value="基隆市" },
                new SelectListItem {Text="桃園市", Value="桃園市" },
                new SelectListItem {Text="新竹縣", Value="新竹縣" },
                new SelectListItem {Text="苗栗縣", Value="苗栗縣" },
            };

        public List<SelectListItem> SelectCityCenter = new List<SelectListItem>()
        {
                new SelectListItem {Text="台中市", Value="台中市" },
                new SelectListItem {Text="彰化縣", Value="彰化縣" },
                new SelectListItem {Text="南投縣", Value="南投縣" },
                new SelectListItem {Text="雲林縣", Value="雲林縣" },
        };

        public List<SelectListItem> SelectCitySouth = new List<SelectListItem>()
        {
                new SelectListItem {Text="嘉義縣", Value="嘉義縣" },
                new SelectListItem {Text="台南市", Value="台南市" },
                new SelectListItem {Text="高雄市", Value="高雄市" },
                new SelectListItem {Text="屏東縣", Value="屏東縣" },
        };

        public List<SelectListItem> SelectCityEast = new List<SelectListItem>()
        {
                new SelectListItem {Text="宜蘭縣", Value="宜蘭縣" },
                new SelectListItem {Text="花蓮縣", Value="花蓮縣" },
                new SelectListItem { Text = "台東縣", Value = "台東縣" },
        };

        public List<DayOff> DayOffs = new List<DayOff>()
        {
            new DayOff { Day ="星期一", Value = "1", Checked = false },
            new DayOff { Day ="星期二", Value = "2", Checked = false },
            new DayOff { Day ="星期三", Value = "3", Checked = false },
            new DayOff { Day ="星期四", Value = "4", Checked = false },
            new DayOff { Day ="星期五", Value = "5", Checked = false },
            new DayOff { Day ="星期六", Value = "6", Checked = false },
            new DayOff { Day ="星期七", Value = "7", Checked = false },
            new DayOff { Day ="無公休日", Value = "0", Checked = false },
        };

    }
}