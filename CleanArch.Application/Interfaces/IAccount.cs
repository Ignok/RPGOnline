using RPGOnline.Application.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Interfaces
{
    public interface IAccount
    {
        Task<Object> Login(LoginRequest loginRequest);
        Task<Object> Register(RegisterRequest registerRequest);
    }
}
