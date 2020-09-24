using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PracticeManagementSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PracticeManagementSystem.PracticeManagement
{
    public class PracticeManagementRepository: IPracticeManagementRepository
    {
        private readonly PracticeMgmntDBContext _practiceMgmntDBContext;

        public PracticeManagementRepository(PracticeMgmntDBContext practiceMgmntDBContext)
        {
            _practiceMgmntDBContext = practiceMgmntDBContext;
        }
        public async Task<string> ModifyPractice(PracticeInfo practiceInfo)
        {
            try
            {
                PracticeInfo respractice = await _practiceMgmntDBContext.PracticeInfo.AsNoTracking().Where(x => x.PracticeId == practiceInfo.PracticeId || x.PracticeName == practiceInfo.PracticeName).FirstOrDefaultAsync();
                PracticeInfo existingInfo = await _practiceMgmntDBContext.PracticeInfo.AsNoTracking().Where(x => x.PracticeId != practiceInfo.PracticeId).FirstOrDefaultAsync();
                
                if (respractice != null && respractice.ActiveFlg == 1 && Regex.IsMatch(practiceInfo.GST, "^[a-zA-Z][a-zA-Z0-9]*$") && practiceInfo.GST.Length == 15)
                {
                    if (existingInfo.PracticeName != practiceInfo.PracticeName)
                    {
                        practiceInfo.CreatedDate = respractice.CreatedDate;
                        practiceInfo.ModifiedDate = DateTime.Now;
                        practiceInfo.ActiveFlg = respractice.ActiveFlg;
                        _practiceMgmntDBContext.PracticeInfo.UpdateRange(practiceInfo);
                        await _practiceMgmntDBContext.SaveChangesAsync();
                        //UserInfo user = new UserInfo();
                        //user.UserId = respractice.UserId;
                        //user.UserName = practiceInfo.PracticeName;
                        //user.EmailId = practiceInfo.EmailId;
                        //user.Location = practiceInfo.LocationDetails;
                        //user.GST = practiceInfo.GST;
                        //var json = JsonConvert.SerializeObject(user);
                        //ApiCall newCall = new ApiCall();
                        //newCall.HttpPostToApi(json, "https://localhost:44385/api/User/EditUser");
                        return "Practice modified successfully";
                    }
                    else
                    {
                        return "Practice Name already exists";
                    }
                }

                return "Unable to Modify:Enter valid details";
            }

            catch (Exception ex)
            {

                Logger.Addlog(ex, "Practice");
                return "Error occurred while modifying Practice Details";
            }
        }
        public async Task<List<PracticeInfo>> ViewAllPractice()
        {
            try
            {
                List<PracticeInfo> respratices = await _practiceMgmntDBContext.PracticeInfo.AsNoTracking().Select(x=>x).ToListAsync();
                return respratices;
            }

            catch (Exception ex)
            {
                Logger.Addlog(ex, "Practice");
                throw ex;
            }
        }
        public async Task<PracticeInfo> ViewPracticeById(int practiceId)
        {
            try
            {
                PracticeInfo respractice= await _practiceMgmntDBContext.PracticeInfo.AsNoTracking().Where(x => x.PracticeId == practiceId).FirstOrDefaultAsync();
                
                  return respractice;
                
            }

            catch (Exception ex)
            {
                Logger.Addlog(ex, "Practice");
                throw ex;

            }
        }
        public async Task<string> AddPractice(PracticeInfo practiceInfo)
        {
            try
            {
                PracticeInfo respractice = await _practiceMgmntDBContext.PracticeInfo.AsNoTracking().Where(x => x.PracticeId == practiceInfo.PracticeId || x.PracticeName == practiceInfo.PracticeName || x.EmailId == practiceInfo.EmailId || x.GST==practiceInfo.GST).FirstOrDefaultAsync();
                if (respractice == null && Regex.IsMatch(practiceInfo.GST, "^[a-zA-Z][a-zA-Z0-9]*$") && practiceInfo.GST.Length == 15)
                {  
                    practiceInfo.CreatedDate = DateTime.Now;
                    practiceInfo.ModifiedDate = DateTime.Now;
                    practiceInfo.ActiveFlg = 1;
                    practiceInfo.CreatedBy = practiceInfo.PracticeId;
                    _practiceMgmntDBContext.PracticeInfo.AddRange(practiceInfo);
                    await _practiceMgmntDBContext.SaveChangesAsync();
                    return "Practice added successfully";
                }
                return "Unable to add practice:Enter valid details";
                
            }

            catch (Exception ex)
            {

                Logger.Addlog(ex, "Practice");
                return "Unable to add Practice";
            }
        }

        public async Task<string> DeletePractice(int practiceId)
        {
            try
            {
                PracticeInfo respractice = _practiceMgmntDBContext.PracticeInfo.FirstOrDefault(x => x.PracticeId == practiceId);
                if (respractice != null)
                {

                    _practiceMgmntDBContext.PracticeInfo.Remove(respractice);
                    await _practiceMgmntDBContext.SaveChangesAsync();
                    return "Practice deleted successfully";
                }

                return "The given practice does not exists";
            }

            catch (Exception ex)
            {

                Logger.Addlog(ex, "Practice");
                return "Unable to delete Practice";
            }
        }

    }
}
