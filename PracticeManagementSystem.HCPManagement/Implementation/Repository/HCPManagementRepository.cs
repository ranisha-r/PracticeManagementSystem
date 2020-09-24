using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PracticeManagementSystem.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PracticeManagementSystem.HCPManagement
{ 
    public class HCPManagementRepository : IHCPManagementRepository
    {

        private readonly HCPManagementDBContext _hCPManagementDBContext;
        public HCPManagementRepository(HCPManagementDBContext hCPManagementDBContext)
        {

            _hCPManagementDBContext = hCPManagementDBContext;
        }
     

        public async Task<string> AddHcpInteractionInfo(HCPInteractionInfo interactionInfo)
        {
            try
            {
                
                HCPInteractionInfo resdiagnosis = await _hCPManagementDBContext.HCPInteractionInfo.AsNoTracking().Where(x => x.HCPId == interactionInfo.HCPId && x.PatientId== interactionInfo.PatientId).FirstOrDefaultAsync();
                //string patientPractice = "";
                //string hcpPractice = "";
                //using (HttpClient client = new HttpClient())
                //{

                //    var patientPracticeresponse= await client.GetAsync("https://localhost:44325/api/PatientManagement/SelectPatientById?PatientId" + interactionInfo.PatientId);
                //    patientPractice = patientPracticeresponse.StatusCode.ToString();


                //}
                //using (HttpClient client = new HttpClient())
                //{

                //    var hcpPracticeresponse = await client.GetAsync("https://localhost:44385/api/User/SelectUserbyId?userId" + interactionInfo.HCPId);
                //    hcpPractice = hcpPracticeresponse.StatusCode.ToString();


                //}
                string respatient = "";
                PatientInfo pobj = new PatientInfo();
                int pat_practiceid = 0;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MDE0NTI2ODQsImlzcyI6IlRlc3QuY29tIiwiYXVkIjoiVGVzdC5jb20ifQ.uQGmxaAJ3kSEycD0xSPhbQUXI2vEQBeOJJ3XuTB-zZk");
                    var response = await client.GetAsync("https://localhost:44325/api/PatientManagement/SelectPatientById?PatientId=" + interactionInfo.PatientId);
                    respatient = response.StatusCode.ToString();
                    pobj = JsonConvert.DeserializeObject<PatientInfo>(await response.Content.ReadAsStringAsync());
                    pat_practiceid = pobj.PracticeID;
                }

                      
                string reshcp = "";
                UserInfo dobj = new UserInfo();
                int doc_practiceid = 0;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MDE0NTI2ODQsImlzcyI6IlRlc3QuY29tIiwiYXVkIjoiVGVzdC5jb20ifQ.uQGmxaAJ3kSEycD0xSPhbQUXI2vEQBeOJJ3XuTB-zZk");
                    var response = await client.GetAsync("https://localhost:44385/api/User/SelectUserbyId?userId=" + interactionInfo.HCPId);
                    reshcp = response.StatusCode.ToString();
                    dobj = JsonConvert.DeserializeObject<UserInfo>(await response.Content.ReadAsStringAsync());
                    doc_practiceid = pobj.PracticeID;

                }

                if ( resdiagnosis == null && pat_practiceid== doc_practiceid)
                {

                    interactionInfo.InsertedDate = DateTime.Now;
                    interactionInfo.UpdatedDate = DateTime.Now;
                    interactionInfo.VisitDate = DateTime.Now;
                    _hCPManagementDBContext.HCPInteractionInfo.AddRange(interactionInfo);
                    //HCPInteractionInfo diaginfo = await _hCPManagementDBContext.HCPInteractionInfo.AsNoTracking().Where(x => x.PatientVisitId != 0).FirstOrDefaultAsync();                                  
                    await _hCPManagementDBContext.SaveChangesAsync();
                    return "Patient Diagnosis Details updated successfully";
                }
                return "Add Valid Diagnosis Details";
                
            }
            catch (Exception ex)
            {

                Logger.Addlog(ex, "HCP");
                return "Error occurred in adding diagnosis details";
            }
        }

        //public async Task<string> AddHCP(HCPInfo newHcp)
        //{
        //    try
        //    {
        //        HCPInfo respatient = await _hCPManagementDBContext.HCPInfo.AsNoTracking().Where(x => x.HCPId == newHcp.HCPId || x.HCPName == newHcp.HCPName || x.EmailId == newHcp.EmailId).FirstOrDefaultAsync();
        //        string res = "";
        //        using (HttpClient client = new HttpClient())
        //        {

        //            var response = await client.GetAsync("https://localhost:44335/api/PracticeManagement/View PracticeById?practiceId=" + newHcp.PracticeID);
        //            res = response.StatusCode.ToString();


        //        }

        //        if (respatient == null  && res.Contains("OK"))
        //        {
        //            newHcp.InsertedDate = DateTime.Now;
        //            newHcp.UpdatedDate = DateTime.Now;
        //            newHcp.ActiveFlg = 1;
        //            _hCPManagementDBContext.HCPInfo.AddRange(newHcp);
        //            await _hCPManagementDBContext.SaveChangesAsync();
        //            return "HCP Details added successfully";
        //        }
        //        return "Enter valid details ";


        //    }

        //    catch (Exception ex)
        //    {

        //        Logger.Addlog(ex, "HCP");
        //        return "Unable to add HCP Details";
        //    }
        //}

        //public async Task<string> ModifyHCP(HCPInfo newHcp)
        //{
        //    try
        //    {
        //        HCPInfo respatient = await _hCPManagementDBContext.HCPInfo.AsNoTracking().Where(x => x.HCPId == newHcp.HCPId).FirstOrDefaultAsync();
        //        string res = "";
        //        using (HttpClient client = new HttpClient())
        //        {

        //            var response = await client.GetAsync("https://localhost:44335/api/PracticeManagement/View PracticeById?practiceId=" + newHcp.PracticeID);
        //            res = response.StatusCode.ToString();


        //        }

        //        if (respatient != null && respatient.ActiveFlg == 1 && res.Contains("OK"))
        //        {
        //            newHcp.InsertedDate = DateTime.Now;
        //            newHcp.UpdatedDate = DateTime.Now;
        //            newHcp.ActiveFlg = 1;
        //            _hCPManagementDBContext.HCPInfo.Update(newHcp);
        //            await _hCPManagementDBContext.SaveChangesAsync();
        //            return "HCP Details modified successfully";
        //        }
        //        return "Enter valid details ";


        //    }

        //    catch (Exception ex)
        //    {

        //        Logger.Addlog(ex, "HCP");
        //        return "Unable to modify HCP Details";
        //    }
        //}

        public async Task<string> AddDocSignature(DocumentInfo docinfo)
        {

            try
            {
                HCPInteractionInfo reshcp = await _hCPManagementDBContext.HCPInteractionInfo.AsNoTracking().Where(x=>x.HCPInteractionId == docinfo.DiagnosisId).FirstOrDefaultAsync();
                DocumentInfo resdoc = await _hCPManagementDBContext.DocumentInfo.AsNoTracking().Where(x=>x.DiagnosisId == docinfo.DiagnosisId && x.HCPSignature== docinfo.HCPSignature).FirstOrDefaultAsync();

                if (reshcp != null && resdoc == null)
                {
                    docinfo.InsertedDate = DateTime.Now;
                    docinfo.UpdatedDate = DateTime.Now;
                    _hCPManagementDBContext.DocumentInfo.AddRange(docinfo);
                    
                    await _hCPManagementDBContext.SaveChangesAsync();
                    return "Signature addedd successfully";
                }
                return "Add Valid Details";

            }
            catch (Exception ex)
            {

                Logger.Addlog(ex, "HCP");
                return "Error occurred in adding Signature";
            }
        }

        public async Task<string> DeleteDocument(int PatientId)
        {
            try
            {
                var respatient = _hCPManagementDBContext.HCPInteractionInfo.FirstOrDefault(x => x.PatientId == PatientId);
                if (respatient != null)
                {
                    var resdiagnosis = _hCPManagementDBContext.DocumentInfo.FirstOrDefault(x => x.DiagnosisId == respatient.HCPInteractionId);
                    _hCPManagementDBContext.DocumentInfo.Remove(resdiagnosis);
                    _hCPManagementDBContext.HCPInteractionInfo.Remove(respatient);
                    await _hCPManagementDBContext.SaveChangesAsync();
                    return "Report Deleted Successfully";
                }

                return "Not deleted";
            }

            catch (Exception ex)
            {
                Logger.Addlog(ex, "HCP");
                return "Error Occurred in Deletion";
            }
        }

        //public async Task<string> GenerateReport(int PatientId)
        //{
        //    try
        //    {
        //        var resdiagnosis = _hCPManagementDBContext.HCPInteractionInfo.FirstOrDefault(x => x.PatientId == PatientId);
        //        string practres = "";
        //        using (HttpClient client = new HttpClient())
        //        {

        //            var response = await client.GetAsync("https://localhost:44335/api/PracticeManagement/ViewPracticeById?practiceId=" + resdiagnosis.PracticeId);
        //            practres = response.StatusCode.ToString();


        //        }
        //        string patres = "";
        //        using (HttpClient client = new HttpClient())
        //        {

        //            var response = await client.GetAsync("https://localhost:44325/api/PatientManagement/SelectPatientById?PatientId=" + resdiagnosis.PatientId);
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer","eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MDEwMjI0MjMsImlzcyI6IlRlc3QuY29tIiwiYXVkIjoiVGVzdC5jb20ifQ.KG9tw2CxxmGmPQ6n41XeMhTRmDGqjxUnKc9UM0o - 3bE");
        //            patres = response.StatusCode.ToString();


        //        }
        //        if (resdiagnosis != null && practres.Contains("OK") && patres.Contains("OK"))
        //        {
        //            var resdoc = _hCPManagementDBContext.DocumentInfo.FirstOrDefault(x => x.DiagnosisId == resdiagnosis.HCPInteractionId);
        //            Report.GeneratePDF(resdiagnosis, resdoc);
        //            await _hCPManagementDBContext.SaveChangesAsync();
        //            return "Report generated Successfully";
        //        }

        //        return "Report not generated";
        //    }
        //    catch (Exception ex)
        //    {

        //        Logger.Addlog(ex, "HCP");
        //        return "Error occurred in Report Generation";
        //    }
        //}
        //public async Task<HCPInteractionInfo> GetHistory(int PatientId)
        //{

        //    try
        //    {
        //        var resdiagnosis = await _hCPManagementDBContext.HCPInteractionInfo.FirstOrDefaultAsync(x => x.PatientId == PatientId);
                
        //            return resdiagnosis;
                

               
        //    }
        //    catch (Exception ex)
        //    {

        //        Logger.Addlog(ex, "HCP");
        //        throw ex;
        //    }
        //}




    }
}
