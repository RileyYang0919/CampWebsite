using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampWebsite.Models
{
    public class AdoMemberModelSample
    {
        public int fMemberID { get; set; }
        public string fName { get; set; }
        public string fEmail { get; set; }
        public string fPassword { get; set; }
        public int fSex { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> fBirthday { get; set; }
        public string fPhoto { get; set; }
        public string fGroup { get; set; }
        public bool fVerified { get; set; }
        public bool fAvailable { get; set; }
    }
}