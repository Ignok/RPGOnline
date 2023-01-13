﻿using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.DTOs.Requests.Asset.Race;
using Microsoft.Extensions.Logging;
using RPGOnline.Application.DTOs.Responses.User;
using RPGOnline.Domain.Enums;
using RPGOnline.Domain.Models;
using RPGOnline.Application.DTOs.Responses.Asset.Race;

namespace RPGOnline.Infrastructure.Services
{
    public class RaceService : IRace
    {
        private readonly IApplicationDbContext _dbContext;
        private ILogger<RaceService> _logger;

        public RaceService(IApplicationDbContext dbContext, ILogger<RaceService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        private readonly int racesOnPageAmount = 10;

        public async Task<(ICollection<GetRaceResponse>, int pageCount)> GetRaces(SearchAssetRequest searchRaceRequest, CancellationToken cancellationToken)
        {
            try
            {

                var page = searchRaceRequest.Page;
                if (searchRaceRequest.Page <= 0) throw new ArgumentOutOfRangeException(nameof(page));



                var result = _dbContext.Races.Include(r => r.Asset)
                                                .Include(r => r.Asset.UserSavedAssets)
                                                .Include(r => r.Asset.Author)
                                                .AsParallel().WithCancellation(cancellationToken)
                    .Where(r => r.Asset.IsPublic)
                    .Where(r => String.IsNullOrEmpty(searchRaceRequest.KeyValueName)
                                || object.Equals(r.KeyAttribute, searchRaceRequest.KeyValueName)
                                || object.Equals(r.KeyAttribute, null)
                            )
                    .Where(r => String.IsNullOrEmpty(searchRaceRequest.Search)
                                || (r.RaceName.Contains(searchRaceRequest.Search, StringComparison.OrdinalIgnoreCase))
                                || (r.Description.Contains(searchRaceRequest.Search, StringComparison.OrdinalIgnoreCase))
                            )
                    .Where(r => searchRaceRequest.PrefferedLanguage.Contains(r.Asset.Language))
                    .Select(r => new GetRaceResponse()
                    {
                        AssetId = r.Asset.AssetId,
                        CreationDate = r.Asset.CreationDate,
                        TimesSaved = r.Asset.UserSavedAssets.Count,
                        RaceId = r.RaceId,
                        RaceName = r.RaceName,
                        RaceDescription = r.Description,
                        RaceTalent = r.Talent,
                        RaceHiddenTalent = r.HiddenTalent,
                        RaceKeyAttribute = r.KeyAttribute,
                        PrefferedLanguage = r.Asset.Language,
                        CreatorNavigation = new UserResponse()
                        {
                            UId = r.Asset.Author.UId,
                            Username = r.Asset.Author.Username,
                            Picture = r.Asset.Author.Picture,
                        }
                    })
                    //.Where(p => p...)  <- kategoria
                    .OrderByDescending(p => p.CreationDate)
                    .ToList();


                int pageCount = (int)Math.Ceiling((double)result.Count / racesOnPageAmount);

                result = result
                    .Skip(racesOnPageAmount * (page - 1))
                    .Take(racesOnPageAmount)
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

        public async Task<ICollection<GetRaceSimplifiedResponse>> GetRacesForCharacter(int uId, GetAssetForCharacterRequest getRaceRequest)
        {
           // select* from asset a join[character] r on a.asset_id = r.asset_id where is_public = 1 or a.author_id = 3;
            var result = await (from asset in _dbContext.Assets
                                join race in _dbContext.Races on asset.AssetId equals race.AssetId
                                where ((object.Equals(race.KeyAttribute, getRaceRequest.KeyValueName))
                                    || (object.Equals(race.KeyAttribute, null)))
                                where getRaceRequest.PrefferedLanguage.Contains(asset.Language)
                                where asset.IsPublic || asset.AuthorId == uId
                                orderby race.RaceName ascending
                                select new GetRaceSimplifiedResponse()
                                {
                                     RaceId = race.RaceId,
                                     AssetId = asset.AssetId,
                                     RaceName = race.RaceName,
                                     Description = race.Description,
                                     Talent = race.Talent,
                                     HiddenTalent = race.HiddenTalent,
                                     KeyAttribute = race.KeyAttribute
                                })
                                .ToListAsync();
            return result;
        }

        public async Task<GetRaceResponse> PostRace(PostRaceRequest postRaceRequest)
        {
            if(postRaceRequest == null) throw new ArgumentNullException(nameof(postRaceRequest));

            //if user exists
            if (!_dbContext.Users.Where(u => u.UId == postRaceRequest.UId).ToList().Any())
                throw new ArgumentException($"User with id: {postRaceRequest.UId} does not exist");

            //if language exists
            if (!Enum.IsDefined(typeof(Language), postRaceRequest.Language))
                throw new InvalidDataException($"Language '{postRaceRequest.Language}' is not supported");

            //if attribute exists, or is null
            if (!Enum.IsDefined(typeof(Attributes), postRaceRequest.KeyAttribute ?? "none"))
                throw new InvalidDataException($"Attribute '{postRaceRequest?.KeyAttribute}' does not exist");


            var asset = new Asset()
            {
                AssetId = (_dbContext.Assets.Max(a => (int)a.AssetId) + 1),
                AuthorId = postRaceRequest.UId,
                IsPublic = postRaceRequest.IsPublic,
                Language = postRaceRequest.Language,
                CreationDate = DateTime.Now,
                Author = await _dbContext.Users.Where(u => u.UId == postRaceRequest.UId).SingleAsync(),
            };

            var race = new Race()
            {
                RaceId = (_dbContext.Races.Max(r => (int)r.AssetId) + 1),
                AssetId = asset.AssetId,
                RaceName = postRaceRequest.RaceName,
                Description = postRaceRequest.Description,
                Talent = postRaceRequest.Talent,
                HiddenTalent = postRaceRequest.HiddenTalent,
                KeyAttribute = postRaceRequest.KeyAttribute.Equals("none") ? null : postRaceRequest.KeyAttribute,
                Asset = asset,
            };

            asset.Races.Add(race);

            _dbContext.Assets.Add(asset);
            _dbContext.Races.Add(race);
            _dbContext.SaveChanges();

            return new GetRaceResponse()
            {
                AssetId = asset.AssetId,
                CreationDate = asset.CreationDate,
                TimesSaved = 0,
                RaceId = race.RaceId,
                RaceName = race.RaceName,
                RaceDescription = race.Description,
                RaceTalent = race.Talent,
                RaceHiddenTalent = race.HiddenTalent,
                RaceKeyAttribute = race.KeyAttribute,
                PrefferedLanguage = race.Asset.Language,
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