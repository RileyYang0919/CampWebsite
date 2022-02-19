namespace CampWebsite.Models
{
    public class CTents
    {
        public int fTentId { get; set; }
        public string fTentName { get; set; }
        public string fTentCategory { get; set; }
        public int fTentPeople { get; set; }
        public int fTentPriceWeekday { get; set; }
        public int fTentPriceWeekend { get; set; }
        public string fCampsiteClosedDay { get; set; }
        public int fTentQuantity { get; set; }
        public int fCampsiteID { get; set; }
    }
}