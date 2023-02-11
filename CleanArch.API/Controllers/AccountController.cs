using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.Interfaces;
using System.Security.Claims;

namespace RPGOnline.API.Controllers
{
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
                    Expires = DateTime.Now.AddMinutes(60)
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
                    tokens.UId,
                    tokens.Username,
                    tokens.UserRole,
                    tokens.Avatar
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
         public async Task<IActionResult> RefreshToken()
         {
             try
             {
                if (Request.Cookies["AccessToken"] == null || Request.Cookies["RefreshToken"] == null)
                {
                    return BadRequest("No access or refresh token provided");
                }
                TokenResponse tokens = await _accountService.RefreshToken(Request.Cookies["AccessToken"], Request.Cookies["RefreshToken"]);

                var cookieOptions = new CookieOptions()
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.Now.AddMinutes(60)
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
                    tokens.UId,
                    tokens.Username,
                    tokens.UserRole,
                    tokens.Avatar
                });
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


        [Authorize]
        [HttpDelete("delete/{uId}")]
        public async Task<IActionResult> DeleteAccount(int uId)
        {
            try
            {
                if (IsSameId(uId) || IsAdminRole())
                {
                    return Ok(await _accountService.DeleteAccount(uId));
                }

                return BadRequest("Permission denied");
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



        private bool IsSameId(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "a";

            return userId.Equals(id.ToString());
        }

        private bool IsAdminRole()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userRole = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value ?? "a";

            return userRole.Equals("admin");
        }
    }
}
