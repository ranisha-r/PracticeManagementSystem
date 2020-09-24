using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PracticeManagementSystem.Core
{
    public class PracticeInfo
    {
        [Key]
        public int PracticeId{ get; set; }
        public string PracticeName { get; set; }     
        public string EmailId { get; set; }
        public string LocationDetails { get; set; }
        public string GST { get; set; }
        public int ActiveFlg { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}



