using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Models;
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

        private readonly int postsOnPageAmount = 5;

        public async Task<(ICollection<PostResponse>, int pageCount)> GetPosts(SearchPostRequest searchPostRequest)
        {
            var page = searchPostRequest.page;
            if (searchPostRequest.page <= 0) throw new ArgumentOutOfRangeException(nameof(page));

            var category = searchPostRequest.category ?? "";
            var search = searchPostRequest.search ?? "";

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
                .ToListAsync();

            int pageCount = (int)Math.Ceiling((double)result.Count / postsOnPageAmount);

            result = result
                .Skip(postsOnPageAmount * (page - 1))
                .Take(postsOnPageAmount)
                .ToList();

            return (result, pageCount);
        }

        

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
                                            CommentId = c.CommentId,
                                            ResponseCommentId = c.ResponseCommentId,
                                            RespondingUserResponse = _dbContext.Comments
                                                                    .Where(cr => cr.CommentId == c.ResponseCommentId)
                                                                    .Select(cr => new UserResponse()
                                                                    {
                                                                        UId = cr.UIdNavigation.UId,
                                                                        Username = cr.UIdNavigation.Username,
                                                                        Picture = cr.UIdNavigation.Picture
                                                                    }).FirstOrDefault(),
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

        public async Task<Object> PostPost(PostRequest postRequest)
        {
            if(postRequest == null)
                throw new ArgumentNullException(nameof(postRequest));

            //if user exists - after authorization has to be deleted
            if (!_dbContext.Users.Where(u => u.UId == postRequest.UId).ToList().Any())
                throw new ArgumentException("User does not exist");

            //var maxId = await _dbContext.Posts.OrderByDescending(p => p.PostId).FirstOrDefaultAsync();
            //int? maxId = _dbContext.Posts.Max(p => (int?)p.PostId);
            //maxId = maxId ?? 0;

            var post = new Post()
            {
                PostId = (_dbContext.Posts.Max(p => (int)p.PostId)+1), //potem jak dodamy automatyczny id można usunąć
                UId = postRequest.UId,
                Title = postRequest.Title,
                Content = postRequest.Content,
                Picture = postRequest.Picture,
                CreationDate = DateTime.Now
            };

            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();

            return new
            {
                post = post
            };

        }

        public async Task<Object> PostComment(CommentRequest commentRequest)
        {
            if(commentRequest == null)
                throw new ArgumentNullException(nameof(commentRequest));

            //if user exists - after authorization has to be deleted
            if (!_dbContext.Users.Where(u => u.UId == commentRequest.UId).ToList().Any())
                throw new ArgumentException("User does not exist");

            //if post exists
            if (!_dbContext.Posts.Where(p => p.PostId == commentRequest.PostId).ToList().Any())
                throw new ArgumentException("Post does not exist");

            //if comment that is responsed exists in post
            if (commentRequest.ResponseCommentId != null && !_dbContext.Comments.Where(c => c.PostId == commentRequest.PostId).Where(c => c.CommentId == commentRequest.ResponseCommentId).ToList().Any())
                throw new ArgumentException("Response comment ID does not exist");
            

            var comment = new Comment()
            {
                CommentId = (_dbContext.Comments.Max(c => (int)c.CommentId) + 1), //potem jak dodamy automatyczny id można usunąć
                UId = commentRequest.UId,
                PostId = commentRequest.PostId,
                Content = commentRequest.Content,
                ResponseCommentId =commentRequest.ResponseCommentId,
                CreationDate = DateTime.Now
            };

            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();

            return new
            {
                commentId = comment.CommentId,
                Uid = comment.UId,
                responseCommentId = comment.ResponseCommentId
            };
        }
    }
}
