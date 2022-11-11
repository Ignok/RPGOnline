using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Models;
using RPGOnline.Infrastructure.Models;

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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user) //<- user request
        {
            if (id != user.UId)
            {
                return BadRequest();
            }

            _dbContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound("No such user in database");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUser(User user) //<- user request
        {
            _dbContext.Users.Add(user);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UId))
                {
                    return Conflict("Such user already exists in database");
                    //raczej sprawdzac login, mail, bo id w bazie bedzie sekwencyjnie inkrementowane
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.UId }, user);
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
