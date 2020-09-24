using PracticeManagementSystem.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace PracticeManagementSystem.PatientManagement
{
    public class PatientManagementRepository:IPatientManagementRepository
    {
        private readonly PatientManagementDBContext _patientManagementDBContext;
        private IConfiguration _config;
        public PatientManagementRepository(PatientManagementDBContext patientManagementDBContext, IConfiguration config)
        {
            _patientManagementDBContext = patientManagementDBContext;
            _config = config;
        }
      

        public async Task<string> Login(Login logininfo)
        {
            try
            {

                PatientInfo reslogin = await _patientManagementDBContext.PatientInfo.AsNoTracking().Where(x => x.PatientName == logininfo.UserName).FirstOrDefaultAsync();

                string textpassword = DecryptPassword(reslogin.Password, "E546C8DF278CD5931069B522E695D4F2");

                if (reslogin != null && logininfo.Password == textpassword && reslogin.ActiveFlg != 0)
                {

                    string tokenString = GenerateJSONWebToken();
                    if (tokenString != null)
                    {
                        return "Patient Logged in successfully Generated Token :" + tokenString;
                    }
                    return "Unable to Login";
                }
                else
                {
                    return "Enter valid credentials";
                }
            }
            catch (Exception ex)
            {

                Logger.Addlog(ex, "User");
                return "Unable to Login";

            }
        }

        //Generate Token
        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(60),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> AddPatient(PatientInfo patientInfo)
        {
            try
            {
                PatientInfo respatient = await _patientManagementDBContext.PatientInfo.AsNoTracking().Where(x => x.PatientId == patientInfo.PatientId || x.PatientName == patientInfo.PatientName || x.EmailId==patientInfo.EmailId).FirstOrDefaultAsync();
                string res = "";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MDE0NTI2ODQsImlzcyI6IlRlc3QuY29tIiwiYXVkIjoiVGVzdC5jb20ifQ.uQGmxaAJ3kSEycD0xSPhbQUXI2vEQBeOJJ3XuTB-zZk");
                    var response = await client.GetAsync("https://localhost:44335/api/PracticeManagement/ViewPracticeById?practiceId=" + patientInfo.PracticeID);
                    res = response.StatusCode.ToString();


                }
                if (respatient == null && res.Contains("OK"))
                {
                    string encryptedpassword = EncryptPassword(patientInfo.Password, "E546C8DF278CD5931069B522E695D4F2");
                    patientInfo.Password = encryptedpassword;
                    patientInfo.CreatedDate = DateTime.Now;
                    patientInfo.ModifiedDate = DateTime.Now;
                    patientInfo.ActiveFlg = 1;                
                    _patientManagementDBContext.PatientInfo.AddRange(patientInfo);
                    await _patientManagementDBContext.SaveChangesAsync();
                    return "Patient added successfully";

                }
                return "Enter valid details";
            }

            catch (Exception ex)
            {
                Logger.Addlog(ex, "Patient");
                return "Unable to add Patient";
            }
        }

        //Encrypt password
        public string EncryptPassword(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        //Decrpt password
        public string DecryptPassword(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        public async Task<string> DeletePatient(int patientID)
        {
            try
            {
                var respatient = _patientManagementDBContext.PatientInfo.FirstOrDefault(x => x.PatientId == patientID);
                if (respatient != null)
                {


                    _patientManagementDBContext.PatientInfo.Remove(respatient);
                    await _patientManagementDBContext.SaveChangesAsync();
                    return "Patient deleted successfully";
                }

                return "Unable to delete patient";
            }

            catch (Exception ex)
            {

                Logger.Addlog(ex, "Patient");
                return "Error occurred while trying to delete patient";
            }
        }

        public async Task<string> ModifyPatient(PatientInfo patientInfo)
        {

            try
            {
                PatientInfo respatient = await _patientManagementDBContext.PatientInfo.AsNoTracking().Where(x => x.PatientId == patientInfo.PatientId).FirstOrDefaultAsync();
                PatientInfo existingpatient =await _patientManagementDBContext.PatientInfo.AsNoTracking().Where(x => x.PatientId != patientInfo.PatientId).FirstOrDefaultAsync();
                string res = "";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MDE0NTI2ODQsImlzcyI6IlRlc3QuY29tIiwiYXVkIjoiVGVzdC5jb20ifQ.uQGmxaAJ3kSEycD0xSPhbQUXI2vEQBeOJJ3XuTB-zZk");
                    var response = await client.GetAsync("https://localhost:44335/api/PracticeManagement/ViewPracticeById?practiceId=" + patientInfo.PracticeID);
                    res = response.StatusCode.ToString();


                }

                if (respatient != null && respatient.ActiveFlg==1 && res.Contains("OK") && patientInfo.PatientName!= existingpatient.PatientName && patientInfo.EmailId!= existingpatient.EmailId)
                {
                    patientInfo.CreatedDate = respatient.CreatedDate;
                    patientInfo.ModifiedDate = DateTime.Now;
                    patientInfo.ActiveFlg = respatient.ActiveFlg;
                    string encryptedpassword = EncryptPassword(patientInfo.Password, "E546C8DF278CD5931069B522E695D4F2");
                    patientInfo.Password = encryptedpassword;
                    _patientManagementDBContext.PatientInfo.UpdateRange(patientInfo);
                    await _patientManagementDBContext.SaveChangesAsync();
                    return "Patient modified successfully";
                }

                return "Enter valid details";
            }

            catch (Exception ex)
            {

                Logger.Addlog(ex, "Patient");
                return "Unable to add Patient";
            }
        }

        public async Task<List<PatientInfo>> SelectAllpatient()
        {
            try
            {
                List<PatientInfo> respatients = await _patientManagementDBContext.PatientInfo.AsNoTracking().Select(x => x).ToListAsync();
                respatients.ForEach(x => x.Password = "xxxxxxx");
                return respatients;
            }

            catch (Exception ex)
            {

                Logger.Addlog(ex, "Patient");
                throw ex;
            }
        }

        public async Task<PatientInfo> SelectPatientById(int patientID)
        {

            try
            {
                PatientInfo respatient= await _patientManagementDBContext.PatientInfo.AsNoTracking().Where(x => x.PatientId == patientID).FirstOrDefaultAsync();
                respatient.Password = "xxxxxxx";
                return respatient;
            }

            catch (Exception ex)
            {

                Logger.Addlog(ex, "Patient");
                throw ex;
            }
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
