using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeManagementSystem.Core;

namespace PracticeManagementSystem.PatientManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientManagementController : ControllerBase
    {
        private readonly IPatientManagementService _patientManagementService;

        public PatientManagementController(IPatientManagementService patientManagementService)
        {
            _patientManagementService = patientManagementService;
        }

        [HttpGet("SelectPatients")]
        [Authorize]
        public async Task<List<PatientInfo>> SelectAllpatient() => await _patientManagementService.SelectAllpatient();

        [HttpGet("SelectPatientById")]
        [Authorize]
        public async Task<PatientInfo> SelectPatientById(int PatientId) => await _patientManagementService.SelectPatientById(PatientId);

        [HttpPut("ModifyPatient")]
        [Authorize]
        public async Task<string> ModifyPatient(PatientInfo Patient) => await _patientManagementService.ModifyPatient(Patient);

        [HttpPost("AddPatient")]
        public async Task<string> AddPatient(PatientInfo Patient) => await _patientManagementService.AddPatient(Patient);

        [HttpPost("Login")]
        public async Task<string> Login(Login loginfo) => await _patientManagementService.Login(loginfo);

        [HttpDelete("DeletePatient")]
        [Authorize]
        public async Task<string> DeletePatient(int PatientId) => await _patientManagementService.DeletePatient(PatientId);

    }
}
