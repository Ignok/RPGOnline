﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Profession;
using RPGOnline.Application.DTOs.Responses.Asset.Profession;
using RPGOnline.Application.DTOs.Responses.User;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Enums;
using RPGOnline.Domain.Models;

namespace RPGOnline.Infrastructure.Services
{
    public class ProfessionService : IProfession
    {
        private readonly IApplicationDbContext _dbContext;
        private ILogger<ProfessionService> _logger;

        public ProfessionService(IApplicationDbContext dbContext, ILogger<ProfessionService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        private readonly int professionsOnPageAmount = 10;

        public async Task<ICollection<GetProfessionSimplifiedResponse>> GetProfessionsForCharacter(int uId, GetAssetForCharacterRequest getProfessionRequest)
        {
            var result = await (from asset in _dbContext.Assets
                                join profession in _dbContext.Professions on asset.AssetId equals profession.AssetId
                                where ((object.Equals(profession.KeyAttribute, getProfessionRequest.KeyValueName))
                                    || (object.Equals(profession.KeyAttribute, null)))
                                where getProfessionRequest.PrefferedLanguage.Contains(asset.Language)
                                where asset.IsPublic || asset.AuthorId == uId
                                orderby profession.ProfessionName ascending
                                select new GetProfessionSimplifiedResponse()
                                {
                                    ProfessionId = profession.ProfessionId,
                                    AssetId = asset.AssetId,
                                    ProfessionName = profession.ProfessionName,
                                    ProfessionDescription = profession.Description,
                                    ProfessionTalent = profession.Talent,
                                    ProfessionHiddenTalent = profession.HiddenTalent,
                                    ProfessionKeyAttribute = profession.KeyAttribute,
                                    WeaponMod = profession.WeaponMod,
                                    ArmorMod = profession.ArmorMod,
                                    GadgetMod = profession.GadgetMod,
                                    CompanionMod = profession.CompanionMod,
                                    PsycheMod = profession.PsycheMod
                                })
                                .ToListAsync();
            return result;
        }

        public async Task<(ICollection<GetProfessionResponse>, int pageCount)> GetProfessions(SearchAssetRequest searchProfessionRequest, CancellationToken cancellationToken)
        {
            try
            {

                var page = searchProfessionRequest.Page;
                if (searchProfessionRequest.Page <= 0) throw new ArgumentOutOfRangeException(nameof(page));



                var result = _dbContext.Professions.Include(p => p.Asset)
                                                .Include(p => p.Asset.UserSavedAssets)
                                                .Include(p => p.Asset.Author)
                                                .AsParallel().WithCancellation(cancellationToken)
                    .Where(p => p.Asset.IsPublic)
                    .Where(p => String.IsNullOrEmpty(searchProfessionRequest.KeyValueName)
                                || object.Equals(p.KeyAttribute, searchProfessionRequest.KeyValueName)
                                || object.Equals(p.KeyAttribute, null)
                            )
                    .Where(p => String.IsNullOrEmpty(searchProfessionRequest.Search)
                                || (p.ProfessionName.Contains(searchProfessionRequest.Search, StringComparison.OrdinalIgnoreCase))
                                || (p.Description.Contains(searchProfessionRequest.Search, StringComparison.OrdinalIgnoreCase))
                            )
                    .Where(p => searchProfessionRequest.PrefferedLanguage.Contains(p.Asset.Language))
                    .Select(p => new GetProfessionResponse()
                    {
                        AssetId = p.Asset.AssetId,
                        CreationDate = p.Asset.CreationDate,
                        TimesSaved = p.Asset.UserSavedAssets.Count,
                        ProfessionId = p.ProfessionId,
                        ProfesionName = p.ProfessionName,
                        ProfessionDescription = p.Description,
                        ProfessionTalent = p.Talent,
                        ProfessionHiddenTalent = p.HiddenTalent,
                        WeaponMod = p.WeaponMod,
                        ArmorMod = p.ArmorMod,
                        GadgetMod = p.GadgetMod,
                        CompanionMod = p.CompanionMod,
                        PsycheMod = p.PsycheMod,
                        PrefferedLanguage = p.Asset.Language,
                        CreatorNavigation = new UserResponse()
                        {
                            UId = p.Asset.Author.UId,
                            Username = p.Asset.Author.Username,
                            Picture = p.Asset.Author.Picture,
                        }
                    })
                    //.Where(p => p...)  <- kategoria
                    .OrderByDescending(p => p.CreationDate)
                    .ToList();


                int pageCount = (int)Math.Ceiling((double)result.Count / professionsOnPageAmount);

                result = result
                    .Skip(professionsOnPageAmount * (page - 1))
                    .Take(professionsOnPageAmount)
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

        public async Task<GetProfessionResponse> PostProfession(PostProfessionRequest postProfessionRequest)
        {
            if (postProfessionRequest == null) throw new ArgumentNullException(nameof(postProfessionRequest));

            //if user exists
            if (!_dbContext.Users.Where(u => u.UId == postProfessionRequest.UId).ToList().Any())
                throw new ArgumentException($"User with id: {postProfessionRequest.UId} does not exist");

            //if language exists
            if (!Enum.IsDefined(typeof(Language), postProfessionRequest.Language))
                throw new InvalidDataException($"Language '{postProfessionRequest.Language}' is not supported");

            //if attribute exists, or is null
            if (!Enum.IsDefined(typeof(Attributes), postProfessionRequest.KeyAttribute ?? "none"))
                throw new InvalidDataException($"Attribute '{postProfessionRequest?.KeyAttribute}' does not exist");


            var asset = new Asset()
            {
                AssetId = (_dbContext.Assets.Max(a => (int)a.AssetId) + 1),
                AuthorId = postProfessionRequest.UId,
                IsPublic = postProfessionRequest.IsPublic,
                Language = postProfessionRequest.Language,
                CreationDate = DateTime.Now,
                Author = await _dbContext.Users.Where(u => u.UId == postProfessionRequest.UId).SingleAsync(),
            };

            var profession = new Profession()
            {
                ProfessionId = (_dbContext.Professions.Max(r => (int)r.AssetId) + 1),
                AssetId = asset.AssetId,
                ProfessionName = postProfessionRequest.ProfessionName,
                Description = postProfessionRequest.Description,
                Talent = postProfessionRequest.Talent,
                HiddenTalent = postProfessionRequest.HiddenTalent,
                KeyAttribute = postProfessionRequest.KeyAttribute.Equals("none") ? null : postProfessionRequest.KeyAttribute,
                WeaponMod = postProfessionRequest.WeaponMod,
                ArmorMod = postProfessionRequest.ArmorMod,
                GadgetMod = postProfessionRequest.GadgetMod,
                CompanionMod = postProfessionRequest.CompanionMod,
                PsycheMod = postProfessionRequest.PsycheMod,
                Asset = asset,
            };

            asset.Professions.Add(profession);

            _dbContext.Assets.Add(asset);
            _dbContext.Professions.Add(profession);
            _dbContext.SaveChanges();

            return new GetProfessionResponse()
            {
                AssetId = profession.Asset.AssetId,
                CreationDate = profession.Asset.CreationDate,
                TimesSaved = profession.Asset.UserSavedAssets.Count,
                ProfessionId = profession.ProfessionId,
                ProfesionName = profession.ProfessionName,
                ProfessionDescription = profession.Description,
                ProfessionTalent = profession.Talent,
                ProfessionHiddenTalent = profession.HiddenTalent,
                WeaponMod = profession.WeaponMod,
                ArmorMod = profession.ArmorMod,
                GadgetMod = profession.GadgetMod,
                CompanionMod = profession.CompanionMod,
                PsycheMod = profession.PsycheMod,
                PrefferedLanguage = asset.Language,
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