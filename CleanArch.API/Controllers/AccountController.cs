using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.Interfaces;
using RPGOnline.Infrastructure.Services;
using System.Net.Http.Headers;

namespace RPGOnline.API.Controllers
{
    //[Authorize]
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

                var cookieOptions = new CookieOptions()
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.Now.AddMinutes(1)
                };

                Response.Cookies.Append(
                    "AccessToken",
                    tokens.AccessToken,
                    cookieOptions
                    );

                Response.Cookies.Append(
                    "RefreshToken",
                    tokens.RefreshToken,
                    cookieOptions
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
         public async Task<IActionResult> RefreshToken([FromHeader] string Cookie)
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

        
        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                Response.Cookies.Delete("AccessToken");
                Response.Cookies.Delete("RefreshToken");

                return Ok("Poprawnie wylogowano");
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
