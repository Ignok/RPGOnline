﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Infrastructure.Models;

namespace RPGOnline.API.Controllers
{
    public class PostsController : CommonController
    {

        private readonly RPGOnlineDbContext _dbContext;

        public PostsController(RPGOnlineDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var result = await _dbContext.Posts
                .Select(p => new PostResponse()
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    Picture = p.Picture,
                    CreationDate = p.CreationDate,
                    Likes = _dbContext.Users
                            .Where(u => p.UIds.Select(d => d.UId).Contains(u.UId))
                            .Count(),
                    CreatorNavigation = _dbContext.Users
                                        .Where(u => u.UId == p.UId)
                                        .Select(u => new UserResponse()
                                        {
                                            UId = u.UId,
                                            Username = u.Username,
                                            Picture = u.Picture
                                        }).First(),
                    Comments = _dbContext.Comments
                                        .Where(c => c.PostId == p.PostId)
                                        .Select(c => new CommentResponse()
                                        {
                                            ResponseCommentId = c.ResponseCommentId,
                                            UserResponse = _dbContext.Users
                                                .Where(u => u.UId == c.UId)
                                                .Select(u => new UserResponse()
                                                {
                                                    UId = u.UId,
                                                    Username = u.Username,
                                                    Picture = u.Picture
                                                }).First(),
                                            Content = c.Content,
                                            CreationDate = c.CreationDate,
                                            PostIdNavigation = c.PostId
                                        }).ToList()
                }).ToListAsync();

            if(result == null)
            {
                return BadRequest("No posts in database.");
            }
            else
            {
                return Ok(result);
            }
        }
    }
}