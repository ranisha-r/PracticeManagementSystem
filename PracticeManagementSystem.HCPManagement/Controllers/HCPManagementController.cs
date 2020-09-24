using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeManagementSystem.Core;

namespace PracticeManagementSystem.HCPManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class HCPManagementController : ControllerBase
    {
        private readonly IHCPManagementService _iHCPManagementService;

        public HCPManagementController(IHCPManagementService iHCPManagementService)
        {
            _iHCPManagementService = iHCPManagementService;
        }

        //[HttpPost("Add HCP")]
        //public async Task<string> AddHCP(HCPInfo newHcp) => await _iHCPManagementService.AddHCP(newHcp);

        //[HttpPost("Modify HCP")]
        //public async Task<string> ModifyHCP(HCPInfo newHcp) => await _iHCPManagementService.ModifyHCP(newHcp);

        [HttpPost("AddHcpInteractionInfo")]
        public async Task<string> AddHcpInteractionInfo(HCPInteractionInfo interactionInfo) => await _iHCPManagementService.AddHcpInteractionInfo(interactionInfo);

        //[HttpGet("GenerateReport")]
        //public async Task<string> GenerateReport(int PatientId) => await _iHCPManagementService.GenerateReport(PatientId);

        [HttpDelete("DeleteReport")]
        public async Task<string> DeleteDocument(int PatientId) => await _iHCPManagementService.DeleteDocument(PatientId);

        [HttpPut("AddSignature")]
        public async Task<string> AddDocSignature(DocumentInfo docinfo) => await _iHCPManagementService.AddDocSignature(docinfo);
       
    }
}
