using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Item;
using RPGOnline.Application.DTOs.Responses.Asset.Item;
using RPGOnline.Application.DTOs.Responses.User;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Enums;
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

        public async Task<(ICollection<GetItemResponse>, int pageCount)> GetItems(SearchAssetRequest searchRaceRequest, CancellationToken cancellationToken)
        {
            try
            {

                var page = searchRaceRequest.Page;
                if (searchRaceRequest.Page <= 0) throw new ArgumentOutOfRangeException(nameof(page));

                

                var result = _dbContext.Items.Include(i => i.Asset).ThenInclude(usa => usa.UserSavedAssets)
                                                //.Include(i => i.Asset.UserSavedAssets)
                                                .Include(i => i.Asset.Author)
                                                .AsParallel().WithCancellation(cancellationToken)
                   .Where(i => i.Asset.IsPublic)
                   .Where(i => String.IsNullOrEmpty(searchRaceRequest.KeyValueName)
                                || object.Equals(i.KeySkill, searchRaceRequest.KeyValueName)
                            )
                    .Where(i => String.IsNullOrEmpty(searchRaceRequest.Search)
                                || (i.ItemName.Contains(searchRaceRequest.Search, StringComparison.OrdinalIgnoreCase))
                                || (i.Description.Contains(searchRaceRequest.Search, StringComparison.OrdinalIgnoreCase))
                            )
                    .Where(i => searchRaceRequest.PrefferedLanguage.Contains(i.Asset.Language))
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

        public async Task<ICollection<GetItemSimplifiedResponse>> GetItemsForCharacter(int uId, GetAssetForCharacterRequest getItemsForCharacterRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<GetItemResponse> PostItem(PostItemRequest postItemRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<Object> GetItemsTest(SearchAssetRequest searchRaceRequest)
        {
            try
            {

                var page = searchRaceRequest.Page;
                if (searchRaceRequest.Page <= 0) throw new ArgumentOutOfRangeException(nameof(page));



                var result = _dbContext.Items.Include(i => i.Asset)
                                                .Include(i => i.Asset.UserSavedAssets)
                                                .Include(i => i.Asset.Author)
                                                //.AsParallel().WithCancellation(cancellationToken)
                    .Where(i => i.Asset.IsPublic)
                   .Where(i => String.IsNullOrEmpty(searchRaceRequest.KeyValueName)
                                || object.Equals(i.KeySkill, searchRaceRequest.KeyValueName)
                            )
                    .Where(i => String.IsNullOrEmpty(searchRaceRequest.Search)
                                || (i.ItemName.Contains(searchRaceRequest.Search, StringComparison.OrdinalIgnoreCase))
                                || (i.Description.Contains(searchRaceRequest.Search, StringComparison.OrdinalIgnoreCase))
                            )
                    .Where(i => searchRaceRequest.PrefferedLanguage.Contains(i.Asset.Language))
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


                return result;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError("=========== I WAS CANCELLED ==========");
                throw ex;
            }
        }
    }
}
