using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Services.Users;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Models;
using RPGOnline.Infrastructure.Models;
using RPGOnline.Infrastructure.Services;

namespace RPGOnline.API.Controllers
{
    public class UsersController : CommonController
    {
        private readonly RPGOnlineDbContext _dbContext;

        private readonly IUser _userService;

        public UsersController(RPGOnlineDbContext dbContext, IUser userService)
        {
            _userService = userService;
            _dbContext = dbContext;
        }

        // GET: api/Users
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _dbContext.Users
                .Select(u => new UserResponse()
                {
                    UId = u.UId,
                    Username = u.Username,
                    Picture = u.Picture
                }).ToListAsync();

            if(result == null)
            {
                return NotFound("No users in database.");
            }
            else
            {
                return Ok(result);
            }
        }

        //GET info in About Me
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAboutMe(int id)
        {
            var result = await _userService.GetAboutMe(id);

            if (result == null)
            {
                return BadRequest("No such user in database.");
            }
            return Ok(result);
        }


        /*
        // GET: api/Users/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            UserResponse result = new UserResponse()
            {
                UId = user.UId,
                Username=user.Username,
                Picture=user.Picture
            };
            {
                return Ok(result);
            }
        }*/

        // PUT: api/Users/id
        [HttpPut("{id}/Details")]
        public async Task<IActionResult> PutUser(int id, UserRequest userRequest) //<- user request
        {
            try
            {
                var result = await _userService.PutUser(id, userRequest);
                return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound("No such user in database");
                }
                else
                {
                    return NoContent();
                }
            }
        }

        // DELETE: api/Users/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _dbContext.Users.Any(e => e.UId == id);
        }
    }
}
