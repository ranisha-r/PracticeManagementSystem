using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PracticeManagementSystem.Core
{
    public class GenerateToken
    {
        private readonly IConfiguration _config;

        public GenerateToken(IConfiguration config)
        {
            _config = config;
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
              expires: DateTime.Now.AddMinutes(60),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
