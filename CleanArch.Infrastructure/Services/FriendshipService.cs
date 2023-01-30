using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Responses.Friendship;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.DTOs.Requests.Friendship;

namespace RPGOnline.Infrastructure.Services
{
    public class FriendshipService : IFriendship
    {

        private readonly IApplicationDbContext _dbContext;
        public FriendshipService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<FriendshipResponse> GetFriendship(int uId, int targetUId)
        {
            if (uId == targetUId)
            {
                return new FriendshipResponse()
                {
                    IsFriend = true,
                    IsFollowed = true,
                    IsBlocked = true,
                    IsRequestSent = true,
                    IsRequestReceived = true,
                };
            }
            else
            {
                var result = await _dbContext.Friendships
                    .Where(f => f.UId == uId && f.FriendUId == targetUId)
                    .Select(f => new FriendshipResponse()
                    {
                        IsFriend = f.IsFriend,
                        IsFollowed = f.IsFollowed,
                        IsBlocked = f.IsBlocked,
                        IsRequestSent = f.IsRequestSent,
                        IsRequestReceived = f.IsRequestReceived,
                    }).FirstOrDefaultAsync();
                
                if(result == null)
                {
                    return new FriendshipResponse()
                    {
                        IsFriend = false,
                        IsFollowed = false,
                        IsBlocked = false,
                        IsRequestSent = false,
                        IsRequestReceived = false,
                    };
                }
                else
                {
                    return result;
                }
            }

        }


        public async Task<ICollection<UserFriendshipResponse>> GetUserFriends(int id)
        {
            var user = await _dbContext.Users.Where(u => u.UId == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException($"User {id} does not exist");
            }
            else
            {
                var result = await _dbContext.Friendships
                    .Include(f => f.UIdNavigation)
                    .Include(f => f.FriendU)
                    .Where(f => f.UIdNavigation.UId == id)
                    .Select(f => new UserFriendshipResponse()
                    {
                        UId = f.FriendU.UId,
                        Username = f.FriendU.Username,
                        Picture = f.FriendU.Picture,
                        Country = f.FriendU.Country,
                        Attitude = f.FriendU.Attitude,
                        IsFriend = f.IsFriend,
                        IsFollowed = f.IsFollowed,
                        IsBlocked = f.IsBlocked,
                        IsRequestSent = f.IsRequestSent,
                        IsRequestReceived = f.IsRequestReceived,
                    }).ToListAsync();


                return result;
            };
        }

