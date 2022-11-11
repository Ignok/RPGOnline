using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.Interfaces;
using RPGOnline.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Infrastructure.Services
{
    public class AccountService : IAccount
    {
        private readonly IConfiguration _configuration;
        private readonly RPGOnlineDbContext _dbContext;
        public AccountService(RPGOnlineDbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<Object> Login(LoginRequest loginRequest)
        {

            var result = await _dbContext.Users
                .Where(u => u.Username.Equals(loginRequest.Login))
                .SingleOrDefaultAsync();

            if (result == null)
            {
                throw new Exception("User nie istnieje");
            }

            if (result.Pswd != loginRequest.Password)
            {
                throw new Exception("Błędne hasło");
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SuperSecretKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://localhost:5001/",
                audience: "https://localhost:5001/",
                // claims: claims,
                expires: DateTime.Now.AddMinutes(420),
                signingCredentials: creds
            );

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
