using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Item;
using RPGOnline.Application.DTOs.Responses.Asset.Item;
using RPGOnline.Application.DTOs.Responses.User;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Enums;
using RPGOnline.Domain.Models;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Infrastructure.Services
{
    public class ItemService : IItem
    {
        private readonly IApplicationDbContext _dbContext;
        private ILogger<ItemService> _logger;

        public ItemService(IApplicationDbContext dbContext, ILogger<ItemService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        private readonly int itemsOnPageAmount = 10;

        public async Task<(ICollection<GetItemResponse>, int pageCount)> GetItems(SearchAssetRequest searchItemRequest, CancellationToken cancellationToken)
        {
            try
            {

                var page = searchItemRequest.Page;
                if (searchItemRequest.Page <= 0) throw new ArgumentOutOfRangeException(nameof(page));

                var prefferedLanguages = searchItemRequest.PrefferedLanguage.Split('-');

                

                var result = _dbContext.Items.Include(i => i.Asset).ThenInclude(usa => usa.UserSavedAssets)
                                                //.Include(i => i.Asset.UserSavedAssets)
                                                .Include(i => i.Asset.Author)
                                                .AsParallel().WithCancellation(cancellationToken)
                   .Where(i => i.Asset.IsPublic)
                   .Where(i => String.IsNullOrEmpty(searchItemRequest.KeyValueName)
                                || object.Equals(i.KeySkill, searchItemRequest.KeyValueName)
                            )
                    .Where(i => String.IsNullOrEmpty(searchItemRequest.Search)
                                || (i.ItemName.Contains(searchItemRequest.Search, StringComparison.OrdinalIgnoreCase))
                                || (i.Description.Contains(searchItemRequest.Search, StringComparison.OrdinalIgnoreCase))
                            )
                    .Where(i => prefferedLanguages.Contains(i.Asset.Language))
                    .Select(i => new GetItemResponse()
                    {
                        AssetId = i.Asset.AssetId,
                        CreationDate = i.Asset.CreationDate,
                        TimesSaved = i.Asset.UserSavedAssets.Count,
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        ItemDescription = i.Description,
                        ItemKeySkill = i.KeySkill,
                        ItemSkillMod = i.SkillMod,
                        ItemGoldMultiplier = i.GoldMultiplier,
                        PrefferedLanguage = i.Asset.Language,
                        CreatorNavigation = new UserResponse()
                        {
                            UId = i.Asset.Author.UId,
                            Username = i.Asset.Author.Username,
                            Picture = i.Asset.Author.Picture,
                        }
                    })
                    //.Where(p => p...)  <- kategoria
                    .OrderByDescending(i => i.CreationDate)
                    
                    .ToList();


                int pageCount = (int)Math.Ceiling((double)result.Count / itemsOnPageAmount);

                result = result
                    .Skip(itemsOnPageAmount * (page - 1))
                    .Take(itemsOnPageAmount)
                    .ToList();

                await Task.Delay(500, cancellationToken);

                return (result, pageCount);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError("=========== I WAS CANCELLED ==========");
                throw ex;
            }
        }

        public async Task<ICollection<GetItemSimplifiedResponse>> GetItemsForCharacter(int uId, GetAssetForCharacterRequest getItemRequest)
        {
            var prefferedLanguages = getItemRequest.PrefferedLanguage.Split('-');

            var result = await (from asset in _dbContext.Assets
                                join item in _dbContext.Items on asset.AssetId equals item.AssetId
                                where object.Equals(item.KeySkill, getItemRequest.KeyValueName)
                                where prefferedLanguages.Contains(asset.Language)
                                where asset.IsPublic || asset.AuthorId == uId
                                orderby item.ItemName ascending
                                select new GetItemSimplifiedResponse()
                                {
                                    AssetId = asset.AssetId,
                                    ItemId = item.ItemId,
                                    ItemName = item.ItemName,
                                    ItemDescription = item.Description,
                                    ItemKeySkill = item.KeySkill,
                                    ItemSkillMod = item.SkillMod,
                                    ItemGoldMultiplier = item.GoldMultiplier
                                })
                                .ToListAsync();
            return result;
        }

        public async Task<GetItemResponse> PostItem(PostItemRequest postItemRequest)
        {
            if (postItemRequest == null) throw new ArgumentNullException(nameof(postItemRequest));

            //if user exists
            if (!_dbContext.Users.Where(u => u.UId == postItemRequest.UId).ToList().Any())
                throw new ArgumentException($"User with id: {postItemRequest.UId} does not exist");

            //if language exists
            if (!Enum.IsDefined(typeof(Language), postItemRequest.Language))
                throw new InvalidDataException($"Language '{postItemRequest.Language}' is not supported");

            //if attribute exists, or is null
            if (!Enum.IsDefined(typeof(Attributes), postItemRequest.KeySkill ?? "none"))
                throw new InvalidDataException($"Attribute '{postItemRequest?.KeySkill}' does not exist");


            var asset = new Asset()
            {
                AssetId = (_dbContext.Assets.Max(a => (int)a.AssetId) + 1),
                AuthorId = postItemRequest.UId,
                IsPublic = postItemRequest.IsPublic,
                Language = postItemRequest.Language,
                CreationDate = DateTime.Now,
                Author = await _dbContext.Users.Where(u => u.UId == postItemRequest.UId).SingleAsync(),
            };

            var item = new Item()
            {
                ItemId = (_dbContext.Items.Max(i => (int)i.AssetId) + 1),
                AssetId = asset.AssetId,
                ItemName = postItemRequest.ItemName,
                Description = postItemRequest.Description,
                KeySkill = postItemRequest.KeySkill.Equals("none") ? null : postItemRequest.KeySkill,
                SkillMod = postItemRequest.SkillMod,
                GoldMultiplier = postItemRequest.GoldMultiplier,
                Asset = asset,
            };

            asset.Items.Add(item);

            _dbContext.Assets.Add(asset);
            _dbContext.Items.Add(item);
            _dbContext.SaveChanges();

            return new GetItemResponse()
            {
                AssetId = asset.AssetId,
                CreationDate = asset.CreationDate,
                TimesSaved = 0,
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemDescription = item.Description,
                ItemKeySkill = item.KeySkill,
                ItemSkillMod = item.SkillMod,
                ItemGoldMultiplier = item.GoldMultiplier,
                PrefferedLanguage = item.Asset.Language,
                CreatorNavigation = new UserResponse()
                {
                    UId = asset.Author.UId,
                    Username = asset.Author.Username,
                    Picture = asset.Author.Picture,
                }
            };
        }
    }
}