        public async Task<FriendshipResponse> ManageFriendship(FriendshipRequest friendshipRequest)
        {
            //if friendship option exists
            if (!Enum.IsDefined(typeof(Friendship), friendshipRequest.Option))
            {
                throw new InvalidDataException($"Friendship option '{friendshipRequest.Option}' is not supported");
            }

            var friendshipStatus = await _dbContext.Friendships.Where(f => f.UId == friendshipRequest.UId && f.FriendUId == friendshipRequest.TargetUId).FirstOrDefaultAsync();

            var viceFriendshipStatus = await _dbContext.Friendships.Where(f => f.UId == friendshipRequest.TargetUId && f.FriendUId == friendshipRequest.UId).FirstOrDefaultAsync();

            if (friendshipStatus == null)
            {
                friendshipStatus = new Domain.Models.Friendship
                {
                    UId = friendshipRequest.UId,
                    FriendUId = friendshipRequest.TargetUId,
                    IsFriend = false,
                    IsFollowed = false,
                    IsBlocked = false,
                    IsRequestSent = false,
                    IsRequestReceived = false,
                };
                _dbContext.Friendships.Add(friendshipStatus);
            }
            if (viceFriendshipStatus == null)
            {
                viceFriendshipStatus = new Domain.Models.Friendship
                {
                    UId = friendshipRequest.TargetUId,
                    FriendUId = friendshipRequest.UId,
                    IsFriend = false,
                    IsFollowed = false,
                    IsBlocked = false,
                    IsRequestSent = false,
                    IsRequestReceived = false,
                };
                _dbContext.Friendships.Add(viceFriendshipStatus);
            }

            //can do?
            switch (Enum.Parse(typeof(Friendship), friendshipRequest.Option))
            {
                case (Friendship.follow):
                    if (viceFriendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user has blocked you");
                    }
                    else if (friendshipStatus.IsFollowed)
                    {
                        throw new Exception("Target user is already followed");
                    }
                    friendshipStatus.IsBlocked = false;
                    friendshipStatus.IsFollowed = true;
                    break;


                case (Friendship.unfollow):
                    if (viceFriendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user has blocked you");
                    }
                    else if (friendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user is blocked");
                    }
                    else if (!friendshipStatus.IsFollowed)
                    {
                        throw new Exception("Target user is not followed");
                    }
                    friendshipStatus.IsFollowed = false;
                    break;


                case (Friendship.friend):
                    if (viceFriendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user has blocked you");
                    }
                    else if (friendshipStatus.IsFriend)
                    {
                        throw new Exception("Target user is already a friend");
                    }
                    else if (friendshipStatus.IsRequestSent)
                    {
                        throw new Exception("Request is already sent");
                    }
                    else if (friendshipStatus.IsRequestReceived)
                    {
                        friendshipStatus.IsBlocked = false;
                        friendshipStatus.IsFriend = true;
                        friendshipStatus.IsRequestReceived = false;
                        friendshipStatus.IsFollowed = true;

                        viceFriendshipStatus.IsFriend = true;
                        viceFriendshipStatus.IsRequestSent = false;
                        viceFriendshipStatus.IsFollowed = true;

                    }
                    else
                    {
                        friendshipStatus.IsBlocked = false;
                        friendshipStatus.IsRequestSent = true;
                        viceFriendshipStatus.IsRequestReceived = true;
                    }

                    break;


                case (Friendship.unfriend):
                    if (viceFriendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user has blocked you");
                    }
                    else if (friendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user is blocked");
                    }
                    else if (friendshipStatus.IsRequestReceived)
                    {
                        friendshipStatus.IsRequestReceived = false;
                        viceFriendshipStatus.IsRequestSent = false;
                    }
                    else if (friendshipStatus.IsRequestSent)
                    {
                        friendshipStatus.IsRequestSent = false;
                        viceFriendshipStatus.IsRequestReceived = false;
                    }
                    else if (!friendshipStatus.IsFriend)
                    {
                        throw new Exception("Target user is not a friend");
                    }
                    else
                    {
                        friendshipStatus.IsRequestSent = false;
                        friendshipStatus.IsRequestReceived = false;
                        friendshipStatus.IsFriend = false;
                        friendshipStatus.IsBlocked = false;

                        viceFriendshipStatus.IsRequestSent = false;
                        viceFriendshipStatus.IsRequestReceived = false;
                        viceFriendshipStatus.IsFriend = false;
                    }

                    break;


                case (Friendship.block):
                    if (friendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user is already blocked");
                    }
                    viceFriendshipStatus.IsFriend = false;
                    viceFriendshipStatus.IsRequestReceived = false;
                    viceFriendshipStatus.IsRequestSent = false;
                    viceFriendshipStatus.IsFollowed = false;

                    friendshipStatus.IsBlocked = true;
                    friendshipStatus.IsFollowed = false;
                    friendshipStatus.IsRequestSent = false;
                    friendshipStatus.IsRequestReceived = false;
                    friendshipStatus.IsFriend = false;

                    break;


                case (Friendship.unblock):
                    if (!friendshipStatus.IsBlocked)
                    {
                        throw new Exception("Target user is not blocked.");
                    }
                    friendshipStatus.IsBlocked = false;
                    break;
            }

            if (!friendshipStatus.IsBlocked
                && !friendshipStatus.IsFriend
                && !friendshipStatus.IsFollowed
                && !friendshipStatus.IsRequestSent
                && !friendshipStatus.IsRequestReceived
                && !viceFriendshipStatus.IsBlocked
                && !viceFriendshipStatus.IsFriend
                && !viceFriendshipStatus.IsFollowed
                && !viceFriendshipStatus.IsRequestSent
                && !viceFriendshipStatus.IsRequestReceived)
            {
                _dbContext.Friendships.Remove(friendshipStatus);
                _dbContext.Friendships.Remove(viceFriendshipStatus);
            }



            _dbContext.SaveChanges();

            if(await _dbContext.Friendships.AnyAsync(f => f.UId == friendshipRequest.UId && f.FriendUId == friendshipRequest.TargetUId))
            {
                return new FriendshipResponse
                {
                    IsFriend = friendshipStatus.IsFriend,
                    IsFollowed = friendshipStatus.IsFollowed,
                    IsBlocked = friendshipStatus.IsBlocked,
                    IsRequestSent = friendshipStatus.IsRequestSent,
                    IsRequestReceived = friendshipStatus.IsRequestReceived,
                };
            }
            else
            {
                return new FriendshipResponse
                {
                    IsFriend = false,
                    IsFollowed = false,
                    IsBlocked = false,
                    IsRequestSent = false,
                    IsRequestReceived = false,
                };
            }
        }
    }
}
