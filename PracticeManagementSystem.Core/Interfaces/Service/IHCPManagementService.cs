using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Core
{
    public interface IHCPManagementService
    {
       
        Task<string> AddHcpInteractionInfo(HCPInteractionInfo interactionInfo);
        //Task<string> GenerateReport(int PatientId);
        Task<string> DeleteDocument(int PatientId);
        Task<string> AddDocSignature(DocumentInfo docinfo);       
    }
}
