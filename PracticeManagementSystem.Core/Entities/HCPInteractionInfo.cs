using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PracticeManagementSystem.Core
{

    public class HCPInteractionInfo
    {
        [Key]
        public int HCPInteractionId { get; set; }
        public int PatientId { get; set; }
        public int HCPId { get; set; }
        public DateTime VisitDate { get; set; }
        public string DiagnosisDetails { get; set; }
        public string HCPComments { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
    public class DocumentInfo
    {
        [Key]
        public int DocId { get; set; }
        public string DocName { get; set; }
        public int DiagnosisId { get; set; }
        public string HCPSignature { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
        
}
