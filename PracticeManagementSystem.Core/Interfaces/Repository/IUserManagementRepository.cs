using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Core
{
    public interface IUserManagementRepository
    {
        Task<string> Registration(UserInfo userInfo);
        Task<string> Login(Login logininfo);
        Task<string> EditUser(UserInfo userInfo);
        Task<string> DeleteUser(int userId);
        Task<string> ChangePassword(UserInfo userInfo);
        Task<UserInfo> SelectUserbyId(int userId);
        Task<UserHistory> ViewHistory(int userId);
        Task<List<UserInfo>> SelectAllUser();
        Task<string> DeactivateUser(int userId);

        
    }
}
