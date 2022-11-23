using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.Interfaces;
using RPGOnline.Infrastructure.Services;

namespace RPGOnline.API.Controllers
{
    [Authorize]
    public class AccountController : CommonController
    {

        private readonly IAccount _accountService;

        public AccountController(IAccount accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                return Ok(await _accountService.Login(loginRequest));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            try
            {
                return Ok(await _accountService.Register(registerRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data);
            }
        }


        
         [AllowAnonymous]
         [HttpPost("refresh")]
         public async Task<IActionResult> RefreshToken([FromHeader(Name = "Authorization")] string token, RefreshTokenRequest refreshToken)
         {
             try
             {
                 return Ok(await _accountService.RefreshToken(token, refreshToken));
             }
             catch (Exception ex)
             {
                 return BadRequest(ex);
             }
         }


        /*
        [AllowAnonymous]
        [HttpPost("first_changes")]
        public async Task<IActionResult> FirstChanges()
        {
            try
            {
                return Ok(await _accountService.HashPassword());
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
        */
    }
}
