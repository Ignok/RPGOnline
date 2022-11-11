using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.Interfaces;
using RPGOnline.Infrastructure.Services;

namespace RPGOnline.API.Controllers
{
    public class AccountController : CommonController
    {

        private readonly IAccount _accountService;

        public AccountController(IAccount accountService)
        {
            _accountService = accountService;
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                return Ok(await _accountService.Login(loginRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(new { n = ex.Message });
            }
        }
    }
}
