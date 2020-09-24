using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Core
{
    public interface IPatientManagementRepository
    {
        Task<string> Login(Login logininfo);
        Task<PatientInfo> SelectPatientById(int patientID);
        Task<int> ViewHistory(int patientID);
        Task<int> ViewReport(int patientID);
        Task<string> ModifyPatient(PatientInfo patient);
        Task<string> DeletePatient(int patientID);
        Task<string> AddPatient(PatientInfo patient);
        Task<List<PatientInfo>> SelectAllpatient();



    }
}
