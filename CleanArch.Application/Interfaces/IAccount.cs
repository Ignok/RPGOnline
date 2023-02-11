using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;

namespace RPGOnline.Application.Interfaces
{
    public interface IAccount
    {
        Task<TokenResponse> Login(LoginRequest loginRequest);
        Task<Object> Register(RegisterRequest registerRequest);
        Task<TokenResponse> RefreshToken(string token, string refreshToken);

        Task<object> DeleteAccount(int uId);
    }
}
