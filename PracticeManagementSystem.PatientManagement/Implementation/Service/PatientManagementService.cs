using PracticeManagementSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PracticeManagementSystem.PatientManagement
{
    public class PatientManagementService : IPatientManagementService
    {
        private readonly IPatientManagementRepository _patientManagementRepository;

        public PatientManagementService(IPatientManagementRepository patientManagementRepository)
        {
            _patientManagementRepository = patientManagementRepository;
        }
        public async Task<string> AddPatient(PatientInfo patientInfo)
        {
            return await _patientManagementRepository.AddPatient(patientInfo);
        }

        public async Task<string> DeletePatient(int patientID)
        {
            return await _patientManagementRepository.DeletePatient(patientID);
        }

        public async Task<string> Login(Login logininfo)
        {
            return await _patientManagementRepository.Login(logininfo);
        }

        public async Task<string> ModifyPatient(PatientInfo patientInfo)
        {
            return await _patientManagementRepository.ModifyPatient(patientInfo);
        }

        public async Task<List<PatientInfo>> SelectAllpatient()
        {
            return await _patientManagementRepository.SelectAllpatient();
        }

        public async Task<PatientInfo> SelectPatientById(int patientID)
        {
            return await _patientManagementRepository.SelectPatientById(patientID);
        }

        public Task<int> ViewHistory(int patientID)
        {
            throw new NotImplementedException();
        }

        public Task<int> ViewReport(int patientID)
        {
            throw new NotImplementedException();
        }
    }

}
