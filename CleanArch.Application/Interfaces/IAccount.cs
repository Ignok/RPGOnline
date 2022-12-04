using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Interfaces
{
    public interface IAccount
    {
        Task<TokenResponse> Login(LoginRequest loginRequest);
        Task<Object> Register(RegisterRequest registerRequest);
        Task<Object> RefreshToken(string token, RefreshTokenRequest refreshTokenRequest);
        //Task<Object> HashPassword();
    }
}
