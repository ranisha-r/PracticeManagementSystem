using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PracticeManagementSystem.Core
{
    public class PatientInfo
    {
        [Key]
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public int PracticeID { get; set; }
        public int ActiveFlg { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }


    }

    



}

