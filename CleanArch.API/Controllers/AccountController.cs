using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;
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
                TokenResponse tokens = await _accountService.Login(loginRequest);

                HttpContext.Response.Cookies.Append(
                    "AccessToken",
                    "Bearer " + tokens.AccessToken,
                    new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(5),
                        HttpOnly = true,
                        Secure = false
                    }
                    );

                HttpContext.Response.Cookies.Append(
                    "RefreshToken",
                    tokens.RefreshToken,
                    new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(5),
                        HttpOnly = true,
                        Secure = false
                    }
                    );
                return Ok("Poprawne logowanie");
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
         public async Task<IActionResult> RefreshToken()
         {
             try
             {
                 return Ok(await _accountService.RefreshToken("token", new RefreshTokenRequest { }));
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
