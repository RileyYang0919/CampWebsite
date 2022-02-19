namespace CampWebsite.Models
{
    public class CTentsBooked
    {
        public int fCampsiteID { get; set; }
        public int fTentId { get; set; }
        public string fTentName { get; set; }
        public int BookedCount { get; set; }
        //返回哪一日
        public int fCheckinDate { get; set; }
        public string fCheckinDate_dt { get; set; }
    }
}