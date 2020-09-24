using PracticeManagementSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeManagementSystem.HCPManagement
{
    public class HCPManagementService : IHCPManagementService
    {
        private readonly IHCPManagementRepository _hCPManagementRepository;

        public HCPManagementService(IHCPManagementRepository hCPManagementRepository)
        {
            _hCPManagementRepository = hCPManagementRepository;
        }

        public async Task<string> AddHcpInteractionInfo(HCPInteractionInfo interactionInfo)
        {
            return await _hCPManagementRepository.AddHcpInteractionInfo(interactionInfo);
        }

        //public async Task<string> AddHCP(HCPInfo newHcp)
        //{
        //    return await _hCPManagementRepository.AddHCP(newHcp);
        //}

        public async Task<string> AddDocSignature(DocumentInfo docinfo)
        {
            return await _hCPManagementRepository.AddDocSignature(docinfo);
        }

        public async Task<string> DeleteDocument(int PatientId)
        {
            return await _hCPManagementRepository.DeleteDocument(PatientId);
        }

        //public async  Task<string> GenerateReport(int PatientId)
        //{
        //    return await _hCPManagementRepository.GenerateReport(PatientId);
        //}

        //public async Task<string> ModifyHCP(HCPInfo newHcp)
        //{
        //    return await _hCPManagementRepository.ModifyHCP(newHcp);
        //}
    }
}
