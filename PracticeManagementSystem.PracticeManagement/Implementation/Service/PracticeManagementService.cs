using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PracticeManagementSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeManagementSystem.PracticeManagement
{
    public class PracticeManagementService : IPracticeManagementService
    {
        private readonly IPracticeManagementRepository _practiceManagementRepository;
        
        public PracticeManagementService(IPracticeManagementRepository practiceManagementRepository)
        {
            _practiceManagementRepository = practiceManagementRepository;
        }
        public async Task<string> ModifyPractice(PracticeInfo practiceInfo)
        {
            return await _practiceManagementRepository.ModifyPractice(practiceInfo);
        }
        public async Task<List<PracticeInfo>> ViewAllPractice()
        {
            return await _practiceManagementRepository.ViewAllPractice();
        }

        public async Task<PracticeInfo> ViewPracticeById(int practiceId)
        {
            return await _practiceManagementRepository.ViewPracticeById(practiceId);
        }


        public async Task<string> AddPractice(PracticeInfo practiceInfo)
        {
            return await _practiceManagementRepository.AddPractice(practiceInfo);
        }

        public async Task<string> DeletePractice(int practiceId)
        {
            return await _practiceManagementRepository.DeletePractice(practiceId);
        }
    }
    
}
