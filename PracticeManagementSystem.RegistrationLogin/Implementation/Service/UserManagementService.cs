using PracticeManagementSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PracticeManagementSystem.RegistrationLogin
{
    public class UserManagementService:IUserManagementService
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public UserManagementService(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<string> ChangePassword(UserInfo userInfo)
        {
            return await _userManagementRepository.ChangePassword(userInfo);
        }

        public async Task<string> DeactivateUser(int userId)
        {
            return await _userManagementRepository.DeactivateUser(userId);
        }

        public async Task<string> DeleteUser(int userId)
        {
            return await _userManagementRepository.DeleteUser(userId);
        }

        public async Task<string> EditUser(UserInfo userInfo)
        {
            return await _userManagementRepository.EditUser(userInfo);
        }

        public async Task<string> Login(Login loginfo)
        {
            return await _userManagementRepository.Login(loginfo);
        }

        public  async Task<string> Registration(UserInfo userInfo)
        {
            return await _userManagementRepository.Registration(userInfo);
        }

        public async Task<List<UserInfo>> SelectAllUser()
        {
            return await _userManagementRepository.SelectAllUser();
        }

        public async Task<UserInfo> SelectUserbyId(int userId)
        {
            return await _userManagementRepository.SelectUserbyId(userId);
        }

        public async Task<UserHistory> ViewHistory(int userId)
        {
            return await _userManagementRepository.ViewHistory(userId);
        }
    }
}
