using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Core
{
   public interface IPracticeManagementRepository
    {
        Task<string> ModifyPractice(PracticeInfo practice);
        Task<List<PracticeInfo>> ViewAllPractice();
        Task<PracticeInfo> ViewPracticeById(int practiceId);    
        Task<string> AddPractice(PracticeInfo practice);
        Task<string> DeletePractice(int practiceId);
        
    }
}
