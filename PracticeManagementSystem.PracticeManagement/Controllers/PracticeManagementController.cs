using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeManagementSystem.Core;

namespace PracticeManagementSystem.PracticeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticeManagementController : ControllerBase
    {
        private readonly IPracticeManagementService _practiceManagementService;


        public PracticeManagementController(IPracticeManagementService practiceManagementService)
        {
            
            _practiceManagementService = practiceManagementService;
        }
        [HttpGet("ViewPractices")]
        [Authorize]
        public async Task<List<PracticeInfo>> ViewAllPractice() => await _practiceManagementService.ViewAllPractice();

        [HttpGet("ViewPracticeById")]
        [Authorize]
        public async Task<PracticeInfo> ViewPracticeById(int practiceId) => await _practiceManagementService.ViewPracticeById(practiceId);

        [HttpPut("ModifyPractice")]
        [Authorize]
        public async Task<string> ModifyPractice(PracticeInfo practice) => await _practiceManagementService.ModifyPractice(practice);
        
        [HttpPost("AddPractice")]
        [Authorize]
        public async Task<string> AddPractice(PracticeInfo practice) => await _practiceManagementService.AddPractice(practice);

        [HttpDelete("DeletePractice")]
        [Authorize]
        public async Task<string> DeletePractice(int practiceId) => await _practiceManagementService.DeletePractice(practiceId);

    }
}
