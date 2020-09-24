using PracticeManagementSystem.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Security.Claims;

namespace PracticeManagementSystem.RegistrationLogin
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly RegistrationLoginDBContext _registrationLoginDBContext;
        private IConfiguration _config;
        public UserManagementRepository(RegistrationLoginDBContext registrationLoginDBContext, IConfiguration config)
        {
            _registrationLoginDBContext = registrationLoginDBContext;
            _config = config;
        }


        public async Task<string> ChangePassword(UserInfo userInfo)
        {
            try
            {
                UserInfo resuserInfo = await _registrationLoginDBContext.UserInfo.AsNoTracking().Where(x => x.UserId == userInfo.UserId && x.UserName == userInfo.UserName).FirstOrDefaultAsync();
                string textpassword = DecryptPassword(resuserInfo.Password, "E546C8DF278CD5931069B522E695D4F2");
                if (resuserInfo != null && resuserInfo.ActiveFlg == 1)
                {
                    if (textpassword != userInfo.Password)
                    {
                        string encryptedpassword = EncryptPassword(userInfo.Password, "E546C8DF278CD5931069B522E695D4F2");
                        resuserInfo.Password = encryptedpassword;

                    }
                    else if(resuserInfo.ActiveFlg == 0)
                    {
                        return "Cannot update password for deactivated user";
                    }
                    else if (textpassword == userInfo.Password)
                    {
                        return "Please enter new password";
                    }                  
                    _registrationLoginDBContext.UserInfo.UpdateRange(resuserInfo);
                    await _registrationLoginDBContext.SaveChangesAsync();
                    return "Password updated successfully";
                }

                return "Unable to update password";
            }

            catch (Exception ex)
            {

                Logger.Addlog(ex, "User");
                return "Unable to update password";
            }
        }

        public async Task<string> DeactivateUser(int userId)
        {
            try
            {
                UserInfo resuserInfo = await _registrationLoginDBContext.UserInfo.AsNoTracking().Where(x => x.UserId == userId).FirstOrDefaultAsync();
                if (resuserInfo != null && resuserInfo.ActiveFlg == 1)
                {
                    resuserInfo.ActiveFlg = 0;
                    _registrationLoginDBContext.UserInfo.UpdateRange(resuserInfo);
                    await _registrationLoginDBContext.SaveChangesAsync();
                    return "User deactivated successfully";
                }

                return "Enter valid details";
            }

            catch (Exception ex)
            {

                Logger.Addlog(ex, "User");
                return "Unable to Deactivate User";



            }
        }

        public async Task<string> DeleteUser(int userId)
        {
            try
            {
                UserInfo resuserInfo = await _registrationLoginDBContext.UserInfo.AsNoTracking().Where(x => x.UserId == userId).FirstOrDefaultAsync();
                if (resuserInfo != null)
                {
                    _registrationLoginDBContext.UserInfo.RemoveRange(resuserInfo);
                    await _registrationLoginDBContext.SaveChangesAsync();
                    return "User deleted successfully";
                }

                return "User not found";
            }

            catch (Exception ex)
            {

                Logger.Addlog(ex, "User");
                return "Unable to Delete User";
            }
        }

        public async Task<string> EditUser(UserInfo userInfo)
        {
            try
            {
                UserInfo resuserInfo = await _registrationLoginDBContext.UserInfo.AsNoTracking().Where(x => x.UserId == userInfo.UserId).FirstOrDefaultAsync();
                string textpassword = DecryptPassword(resuserInfo.Password, "E546C8DF278CD5931069B522E695D4F2");
                UserInfo existingInfo=await _registrationLoginDBContext.UserInfo.AsNoTracking().Where(x => x.UserId != userInfo.UserId).FirstOrDefaultAsync();
                    if (resuserInfo != null)
                {
                    if (resuserInfo.ActiveFlg == 1 )
                    {
                        
                        if (resuserInfo.UserName != userInfo.UserName || resuserInfo.EmailId != userInfo.EmailId || resuserInfo.Location != userInfo.Location || textpassword != userInfo.Password)
                        {
                            if (existingInfo.UserName != userInfo.UserName)
                            {
                                userInfo.CreatedDate = resuserInfo.CreatedDate;
                                userInfo.UpdatedDate = DateTime.Now;
                                userInfo.RoleId = resuserInfo.RoleId;
                                userInfo.ActiveFlg = resuserInfo.ActiveFlg;
                                string encryptedpassword = EncryptPassword(userInfo.Password, "E546C8DF278CD5931069B522E695D4F2");
                                userInfo.Password = encryptedpassword;
                                _registrationLoginDBContext.UserInfo.UpdateRange(userInfo);
                                await _registrationLoginDBContext.SaveChangesAsync();
                                return "User modified successfully";
                            }
                            else
                            {
                                return "User name already exists";
                            }
                        }
                        if (resuserInfo.UserName == userInfo.UserName && resuserInfo.EmailId == userInfo.EmailId && resuserInfo.Location == userInfo.Location && resuserInfo.Password == userInfo.Password)
                        {
                            return "No details modified";
                        }

                    }
                    else
                    {
                        return "User details cannot  be modified for deactivated user";
                    }

                    //if (userInfo.RoleId == 2)
                    //{
                    //    PracticeInfo practice = new PracticeInfo();
                    //    practice.PracticeName = userInfo.UserName;
                    //    practice.EmailId = userInfo.EmailId;
                    //    practice.UserId = userInfo.UserId;
                    //    practice.LocationDetails = userInfo.Location;
                    //    practice.GST = userInfo.GST;
                    //    var json = JsonConvert.SerializeObject(practice);
                    //    ApiCall newCall = new ApiCall();
                    //    newCall.HttpPostToApi(json, "https://localhost:44335/api/PracticeManagement/Add Practice");
                    //}
                    //if (userInfo.RoleId == 4)
                    //{
                    //    PatientInfo patient = new PatientInfo();
                    //    patient.PatientName = userInfo.UserName;
                    //    patient.EmailId = userInfo.EmailId;
                    //    patient.UserId = userInfo.UserId;
                    //    patient.PracticeID = userInfo.PracticeId;
                    //    patient.Address = userInfo.Location;
                    //    var json = JsonConvert.SerializeObject(patient);
                    //    ApiCall newCall = new ApiCall();
                    //    newCall.HttpPostToApi(json, "https://localhost:44325/api/PatientManagement/AddPatient");
                    //}
                    //if (userInfo.RoleId == 3)
                    //{
                    //    HCPInfo hCP = new HCPInfo();
                    //    hCP.HCPName = userInfo.UserName;
                    //    hCP.EmailId = userInfo.EmailId;
                    //    hCP.UserId = userInfo.UserId;
                    //    hCP.PracticeID = userInfo.PracticeId;
                    //    hCP.Address = userInfo.Location;
                    //    var json = JsonConvert.SerializeObject(hCP);
                    //    ApiCall newCall = new ApiCall();
                    //    newCall.HttpPostToApi(json, "https://localhost:44318/api/HCPManagement/Add HCP");
                    //}
                    

                }


                return "Unable to modify user details";



            }

            catch (Exception ex)
            {

                Logger.Addlog(ex, "User");
                return "Unable to Edit User";

            }
        }

        public async Task<string> Login(Login logininfo)
        {
            try
            {

                UserInfo reslogin = await _registrationLoginDBContext.UserInfo.AsNoTracking().Where(x => x.UserName == logininfo.UserName).FirstOrDefaultAsync();

                string textpassword = DecryptPassword(reslogin.Password, "E546C8DF278CD5931069B522E695D4F2");

                if (reslogin != null && logininfo.Password == textpassword && reslogin.ActiveFlg != 0)
                {

                    string tokenString = GenerateJSONWebToken();
                    if (tokenString != null)
                    {
                        return "User Logged in successfully. Generated Token :" + tokenString;
                    }
                    return "No Token Generated";
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
            //var authclaims = new[]
            //{
            //new Claim("UserId","1"),
            //new Claim("RoleId", "1")
            //};
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              /*claims: authclaims*/null,
              expires: DateTime.Now.AddMinutes(8640),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<string> Registration(UserInfo userInfo)
        {
            try
            {
                UserInfo resreg = await _registrationLoginDBContext.UserInfo.AsNoTracking().Where(x => x.UserName == userInfo.UserName || x.UserId == userInfo.UserId || x.EmailId == userInfo.EmailId ).FirstOrDefaultAsync();

                if (resreg == null)
                {
                    string encryptedpassword = EncryptPassword(userInfo.Password, "E546C8DF278CD5931069B522E695D4F2");
                    userInfo.Password = encryptedpassword;
                    userInfo.CreatedDate = DateTime.Now;
                    userInfo.UpdatedDate = DateTime.Now;
                    userInfo.ActiveFlg = 1;                   
                        string caseRole = userInfo.UserType;
                    switch (caseRole)
                    {
                        //Master Admin
                        case "MasterAdmin":
                            userInfo.RoleId = 1;
                            _registrationLoginDBContext.UserInfo.Add(userInfo);
                            await _registrationLoginDBContext.SaveChangesAsync();
                            return "MasterAdmin Registered successfully";
                         //break;

                        //Practice
                        case "PracticeAdmin":
                            {
                                userInfo.RoleId = 2;

                                string res = "";
                                using (HttpClient client = new HttpClient())
                                {
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MDE0NTI2ODQsImlzcyI6IlRlc3QuY29tIiwiYXVkIjoiVGVzdC5jb20ifQ.uQGmxaAJ3kSEycD0xSPhbQUXI2vEQBeOJJ3XuTB-zZk");
                                    var response = await client.GetAsync("https://localhost:44335/api/PracticeManagement/ViewPracticeById?practiceId=" + userInfo.PracticeId);
                                    res = response.StatusCode.ToString();


                                }

                                if (res.Contains("OK"))
                                {
                                    _registrationLoginDBContext.UserInfo.Add(userInfo);
                                    await _registrationLoginDBContext.SaveChangesAsync();
                                    return "PracticeAdmin Registered successfully";

                                }
                                break;
                            }
                        //HCP
                        case "HCP":
                            {
                                userInfo.RoleId = 3;
                                string res = "";
                                using (HttpClient client = new HttpClient())
                                {
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MDE0NTI2ODQsImlzcyI6IlRlc3QuY29tIiwiYXVkIjoiVGVzdC5jb20ifQ.uQGmxaAJ3kSEycD0xSPhbQUXI2vEQBeOJJ3XuTB-zZk");
                                    var response = await client.GetAsync("https://localhost:44335/api/PracticeManagement/ViewPracticeById?practiceId=" + userInfo.PracticeId);
                                    res = response.StatusCode.ToString();


                                }

                                if (res.Contains("OK"))
                                {
                                    _registrationLoginDBContext.UserInfo.Add(userInfo);
                                    await _registrationLoginDBContext.SaveChangesAsync();
                                    return "HCP Registered successfully";

                                }
                                break;
                            }
                            }
                        
                        //_registrationLoginDBContext.UserInfo.Add(userInfo);                   
                        //await _registrationLoginDBContext.SaveChangesAsync();
                        //if (userInfo.RoleId == 2)
                        //{
                        //    PracticeInfo practice = new PracticeInfo();
                        //    practice.PracticeName = userInfo.UserName;
                        //    practice.EmailId = userInfo.EmailId;
                        //    practice.UserId = userInfo.UserId;
                        //    practice.LocationDetails = userInfo.Location;
                        //    practice.GST = userInfo.GST;
                        //    var json = JsonConvert.SerializeObject(practice);
                        //    ApiCall newCall = new ApiCall();
                        //    newCall.HttpPostToApi(json, "https://localhost:44335/api/PracticeManagement/Add Practice");
                        //}
                        //if (userInfo.RoleId == 4)
                        //{
                        //    PatientInfo patient = new PatientInfo();
                        //    patient.PatientName = userInfo.UserName;
                        //    patient.EmailId = userInfo.EmailId;
                        //    patient.UserId = userInfo.UserId;
                        //    patient.PracticeID = userInfo.PracticeId;
                        //    patient.Address = userInfo.Location;                           
                        //    var json = JsonConvert.SerializeObject(patient);
                        //    ApiCall newCall = new ApiCall();
                        //    newCall.HttpPostToApi(json, "https://localhost:44325/api/PatientManagement/AddPatient");
                        //}
                        //if (userInfo.RoleId == 3)
                        //{
                        //    HCPInfo hCP = new HCPInfo();
                        //    hCP.HCPName = userInfo.UserName;
                        //    hCP.EmailId= userInfo.EmailId;
                        //    hCP.UserId = userInfo.UserId;
                        //    hCP.PracticeID = userInfo.PracticeId;
                        //    hCP.Address = userInfo.Location;
                        //    var json = JsonConvert.SerializeObject(hCP);
                        //    ApiCall newCall = new ApiCall();
                        //    newCall.HttpPostToApi(json, "https://localhost:44318/api/HCPManagement/Add HCP");
                        //}

                       
                }
                return "Enter valid details";
            }
            catch (Exception ex)
            {

                Logger.Addlog(ex, "User");
                return "Unable to Register User";

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

        public async Task<List<UserInfo>> SelectAllUser()
        {
            try
            {
                List<UserInfo> respratices = await _registrationLoginDBContext.UserInfo.AsNoTracking().Select(x => x).ToListAsync();
                respratices.ForEach(x => x.Password = "xxxxxxx");

                return respratices;
            }

            catch (Exception ex)
            {
               
                Logger.Addlog(ex, "User");
                throw ex;

            }
        }

        public async Task<UserInfo> SelectUserbyId(int userId)
        {
            try
            {

                UserInfo resuser=await _registrationLoginDBContext.UserInfo.AsNoTracking().Where(x => x.UserId == userId).FirstOrDefaultAsync();
                if (resuser != null)
                {
                    resuser.Password = "xxxxxxx";
                    return resuser;
                }
                return resuser;

            }

            catch (Exception ex)
            {

                Logger.Addlog(ex, "User");
                throw ex;
            }
        }

        public async Task<UserHistory> ViewHistory(int userId)
        {
            try
            {
                UserInfo respractices = await _registrationLoginDBContext.UserInfo.AsNoTracking().Where(x => x.UserId == userId).FirstOrDefaultAsync();
                UserHistory usrhstry = new UserHistory();
                if (respractices != null)
                {
                    
                    usrhstry.UserId = respractices.UserId;
                    usrhstry.UserName = respractices.UserName;
                    usrhstry.Password = "xxxxxxx";
                    usrhstry.Location = respractices.Location;
                    usrhstry.EmailId = respractices.EmailId;
                    usrhstry.UserType = respractices.UserType;
                    if (respractices.ActiveFlg == 1)
                    {
                        usrhstry.ActiveUser = true;
                    }
                    else
                    {
                        usrhstry.ActiveUser = false;
                    }
                    usrhstry.LastLogin = respractices.UpdatedDate;
                    return usrhstry;
                }
                else
                {
                    usrhstry.UserType = "False";
                    usrhstry.UserType = "User does not exist";
                    return usrhstry;

                }
            }
            catch (Exception ex)
            {

                Logger.Addlog(ex, "User");
                throw ex;
            }
        }


    }
}
