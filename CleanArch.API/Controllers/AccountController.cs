using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.Interfaces;


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
                    Expires = DateTime.Now.AddMinutes(10)
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
                return Ok(new
                {
                    UId = tokens.UId,
                    Username = tokens.Username,
                    UserRole = tokens.UserRole
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data);
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

        
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var cookieOptions = new CookieOptions()
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.Now.AddDays(-1)
                };

                if (Request.Cookies["AccessToken"] != null)
                {
                    Response.Cookies.Append("AccessToken", "", cookieOptions);
                }
                if (Request.Cookies["RefreshToken"] != null)
                {
                    Response.Cookies.Append("RefreshToken", "", cookieOptions);
                }

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
