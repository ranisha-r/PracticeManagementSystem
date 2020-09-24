using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PracticeManagementSystem.Core;

namespace PracticeManagementSystem.RegistrationLogin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManagementService _userManagementservice;
        
        public UserController(IUserManagementService userManagementservice)
        {           
            _userManagementservice = userManagementservice;
        }

        [HttpPost("Login")]
        public async Task<string> Login(Login loginfo) => await _userManagementservice.Login(loginfo);

        [HttpPost("Registration")]
        public async Task<string> Registration(UserInfo userInfo) => await _userManagementservice.Registration(userInfo);     

        [HttpPut("EditUser")]
        [Authorize]
        public async Task<string> EditUser(UserInfo userInfo) => await _userManagementservice.EditUser(userInfo);

        [HttpDelete("DeleteUser")]
        [Authorize]
        public async Task<string> DeleteUser(int userId) => await _userManagementservice.DeleteUser(userId);

        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<string> ChangePassword(UserInfo userInfo) => await _userManagementservice.ChangePassword(userInfo);

        [HttpGet("SelectUserbyId")]
        [Authorize]
        public async Task<UserInfo> SelectUserbyId(int userId) => await _userManagementservice.SelectUserbyId(userId);

        [HttpGet("ViewHistory")]
        [Authorize]
        public async Task<UserHistory> ViewHistory(int userId) => await _userManagementservice.ViewHistory(userId);

        [HttpGet("SelectAllUser")]
        [Authorize]
        public async Task<List<UserInfo>> SelectAllUser()
        {
            //var x = HttpContext.Request.Query["Roleid"];
            return await _userManagementservice.SelectAllUser();
        }

        [HttpPut("DeactivateUser")]
        [Authorize]
        public async Task<string> DeactivateUser(int userId) => await _userManagementservice.DeactivateUser(userId);

    }
}
