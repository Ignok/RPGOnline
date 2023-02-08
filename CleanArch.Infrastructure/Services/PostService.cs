﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.Forum;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.DTOs.Responses.User;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Models;
//using System.Data.Entity;
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
        private ILogger<PostService> _logger;

        public PostService(IApplicationDbContext dbContext, ILogger<PostService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        private readonly int postsOnPageAmount = 10;

        public async Task<(ICollection<PostResponse>, int pageCount)> GetPosts(int uId, SearchPostRequest searchPostRequest, CancellationToken cancellationToken)
        {
            try
            {

                var page = searchPostRequest.Page;
                if (searchPostRequest.Page <= 0) throw new ArgumentOutOfRangeException(nameof(page));

                var category = searchPostRequest.Category ?? "";
                var search = searchPostRequest.Search ?? "";

                //Search
                //clear string to prevent sql injection

                var result = _dbContext.Posts
                    .Include(u => u.UIdNavigation)
                        .ThenInclude(u => u.FriendshipUIdNavigations)
                        .Where(u => !u.UIdNavigation.FriendshipUIdNavigations
                            .Where(f => f.UId == u.UId && f.FriendUId == uId)
                            .Where(f => f.IsBlocked).Any()
                        )
                        .Where(p => !searchPostRequest.OnlyFollowed || p.UIdNavigation.FriendshipFriendUs
                            .Where(f => f.UIdNavigation.UId ==uId)
                            .Where(f => f.IsFollowed).Any()
                        )
                    .Include(ulp => ulp.UserLikedPosts)
                        .Where(p => !searchPostRequest.OnlyFavourite || p.UserLikedPosts
                            .Where(ulp => ulp.UId == uId).Any())
                    .Include(c => c.Comments)
                    .AsParallel().WithCancellation(cancellationToken)
                    .Select(p => new PostResponse()
                    {
                        PostId = p.PostId,
                        Title = p.Title,
                        Content = p.Content,
                        Picture = p.Picture,
                        CreationDate = p.CreationDate,
                        Tag = p.Tag,
                        Likes = p.UserLikedPosts.Count,
                        IsLiked = p.UserLikedPosts.Any(ulp => ulp.UId == uId),
                        CreatorNavigation = new UserSimplifiedResponse()
                        {
                            UId = p.UIdNavigation.UId,
                            Username = p.UIdNavigation.Username,
                            Picture = p.UIdNavigation.Picture
                        },
                        Comments = p.Comments.Count
                    })
                    .Where(p => String.IsNullOrEmpty(category)
                                || p.Tag?.IndexOf(category, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Where(p => String.IsNullOrEmpty(search)
                                || (p.Title.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                                || (p.Content.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                            )
                    .OrderByDescending(p => p.CreationDate)
                    .ToList();


                int pageCount = (int)Math.Ceiling((double)result.Count / postsOnPageAmount);

                result = result
                    .Skip(postsOnPageAmount * (page - 1))
                    .Take(postsOnPageAmount)
                    .ToList();

                await Task.Delay(500, cancellationToken);

                return (result, pageCount);
            }
            catch(TaskCanceledException ex)
            {
                _logger.LogError("=========== I WAS CANCELLED ==========");
                throw ex;
            }

        }

        

        public async Task<PostDetailsResponse> GetPostDetails(int uId, int postId)
        {
            
            var result = await _dbContext.Posts
                .Include(p => p.UserLikedPosts)
                .Where(p => p.PostId == postId)
                .Select(p => new PostDetailsResponse()
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    Picture = p.Picture,
                    CreationDate = p.CreationDate,
                    Tag = p.Tag,
                    Likes = p.UserLikedPosts.Count,
                    IsLiked = p.UserLikedPosts.Any(ulp => ulp.UId == uId),
                    CreatorNavigation = _dbContext.Users
                                        .Where(u => u.UId == p.UId)
                                        .Select(u => new UserSimplifiedResponse()
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
                                                                    .Select(cr => new UserSimplifiedResponse()
                                                                    {
                                                                        UId = cr.UIdNavigation.UId,
                                                                        Username = cr.UIdNavigation.Username,
                                                                        Picture = cr.UIdNavigation.Picture
                                                                    }).FirstOrDefault(),
                                            UserResponse = _dbContext.Users
                                                .Where(u => u.UId == c.UId)
                                                .Select(u => new UserSimplifiedResponse()
                                                {
                                                    UId = u.UId,
                                                    Username = u.Username,
                                                    Picture = u.Picture
                                                }).First(),
                                            Content = c.Content,
                                            CreationDate = c.CreationDate,
                                            PostIdNavigation = c.PostId
                                        })
                                        .OrderByDescending(c => c.CreationDate)
                                        .ToList()
                }).SingleOrDefaultAsync();

            if (HasBlockedMe(uId, result.CreatorNavigation.UId))
            {
                throw new ArgumentException("Blocked");
            }
            else if (result == null)
            {
                throw new ArgumentNullException($"There is no post with id: {postId}");
            }
            return result;
        }

        public async Task<Object> PostPost(PostRequest postRequest)
        {
            if(postRequest == null)
                throw new ArgumentNullException(nameof(postRequest));

            //if user exists - after authorization has to be deleted
            if (!_dbContext.Users.Where(u => u.UId == postRequest.UId).ToList().Any())
                throw new ArgumentException("User does not exist");

            //if context is not empty or only with spaces
            if (postRequest.Content.Trim().Length == 0)
                throw new ArgumentException("Content cannot be empty");

            var post = new Post()
            {
                PostId = (_dbContext.Posts.Max(p => (int)p.PostId)+1), //potem jak dodamy automatyczny id można usunąć
                UId = postRequest.UId,
                Title = postRequest.Title,
                Content = postRequest.Content,
                Tag = postRequest.Tag,
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


        public async Task<CommentResponse> PostComment(int postId, CommentRequest commentRequest)
        {
            if(commentRequest == null)
                throw new ArgumentNullException(nameof(commentRequest));

            //if post exists
            if (!_dbContext.Posts.Where(p => p.PostId == postId).ToList().Any())
                throw new ArgumentException("Post does not exist");

            //if context is not empty or only with spaces
            if (commentRequest.Content.Trim().Length == 0)
                throw new ArgumentException("Content cannot be empty");

            //if user exists - after authorization has to be deleted
            if (!_dbContext.Users.Where(u => u.UId == commentRequest.UId).ToList().Any())
                throw new ArgumentException("User does not exist");

            

            //if comment that is responsed exists in post
            if (commentRequest.ResponseCommentId != null && !_dbContext.Comments.Where(c => c.PostId == postId).Where(c => c.CommentId == commentRequest.ResponseCommentId).ToList().Any())
                throw new ArgumentException("Response comment ID does not exist");
            

            var comment = new Comment()
            {
                CommentId = (_dbContext.Comments.Max(c => (int)c.CommentId) + 1), //potem jak dodamy automatyczny id można usunąć
                UId = commentRequest.UId,
                PostId = postId,
                Content = commentRequest.Content,
                ResponseCommentId =commentRequest.ResponseCommentId,
                CreationDate = DateTime.Now
            };

            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();

            return new CommentResponse
            {
                CommentId = comment.CommentId,
                ResponseCommentId = comment.ResponseCommentId,
                RespondingUserResponse = _dbContext.Comments
                                            .Where(cr => cr.CommentId == comment.ResponseCommentId)
                                            .Select(cr => new UserSimplifiedResponse()
                                            {
                                                UId = cr.UIdNavigation.UId,
                                                Username = cr.UIdNavigation.Username,
                                                Picture = cr.UIdNavigation.Picture
                                            }).FirstOrDefault(),
                UserResponse = _dbContext.Users
                                .Where(u => u.UId == comment.UId)
                                .Select(u => new UserSimplifiedResponse()
                                {
                                    UId = u.UId,
                                    Username = u.Username,
                                    Picture = u.Picture
                                }).First(),
                Content = comment.Content,
                CreationDate = comment.CreationDate,
                PostIdNavigation = comment.PostId
            };
        }

        private bool HasBlockedMe(int myId, int targetId)
        {
            if (myId == targetId) return false;
            return myId == targetId || _dbContext.Friendships
                .Where(f => f.UId == targetId && f.FriendUId == myId)
                .Where(f => f.IsBlocked).Any();
        }
    }
}
