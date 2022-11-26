using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Infrastructure.Services
{
    public class PostService : IPost
    {
        private readonly IApplicationDbContext _dbContext;
        public PostService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly int postsOnPageAmount = 2;

        public async Task<ICollection<PostResponse>> GetPosts(PostRequest postRequest)
        {
            var page = postRequest.page;
            if (postRequest.page <= 0) throw new ArgumentOutOfRangeException(nameof(page));

            var category = postRequest.category ?? "";
            var search = postRequest.search ?? "";

            //Search
            //clear string to prevent sql injection

            var result = await _dbContext.Posts
                .Select(p => new PostResponse()
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    Picture = p.Picture,
                    CreationDate = p.CreationDate,
                    Likes = p.UserLikedPosts.Count(),
                    CreatorNavigation = _dbContext.Users
                                        .Where(u => u.UId == p.UId)
                                        .Select(u => new UserResponse()
                                        {
                                            UId = u.UId,
                                            Username = u.Username,
                                            Picture = u.Picture
                                        }).First(),
                    Comments = p.Comments.Count()
                })
                //.Where(p => p...)  <- kategoria
                .Where(p => String.IsNullOrEmpty(search) || p.Title.Contains(search) || p.Content.Contains(search))
                .Skip(postsOnPageAmount*(page-1))
                .Take(postsOnPageAmount)
                .ToListAsync();
            return result;
        }

        /*
        public async Task<ICollection<PostResponse>> GetPosts()
        {
            var result = await _dbContext.Posts
                .Select(p => new PostResponse()
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    Picture = p.Picture,
                    CreationDate = p.CreationDate,
                    Likes = p.UserLikedPosts.Count(),
                    CreatorNavigation = _dbContext.Users
                                        .Where(u => u.UId == p.UId)
                                        .Select(u => new UserResponse()
                                        {
                                            UId = u.UId,
                                            Username = u.Username,
                                            Picture = u.Picture
                                        }).First(),
                    Comments = p.Comments.Count()
                }).ToListAsync();
            return result;
        }
        */

        public async Task<PostDetailsResponse> GetPostDetails(int id)
        {
            var result = await _dbContext.Posts
                .Where(p => p.PostId == id)
                .Select(p => new PostDetailsResponse()
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    Picture = p.Picture,
                    CreationDate = p.CreationDate,
                    Likes = p.UserLikedPosts.Count(),
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
                }).SingleOrDefaultAsync();

            return result;
        }
    }
}
