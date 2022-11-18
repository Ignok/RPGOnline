﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Models;
using RPGOnline.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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



        // Register service
        public async Task<Object> Register(RegisterRequest registerRequest)
        {
            var result = await _dbContext.Users
                .Where(u => u.Username.Equals(registerRequest.Username))
                .SingleOrDefaultAsync();
            // is username in database
            if (result != null)
            {
                Exception e = new Exception();
                e.Data.Add("Username", "This username is already taken. Try different one.");
                throw e;
            }

            result = await _dbContext.Users
                .Where(u => u.Email.Equals(registerRequest.Email))
                .SingleOrDefaultAsync();

            // is email in database
            if (result != null)
            {
                Exception e = new Exception();
                e.Data.Add("Email", "Account registered on this email already exists.");
                throw e;

                throw new Exception("Account registered on this email already exists.");
            }

            // is password correct
            if (registerRequest.Pswd == null || registerRequest.Pswd.Length == 0)
            {
                throw new Exception("Password cannot be empty.");
            }
            if (registerRequest.Pswd.Length < 8 || registerRequest.Pswd.Length > 30)
            {
                Exception e = new Exception();
                e.Data.Add("Pswd", "Password must have min of 8 and max of 30 characters.");
                throw e;
            }

            //var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(model.Password);

            var user = new User()
            {
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                Pswd = registerRequest.Pswd
            };

            //_dbContext.Users.Add(user);
            //_dbContext.SaveChanges();

            return new
            {
                user = user
            };
        }



        // Login service
        public async Task<Object> Login(LoginRequest loginRequest)
        {

            var result = await _dbContext.Users
                .Where(u => u.Username.Equals(loginRequest.Username))
                .SingleOrDefaultAsync();

            if (result == null)
            {
                throw new Exception("User does not exist");
            }

            if (result.Pswd != loginRequest.Pswd)
            {
                throw new Exception("Incorrect password");
            }

            Claim[] UserClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, result.UId.ToString()),
                new Claim(ClaimTypes.Name, loginRequest.Username),
                new Claim(ClaimTypes.Role, ((result.UId == 1 || result.UId == 2) ? "admin" : "user"))
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: UserClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );


            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        //Refresh token service
        /*
        public async Task<Object> RefreshToken(string token, RefreshTokenRequest refreshTokenRequest)
        {
            var result = await _dbContext.Users
               //.Where(u => u.RefreshToken.Equals(refreshTokenRequest.RefreshToken))
               .SingleOrDefaultAsync();


            if (result == null)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            if (result.RefreshTokenExp < DateTime.Now)
            {
                throw new SecurityTokenException("Refresh token expired");
            }

            string login = SecurityHelpers.GetUserIdFromAccessToken(token.Replace("Bearer ", ""), _configuration["JWT:Secret"]);

            Claim[] UserClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, result.UId.ToString()),
                new Claim(ClaimTypes.Name, result.Username),
                new Claim(ClaimTypes.Role, ((result.UId == 1 || result.UId == 2) ? "admin" : "user"))
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: UserClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            result.RefreshToken = SecurityHelpers.GenerateRefreshToken();
            result.RefreshTokenExp = DateTime.Now.AddDays(1);
            _context.SaveChanges();

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                refreshToken = user.RefreshToken
            });
        }
        */
    }
}